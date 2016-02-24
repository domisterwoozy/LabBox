using BasicVisualization.HUD;
using BasicVisualization.Input;
using BasicVisualization.Universe.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicVisualization
{
    public interface ILabBoxVis : IDisposable
    {
        ICollection<IGraphicalBody> Bodies { get; }
        IInputHandler InputHandler { get; }
        ICamera Camera { get; }
        ICollection<IHUDView> HUDs { get; }

        void RunVis();
    }
}
