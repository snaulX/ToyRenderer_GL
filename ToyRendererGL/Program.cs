#if DEBUG
using System;
#endif
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
        private const string FragLightShaderPath = "light.frag";
        private const string DiffuseTexturePath = "Brick_Wall_008_SD\\Brick_wall_008_COLOR.jpg";
        private const string SpecularTexturePath = "Brick_Wall_008_SD\\Brick_wall_008_SPEC.jpg";
        private const PrimitiveType Primitive = PrimitiveType.Triangles;

        private static IWindow Window;
        private static GL Gl;
        private static Input Input;

        private static Buffer<float> VertexBuffer;
        private static Buffer<uint> IndexBuffer;
        private static VertexArray<float, uint> VertexArray;
        private static Pipeline LampPipeline;
        private static Pipeline LightPipeline;
        private static Texture DiffuseTexture;
        private static Texture SpecularTexture;

        private static Camera Camera;
        private readonly static Vector3 LampPosition = new Vector3(1.2f, 1.0f, 2.0f);

        private static readonly float[] Vertices =
        {
            //X    Y      Z       Normals             U     V
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f, 0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f, 1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f, 1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f, 1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f, 0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f, 0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f, 0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f, 1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f, 1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f, 1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f, 0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f, 0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f, 0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f, 1.0f, 0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f, 1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f, 1.0f, 1.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f, 0.0f, 1.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f, 0.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f, 0.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f, 1.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f, 1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f, 1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f, 0.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f, 0.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f, 0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f, 1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f, 1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f, 1.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f, 0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f, 0.0f, 0.0f,

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f, 0.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f, 1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f, 1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f, 1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f, 0.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f, 0.0f, 0.0f
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
            LampPipeline.Dispose();
            LightPipeline.Dispose();
            DiffuseTexture.Dispose();
            SpecularTexture.Dispose();
            Gl.Dispose();
            Input.Dispose();
        }

        private static unsafe void OnRender(double deltaTime)
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            VertexArray.Bind();
            LightPipeline.Use();

            DiffuseTexture.Bind(TextureUnit.Texture0);
            SpecularTexture.Bind(TextureUnit.Texture1);

            //Setup the coordinate systems for our view
            LightPipeline.SetUniform("uModel", Matrix4x4.Identity);
            LightPipeline.SetUniform("uView", Camera.ViewMatrix);
            LightPipeline.SetUniform("uProjection", Camera.PerspectiveMatrix);
            //Let the shaders know where the Camera is looking from
            LightPipeline.SetUniform("viewPos", Camera.Position);
            //Configure the materials variables.
            //Diffuse is set to 0 because our diffuseMap is bound to Texture0
            LightPipeline.SetUniform("material.diffuse", 0);
            //Specular is set to 1 because our diffuseMap is bound to Texture1
            LightPipeline.SetUniform("material.specular", 1);
            LightPipeline.SetUniform("material.shininess", 32.0f);

            var diffuseColor = new Vector3(0.5f);
            var ambientColor = diffuseColor * new Vector3(0.2f);

            LightPipeline.SetUniform("light.ambient", ambientColor);
            LightPipeline.SetUniform("light.diffuse", diffuseColor); // darkened
            LightPipeline.SetUniform("light.specular", new Vector3(1.0f, 1.0f, 1.0f));
            LightPipeline.SetUniform("light.position", LampPosition);

            Gl.DrawArrays(Primitive, 0, 36);

            LampPipeline.Use();

            //The Lamp cube is going to be a scaled down version of the normal cubes verticies moved to a different screen location
            var lampMatrix = Matrix4x4.Identity;
            lampMatrix *= Matrix4x4.CreateScale(0.2f);
            lampMatrix *= Matrix4x4.CreateTranslation(LampPosition);

            LampPipeline.SetUniform("uModel", lampMatrix);
            LampPipeline.SetUniform("uView", Camera.ViewMatrix);
            LampPipeline.SetUniform("uProjection", Camera.PerspectiveMatrix);

            Gl.DrawArrays(Primitive, 0, 36);
        }

        private static void OnLoad()
        {
            Input = new Input(Window);
            Input.OnKeyDown += KeyDown;

            Gl = GL.GetApi(Window);

            Gl.Enable(EnableCap.DepthTest);
            Gl.DepthMask(false);
            Gl.DepthFunc(DepthFunction.Less);

            VertexBuffer = new Buffer<float>(Gl, Vertices, BufferTargetARB.ArrayBuffer);
            IndexBuffer = new Buffer<uint>(Gl, Indices, BufferTargetARB.ElementArrayBuffer);
            VertexArray = new VertexArray<float, uint>(Gl, VertexBuffer, IndexBuffer, 5);
            VertexArray.SetVertexAttrib(VertexAttribPointerType.Float, 3); // position
            VertexArray.SetVertexAttrib(VertexAttribPointerType.Float, 3); // normals
            VertexArray.SetVertexAttrib(VertexAttribPointerType.Float, 2); // uv
            string vertexCode = File.ReadAllText(VertexShaderPath);
            LampPipeline = new Pipeline(Gl, vertexCode, File.ReadAllText(FragShaderPath));
            LightPipeline = new Pipeline(Gl, vertexCode, File.ReadAllText(FragLightShaderPath));
            DiffuseTexture = new Texture(Gl, DiffuseTexturePath);
            SpecularTexture = new Texture(Gl, SpecularTexturePath);

            Camera = new Camera(Window.Size.X, Window.Size.Y);
            Window.Resize += (size) => Camera.OnResized(size.X, size.Y);
            Camera.Position = Vector3.UnitZ * 6;
            Camera.LookAt(Vector3.UnitZ * -1);
            //Camera.UpdateViewMatrix(); // ViewMatrix updates in LookAt method
            Camera.UpdatePerspectiveMatrix();

#if DEBUG
            Console.WriteLine(Camera.LookDirection);
            Console.WriteLine(Camera.Position);
            Console.WriteLine(LampPosition);
#endif
        }

        private static void KeyDown(IKeyboard keyboard, Key key, int arg3)
        {
            if (key == Key.Escape)
                Window.Close();
            if (key == Key.W)
            {
                Camera.Position += Vector3.UnitZ;
            }
            else if (key == Key.S)
            {
                Camera.Position -= Vector3.UnitZ;
            }
#if DEBUG
            Console.WriteLine(Camera.Position);
#endif

            Camera.UpdateViewMatrix();
        }
    }
}
