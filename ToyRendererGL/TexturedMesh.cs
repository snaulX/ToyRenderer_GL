using System;
using System.Collections.Generic;
using Silk.NET.OpenGL;

namespace ToyRendererGL
{
    public abstract class TexturedMesh<TVertex, TIndex> : Mesh<TVertex, TIndex>
        where TVertex : unmanaged
        where TIndex : unmanaged
    {
        public Texture DiffuseTexture;

        public TexturedMesh(GL gl) : base(gl)
        {
        }

        public void SetDiffuseTexture(string path)
        {
            DiffuseTexture = new Texture(Gl, path);
        }
    }
}
