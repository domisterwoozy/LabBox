using LabBox.OpenGLVisualization.Shaders;
using LabBox.Visualization.Universe.ViewModel;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.OpenGLVisualization
{
    public class DebugVis : GameWindow
    {
        private int vertexArrayID;
        private int vertexBufferID;

        private static OpenGLVertex[] vertexBufferData = SphereFactory.NewSphere(Color.Blue, 2, 3).Vertices();
        //private static OpenGLVertex[] vertexBufferData = FlatFactory.NewCuboid(2, 2, 2, Color.Blue).Vertices();

        private LitMaterialProgram myProgram;                                      

        public DebugVis()
        {
            Load += BasicVis_Load;
            UpdateFrame += BasicVis_UpdateFrame;
            RenderFrame += BasicVis_RenderFrame;
        }

        private void BasicVis_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.DepthTest); // enable depth testing
            GL.DepthFunc(DepthFunction.Less); // only accept fragment if it is closer to the camera than whats in there already

            vertexArrayID = OpenGLUtil.CreateVertexArrayObject();
            OpenGLUtil.UseVertexArrayObject(vertexArrayID);

            vertexBufferID = OpenGLUtil.CreateBufferObject();
            OpenGLUtil.PopulateBuffer(vertexBufferID, vertexBufferData);

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
            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView((float)(Math.PI / 3), (float)Width / Height, 0.1f, 100.0f);
            Matrix4 view = Matrix4.LookAt(new Vector3(4, 3, 3), Vector3.Zero, new Vector3(0, 1, 0));

            // light properties
            Vector3 lightPos = new Vector3(0, 4, 0);
            //float lightPower = 50.0f;
            Vector4 lightColor = new Vector4(1,1,1,1);
           // myProgram.SetDiffuseProperties(lightPos, lightColor, lightPower);

            // material properties

            myProgram.EnableAttributes();
            myProgram.LoadBuffer(vertexBufferID);

            // START LOOP            
            Matrix4 model = Matrix4.Identity;
            myProgram.SetMVP(model, view, proj);        
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexBufferData.Length);
            // END LOOP

            myProgram.DisableAttributes();

            SwapBuffers();
        }
    }
}
