using Math3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.Visualization.Universe.ViewModel
{
    public enum LightType { Spotlight, Directional }
    public class LightSource
    {
        public LightType LightType { get; set; } = LightType.Spotlight;
        public Vector3 Pos { get; set; }
        public Color LightColor { get; set; } = Color.White;
        public float DiffusePower { get; set; } = 1.0f;
        public float AttenuationCoef { get; set; } = 0.2f;
        public float AmbientIntensity { get; set; } = 0.1f;

        public float ConeAngle { get; set; } = (float)Math.PI;
        public Vector3 ConeDir { get; set; }

        public LightSource(Vector3 pos)
        {
            Pos = pos;
        }
    }
}
