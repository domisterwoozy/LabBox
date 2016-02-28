using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.OpenGLVisualization.Shaders
{
    public class LitMaterialProgram : OpenGLProgram
    {
        public LitMaterialProgram() : base("plvs.glsl", "plfs.glsl") { }

        public void SetCameraPosition(Vector3 pos)
        {
            GL.Uniform3(GetUniformID("cameraPos"), ref pos);
        }

        public void SetMVP(Matrix4 m, Matrix4 v, Matrix4 p)
        {
            GL.UniformMatrix4(GetUniformID("model"), false, ref m);
            Matrix4 mvp = m * v * p;
            GL.UniformMatrix4(GetUniformID("mvp"), false, ref mvp);
        }

        public void SetMaterialProperties(Vector3 specularColor, float shininess)
        {
            GL.Uniform1(GetUniformID("materialShininess"), shininess);
            GL.Uniform3(GetUniformID("materialSpecularColor"), ref specularColor);
        }

        public void AddLights(params OpenGLLightSource[] lights)
        {
            GL.Uniform1(GetUniformID("numLights"), lights.Length);

            GL.Uniform1(GetUniformID("ambientIntensities"), lights.Length, lights.Select(l => l.AmbientIntensity).ToArray());
            GL.Uniform1(GetUniformID("lightAttenuations"), lights.Length, lights.Select(l => l.AttenuationCoef).ToArray());
            GL.Uniform1(GetUniformID("lightPowers"), lights.Length, lights.Select(l => l.DiffusePower).ToArray());
            GL.Uniform1(GetUniformID("coneAngles"), lights.Length, lights.Select(l => l.ConeAngle).ToArray());

            GL.Uniform4(GetUniformID("lightPositions"), lights.Length, lights.SelectMany(l => new[] { l.Pos.X, l.Pos.Y, l.Pos.Z, l.Pos.W }).ToArray());
            GL.Uniform3(GetUniformID("lightColors"), lights.Length, lights.SelectMany(l => new[] { l.Color.X, l.Color.Y, l.Color.Z }).ToArray());            
            GL.Uniform3(GetUniformID("coneDirections"), lights.Length, lights.SelectMany(l => new[] { l.ConeDir.X, l.ConeDir.Y, l.ConeDir.Z }).ToArray());            
        }
    }
}
