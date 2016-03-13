using LabBox.Visualization.Input;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabBox.OpenGLVisualization
{
    public class OpenGLInputHandler : IInputHandler
    {
        private const int MouseMoveMax = 10000;
        private readonly GameWindow glWindow;
        private readonly Dictionary<Key, KeyboardInput> keys = new Dictionary<Key, KeyboardInput>();
        private readonly MouseInput mouseUp = new MouseInput();
        private readonly MouseInput mouseDown = new MouseInput();
        private readonly MouseInput mouseLeft = new MouseInput();
        private readonly MouseInput mouseRight = new MouseInput();

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

        public IInput Exit { get; }
        public IInput Pause { get; }

        public OpenGLInputHandler(GameWindow glWindow)
        {
            this.glWindow = glWindow;
            glWindow.KeyDown += GlWindow_KeyDown;
            glWindow.KeyUp += GlWindow_KeyUp;
            glWindow.Mouse.Move += Mouse_Move;

            // key bindings here
            Down = keys[Key.Keypad9] = new KeyboardInput(Key.Keypad9);
            Left = keys[Key.Keypad4] = new KeyboardInput(Key.Keypad4);
            Right = keys[Key.Keypad6] = new KeyboardInput(Key.Keypad6);
            Up = keys[Key.Keypad7] = new KeyboardInput(Key.Keypad7);
            Forward = keys[Key.Keypad8] = new KeyboardInput(Key.Keypad8);
            Backward = keys[Key.Keypad5] = new KeyboardInput(Key.Keypad5);

            //TurnLeft = keys[Key.Left] = new KeyboardInput(Key.S);
            //TurnRight = keys[Key.Right] = new KeyboardInput(Key.S);
            //TurnUp = keys[Key.Up] = new KeyboardInput(Key.S);
            //TurnDown = keys[Key.Down] = new KeyboardInput(Key.S);
            TurnLeft = mouseLeft;
            TurnRight = mouseRight;
            TurnUp = mouseUp;
            TurnDown = mouseDown;

            Exit = keys[Key.Escape] = new KeyboardInput(Key.Escape);
            Pause = keys[Key.P] = new KeyboardInput(Key.P);
        }

        private void Mouse_Move(object sender, MouseMoveEventArgs e)
        {
            int xDelta = e.X - glWindow.Width / 2;
            int yDelta = e.Y - glWindow.Height / 2;

            if (xDelta < 0) mouseLeft.Fire(-xDelta);
            if (xDelta > 0) mouseRight.Fire(xDelta);
            if (yDelta < 0) mouseUp.Fire(-yDelta);
            if (yDelta > 0) mouseDown.Fire(yDelta);

            Cursor.Position = new Point(glWindow.Width / 2, glWindow.Height / 2); // keep mouse centered
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

        private class MouseInput : IInput
        {
            public bool IsActive { get; private set; }
            public double Weight { get; private set; }

            public event EventHandler Finish;
            public event EventHandler Start;

            public void Fire(float weight)
            {
                IsActive = true;
                Weight = weight;
                Start?.Invoke(this, EventArgs.Empty);
                IsActive = false;
                Weight = 0;
                Finish?.Invoke(this, EventArgs.Empty);            
            }
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
                GLKey = glKey;
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
