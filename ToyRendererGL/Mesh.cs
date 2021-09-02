using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyRendererGL
{
    public abstract class Mesh<TVertex, TIndex> 
        where TVertex : unmanaged
        where TIndex : unmanaged
    {
        public readonly GL Gl;

        public abstract TVertex[] Vertices { get; }
        public abstract TIndex[] Indices { get; }
        public abstract uint Size { get; }

        public readonly Buffer<TVertex> VertexBuffer;
        public readonly Buffer<TIndex> IndexBuffer;
        public VertexArray<TVertex, TIndex> VertexArray;
        public Transform Transform;

        public Mesh(GL gl)
        {
            Gl = gl;

            VertexBuffer = new Buffer<TVertex>(Gl, Vertices, BufferTargetARB.ArrayBuffer);
            IndexBuffer = new Buffer<TIndex>(Gl, Indices, BufferTargetARB.ElementArrayBuffer);
            VertexArray = new VertexArray<TVertex, TIndex>(Gl, VertexBuffer, IndexBuffer, Size);
        }
    }
}
