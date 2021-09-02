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

        public Func<Transform, double, Transform>[] Animations = Array.Empty<Func<Transform, double, Transform>>();

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
            foreach (var cube in meshes)
            {
                cube.VertexArray.Bind();
                cube.DiffuseTexture.Bind(TextureUnit.Texture0);
                Matrix4x4 model = ExecuteAnimations(cube.Transform, deltaTime).ViewMatrix;
                pipeline.SetUniform("model", model);
                Gl.DrawArrays(Primitive, 0, (uint)(cube.Vertices.Length/cube.Size));
            }
        }

        public Transform ExecuteAnimations(Transform transform, double deltaTime)
        {
            Transform result = transform;
            foreach (var anim in Animations)
            {
                result = anim.Invoke(result, deltaTime);
            }
            return result;
        }
    }
}
