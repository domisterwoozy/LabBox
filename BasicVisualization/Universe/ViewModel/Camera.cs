using Math3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicVisualization.Universe.ViewModel
{
    public class Camera
    {
        public Vector3 Pos { get; set; } = new Vector3(10, 10, 10);
        public Vector3 UpDir { get; set; } = Vector3.K;
        public Vector3 LookAtPos { get; set; } = Vector3.Zero;

        /// <summary>
        /// Vertical field of view in degrees.
        /// </summary>
        public float VertFOV { get; set; } = 65.0f;

        public float MaxRange { get; set; } = 100.0f;
        public float MinRange { get; set; } = 0.1f;
        public float AspectRatio { get; set; } = 16.0f / 9.0f;
    }
}
