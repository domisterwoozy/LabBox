﻿using Math3D;
using Math3D.Geometry;
using Physics3D.Dynamics;
using Physics3D.ElectroMagnetism;
using Physics3D.Materials;
using Physics3D.Universes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics3D.Bodies
{
    public interface IBody
    {
        event EventHandler<FrameLengthArgs> FrameFinished;

        IDynamicBody Dynamics { get; }

        IEdgeIntersector CollisionShape { get; }
        IOverlapable BoundVolume { get; }
        IVolume Shape { get; }

        IMaterial Material { get; }
        IElectroMag EMProps { get; }
    }

    public static class BodyExtensions
    {
    }
}
