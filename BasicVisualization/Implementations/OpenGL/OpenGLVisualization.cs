using BasicVisualization.Universe;
using BasicVisualization.Universe.ViewModel;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicVisualization.HUD;
using BasicVisualization.Input;
using System.Runtime.InteropServices;

namespace BasicVisualization.Implementations.OpenGL
{
    public class OpenGLVisualization : GameWindow, ILabBoxVis
    {       

        // program addresses
        private int pgmID;
        private int vsID;
        private int fsID;

        // vertex shader components
        private int vsPos;
        private int vsColor;
        private int vsModel;
        private int vsView;
        private int vsProj;

        // vertex buffer object
        private int vbo;

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

            // opengl setup
            /// use our program and buffers
            InitProgram();            
            GL.GenBuffers(1, out vbo);
            GL.UseProgram(pgmID);
            // z ordering
            GL.Enable(EnableCap.DepthTest);
            //// transparency
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            // background color
            GL.ClearColor(Color.Black);
            // smallest point diameter
            GL.PointSize(5.0f);
        }

        private void BasicVis_UpdateFrame(object sender, FrameEventArgs e)
        {
            // fill all of the positions and colors into one large vertex
            var verts = Bodies.SelectMany(gb => gb.Vertices()).ToArray();

            // send the vertexes
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo); // tell opengl what buffer were using for the frame
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verts.Length * OpenGLVertex.Stride), verts, BufferUsageHint.StaticDraw); // send the vertex data to the array buffer so drawarrays can use it
            GL.VertexAttribPointer(vsPos, 3, VertexAttribPointerType.Float, false, OpenGLVertex.Stride, OpenGLVertex.PositionOffset); // bind teh buffer to the vertex shader position attirbute
            GL.VertexAttribPointer(vsColor, 4, VertexAttribPointerType.Float, false, OpenGLVertex.Stride, OpenGLVertex.ColorOffset); // bind teh buffer to the vertex shader color attirbute

            // tell opengl were using these attributes
            GL.EnableVertexAttribArray(vsPos);
            GL.EnableVertexAttribArray(vsColor);
        }

        private void BasicVis_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.Viewport(0, 0, Width, Height); // set view prot to cover full window

            // clear the frame
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);            

            // constant view/proj transformations and send them to opengl
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(Camera.VertFOV, Camera.AspectRatio, Camera.MinRange, Camera.MaxRange); // camera props
            Matrix4 view = Matrix4.LookAt(Camera.Pos.ToGLVector3(), Camera.LookAtPos.ToGLVector3(), Camera.UpDir.ToGLVector3()); // camera state
            GL.UniformMatrix4(vsView, false, ref view); // send to opengl
            GL.UniformMatrix4(vsProj, false, ref proj); // send to opengl

            int startIndex = 0;
            foreach (IGraphicalBody body in Bodies)
            {
                // get the model matrix and send it
                Matrix4 scale = Matrix4.Identity;
                Matrix4 rotation = Matrix4.CreateFromQuaternion(body.Orientation.ToGLQuaternion());
                Matrix4 translation = Matrix4.CreateTranslation(body.Translation.ToGLVector3());
                Matrix4 model = scale * rotation * translation;
                GL.UniformMatrix4(vsModel, false, ref model);                

                int numVerts = body.Triangles.Length * 3;
                GL.DrawArrays(PrimitiveType.Triangles, startIndex, numVerts);
                startIndex += numVerts;                
            }
                       
            SwapBuffers();
        }        

        private void InitProgram()
        {
            pgmID = GL.CreateProgram();
            vsID = LoadShader("Implementations\\OpenGL\\Shaders\\vs.glsl", ShaderType.VertexShader, pgmID);
            fsID = LoadShader("Implementations\\OpenGL\\Shaders\\fs.glsl", ShaderType.FragmentShader, pgmID);
            GL.LinkProgram(pgmID);
            Console.WriteLine(GL.GetProgramInfoLog(pgmID));

            vsPos = GL.GetAttribLocation(pgmID, "vPosition");
            vsColor = GL.GetAttribLocation(pgmID, "vColor");
            vsModel = GL.GetUniformLocation(pgmID, "model");
            vsView = GL.GetUniformLocation(pgmID, "view");
            vsProj = GL.GetUniformLocation(pgmID, "proj");

            if (vsPos == -1 || vsColor == -1 || vsModel == -1 || vsView == -1 || vsProj == -1)
            {
                Console.WriteLine("Error binding attributes");
            }            
        }

        /// <summary>
        /// Loads a shader into the OpenGL pipeline.
        /// It loads the shader from a file, compiles the shader, attaches the shader to a program, and then returns the shaders address.
        /// </summary>
        private static int LoadShader(string filename, ShaderType shaderType, int programAddr)
        {
            int shaderAddr = GL.CreateShader(shaderType); // create the new address
            using (var sr = new StreamReader(filename)) // apply the file source code to that address
            {
                GL.ShaderSource(shaderAddr, sr.ReadToEnd());
            }
            GL.CompileShader(shaderAddr); // compile the source code
            GL.AttachShader(programAddr, shaderAddr); // attach the shader to the program
            Console.WriteLine(GL.GetShaderInfoLog(shaderAddr));
            return shaderAddr;
        }
    }
}
