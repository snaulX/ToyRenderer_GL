using System.Numerics;
using System.IO;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Input;

namespace ToyRendererGL
{
    static class Program
    {
        private const string VertexShaderPath = "shader.vert";
        private const string FragShaderPath = "shader.frag";
        private const string TexturePath = "snaulx.jpg";

        private static IWindow Window;
        private static GL Gl;
        private static Input Input;

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
            WindowOptions options = WindowOptions.Default;
            options.Title = "ToyRenderer by snaulX";
            Window = Silk.NET.Windowing.Window.Create(options);

            Window.Load += OnLoad;
            Window.Render += OnRender;
            Window.Closing += OnClosing;

            Window.Run();
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
            Input = new Input(Window);
            Input.OnKeyDown += KeyDown;

            Gl = GL.GetApi(Window);

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

        private static void KeyDown(IKeyboard keyboard, Key key, int arg3)
        {
            if (key == Key.Escape)
                Window.Close();
        }
    }
}
