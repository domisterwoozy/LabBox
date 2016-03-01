using LabBox.OpenGLVisualization.Shaders;
using LabBox.OpenGLVisualization.ViewModel;
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
        //private int vertexArrayID;
        private int vertexBufferID;
        private int frameBufferID;
        private int textureID;

        private static OpenGLVertex[] vertexBufferData = FlatFactory.NewCuboid(1, 1, 1).Vertices();
        //private static OpenGLVertex[] vertexBufferData = FlatFactory.NewCuboid(2, 2, 2, Color.Blue).Vertices();

        private DepthMapProgram depthProgram;
        private SimpleTextureProgram textureProgram;                                      

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

            //vertexArrayID = OpenGLUtil.CreateVertexArrayObject();
            //OpenGLUtil.UseVertexArrayObject(vertexArrayID);

            vertexBufferID = OpenGLUtil.CreateBufferObject();
            OpenGLUtil.PopulateBuffer(vertexBufferID, vertexBufferData);

            depthProgram = new DepthMapProgram();
            textureProgram = new SimpleTextureProgram();

            frameBufferID = OpenGLUtil.CreateFrameBuffer();            
            textureID = OpenGLUtil.CreateDepthTexture(frameBufferID);    
        }

        private void BasicVis_UpdateFrame(object sender, FrameEventArgs e)
        {
            
        }

        private void BasicVis_RenderFrame(object sender, FrameEventArgs e)
        {
            OpenGLUtil.UseFrameBuffer(frameBufferID); // render to our custom framebuffer
            GL.Viewport(0, 0, 1024, 1024); // render on teh entire framebuffer

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // clear the screen
            depthProgram.UseProgram(); // use the depth program

            // camera properties
            Matrix4 proj = Matrix4.CreateOrthographic(100, 100, 0, 100);
            Matrix4 view = Matrix4.LookAt(new Vector3(4, 3, 3), Vector3.Zero, new Vector3(0, 1, 0));
            Matrix4 model = Matrix4.Identity;
            depthProgram.SetMVP(model * view * proj);

            depthProgram.EnableAttributes();
            depthProgram.LoadBuffer(vertexBufferID);

            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexBufferData.Length); // here the fragment shader will automatically write the depth to the texture bc of location 0
            depthProgram.DisableAttributes();
            //OpenGLUtil.CheckInvalidFrameBuffer();


            OpenGLUtil.UseFrameBuffer(0); // now render to the screen
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // clear the screen

            textureProgram.UseProgram(); // use the texture program to display the 2d texture
            textureProgram.SetTexture(textureID);
            textureProgram.EnableAttributes();
            textureProgram.LoadBuffer(vertexBufferID); // load the positions so that it can use them to map to the UV coordinates
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexBufferData.Length);
            textureProgram.DisableAttributes();

            SwapBuffers();
        }
    }
}
