using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;

namespace ToyRendererGL
{
    public class ColorShadingTask : IRenderTask
    {
        private const PrimitiveType Primitive = PrimitiveType.Triangles;
        private const string VertexShaderPath = "Shaders\\color_shading.vert";
        private const string FragShaderPath = "Shaders\\color_shading.frag";

        public GL Gl { get; set; }
        public Vector4 Color { get; set; } = new Vector4(1, 1, 0, 1);

        private Pipeline pipeline;

        private readonly string vertCode, fragCode;

        public ColorShadingTask(GL gl)
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
            //pipeline.SetUniform("color", Color);
            foreach (var mesh in scene.Meshes)
            {
                mesh.VertexArray.Bind();
                //mesh.Material.DiffuseTexture.Bind(TextureUnit.Texture0); // diffuse texture
                mesh.Material.SpecularTexture.Bind(TextureUnit.Texture0); // color mapping texture
                mesh.ExecuteAnimation(deltaTime);
                Matrix4x4 model = mesh.Transform.ViewMatrix;
                pipeline.SetUniform("model", model);
                Gl.DrawArrays(Primitive, 0, mesh.Count);
            }
        }
    }
}
