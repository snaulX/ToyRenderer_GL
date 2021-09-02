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
        private const string VertexShaderPath = "shader.vert";
        private const string FragShaderPath = "shader.frag";

        public GL Gl { get; set; }

        public event Func<Transform, double, Transform> Animations;

        private Pipeline pipeline;
        private TexturedCube[] cubes;

        private readonly string vertCode, fragCode;

        public RenderTextured(GL gl, params TexturedCube[] cubes)
        {
            Gl = gl;
            this.cubes = cubes;

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
            foreach (var cube in cubes)
            {
                cube.VertexArray.Bind();
                cube.DiffuseTexture.Bind(TextureUnit.Texture0);
                Matrix4x4 model = (Animations?.Invoke(cube.Transform, deltaTime) ?? cube.Transform).ViewMatrix;
                pipeline.SetUniform("model", model);
            }
        }
    }
}
