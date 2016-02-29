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
        IEnumerable<IGraphicalBody> Bodies { get; }
        IEnumerable<ILightSource> LightSources { get; }
        IEnumerable<IHUDView> HUDs { get; }

        IInputHandler InputHandler { get; }
        ICamera Camera { get; }

        void AddBody(IGraphicalBody b);
        bool RemoveBody(IGraphicalBody b);

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
