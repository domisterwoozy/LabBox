﻿using LabBox.Visualization.Universe;
using LabBox.Visualization.Universe.ViewModel;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBox.Visualization.HUD;
using LabBox.Visualization.Input;
using System.Runtime.InteropServices;
using BasicVisualization;
using LabBox.OpenGLVisualization.Shaders;
using LabBox.OpenGLVisualization.ViewModel;

namespace LabBox.OpenGLVisualization
{
    public class OpenGLVisualization : GameWindow, ILabBoxVis
    {
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
        private readonly List<ILightSource> lightSources = new List<ILightSource>();
        private List<IGraphicalBody> bodies;
        private readonly List<IGraphicalBody> bodiesToAdd = new List<IGraphicalBody>();
        private readonly List<IGraphicalBody> bodiesToRemove = new List<IGraphicalBody>();

        // framework interface
        public IEnumerable<ILightSource> LightSources => lightSources;
        public IEnumerable<IGraphicalBody> Bodies => bodies;
        public IEnumerable<IHUDView> HUDs => Enumerable.Empty<IHUDView>(); // not yet implemented 
        public ICamera Camera { get;}
        public IInputHandler InputHandler { get; }             

        public OpenGLVisualization(IInputHandler inputHandler, ICamera camera, IEnumerable<IGraphicalBody> graphicalBodies, params ILightSource[] lights) : base()
        {
            InputHandler = inputHandler;
            Camera = camera;
            bodies = graphicalBodies.ToList();
            lightSources = lights.ToList();

            BindEvents();
        }

        public OpenGLVisualization(IEnumerable<IGraphicalBody> graphicalBodies, params ILightSource[] lights) : base()
        {
            // use opengl defaults
            InputHandler = new OpenGLInputHandler(this);
            Camera = new FreeCamera(InputHandler);
            bodies = graphicalBodies.ToList();
            lightSources = lights.ToList();

            BindEvents();           
        }


        private void BindEvents()
        {
            Load += BasicVis_Load;
            UpdateFrame += BasicVis_UpdateFrame;
            RenderFrame += BasicVis_RenderFrame;            
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
            // window setup
            Title = "Basic Vis";
            WindowBorder = WindowBorder.Hidden;
            WindowState = WindowState.Fullscreen;
            CursorVisible = false;

            // my model setup
            Camera.IsLocked = false;

            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.DepthTest); // enable depth testing
            GL.DepthFunc(DepthFunction.Less); // only accept fragment if it is closer to the camera than whats in there already

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
                    glLight.ShadowMapID = OpenGLUtil.CreateDepthTexture(glLight.FrameBufferID);
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
            if (UpdateBodiesCollection()) PopulateVertexBuffer();
        }

        private void BasicVis_RenderFrame(object sender, FrameEventArgs e)
        {
            RenderLightMapDepths();            
            RenderGraphicsToScreen();
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
                    float sceneDimSize = Camera.MaxRange;
                    var upDir = Vector3.UnitZ;
                    if (Vector3.Cross(upDir, light.Pos.Xyz).LengthSquared == 0) upDir = Vector3.UnitY; // up can be any direction except the look at direction                       
                    depthProj = Matrix4.CreateOrthographic(sceneDimSize, sceneDimSize, -sceneDimSize / 2, 2 * sceneDimSize);
                    depthView = Matrix4.LookAt(sceneDimSize * light.Pos.Xyz, Vector3.Zero, upDir);
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
                GL.Viewport(0, 0, 4096, 4096); // render on teh entire framebuffer
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // clear the screen

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
            GL.Viewport(0, 0, 512, 512);
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // clear the screen            

            textureProgram.UseProgram(); // use the texture program to display the 2d texture
            textureProgram.SetTexture(openGLLights[0].ShadowMapID);
            textureProgram.EnableAttributes();
            textureProgram.LoadBuffer(vertexBufferID); // load the positions so that it can use them to map to the UV coordinates
            OpenGLUtil.DisableTextureCompare(openGLLights[0].ShadowMapID);
            GL.DrawArrays(PrimitiveType.Triangles, 0, totalNumVerts);
            OpenGLUtil.EnableTextureCompare(openGLLights[0].ShadowMapID);
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
    }
}
