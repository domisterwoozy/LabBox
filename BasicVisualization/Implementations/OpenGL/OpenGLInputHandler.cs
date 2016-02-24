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
        private readonly Dictionary<Key, KeyboardInput> keys = new Dictionary<Key, KeyboardInput>();      

        public IInput Down { get; }
        public IInput Left { get; }
        public IInput Right { get; }
        public IInput Up { get; }
        public IInput Forward { get; }
        public IInput Backward { get; }

        public IInput TurnLeft { get; }
        public IInput TurnRight { get; }
        public IInput TurnUp { get; }
        public IInput TurnDown { get; }

        public OpenGLInputHandler(GameWindow glWindow)
        {
            this.glWindow = glWindow;
            glWindow.KeyDown += GlWindow_KeyDown;
            glWindow.KeyUp += GlWindow_KeyUp;

            // key bindings here
            Down = keys[Key.Keypad9] = new KeyboardInput(Key.Keypad9);
            Left = keys[Key.Keypad4] = new KeyboardInput(Key.Keypad4);
            Right = keys[Key.Keypad6] = new KeyboardInput(Key.Keypad6);
            Up = keys[Key.Keypad7] = new KeyboardInput(Key.Keypad7);
            Forward = keys[Key.Keypad8] = new KeyboardInput(Key.Keypad8);
            Backward = keys[Key.Keypad5] = new KeyboardInput(Key.Keypad5);

            TurnLeft = keys[Key.Left] = new KeyboardInput(Key.S);
            TurnRight = keys[Key.Right] = new KeyboardInput(Key.S);
            TurnUp = keys[Key.Up] = new KeyboardInput(Key.S);
            TurnDown = keys[Key.Down] = new KeyboardInput(Key.S);
        }

        private void GlWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (!keys.ContainsKey(e.Key)) return;
            keys[e.Key].RaiseStart();
        }

        private void GlWindow_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (!keys.ContainsKey(e.Key)) return;
            keys[e.Key].RaiseFinish();
        }

        private class KeyboardInput : IInput
        {
            public Key GLKey { get; }

            public bool IsActive { get; private set; } = false;
            public double Weight => IsActive ? 1.0 : 0.0;
            public event EventHandler Finish;
            public event EventHandler Start;

            public KeyboardInput(Key glKey)
            {
                this.GLKey = glKey;
            }

            public void RaiseStart()
            {
                IsActive = true;
                Start?.Invoke(this, EventArgs.Empty);
            }

            public void RaiseFinish()
            {
                IsActive = false;
                Finish?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
