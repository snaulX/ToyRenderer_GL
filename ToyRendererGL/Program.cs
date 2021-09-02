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
        private const string DiffuseTexturePath = "Brick_Wall_008_SD\\Brick_wall_008_COLOR.jpg";
        private const string SpecularTexturePath = "Brick_Wall_008_SD\\Brick_wall_008_SPEC.jpg";
        private const PrimitiveType Primitive = PrimitiveType.Triangles;
        private const float DegreesPerSecond = 180;
        private const float MaxScale = 1.5f;
        private const float MinScale = 1;

        private static IWindow Window;
        private static GL Gl;
        private static Input Input;

        private static RenderTextured RenderTask;
        private static TexturedCube Cube;
        private static Camera Camera;
        private static bool ScaleAnimationUp = true;

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
            Gl.Dispose();
            Input.Dispose();
        }

        private static unsafe void OnRender(double deltaTime)
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            RenderTask.Render(Camera, deltaTime);
        }

        private static unsafe void OnLoad()
        {
            Input = new Input(Window);
            Input.OnKeyDown += KeyDown;

            Gl = GL.GetApi(Window);

            Gl.Enable(EnableCap.DepthTest);
#if DEBUG
            Gl.Enable(EnableCap.DebugOutput);
            Gl.DebugMessageCallback(DebugMessage, null);
#endif
            //Gl.DepthMask(false);
            //Gl.DepthFunc(DepthFunction.Less);

            Cube = new TexturedCube(Gl, DiffuseTexturePath);
            TexturedCube snaulXCube = new TexturedCube(Gl, "snaulx.jpg");
            snaulXCube.Transform.Position = new Vector3(1, 2, 3);
            RenderTask = new RenderTextured(Gl, Cube, snaulXCube)
            {
                Animations = new Func<Transform, double, Transform>[] { ScaleAnimation, ScaleRotation }
            };
            RenderTask.Init();

            Camera = new Camera(Window.Size.X, Window.Size.Y);
            Window.Resize += (size) => Camera.OnResized(size.X, size.Y);
            Camera.Position = new Vector3(0, 0, 3);
            Camera.LookTarget = Cube.Transform.Position;
            Camera.Up = Vector3.UnitY;
            Camera.UpdateViewMatrix();
            Camera.UpdatePerspectiveMatrix();
        }

        private static void KeyDown(IKeyboard keyboard, Key key, int arg3)
        {
            if (key == Key.Escape)
                Window.Close();

            if (key == Key.W)
            {
                Camera.Position -= Vector3.UnitZ;
            }
            else if (key == Key.S)
            {
                Camera.Position += Vector3.UnitZ;
            }
            else if (key == Key.A)
            {
                Camera.Position -= Vector3.UnitX;
                Camera.LookTarget -= Vector3.UnitX;
            }
            else if (key == Key.D)
            {
                Camera.Position += Vector3.UnitX;
                Camera.LookTarget += Vector3.UnitX;
            }
            else if (key == Key.Space)
            {
                Camera.Position += Vector3.UnitY;
                Camera.LookTarget += Vector3.UnitY;
            }
            else if (key == Key.ControlLeft)
            {
                Camera.Position -= Vector3.UnitY;
                Camera.LookTarget -= Vector3.UnitY;
            }

            if (key == Key.Up)
            {
                Camera.LookTarget += Vector3.UnitY;
            }
            else if (key == Key.Down)
            {
                Camera.LookTarget -= Vector3.UnitY;
            }
            else if (key == Key.Right)
            {
                Camera.LookTarget += Vector3.UnitX;
            }
            else if (key == Key.Left)
            {
                Camera.LookTarget -= Vector3.UnitX;
            }
#if DEBUG
            Console.WriteLine(Camera.Position);
            Console.WriteLine(arg3);
#endif

            Camera.UpdateViewMatrix();
        }

        private static Transform ScaleAnimation(Transform transform, double deltaTime)
        {
            float scale = transform.Scale;
            if (scale >= MaxScale)
            {
                ScaleAnimationUp = false;
            }
            else if (scale <= MinScale)
            {
                ScaleAnimationUp = true;
            }
            transform.Scale = (float)(scale + (ScaleAnimationUp ? deltaTime : -deltaTime));
            return transform;
        }
        private static Transform ScaleRotation(Transform transform, double deltaTime)
        {
            transform.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, -((float)(DegreesPerSecond * deltaTime)).ToRadians());
            return transform;
        }

#if DEBUG
        private static void DebugMessage(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint message, nint userParam)
        {
            Console.WriteLine($"{source} {type} {id} {severity} {length} {message} {userParam}");
        }
#endif
    }
}
