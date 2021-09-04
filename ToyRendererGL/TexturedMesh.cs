using System;
using System.Collections.Generic;
using Silk.NET.OpenGL;

namespace ToyRendererGL
{
    public abstract class TexturedMesh<TVertex, TIndex> : Mesh<TVertex, TIndex>, IAnimate
        where TVertex : unmanaged
        where TIndex : unmanaged
    {
        public Texture DiffuseTexture;

        public Func<Transform, double, Transform>[] Animations { get; private set; } = Array.Empty<Func<Transform, double, Transform>>();

        public TexturedMesh(GL gl) : base(gl)
        {
        }

        public void ExecuteAnimation(double deltaTime)
        {
            foreach (var anim in Animations)
            {
                Transform = anim.Invoke(Transform, deltaTime);
            }
        }

        public void SetDiffuseTexture(string path)
        {
            DiffuseTexture = new Texture(Gl, path);
        }

        public void SetAnimations(params Func<Transform, double, Transform>[] animations)
            => Animations = animations;
    }
}
