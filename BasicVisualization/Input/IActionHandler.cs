using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicVisualization.Input
{  
    /// <summary>
    /// Broadcasts actions. These actions can be triggered by a combination of input events, ViewModel state, and Model state.
    /// </summary>
    public interface IActionHandler
    {
        event EventHandler AddBody;
    }

    public interface IInput
    {
        event EventHandler Start;
        event EventHandler Finish;
        bool IsActive { get; }
        double Weight { get; }
    }

    public interface IInputHandler
    {
        IInput Left { get; }
        IInput Right { get; }
        IInput Up { get; }
        IInput Down { get; }
    }
}
