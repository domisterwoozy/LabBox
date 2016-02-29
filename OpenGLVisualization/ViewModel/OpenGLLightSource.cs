using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.OpenGLVisualization.ViewModel
{
    public class OpenGLLightSource
    {
        public float AmbientIntensity { get; set; }
        public float AttenuationCoef { get; set; }
        public float DiffusePower { get; set; }
        public Vector4 Pos { get; set; }
        public Vector3 Color { get; set; }
        public float ConeAngle { get; set; }
        public Vector3 ConeDir { get; set; }
        public int ShadowMapID { get; set; }
        public int FrameBufferID { get; set; }

        public bool IsDirectional => Pos.W == 0.0f;
    }
}
