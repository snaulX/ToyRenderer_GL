using System;
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
        private static IWindow window;
        private static GL Gl;

        private static Buffer<float> VertexBuffer;
        private static Buffer<uint> IndexBuffer;
        private static VertexArray<float, uint> VertexArray;
        private static Pipeline Pipeline;

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
            window = Window.Create(WindowOptions.Default);
            window.Load += OnLoad;
            window.Render += OnRender;
            window.Closing += OnClosing;
        }

        private static void OnClosing()
        {
            VertexBuffer.Dispose();
            IndexBuffer.Dispose();
            VertexArray.Dispose();
            Pipeline.Dispose();
            Gl.Dispose();
        }

        private static void OnRender(double obj)
        {
            throw new NotImplementedException();
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
        }
    }
}
