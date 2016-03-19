using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.Visualization.Universe
{
    public class FixedTimePhysicsRunner : IPhysicsRunner
    {
        private double lastFrameLength = 1.0;

        public IUniverse Universe { get; }

        public bool IsPaused => FrameLength == 0.0f;
        public double FrameLength { get; set; } = 5 * Math.Pow(10, -3);

        public FixedTimePhysicsRunner(IUniverse uni, double timeStep = 0)
        {
            Universe = uni;
            if (timeStep != 0) FrameLength = timeStep;
        }

        public void StartPhysics()
        {
            Task.Run(() => PhysicsLoop());
        }

        public void TogglePause()
        {
            if (IsPaused) ResumePhysics();
            else PausePhysics();
        }

        public void ResumePhysics()
        {
            FrameLength = lastFrameLength;
        }

        public void PausePhysics()
        {
            lastFrameLength = FrameLength;
            FrameLength = 0.0;
        }

        private void PhysicsLoop()
        {
            while (true)
            {
                Universe.Update(FrameLength);
            }
        }
    }
}
