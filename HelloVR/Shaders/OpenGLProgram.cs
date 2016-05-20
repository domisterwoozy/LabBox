using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace HelloVR.Shaders
{
    public abstract class OpenGLProgram
    {
        public int ProgramID { get; }
        public int VertexShaderID { get; }
        public int FragmentShaderID { get; }

        private Dictionary<string, int> uniformIDs = new Dictionary<string, int>();
        private Dictionary<string, int> attribIDs = new Dictionary<string, int>();

        public OpenGLProgram(string vertexShaderFileName, string fragmentShaderFileName)
        {
            ProgramID = GL.CreateProgram();
            VertexShaderID = OpenGLUtil.LoadShader($"Shaders\\{vertexShaderFileName}", ShaderType.VertexShader, ProgramID);
            FragmentShaderID = OpenGLUtil.LoadShader($"Shaders\\{fragmentShaderFileName}", ShaderType.FragmentShader, ProgramID);
            GL.LinkProgram(ProgramID);
            CheckErrors();
        }

        public void CheckErrors()
        {
            string logInfo = GL.GetProgramInfoLog(ProgramID);
            if (!string.IsNullOrEmpty(logInfo))
            {
                throw new ArgumentException("Error creating program from specified shaders: " + logInfo);
            }
            string vShaderInfo = GL.GetShaderInfoLog(VertexShaderID);
            if (!string.IsNullOrEmpty(logInfo))
            {
                throw new ArgumentException("Error with vertex shader: " + vShaderInfo);
            }
            string fShaderInfo = GL.GetShaderInfoLog(FragmentShaderID);
            if (!string.IsNullOrEmpty(logInfo))
            {
                throw new ArgumentException("Error with vertex shader: " + fShaderInfo);
            }
        }

        public void UseProgram()
        {
            GL.UseProgram(ProgramID);
        }

        public int GetUniformID(string name)
        {
            if (!uniformIDs.ContainsKey(name))
            {
                int id = GL.GetUniformLocation(ProgramID, name);
                if (id == -1) throw new ArgumentException(nameof(name));
                uniformIDs[name] = id;
            }            
            return uniformIDs[name];
        }

        public int GetAttribID(string name)
        {
            if (!attribIDs.ContainsKey(name))
            {
                int id = GL.GetAttribLocation(ProgramID, name);
                if (id == -1) throw new ArgumentException(nameof(name));
                attribIDs[name] = id;
            }            
            return attribIDs[name];
        }

        public void EnableAttributes()
        {
            foreach(int attribID in attribIDs.Values)
            {
                GL.EnableVertexAttribArray(attribID);
            }
        }

        public void DisableAttributes()
        {
            foreach (int attribID in attribIDs.Values)
            {
                GL.DisableVertexAttribArray(attribID);
            }
        }

        public abstract void LoadBuffer(int vertexBufferID);
    }
}
