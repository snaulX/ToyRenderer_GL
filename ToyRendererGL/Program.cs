﻿#if DEBUG
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
        private const float Angle = 30;

        private static IWindow Window;
        private static GL Gl;
        private static Input Input;

        private static Buffer<float> VertexBuffer;
        private static Buffer<uint> IndexBuffer;
        private static VertexArray<float, uint> VertexArray;
        private static Pipeline DefaultPipeline;
        private static Texture DiffuseTexture;
        private static Texture SpecularTexture;

        private static Camera Camera;
        private static Transform ObjectTransform = new Transform();

        private static readonly float[] Vertices =
        {
            //X    Y      Z       U     V
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
            0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
            0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
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
            DefaultPipeline.Dispose();
            DiffuseTexture.Dispose();
            SpecularTexture.Dispose();
            Gl.Dispose();
            Input.Dispose();
        }

        private static unsafe void OnRender(double deltaTime)
        {
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            VertexArray.Bind();

            DiffuseTexture.Bind(TextureUnit.Texture0);

            DefaultPipeline.Use();

            DefaultPipeline.SetUniform("view", Camera.ViewMatrix);
            DefaultPipeline.SetUniform("projection", Camera.PerspectiveMatrix);
            {
                ObjectTransform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathF.PI / 180 * Angle);
                DefaultPipeline.SetUniform("model", ObjectTransform.ViewMatrix);
            }

//#if DEBUG
//            Vector4 pos = new Vector4(-0.5f, -0.5f, -0.5f, 1);
//            Console.WriteLine(Multiply(Camera.PerspectiveMatrix * Camera.ViewMatrix * ObjectTransform.ViewMatrix, pos));
//#endif

            Gl.DrawArrays(Primitive, 0, 36);
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

            VertexBuffer = new Buffer<float>(Gl, Vertices, BufferTargetARB.ArrayBuffer);
            IndexBuffer = new Buffer<uint>(Gl, Indices, BufferTargetARB.ElementArrayBuffer);
            VertexArray = new VertexArray<float, uint>(Gl, VertexBuffer, IndexBuffer, 5);
            VertexArray.SetVertexAttrib(VertexAttribPointerType.Float, 3); // position
            VertexArray.SetVertexAttrib(VertexAttribPointerType.Float, 2); // uv
            DefaultPipeline = new Pipeline(Gl, File.ReadAllText(VertexShaderPath), File.ReadAllText(FragShaderPath));
            DiffuseTexture = new Texture(Gl, DiffuseTexturePath);
            SpecularTexture = new Texture(Gl, SpecularTexturePath);

            Camera = new Camera(Window.Size.X, Window.Size.Y);
            Window.Resize += (size) => Camera.OnResized(size.X, size.Y);
            Camera.Position = new Vector3(0, 0, 3);
            Camera.LookTarget = Vector3.Zero;
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


        private static Matrix4x4 Multiply(Matrix4x4 mat4x4, Vector4 vec4)
        {
            Matrix4x4 ret = new Matrix4x4();
            {
                ret.M11 = mat4x4.M11 * vec4.X;
                ret.M12 = mat4x4.M12 * vec4.Y;
                ret.M13 = mat4x4.M13 * vec4.Z;
                ret.M14 = mat4x4.M14 * vec4.W;
            }
            {
                ret.M21 = mat4x4.M21 * vec4.X;
                ret.M22 = mat4x4.M22 * vec4.Y;
                ret.M23 = mat4x4.M23 * vec4.Z;
                ret.M24 = mat4x4.M24 * vec4.W;
            }
            {
                ret.M31 = mat4x4.M31 * vec4.X;
                ret.M32 = mat4x4.M32 * vec4.Y;
                ret.M33 = mat4x4.M33 * vec4.Z;
                ret.M34 = mat4x4.M34 * vec4.W;
            }
            {
                ret.M41 = mat4x4.M41 * vec4.X;
                ret.M42 = mat4x4.M42 * vec4.Y;
                ret.M43 = mat4x4.M43 * vec4.Z;
                ret.M44 = mat4x4.M44 * vec4.W;
            }
            return ret;
        }

#if DEBUG
        private static void DebugMessage(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint message, nint userParam)
        {
            Console.WriteLine($"{source} {type} {id} {severity} {length} {message} {userParam}");
        }
#endif
    }
}
