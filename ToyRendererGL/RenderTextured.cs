using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ToyRendererGL
{
    public class RenderTextured : IRenderTask
    {
        private const PrimitiveType Primitive = PrimitiveType.Triangles;
        private const string VertexShaderPath = "shader.vert";
        private const string FragShaderPath = "shader.frag";

        public GL Gl { get; set; }

        private Pipeline pipeline;
        private TexturedMesh<float, uint>[] meshes;

        private readonly string vertCode, fragCode;

        public RenderTextured(GL gl, params TexturedMesh<float, uint>[] meshes)
        {
            Gl = gl;
            this.meshes = meshes;

            vertCode = File.ReadAllText(VertexShaderPath);
            fragCode = File.ReadAllText(FragShaderPath);
        }

        public void Init()
        {
            pipeline = new Pipeline(Gl, vertCode, fragCode);
        }

        public void Render(Camera cam, double deltaTime)
        {
            pipeline.Use();
            pipeline.SetUniform("projection", cam.PerspectiveMatrix);
            pipeline.SetUniform("view", cam.ViewMatrix);
            foreach (var mesh in meshes)
            {
                mesh.VertexArray.Bind();
                mesh.DiffuseTexture.Bind(TextureUnit.Texture0);
                mesh.ExecuteAnimation(deltaTime);
                Matrix4x4 model = mesh.Transform.ViewMatrix;
                pipeline.SetUniform("model", model);
                Gl.DrawArrays(Primitive, 0, (uint)(mesh.Vertices.Length/mesh.Size));
            }
        }
    }
}
