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
        private const string VertexShaderPath = "Shaders\\default_shader.vert";
        private const string FragShaderPath = "Shaders\\default_shader.frag";

        public GL Gl { get; set; }

        private Pipeline pipeline;

        private readonly string vertCode, fragCode;

        public RenderTextured(GL gl)
        {
            Gl = gl;

            vertCode = File.ReadAllText(VertexShaderPath);
            fragCode = File.ReadAllText(FragShaderPath);
        }

        public void Init()
        {
            pipeline = new Pipeline(Gl, vertCode, fragCode);
        }

        public void Render(Scene scene, double deltaTime)
        {
            pipeline.Use();
            Camera cam = scene.Camera;
            pipeline.SetUniform("projection", cam.PerspectiveMatrix);
            pipeline.SetUniform("view", cam.ViewMatrix);
            foreach (var mesh in scene.Meshes)
            {
                mesh.VertexArray.Bind();
                mesh.Material.DiffuseTexture.Bind(TextureUnit.Texture0);
                mesh.ExecuteAnimation(deltaTime);
                Matrix4x4 model = mesh.Transform.ViewMatrix;
                pipeline.SetUniform("model", model);
                Gl.DrawArrays(Primitive, 0, mesh.Count);
            }
        }
    }
}
