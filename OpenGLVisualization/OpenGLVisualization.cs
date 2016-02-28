using LabBox.Visualization.Universe;
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

namespace LabBox.OpenGLVisualization
{
    public class OpenGLVisualization : GameWindow, ILabBoxVis
    {
        //private int vertexArrayID;
        private int vertexBufferID;

        private LitMaterialProgram myProgram;


        public ICollection<IGraphicalBody> Bodies { get; }
        public ICamera Camera { get;}
        public IInputHandler InputHandler { get; }
        public ICollection<IHUDView> HUDs => null;        

        public OpenGLVisualization(IInputHandler inputHandler, ICamera camera, params IGraphicalBody[] graphicalBodies) : base()
        {
            InputHandler = inputHandler;
            Camera = camera;
            Bodies = graphicalBodies.ToList();

            BindEvents();
        }

        public OpenGLVisualization(params IGraphicalBody[] graphicalBodies) : base()
        {
            // use opengl defaults
            InputHandler = new OpenGLInputHandler(this);
            Camera = new FreeCamera(InputHandler);
            Bodies = graphicalBodies.ToList();

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

            //vertexArrayID = OpenGLUtil.CreateVertexArrayObject();
            //OpenGLUtil.UseVertexArrayObject(vertexArrayID);

            vertexBufferID = OpenGLUtil.CreateBufferObject();
            OpenGLUtil.PopulateBuffer(vertexBufferID, Bodies.SelectMany(gb => gb.Vertices()).ToArray());

            myProgram = new LitMaterialProgram();
        }

        private void BasicVis_UpdateFrame(object sender, FrameEventArgs e)
        {
        }

        private void BasicVis_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.Viewport(0, 0, Width, Height); // set view prot to cover full window
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            myProgram.UseProgram();          

            // camera properties
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(Camera.VertFOV, Camera.AspectRatio, Camera.MinRange, Camera.MaxRange); // camera props
            Matrix4 view = Matrix4.LookAt(Camera.Pos.ToGLVector3(), Camera.LookAtPos.ToGLVector3(), Camera.UpDir.ToGLVector3()); // camera state
            myProgram.SetCameraPosition(Camera.Pos.ToGLVector3());

            // light properties
            var light = LightSource.Directional(new Math3D.Vector3(1, 1, -1));

            var light2 = LightSource.SpotLight(new Math3D.Vector3(10, 0, 5), new Math3D.Vector3(0, 0, -1), Math.PI / 6.5);

            var light3 = LightSource.PointLight(new Math3D.Vector3(-10, 0, -3), 10.0f);
 
            var test = light2.ToGLLight();
            myProgram.AddLights(light.ToGLLight(), light2.ToGLLight(), light3.ToGLLight());

            // material properties
            float shininess = 50.0f;
            Vector3 specularColor = (Color.White.ToGLVector3());
            myProgram.SetMaterialProperties(specularColor, shininess);

            myProgram.EnableAttributes();
            myProgram.LoadBuffer(vertexBufferID);

            int startIndex = 0;
            foreach (IGraphicalBody body in Bodies)
            {
                // get the model matrix and send it
                Matrix4 scale = Matrix4.Identity;
                Matrix4 rotation = Matrix4.CreateFromQuaternion(body.Orientation.ToGLQuaternion());
                Matrix4 translation = Matrix4.CreateTranslation(body.Translation.ToGLVector3());
                Matrix4 model = scale * rotation * translation;
                myProgram.SetMVP(model, view, proj);              

                int numVerts = body.Triangles.Length * 3;
                GL.DrawArrays(PrimitiveType.Triangles, startIndex, numVerts);
                startIndex += numVerts;                
            }

            myProgram.DisableAttributes();
                       
            SwapBuffers();
        }      
    }
}
