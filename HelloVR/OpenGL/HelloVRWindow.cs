using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;

namespace HelloVR.OpenGL
{
    public class HelloVRWindow : GameWindow
    {
        private OpenVRScene scene;      

        public HelloVRWindow(string windowTitle) : base(1280, 720, GraphicsMode.Default, windowTitle, GameWindowFlags.Default)
        {
            scene = OpenVRScene.Create(0.1f, 20.0f);

            Load += HelloVRWindow_Load;
            UpdateFrame += HelloVRWindow_UpdateFrame;
            RenderFrame += HelloVRWindow_RenderFrame;
        }
        

        private void HelloVRWindow_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.White);
            scene.InitGraphics(new GLVRGraphics(scene, true));

            // setup input handling
            KeyDown += HelloVRWindow_KeyDown;
        }

        private void HelloVRWindow_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == OpenTK.Input.Key.Escape) Exit();
        }

        private void HelloVRWindow_UpdateFrame(object sender, FrameEventArgs e)
        {
            scene.UpdateTracking();
        }

        private void HelloVRWindow_RenderFrame(object sender, FrameEventArgs e)
        {
            scene.RenderFrame();
        }        

    }
}
