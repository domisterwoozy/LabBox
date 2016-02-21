using BasicVisualization.Input;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicVisualization.Implementations.OpenGL
{
    public class OpenGLInputHandler : IInputHandler
    {
        private readonly GameWindow glWindow;

        private KeyboardInput down { get; }
        private KeyboardInput left { get; }
        private KeyboardInput right { get; }
        private KeyboardInput up { get; }

        public IInput Down => down;
        public IInput Left => left;
        public IInput Right => right;
        public IInput Up => up;

        public OpenGLInputHandler(GameWindow glWindow)
        {
            this.glWindow = glWindow;
            glWindow.KeyDown += GlWindow_KeyDown;
            glWindow.KeyUp += GlWindow_KeyUp;
        }

        private void GlWindow_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    left.RaiseStart();
                    break;
                case Key.Right:
                    right.RaiseStart();
                    break;
                case Key.Up:
                    up.RaiseStart();
                    break;
                case Key.Down:
                    down.RaiseStart();
                    break;
                default:
                    break;
            }
        }

        private void GlWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    left.RaiseFinish();
                    break;
                case Key.Right:
                    right.RaiseFinish();
                    break;
                case Key.Up:
                    up.RaiseFinish();
                    break;
                case Key.Down:
                    down.RaiseFinish();
                    break;
                default:
                    break;
            }
        }

        private class KeyboardInput : IInput
        {
            private readonly Key glKey;

            public bool IsActive => Keyboard.GetState()[glKey];
            public double Weight => IsActive ? 1.0 : 0.0;
            public event EventHandler Finish;
            public event EventHandler Start;

            public KeyboardInput(Key glKey)
            {
                this.glKey = glKey;
            }

            public void RaiseStart()
            {
                Start?.Invoke(this, EventArgs.Empty);
            }

            public void RaiseFinish()
            {
                Finish?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
