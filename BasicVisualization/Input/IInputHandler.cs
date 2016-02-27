using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicVisualization.Input
{   
    public interface IInputHandler
    {
        IInput Left { get; }
        IInput Right { get; }
        IInput Up { get; }
        IInput Down { get; }
        IInput Forward { get; }
        IInput Backward { get; }

        IInput TurnLeft { get; }
        IInput TurnRight { get; }
        IInput TurnUp { get; }
        IInput TurnDown { get; }

        IInput Exit { get; }
        IInput Pause { get; }
    }
}
