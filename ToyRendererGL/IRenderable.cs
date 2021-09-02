using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;

namespace ToyRendererGL
{
    public interface IRenderable
    {
        void Init();
        void Render(GL gl, Pipeline pipeline);
    }
}
