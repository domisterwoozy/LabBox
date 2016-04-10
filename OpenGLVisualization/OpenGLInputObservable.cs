using LabBox.Visualization.Input;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabBox.OpenGLVisualization
{
    public class OpenGLInputObservable : IInputObservable
    { 
        private readonly GameWindow gameWindow;

        public IObservable<IInput> InputEvents { get; }

        public OpenGLInputObservable(GameWindow window)
        {
            gameWindow = window;

            IObservable<IInput> downEvents =
                from evt in Observable.FromEventPattern<KeyboardKeyEventArgs>(window, "KeyDown")
                let keyInput = GLKeyInput.FromArgs(evt.EventArgs, false)
                where keyInput != null
                select keyInput;

            IObservable<IInput> upEvents =
                from evt in Observable.FromEventPattern<KeyboardKeyEventArgs>(window, "KeyUp")
                let keyInput = GLKeyInput.FromArgs(evt.EventArgs, true)
                where keyInput != null
                select keyInput;

            IObservable<IInput> mouseMoveEvents =
                from evt in Observable.FromEventPattern<MouseMoveEventArgs>(window, "MouseMove")
                from mouseInput in GLMouseInput.FromArgs(evt.EventArgs, gameWindow.Width, gameWindow.Height)
                select mouseInput;

            IObservable<IInput> mouseDownEvents =
                from evt in Observable.FromEventPattern<MouseButtonEventArgs>(window, "MouseDown")
                select GLMouseClickInput.FromArgs(evt.EventArgs, false);

            IObservable<IInput> mouseUpEvents =
                from evt in Observable.FromEventPattern<MouseButtonEventArgs>(window, "MouseUp")
                select GLMouseClickInput.FromArgs(evt.EventArgs, true);

            InputEvents = downEvents.Merge(upEvents).Merge(mouseMoveEvents).Merge(mouseDownEvents).Merge(mouseUpEvents);

            window.MouseMove += (sender, e) => Cursor.Position = new Point(window.Width / 2, window.Height / 2); // keep mouse centered (bc fps)
        }

        private class GLMouseClickInput : IInput
        {
            public InputType Input { get; }
            public InputState State { get; }
            public double Weight { get; } = 1.0;

            private GLMouseClickInput(InputType type, InputState state)
            {
                Input = type;
                State = state;
            }

            public static GLMouseClickInput FromArgs(MouseButtonEventArgs args, bool up)
            {
                return new GLMouseClickInput(args.Button == MouseButton.Left ? InputType.PrimarySelect : InputType.SecondarySelect, up ? InputState.Finish : InputState.Start);
            }
        }

        private class GLMouseInput : IInput
        {
            public InputType Input { get; }
            public InputState State { get; }
            public double Weight { get; }

            private GLMouseInput(InputType type, InputState state, double weight)
            {
                Input = type;
                State = state;
                Weight = weight;
            }

            public static IEnumerable<GLMouseInput> FromArgs(MouseMoveEventArgs args, int windowWidth, int windowHeight)
            {
                int xDelta = args.X - windowWidth / 2;
                int yDelta = args.Y - windowHeight / 2;

                if (xDelta < 0) yield return new GLMouseInput(InputType.TurnLeft, InputState.Maintained, -xDelta);
                if (xDelta > 0) yield return new GLMouseInput(InputType.TurnRight, InputState.Maintained, xDelta);
                if (yDelta < 0) yield return new GLMouseInput(InputType.TurnUp, InputState.Maintained, -yDelta);
                if (yDelta > 0) yield return new GLMouseInput(InputType.TurnDown, InputState.Maintained, yDelta);
            }
        }

        private class GLKeyInput : IInput
        {
            public InputType Input { get; }

            public InputState State { get; }
            public double Weight { get; } = 1.0;

            private GLKeyInput(InputType type, InputState state)
            {
                Input = type;
                State = state;
            }

            public static GLKeyInput FromArgs(KeyboardKeyEventArgs args, bool up)
            {
                var type = FromGLKey(args.Key);
                if (!type.HasValue) return null;
                return new GLKeyInput(type.Value, up ? InputState.Finish : (args.IsRepeat ? InputState.Maintained : InputState.Start));
            }

            private static InputType? FromGLKey(Key glKey)
            {
                // key bindings are currently here, going to abstract this away
                switch (glKey)
                {
                    case Key.Keypad9:
                        return InputType.Down;
                    case Key.Keypad4:
                        return InputType.Left;
                    case Key.Keypad6:
                        return InputType.Right;
                    case Key.Keypad7:
                        return InputType.Up;
                    case Key.Keypad8:
                        return InputType.Forward;
                    case Key.Keypad5:
                        return InputType.Backward;
                    case Key.Escape:
                        return InputType.Exit;
                    case Key.P:
                        return InputType.Pause;
                    case Key.ControlLeft:
                    case Key.ControlRight:
                        return InputType.MultiSelect;
                    default:
                        return null;
                }
            }
        }      
    }
}
