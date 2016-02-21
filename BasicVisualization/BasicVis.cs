using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicVisualization
{
    public class BasicVis : GameWindow
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

        // vertex buffer components
        private int vboPos;
        private int vboColor;

        public ICollection<IGraphicalBody> GraphicalBodies { get; }

        public BasicVis(params IGraphicalBody[] graphicalBodies) : base()
        {
            Load += BasicVis_Load;
            UpdateFrame += BasicVis_UpdateFrame;
            RenderFrame += BasicVis_RenderFrame;

            GraphicalBodies = graphicalBodies.ToList();
        }

        private void BasicVis_Load(object sender, EventArgs e)
        {
            InitProgram();
            Title = "Basic Vis";
            GL.GenBuffers(1, out vboPos);
            GL.GenBuffers(1, out vboColor);
            GL.UseProgram(pgmID);
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(Color.Blue);
            GL.PointSize(5.0f);
        }

        private void BasicVis_UpdateFrame(object sender, FrameEventArgs e)
        {          
            // update physics
            foreach (var body in GraphicalBodies)
            {
                body.Translation -= e.Time * Math3D.Vector3.K;
                body.Orientation *= Math3D.Quaternion.UnitQuaternion(e.Time, new Math3D.Vector3(1, 1, 1));
            }

            // fill all of the verts and colors onto two large buffers
            var verts = GraphicalBodies.SelectMany(gb => gb.Vertices()).ToArray();
            var colors = GraphicalBodies.SelectMany(gb => gb.Colors()).ToArray();
            
            // send the vertexes
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboPos); // tell opengl were about to use the vertex position buffer
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verts.Length * Vector3.SizeInBytes), verts, BufferUsageHint.StaticDraw); // send the vertex position to the buffer
            GL.VertexAttribPointer(vsPos, 3, VertexAttribPointerType.Float, false, 0, 0); // bind teh buffer to the vertex shader position attirbute

            // send the colors        
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboColor);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colors.Length * Vector3.SizeInBytes), colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(vsColor, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void BasicVis_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.Viewport(0, 0, Width, Height); // set view prot to cover full window

            // clear the frame
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
           // GL.Enable(EnableCap.DepthTest);

            GL.EnableVertexAttribArray(vsPos);
            GL.EnableVertexAttribArray(vsColor);

            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView((float)(Math.PI / 2), (float)Width / Height, 0.1f, 100.0f); // camera props
            Matrix4 view = Matrix4.LookAt(new Vector3(0, 0, 10), Vector3.Zero, new Vector3(0, 1, 0)); // camera state

            int startIndex = 0;
            foreach (IGraphicalBody body in GraphicalBodies)
            {
                // send the transformation
                Matrix4 scale = Matrix4.Identity;
                Matrix4 rotation = Matrix4.CreateFromQuaternion(body.Orientation.ToGLQuaternion());
                Matrix4 translation = Matrix4.CreateTranslation((float)body.Translation.X, (float)body.Translation.Y, (float)body.Translation.Z);
                Matrix4 model = scale * rotation * translation;
                GL.UniformMatrix4(vsModel, false, ref model);
                GL.UniformMatrix4(vsView, false, ref view);
                GL.UniformMatrix4(vsProj, false, ref proj);
                int numVerts = body.Triangles.Length * 3;
                GL.DrawArrays(PrimitiveType.Triangles, startIndex, startIndex + numVerts);
                startIndex += numVerts;                
            }            

            GL.DisableVertexAttribArray(vsPos);
            GL.DisableVertexAttribArray(vsColor);

            SwapBuffers();
        }

        

        private void InitProgram()
        {
            pgmID = GL.CreateProgram();
            vsID = LoadShader("vs.glsl", ShaderType.VertexShader, pgmID);
            fsID = LoadShader("fs.glsl", ShaderType.FragmentShader, pgmID);
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
