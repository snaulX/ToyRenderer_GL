using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace ToyRendererGL
{
    public class DiffuseLightTask : IRenderTask
    {
        private const PrimitiveType Primitive = PrimitiveType.Triangles;
        private const string VertexShaderPath = "Shaders\\diffuse_lighting.vert";
        private const string FragShaderPath = "Shaders\\diffuse_lighting.frag";

        public GL Gl { get; set; }

        private Pipeline pipeline;

        private readonly string vertCode, fragCode;

        public DiffuseLightTask(GL gl)
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
            Light light = scene.Lights[0];
            pipeline.SetUniform("projection", cam.PerspectiveMatrix);
            pipeline.SetUniform("view", cam.ViewMatrix);
            pipeline.SetUniform("light.position", light.Position);
            pipeline.SetUniform("light.ambient", light.Ambient);
            pipeline.SetUniform("light.diffuse", light.Diffuse);
            pipeline.SetUniform("light.specular", light.Specular);
            foreach (var mesh in scene.Meshes)
            {
                mesh.VertexArray.Bind();
                mesh.Material.DiffuseTexture.Bind(TextureUnit.Texture0);
                mesh.Material.SpecularTexture.Bind(TextureUnit.Texture1);
                pipeline.SetUniform("material.shininess", mesh.Material.Shiness);
                mesh.ExecuteAnimation(deltaTime);
                Matrix4x4 model = mesh.Transform.ViewMatrix;
                pipeline.SetUniform("model", model);
                Gl.DrawArrays(Primitive, 0, (uint)(mesh.Vertices.Length / mesh.Size));
            }
        }
    }
}
