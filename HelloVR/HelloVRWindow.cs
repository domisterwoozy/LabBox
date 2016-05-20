using HelloVR.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Valve.VR;

namespace HelloVR
{
    public class HelloVRWindow : GameWindow
    {
        private GLVRScene scene;        

        public HelloVRWindow(string windowTitle) : base(1280, 720, GraphicsMode.Default, windowTitle, GameWindowFlags.Default)
        {
            Load += HelloVRWindow_Load;
            UpdateFrame += HelloVRWindow_UpdateFrame;
            RenderFrame += HelloVRWindow_RenderFrame;
        }
        

        private void HelloVRWindow_Load(object sender, EventArgs e)
        {
            
            scene = GLVRScene.InitScene(0.1f, 20.0f);

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
