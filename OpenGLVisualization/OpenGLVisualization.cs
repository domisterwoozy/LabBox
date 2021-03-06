﻿using LabBox.Visualization.Universe.ViewModel;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LabBox.Visualization.HUD;
using LabBox.Visualization.Input;
using BasicVisualization;
using LabBox.OpenGLVisualization.Shaders;
using LabBox.OpenGLVisualization.ViewModel;
using OpenTK.Graphics;
using HelloVR;
using HelloVR.OpenGL;

namespace LabBox.OpenGLVisualization
{
    public class OpenGLVisualization : GameWindow, ILabBoxVis
    {
        private const int NumFSAASamples = 2;
        private const int ShadowMapSize = 8196;

        private const float FarClip = 10;
        private const float NearClip = 0.1f;

        private int vertexBufferID;
        private int totalNumVerts;

        // programs
        private LitMaterialProgram mainProgram;
        private DepthMapProgram depthProgram;
        private SimpleTextureProgram textureProgram;
        
        // opengl shadow data
        private List<OpenGLLightSource> openGLLights = new List<OpenGLLightSource>();
        private List<Matrix4> depthProjs = new List<Matrix4>();
        private List<Matrix4> depthViews = new List<Matrix4>();

        // internal framework data
        private bool hasStarted = false;
        private readonly List<ILightSource> lightSources = new List<ILightSource>();
        private List<IGraphicalBody> bodies;
        private readonly List<IGraphicalBody> bodiesToAdd = new List<IGraphicalBody>();
        private readonly List<IGraphicalBody> bodiesToRemove = new List<IGraphicalBody>();
        private readonly List<IDisposable> toDispose = new List<IDisposable>();

        // framework interface
        public event EventHandler VisStarted;
        public IEnumerable<ILightSource> LightSources => lightSources;
        public IEnumerable<IGraphicalBody> Bodies => bodies;
        public IEnumerable<IHUDView> HUDs => Enumerable.Empty<IHUDView>(); // not yet implemented 
        public ICamera Camera { get; }
        public IInputObservable Input { get; private set; }

        // VR!
        private readonly bool VRFlag = true;
        private readonly BasicDrawable vrDrawable;
        private readonly OpenVRScene vrScene;        

        public OpenGLVisualization(IEnumerable<IGraphicalBody> graphicalBodies, bool vr, params ILightSource[] lights) : 
            base(1220, 720, new GraphicsMode(GraphicsMode.Default.ColorFormat, GraphicsMode.Default.Depth, GraphicsMode.Default.Stencil, NumFSAASamples, GraphicsMode.Default.AccumulatorFormat),
                "LabBox Vis")
        {
            VRFlag = vr;
            if (VRFlag)
            {
                vrScene = OpenVRScene.Create(NearClip, FarClip);
                Input = vrScene.OpenVRInput;
                vrDrawable = new BasicDrawable((vpMat, camPos) => RenderVR(vpMat, camPos));
            }
            else
            {
                Input = new OpenGLInputObservable(this);                
            }

            // not sure how we should handle the ICamera interface in VR yet
            var cam = new FreeCamera(Input);
            cam.MaxRange = FarClip;
            cam.MinRange = NearClip;
            toDispose.Add(cam);
            Camera = cam;

            bodies = graphicalBodies.ToList();
            lightSources = lights.ToList();

            BindEvents();           
        }

        private void BindEvents()
        {
            Load += BasicVis_Load;
            UpdateFrame += BasicVis_UpdateFrame;
            RenderFrame += BasicVis_RenderFrame;
            Disposed += OpenGLVisualization_Disposed;   
        }

        private void OpenGLVisualization_Disposed(object sender, EventArgs e)
        {
            foreach (var dis in toDispose) dis.Dispose();
        }

        public void RunVis()
        {
            Run();
        }

        public void EndVis()
        {
            Exit();
        }

        public void AddBody(IGraphicalBody b)
        {
            bodiesToAdd.Add(b);            
        }

        public bool RemoveBody(IGraphicalBody b)
        {
            if (bodies.Contains(b))
            {
                bodiesToRemove.Add(b);
                return true;
            }
            return false;
        }

        private void BasicVis_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.White);
            GL.Enable(EnableCap.DepthTest); // enable depth testing
            GL.DepthFunc(DepthFunction.Less); // only accept fragment if it is closer to the camera than whats in there already
            GL.Enable(EnableCap.Multisample); // standard AA
            GL.Enable(EnableCap.Blend); // transparency
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha); // transparency func
                     
            if (VRFlag)
            {
                WindowState = WindowState.Minimized;
                vrScene.InitGraphics(new GLVRGraphics(vrScene, true)); // always use GL graphics
            }
            else
            {
                WindowBorder = WindowBorder.Hidden;
                WindowState = WindowState.Fullscreen;
                CursorVisible = false;
                Camera.IsLocked = false;
            }         

            mainProgram = new LitMaterialProgram();
            depthProgram = new DepthMapProgram();
            textureProgram = new SimpleTextureProgram();
            vertexBufferID = OpenGLUtil.CreateBufferObject();

            UpdateBodiesCollection(); // incase any bodies were added/removed before the window loaded
            PopulateVertexBuffer();

            // create and frame buffer and shadow map for each light source if they cast shadows
            // this only occurs once
            foreach (var light in LightSources)
            {
                OpenGLLightSource glLight = light.ToGLLight();
                if (light.CastsDynamicShadows)
                {
                    glLight.FrameBufferID =  OpenGLUtil.CreateFrameBuffer();
                    glLight.ShadowMapID = OpenGLUtil.CreateDepthTexture(glLight.FrameBufferID, ShadowMapSize);
                }                
                openGLLights.Add(glLight); 
            }       
        }

        private void PopulateVertexBuffer()
        {
            var vertArr = Bodies.SelectMany(gb => gb.Vertices()).ToArray();
            totalNumVerts = vertArr.Length;
            OpenGLUtil.PopulateBuffer(vertexBufferID, vertArr);
        }

        // returns whether any bodies were added or removed
        private bool UpdateBodiesCollection()
        {
            if (bodiesToAdd.Count == 0 && bodiesToRemove.Count == 0) return false;
            bodies.AddRange(bodiesToAdd);
            foreach (var b in bodiesToRemove) bodies.Remove(b);
            bodiesToAdd.Clear();
            bodiesToRemove.Clear();
            return true;
        }

        private void BasicVis_UpdateFrame(object sender, FrameEventArgs e)
        {            
            if (!hasStarted)
            {
                hasStarted = true;
                VisStarted?.Invoke(this, e);
            }
            if (UpdateBodiesCollection()) PopulateVertexBuffer();
            if (VRFlag) vrScene.UpdateTracking(); // update tracking every frame
        }

        private void BasicVis_RenderFrame(object sender, FrameEventArgs e)
        {            
            RenderLightMapDepths();           

            if (VRFlag)
            {
                vrScene.RenderFrame(vrDrawable);
                //RenderGraphicsToScreen();
            }
            else
            {
                RenderGraphicsToScreen();                
            }

            //RenderDepthToScreen();
            SwapBuffers();         
        }

        private void RenderLightMapDepths()
        {       
            foreach (var light in openGLLights)
            {
                Matrix4 depthProj = default(Matrix4);
                Matrix4 depthView = default(Matrix4);

                if (light.FrameBufferID == -1 || light.ShadowMapID == -1) // no dynamic shadows
                {
                    depthProjs.Add(depthProj);
                    depthViews.Add(depthView);
                    continue;
                }                

                if (light.IsDirectional)
                {
                    // ortho view from light (directional)
                    var upDir = Vector3.UnitZ;
                    if (Vector3.Cross(upDir, light.Pos.Xyz).LengthSquared == 0) upDir = Vector3.UnitY; // up can be any direction except the look at direction                       
                    depthProj = Matrix4.CreateOrthographic(FarClip, FarClip, -FarClip / 2, 2 * FarClip);
                    depthView = Matrix4.LookAt(FarClip * light.Pos.Xyz, Vector3.Zero, upDir);
                }
                else
                {
                    // will look shitty for a point light
                    // projective view from light (spotlight)
                    var upDir = Vector3.UnitZ;
                    if (Vector3.Cross(upDir, light.ConeDir).LengthSquared == 0) upDir = Vector3.UnitY; // up can be any direction except the look at direction  
                    depthProj = Matrix4.CreatePerspectiveFieldOfView(light.ConeAngle * 2, 1.0f, Camera.MinRange, Camera.MaxRange); // uses camera specs to guide percision
                    depthView = Matrix4.LookAt(light.Pos.Xyz, light.Pos.Xyz + light.ConeDir, upDir); 
                }

                OpenGLUtil.UseFrameBuffer(light.FrameBufferID); // use our custom framebuffer instead of the screen
                GL.Viewport(0, 0, ShadowMapSize, ShadowMapSize); // render on teh entire framebuffer
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // clear the framebuffer

                depthProgram.UseProgram();
                depthProgram.EnableAttributes();
                depthProgram.LoadBuffer(vertexBufferID);

                int startIndex = 0;
                foreach (IGraphicalBody body in Bodies)
                {
                    Matrix4 scale = Matrix4.Identity;
                    Matrix4 rotation = Matrix4.CreateFromQuaternion(body.Orientation.ToGLQuaternion());
                    Matrix4 translation = Matrix4.CreateTranslation(body.Translation.ToGLVector3());
                    Matrix4 model = scale * rotation * translation;
                    depthProgram.SetMVP(model * depthView * depthProj);

                    int numVerts = body.Triangles.Length * 3;
                    GL.DrawArrays(PrimitiveType.Triangles, startIndex, numVerts);  // here the fragment shader will automatically write the depth to the texture bc of location 0
                    startIndex += numVerts;
                }

                depthProgram.DisableAttributes();
                
                depthProjs.Add(depthProj);
                depthViews.Add(depthView);
                OpenGLUtil.CheckInvalidFrameBuffer();  
            }      
        }

        // for debugging
        private void RenderDepthToScreen()
        {
            OpenGLUtil.UseFrameBuffer(0); // now render to the screen
            GL.Viewport(0, 0, 512, 512); // in the corner                   
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // clear the screen    

            textureProgram.UseProgram(); // use the texture program to display the 2d texture
            textureProgram.SetTexture(openGLLights[1].ShadowMapID);
            textureProgram.EnableAttributes();
            textureProgram.LoadBuffer(vertexBufferID); // load the positions so that it can use them to map to the UV coordinates
            OpenGLUtil.DisableTextureCompare(openGLLights[1].ShadowMapID);
            GL.DrawArrays(PrimitiveType.Triangles, 0, totalNumVerts);
            OpenGLUtil.EnableTextureCompare(openGLLights[1].ShadowMapID);
            textureProgram.DisableAttributes();
        }

        private void RenderGraphicsToScreen()
        {
            OpenGLUtil.UseFrameBuffer(0); // make sure we are rendering to the screen
            GL.Viewport(0, 0, Width, Height); // set view prot to cover full window
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            mainProgram.UseProgram();

            // camera properties
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(Camera.VertFOV, Camera.AspectRatio, Camera.MinRange, Camera.MaxRange); // camera props
            Matrix4 view = Matrix4.LookAt(Camera.Pos.ToGLVector3(), Camera.LookAtPos.ToGLVector3(), Camera.UpDir.ToGLVector3()); // camera state
            mainProgram.SetCameraPosition(Camera.Pos.ToGLVector3());

            // light/shadow properties           
            mainProgram.AddLights(openGLLights.ToArray());

            // material properties
            float shininess = 50.0f;
            Vector3 specularColor = (Color.White.ToGLVector3());
            mainProgram.SetMaterialProperties(specularColor, shininess);

            mainProgram.EnableAttributes();
            mainProgram.LoadBuffer(vertexBufferID);

            int startIndex = 0;
            foreach (IGraphicalBody body in Bodies)
            {
                // get the model matrix and send it
                Matrix4 scale = Matrix4.Identity;
                Matrix4 rotation = Matrix4.CreateFromQuaternion(body.Orientation.ToGLQuaternion());
                Matrix4 translation = Matrix4.CreateTranslation(body.Translation.ToGLVector3());
                Matrix4 model = scale * rotation * translation;
                mainProgram.SetMVP(model, view, proj);
                mainProgram.SetShadowCasterMVPs(LightSources.Select((l,i) => model * depthViews[i] * depthProjs[i]).ToArray());

                int numVerts = body.Triangles.Length * 3;
                GL.DrawArrays(PrimitiveType.Triangles, startIndex, numVerts);
                startIndex += numVerts;
            }
            mainProgram.DisableAttributes();
        } 

        private void RenderVR(Matrix4 viewProjMatrix, Vector3 camPos)
        {                 
            mainProgram.UseProgram();

            // camera properties
            mainProgram.SetCameraPosition(camPos);

            // light/shadow properties           
            mainProgram.AddLights(openGLLights.ToArray());

            // material properties
            float shininess = 50.0f;
            Vector3 specularColor = (Color.White.ToGLVector3());
            mainProgram.SetMaterialProperties(specularColor, shininess);

            mainProgram.EnableAttributes();
            mainProgram.LoadBuffer(vertexBufferID);

            int startIndex = 0;
            foreach (IGraphicalBody body in Bodies)
            {
                // get the model matrix and send it
                Matrix4 scale = Matrix4.Identity;
                Matrix4 rotation = Matrix4.CreateFromQuaternion(body.Orientation.ToGLQuaternion());
                Matrix4 translation = Matrix4.CreateTranslation(body.Translation.ToGLVector3());
                Matrix4 model = scale * rotation * translation;
                mainProgram.SetMVP(viewProjMatrix, model);
                mainProgram.SetShadowCasterMVPs(LightSources.Select((l, i) => model * depthViews[i] * depthProjs[i]).ToArray());

                int numVerts = body.Triangles.Length * 3;
                GL.DrawArrays(PrimitiveType.Triangles, startIndex, numVerts);
                startIndex += numVerts;
            }
            mainProgram.DisableAttributes();
        }
    }
}

