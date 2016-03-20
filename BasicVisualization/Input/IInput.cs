using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.Visualization.Input
{
    public interface IInput
    {
        InputState State { get; }
        InputType Input { get; }
        double Weight { get; }
    }

    public enum InputState { Start, Finish, Maintained }

    public enum InputType
    {
        // Selection
        PrimarySelect, SecondarySelect,

        // Movement
        Forward, Backward, Left, Right,
        Up, Down,
        TurnLeft, TurnRight, TurnUp, TurnDown,

        // State
        Exit, Pause            
    }
}
