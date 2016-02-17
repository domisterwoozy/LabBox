﻿using Math3D.Geometry;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Measurement
{
    public class BodyCountMeasurement : IInstantMeasurement<int>
    {
        public string Name => "Number Bodies [#]";
        public IVolume Volume { get; }

        public BodyCountMeasurement(IVolume vol)
        {
            Volume = vol;
        }

        public int TakeMeasurement(IUniverse uni) => uni.Bodies.Count(b => Volume.VolumeFunc(b.Dynamics.Transform.Pos));
    } 
}
