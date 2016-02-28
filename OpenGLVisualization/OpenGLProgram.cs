using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBox.OpenGLVisualization
{
    public class OpenGLProgram
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
            string logInfo = GL.GetProgramInfoLog(ProgramID);
            if (!string.IsNullOrEmpty(logInfo))
            {
                throw new ArgumentException("Error creating program from specified shaders: " + logInfo);
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

        public void LoadBuffer(int vertexBufferID)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferID);
            GL.VertexAttribPointer(GetAttribID("vertexPos"), 3, VertexAttribPointerType.Float, false, OpenGLVertex.Stride, OpenGLVertex.PositionOffset);
            GL.VertexAttribPointer(GetAttribID("vertexColor"), 4, VertexAttribPointerType.Float, false, OpenGLVertex.Stride, OpenGLVertex.ColorOffset);
            GL.VertexAttribPointer(GetAttribID("vertexNormal"), 3, VertexAttribPointerType.Float, false, OpenGLVertex.Stride, OpenGLVertex.NormalOffset);
        }
    }
}
