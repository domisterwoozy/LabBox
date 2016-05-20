using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;
using Valve.VR;
using OpenTK;
using System;
using HelloVR.Shaders;
using System.Threading;
using System.Collections.Generic;

namespace HelloVR
{
    public class GLRenderModels : IGLDrawable
    {
        private readonly RenderModelProgram program = new RenderModelProgram();        
        private readonly Dictionary<uint, GLRenderModel> models = new Dictionary<uint, GLRenderModel>();

        private readonly CVRSystem hmd;
        private TrackedDevicePose_t[] devicePoses = new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount];

        public GLRenderModels(CVRSystem hmd)
        {
            this.hmd = hmd;

            ETrackedPropertyError err = ETrackedPropertyError.TrackedProp_Success;
            for (uint trackedDeviceIndex = OpenVR.k_unTrackedDeviceIndex_Hmd + 1; trackedDeviceIndex < OpenVR.k_unMaxTrackedDeviceCount; trackedDeviceIndex++)
            {
                if (!hmd.IsTrackedDeviceConnected(trackedDeviceIndex)) continue;
                string renderModelName = hmd.GetTrackedDeviceStr(trackedDeviceIndex, ETrackedDeviceProperty.Prop_RenderModelName_String, ref err);
                if (err != ETrackedPropertyError.TrackedProp_Success) throw new InvalidOperationException("Failed to retrieve prop name for connected tracked device");

                // load render model async
                IntPtr renderModelPtr = IntPtr.Zero;
                EVRRenderModelError rmErr = EVRRenderModelError.Loading;
                while (rmErr == EVRRenderModelError.Loading) // is this seriously how im supposed to do this? no callback?
                {
                    rmErr = OpenVR.RenderModels.LoadRenderModel_Async(renderModelName, ref renderModelPtr);
                    Thread.Sleep(100); // async seems to be breaking opengl?
                    //await Task.Delay(100); // try again every 0.1 seconds
                }
                if (rmErr != EVRRenderModelError.None || renderModelPtr == IntPtr.Zero) throw new InvalidOperationException($"Failed to load Render Model: {renderModelName}");
                var renderModel = (RenderModel_t)Marshal.PtrToStructure(renderModelPtr, typeof(RenderModel_t));

                // load the render texture async
                IntPtr texturePtr = IntPtr.Zero;
                rmErr = EVRRenderModelError.Loading;
                while (rmErr == EVRRenderModelError.Loading) // is this seriously how im supposed to do this? no callback?
                {
                    rmErr = OpenVR.RenderModels.LoadTexture_Async(renderModel.diffuseTextureId, ref texturePtr);
                    Thread.Sleep(100); // async seems to be breaking opengl?
                    //await Task.Delay(100); // try again every 0.1 seconds
                }
                if (rmErr != EVRRenderModelError.None || texturePtr == IntPtr.Zero) throw new InvalidOperationException($"Failed to load texture for Render Model: {renderModelName}");
                var diffuseTexture = (RenderModel_TextureMap_t)Marshal.PtrToStructure(texturePtr, typeof(RenderModel_TextureMap_t));

                models[trackedDeviceIndex] = GLRenderModel.Create(renderModelName, renderModel, diffuseTexture);

                OpenVR.RenderModels.FreeRenderModel(renderModelPtr);
                OpenVR.RenderModels.FreeTexture(texturePtr);
            }
        }

        public void UpdateTracking(TrackedDevicePose_t[] devicePoses)
        {
            this.devicePoses = devicePoses;
        }

        public void Update()
        {            
        }

        public void Draw(Matrix4 viewProjmatrix)
        {
            program.UseProgram();
            foreach (uint deviceIndex in models.Keys)
            {
                var pose = devicePoses[deviceIndex];
                var model = models[deviceIndex];
                if (!pose.bPoseIsValid) throw new InvalidOperationException("Pose is invalid");

                Matrix4 devToTracking = pose.mDeviceToAbsoluteTracking.ToGLMatrix4();
                //Matrix4 mvp = viewProjmatrix * devToTracking;
                Matrix4 mvp = devToTracking * viewProjmatrix;
                program.SetMatrix(mvp);
                model.Draw();
                GL.BindVertexArray(0);
            }
            GL.UseProgram(0);
        }

        

        private class GLRenderModel
        {
            public int VertBufferID { get; }
            public int IndexBufferID { get; }
            public int VertArrID { get; }
            public int TextureID { get; }
            public int VertCount { get; }
            public string ModelName { get; }            

            public GLRenderModel(int vertBuffer, int indexBuffer, int vertArr, int texture, int vertCount, string name)
            {
                VertBufferID = vertBuffer;
                IndexBufferID = indexBuffer;
                VertArrID = vertArr;
                TextureID = texture;
                VertCount = vertCount;
                ModelName = name;
            }

            public static GLRenderModel Create(string name, RenderModel_t renderModel, RenderModel_TextureMap_t diffuseTexture)
            {
                // create and bind a VAO to hold the state for this model
                int vao = OpenGLUtil.CreateVertexArrayObject();
                GL.BindVertexArray(vao);

                // populate the vertex buffer
                int vbo = OpenGLUtil.CreateBufferObject();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, Marshal.SizeOf<RenderModel_Vertex_t>() * (int)renderModel.unVertexCount, renderModel.rVertexData, BufferUsageHint.StaticDraw);

                // identity the components in teh vertex buffer
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf<RenderModel_Vertex_t>(), Marshal.OffsetOf<RenderModel_Vertex_t>("vPosition")); // this might have to be size of 4?
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf<RenderModel_Vertex_t>(), Marshal.OffsetOf<RenderModel_Vertex_t>("vNormal"));
                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Marshal.SizeOf<RenderModel_Vertex_t>(), Marshal.OffsetOf<RenderModel_Vertex_t>("rfTextureCoord0"));

                // create and populate the index buffer
                int indexBuffer = OpenGLUtil.CreateBufferObject();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer);
                GL.BufferData(BufferTarget.ElementArrayBuffer, Marshal.SizeOf<ushort>() * (int)renderModel.unTriangleCount * 3, renderModel.rIndexData, BufferUsageHint.StaticDraw);

                GL.BindVertexArray(0);

                // create and populate the texture
                int texture = OpenGLUtil.CreateTexture();
                GL.BindTexture(TextureTarget.Texture2D, texture);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, diffuseTexture.unWidth, diffuseTexture.unHeight,
                    0, PixelFormat.Rgba, PixelType.UnsignedByte, diffuseTexture.rubTextureMapData);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

                GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, new[] { (int)TextureParameterName.ClampToEdge });
                GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, new[] { (int)TextureParameterName.ClampToEdge });
                GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, new[] { (int)TextureMagFilter.Linear });
                GL.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, new[] { (int)TextureMinFilter.LinearMipmapLinear });

                // *****missing stuff about anisotropy ***** // not sure if this is right
                float largest = GL.GetFloat((GetPName)0x84FF);
                GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName)0x84FE, largest);

                GL.BindTexture(TextureTarget.Texture2D, 0);

                return new GLRenderModel(vbo, indexBuffer, vao, texture, (int)renderModel.unTriangleCount * 3, name);
            }

            public void Draw()
            {
                GL.BindVertexArray(VertArrID);
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, TextureID);
                GL.DrawElements(BeginMode.Triangles, VertCount, DrawElementsType.UnsignedShort, 0);
                GL.BindVertexArray(0);
            }
        }
    }

    
}
