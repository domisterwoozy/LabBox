using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.VR;

namespace HelloVR
{
    public struct EyeTextures
    {
        public Texture_t LeftEye { get; }
        public Texture_t RightEye { get; }

        public EyeTextures(Texture_t left, Texture_t right)
        {
            LeftEye = left;
            RightEye = right;
        }
    }

    public interface IVRGraphics
    {
        EyeTextures RenderToTextures(Matrix4 leftViewProj, Matrix4 rightViewProj, Vector3 leftEyePos, Vector3 rightEyePos, params IVRDrawable[] drawables);
    }


}
