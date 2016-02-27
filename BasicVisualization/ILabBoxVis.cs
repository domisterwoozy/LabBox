using LabBox.Visualization.HUD;
using LabBox.Visualization.Input;
using LabBox.Visualization.Universe.ViewModel;
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

        /// <summary>
        /// Should block the main thread and run the graphical simulation
        /// </summary>
        void RunVis();
        /// <summary>
        /// Ends the simulation. RunVis() will return after this is called.
        /// </summary>
        void EndVis();
    }
}
