using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;

namespace ToyRendererGL
{
    public interface IRenderTask
    {
        GL Gl { get; set; }
        void Init();
        void Render(Camera cam, double deltaTime);
    }
}
