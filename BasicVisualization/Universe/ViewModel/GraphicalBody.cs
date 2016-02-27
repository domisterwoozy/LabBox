﻿using Math3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.Visualization.Universe.ViewModel
{
    public interface IGraphicalBody
    {
        Quaternion Orientation { get; }
        Vector3 Translation { get; }
        PrimitiveTriangle[] Triangles { get; }
    }

    public class MoveableBody : IGraphicalBody
    {
        public Quaternion Orientation { get; set; } = Quaternion.FromRotMatrix(Matrix3.Identity);

        public Vector3 Translation { get; set; } = Vector3.Zero;

        public PrimitiveTriangle[] Triangles { get; }

        public MoveableBody(IEnumerable<PrimitiveTriangle> tris)
        {
            Triangles = tris.ToArray();
        }
    }
}
