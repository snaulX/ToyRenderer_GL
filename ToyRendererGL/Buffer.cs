using System;
using Silk.NET.OpenGL;

namespace ToyRendererGL
{
    public class Buffer<T> : IDisposable where T : unmanaged
    {
        private readonly uint code;
        private readonly GL gl;
        private readonly BufferTargetARB target;

        public Buffer(GL gl, Span<T> data, BufferTargetARB target)
        {
            this.gl = gl;
            this.target = target;
            code = gl.GenBuffer();
            Bind();
            SetData(data);
        }

        public void Bind()
        {
            gl.BindBuffer(target, code);
        }

        public unsafe void SetData(Span<T> data)
        {
            fixed (void* dataPointer = data)
                gl.BufferData(target, (nuint)(sizeof(T) * data.Length), dataPointer, BufferUsageARB.StaticDraw);
        }

        public void Dispose()
        {
            gl.DeleteBuffer(code);
        }
    }
}
