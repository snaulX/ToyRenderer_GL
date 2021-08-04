using System;
using System.Numerics;
using System.IO;
using Silk.NET.OpenGL;
using Silk.NET.Input;
using Silk.NET.Windowing;
using System.Drawing;
using Silk.NET.Maths;

namespace ToyRendererGL
{
    static class Program
    {
        private const string VertexShaderPath = "shader.vert";
        private const string FragShaderPath = "shader.frag";
        private const string TexturePath = "snaulx.jpg";

        private static IWindow window;
        private static GL Gl;

        private static Buffer<float> VertexBuffer;
        private static Buffer<uint> IndexBuffer;
        private static VertexArray<float, uint> VertexArray;
        private static Pipeline Pipeline;
        private static Texture Texture;

        private readonly static Transform[] Transforms = new Transform[4];

        private static readonly float[] Vertices =
        {
            //X    Y      Z     U   V
             0.5f,  0.5f, 0.0f, 1f, 1f,
             0.5f, -0.5f, 0.0f, 1f, 0f,
            -0.5f, -0.5f, 0.0f, 0f, 0f,
            -0.5f,  0.5f, 0.5f, 0f, 1f
        };

        private static readonly uint[] Indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        static void Main(string[] args)
        {
            WindowOptions wo = new WindowOptions()
            {
                Size = new Vector2D<int>(800, 600),
                Title = "ToyRenderer by snaulX"
            };
            window = Window.Create(wo);

            window.Load += OnLoad;
            window.Render += OnRender;
            window.Closing += OnClosing;

            window.Run();
        }

        private static void OnClosing()
        {
            VertexBuffer.Dispose();
            IndexBuffer.Dispose();
            VertexArray.Dispose();
            Pipeline.Dispose();
            Texture.Dispose();
            Gl.Dispose();
        }

        private static unsafe void OnRender(double deltaTime)
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit);

            VertexArray.Bind();
            Texture.Bind();
            Pipeline.Use();
            Pipeline.SetUniform("uTexture0", 0);

            for (int i = 0; i < Transforms.Length; i++)
            {
                Pipeline.SetUniform("uModel", Transforms[i].ViewMatrix);
                Gl.DrawElements(PrimitiveType.Triangles, (uint)Indices.Length, DrawElementsType.UnsignedInt, null);
            }
        }

        private static void OnLoad()
        {
            Gl = GL.GetApi(window);

            VertexBuffer = new Buffer<float>(Gl, Vertices, BufferTargetARB.ArrayBuffer);
            IndexBuffer = new Buffer<uint>(Gl, Indices, BufferTargetARB.ElementArrayBuffer);
            VertexArray = new VertexArray<float, uint>(Gl, VertexBuffer, IndexBuffer, 5);
            VertexArray.SetVertexAttrib(VertexAttribPointerType.Float, 3); // position
            VertexArray.SetVertexAttrib(VertexAttribPointerType.Float, 2); // uv
            Pipeline = new Pipeline(Gl, File.ReadAllText(VertexShaderPath), File.ReadAllText(FragShaderPath));
            Texture = new Texture(Gl, TexturePath);

            // Set transformations
            //Translation.
            Transforms[0] = new Transform();
            Transforms[0].Position = new Vector3(0.5f, 0.5f, 0f);
            //Rotation.
            Transforms[1] = new Transform();
            Transforms[1].Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, 1f);
            //Scaling.
            Transforms[2] = new Transform();
            Transforms[2].Scale = 0.5f;
            //Mixed transformation.
            Transforms[3] = new Transform();
            Transforms[3].Position = new Vector3(-0.5f, 0.5f, 0f);
            Transforms[3].Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, 1f);
            Transforms[3].Scale = 0.5f;
        }
    }
}
