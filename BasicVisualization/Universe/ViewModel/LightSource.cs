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

    public interface ILightSource
    {
        LightType LightType { get; }
        Vector3 Pos { get; }
        Vector3 LightDir { get; }
        Color LightColor { get; }

        float DiffusePower { get; }
        float AttenuationCoef { get; }
        float AmbientIntensity { get; }

        float ConeAngle { get; }
        bool CastsDynamicShadows { get; }

    }

    public class LightSource : ILightSource
    {
        public LightType LightType { get; set; } = LightType.Spotlight;
        public Vector3 Pos { get; set; }
        public Color LightColor { get; set; } = Color.White;

        private float diffusePower = 10.0f;
        public float DiffusePower
        {
            get { return diffusePower; }
            set
            {
                if (value < 0) throw new ArgumentException(nameof(value) + "must be larger than 0");
                diffusePower = value;
            }
        }

        private float attenuationCoef = 0.2f;
        public float AttenuationCoef
        {
            get { return attenuationCoef; }
            set
            {
                if (value < 0 || value > 1) throw new ArgumentException(nameof(value) + "must be between 0 and 1");
                attenuationCoef = value;
            }
        }

        private float ambientIntensity = 0.1f;
        public float AmbientIntensity
        {
            get { return ambientIntensity; }
            set
            {
                if (value < 0 || value > 1) throw new ArgumentException(nameof(value) + "must be between 0 and 1");
                ambientIntensity = value;
            }
        } 

        /// <summary>
        /// Not relevant for directional lights. The 'half angle'.
        /// </summary>
        public float ConeAngle { get; set; } = (float)Math.PI;

        /// <summary>
        /// Not rele
        /// </summary>
        public Vector3 LightDir { get; set; } = -Vector3.K;

        public bool CastsDynamicShadows { get; set; } = false;

        private LightSource(Vector3 pos)
        {
            Pos = pos;
        }

        public static LightSource PointLight(Vector3 pos, float power) => new LightSource(pos) { DiffusePower = power };

        public static LightSource SpotLight(Vector3 pos, Vector3 dir, double coneAngle) => SpotLight(pos, dir, (float)coneAngle);
        public static LightSource SpotLight(Vector3 pos, Vector3 dir, float coneAngle) => new LightSource(pos) { LightDir = dir, ConeAngle = coneAngle, AmbientIntensity = 0.0f };

        public static LightSource Directional(Vector3 dir) => 
            new LightSource(Vector3.Zero) { LightDir = dir, DiffusePower = 1.0f, LightType = LightType.Directional };
               
    }
}
