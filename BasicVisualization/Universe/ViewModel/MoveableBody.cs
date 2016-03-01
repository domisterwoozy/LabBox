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
    public class MoveableBody : IGraphicalBody
    {
        public Quaternion Orientation { get; set; } = Quaternion.FromRotMatrix(Matrix3.Identity);

        public Vector3 Translation { get; set; } = Vector3.Zero;

        public ImmutableArray<PrimitiveTriangle> Triangles { get; }

        public MoveableBody(IEnumerable<PrimitiveTriangle> tris)
        {
            Triangles = tris.ToImmutableArray();
        }

        public IGraphicalBody NewColor(Color c) => new MoveableBody(Triangles.NewColor(c).ToImmutableArray()) { Translation = Translation, Orientation = Orientation };
        public IGraphicalBody NewShape(ImmutableArray<PrimitiveTriangle> tris) => new MoveableBody(tris) { Translation = Translation, Orientation = Orientation };
    }
}
