using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.OpenGLVisualization.ViewModel
{
    /// <summary>
    /// This is gross and mutable but it is required for compatibility with OpenGL buffer objects.
    /// </summary>
    public struct OpenGLVertex
    {
        public Vector3 Position;
        public Vector4 Color;
        public Vector3 Normal;

        public static readonly int PositionOffset = 0;
        public static readonly int ColorOffset = Vector3.SizeInBytes;
        public static readonly int NormalOffset = ColorOffset + Vector4.SizeInBytes;
        public static readonly int Stride = NormalOffset + Vector3.SizeInBytes;
    }
}
