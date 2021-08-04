using System;
using Silk.NET.OpenGL;

namespace ToyRendererGL
{
    public class VertexArray<TVertex, TIndex> : IDisposable
        where TIndex : unmanaged
        where TVertex : unmanaged
    {
        public unsafe uint VertexSize { get => vertexSize; set => vertexSize = (uint)(value * sizeof(TVertex)); }

        private uint code, vertexSize, index = 0;
        private int offset = 0;
        private GL gl;

        public VertexArray(GL gl, Buffer<TVertex> vertexBuffer, Buffer<TIndex> indexBuffer, uint vertexSize)
        {
            this.gl = gl;
            VertexSize = vertexSize;
            code = gl.GenVertexArray();
            Bind();
            vertexBuffer.Bind();
            indexBuffer.Bind();
        }

        public unsafe void SetVertexAttrib(VertexAttribPointerType type, int size)
        {
            gl.VertexAttribPointer(index, size, type, false, VertexSize, (void*)(offset * sizeof(TVertex)));
            gl.EnableVertexAttribArray(index);
            index++;
            offset += size;
        }

        public void Bind()
        {
            gl.BindVertexArray(code);
        }

        public void Dispose()
        {
            gl.DeleteVertexArray(code);
        }

    }
}
