using Math3D;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.Visualization.Universe.ViewModel
{
    public interface IGraphicalBody
    {
        // can be mutable
        Quaternion Orientation { get; }
        Vector3 Translation { get; }

        // immutable
        ImmutableArray<PrimitiveTriangle> Triangles { get; }

        IGraphicalBody NewColor(Color c);
        IGraphicalBody NewShape(ImmutableArray<PrimitiveTriangle> tris);
    }           
}
