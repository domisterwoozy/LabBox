﻿using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.Visualization.Universe
{
    public class RealTimePhysicsRunner : IPhysicsRunner
    {        
        public IUniverse Universe { get; }
        
        public bool IsPaused => TimeMultiplier == 0.0f;
        public double TimeMultiplier { get; set; } = 1.0;
        public double MaxFrameLength { get; set; } = 3 * Math.Pow(10, -3);

        public RealTimePhysicsRunner(IUniverse uni)
        {
            Universe = uni;
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
            TimeMultiplier = 1.0f;
        }

        public void PausePhysics()
        {
            TimeMultiplier = 0.0f;
        }

        private void PhysicsLoop()
        {
            var sw = new Stopwatch();
            sw.Start();
            TimeSpan lastTime = sw.Elapsed;
            while (true)
            {
                TimeSpan delta = sw.Elapsed - lastTime;
                lastTime = sw.Elapsed;
                Universe.Update(Math.Min(delta.TotalSeconds * TimeMultiplier, MaxFrameLength));
            }
        }
    }


}