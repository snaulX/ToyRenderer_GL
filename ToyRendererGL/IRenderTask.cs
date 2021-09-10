using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;

namespace ToyRendererGL
{
    public interface IRenderTask
    {
        GL Gl { get; set; }
        void Init();
        void Render(Scene scene, double deltaTime);
    }
}
