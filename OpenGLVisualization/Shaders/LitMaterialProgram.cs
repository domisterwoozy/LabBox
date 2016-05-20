using LabBox.OpenGLVisualization.ViewModel;
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
        private static readonly Matrix4 BiasMatrix = new Matrix4(
                                                        0.5f, 0.0f, 0.0f, 0.0f,
                                                        0.0f, 0.5f, 0.0f, 0.0f,
                                                        0.0f, 0.0f, 0.5f, 0.0f,
                                                        0.5f, 0.5f, 0.5f, 1.0f);

        public LitMaterialProgram() : base("vs.glsl", "fs.glsl") { }

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

        public void SetMVP(Matrix4 vp, Matrix4 m)
        {
            GL.UniformMatrix4(GetUniformID("model"), false, ref m);
            Matrix4 mvp = m * vp;
            GL.UniformMatrix4(GetUniformID("mvp"), false, ref mvp);
        }

        public void SetMaterialProperties(Vector3 specularColor, float shininess)
        {
            GL.Uniform1(GetUniformID("materialShininess"), shininess);
            GL.Uniform3(GetUniformID("materialSpecularColor"), ref specularColor);
        }

        public void AddLights(params OpenGLLightSource[] lights)
        {
            if (lights.Length > 10) throw new ArgumentException("Can only add 10 lights.");
            GL.Uniform1(GetUniformID("numLights"), lights.Length);

            GL.Uniform1(GetUniformID("ambientIntensities"), lights.Length, lights.Select(l => l.AmbientIntensity).ToArray());
            GL.Uniform1(GetUniformID("lightAttenuations"), lights.Length, lights.Select(l => l.AttenuationCoef).ToArray());
            GL.Uniform1(GetUniformID("lightPowers"), lights.Length, lights.Select(l => l.DiffusePower).ToArray());
            GL.Uniform1(GetUniformID("coneAngles"), lights.Length, lights.Select(l => l.ConeAngle).ToArray());

            // this is very gross and not very well documented but in order to pass an array of vectors to a shader
            // you just flatten it into one long array and use the below functions
            GL.Uniform4(GetUniformID("lightPositions"), lights.Length, lights.SelectMany(l => new[] { l.Pos.X, l.Pos.Y, l.Pos.Z, l.Pos.W }).ToArray());
            GL.Uniform3(GetUniformID("lightColors"), lights.Length, lights.SelectMany(l => new[] { l.Color.X, l.Color.Y, l.Color.Z }).ToArray());            
            GL.Uniform3(GetUniformID("coneDirections"), lights.Length, lights.SelectMany(l => new[] { l.ConeDir.X, l.ConeDir.Y, l.ConeDir.Z }).ToArray());            
            AddShadows(lights.Select(l => l.ShadowMapID).ToArray());
        }

        public void SetShadowCasterMVPs(Matrix4[] mvp)
        {
            if (mvp.Length > 10) throw new ArgumentException("Can only add 10 shadowMaps.");
            // bias the mvp matrix so that it works on the texture
            Matrix4[] mvpWithBias = mvp.Select(m => m * BiasMatrix).ToArray();
            GL.UniformMatrix4(GetUniformID("depthBiasMVPs"), mvpWithBias.Length, false, mvpWithBias.SelectMany(m => m.Flatten()).ToArray());
        }

        private void AddShadows(params int[] shadowMapTextureIDs)
        {
            if (shadowMapTextureIDs.Length > 10) throw new ArgumentException("Can only add 10 shadowMaps.");
            GL.Uniform1(GetUniformID("castsShadows"), shadowMapTextureIDs.Length, shadowMapTextureIDs.Select(id => id == -1 ? 0 : 1).ToArray());
            // lol this codes fucking insane
            // the texture units are reffered to through the enum when activating/binding
            // then when sending to the shader you refer to the numeral value of the texture unit
            // i believe THIS IS NOT WORKING IN VR
            int currentTextureUnit = (int)TextureUnit.Texture0;
            foreach (int i in Enumerable.Range(0, shadowMapTextureIDs.Length))
            {
                // bind the texture to a texture unit
                GL.ActiveTexture((TextureUnit)currentTextureUnit);
                GL.BindTexture(TextureTarget.Texture2D, shadowMapTextureIDs[i]);
                currentTextureUnit++;
            }

            GL.Uniform1(GetUniformID("shadowMaps"), shadowMapTextureIDs.Length, Enumerable.Range(0, shadowMapTextureIDs.Length).ToArray()); 
        }
    }
}
