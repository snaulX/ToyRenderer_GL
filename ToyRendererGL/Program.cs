using System;
using Silk.NET.OpenGL;
using Silk.NET.Input;
using Silk.NET.Windowing;
using System.Drawing;
using Silk.NET.Maths;

namespace ToyRendererGL
{
    static class Program
    {
        private static IWindow window;
        private static GL Gl;

        private static Buffer<float> VertexBuffer;
        private static Buffer<uint> IndexBuffer;

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
            throw new NotImplementedException();
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
        }
    }
}
