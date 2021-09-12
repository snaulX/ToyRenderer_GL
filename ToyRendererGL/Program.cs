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
        private const string DiffuseTexturePath = "Brick_Wall_008_SD\\Brick_wall_008_COLOR.jpg";
        private const string SpecularTexturePath = "Brick_Wall_008_SD\\Brick_wall_008_SPEC.jpg";

        private static IWindow Window;
        private static GL Gl;
        private static Input Input;

        private static IRenderTask RenderTask;
        private static Scene Scene;

        private static Camera Camera => Scene.Camera;
        private static Light Light;

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

            RenderTask.Render(Scene, deltaTime);
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

            TexturedCube brickCube = new TexturedCube(Gl, DiffuseTexturePath);
            brickCube.SetAnimations(Animations.RotationAnimation);
            TexturedCube snaulXCube = new TexturedCube(Gl, "snaulx.jpg");
            snaulXCube.Transform.Position = new Vector3(1, 3, 1);
            snaulXCube.SetAnimations(Animations.ScaleAnimation);
            Camera cam = new Camera(Window.Size.X, Window.Size.Y);
            Window.Resize += (size) => cam.OnResized(size.X, size.Y);
            cam.Position = new Vector3(0, 0, 3);
            cam.LookTarget = brickCube.Transform.Position;
            cam.Up = Vector3.UnitY;
            cam.UpdateViewMatrix();
            cam.UpdatePerspectiveMatrix();
            Scene = new Scene(cam, brickCube, snaulXCube);
            Light = new Light(Vector3.One, new Vector3(0.2f, 0.2f, 0.2f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1.0f, 1.0f, 1.0f));
            Scene.AddLight(Light);
            RenderTask = new DiffuseLightTask(Gl);//new RenderTextured(Gl);
            RenderTask.Init();
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

#if DEBUG
        private static void DebugMessage(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint message, nint userParam)
        {
            Console.WriteLine($"{source} {type} {id} {severity} {length} {message} {userParam}");
        }
#endif
    }
}
