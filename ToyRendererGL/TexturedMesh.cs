using System;
using System.Collections.Generic;
using Silk.NET.OpenGL;

namespace ToyRendererGL
{
    public abstract class TexturedMesh<TVertex, TIndex> : Mesh<TVertex, TIndex>, IAnimate
        where TVertex : unmanaged
        where TIndex : unmanaged
    {
        public Material Material;

        public Func<Transform, double, Transform>[] Animations { get; private set; } = Array.Empty<Func<Transform, double, Transform>>();

        public TexturedMesh(GL gl, string texturePath, string specularPath) : base(gl)
        {
            Material = new Material(gl, texturePath, specularPath, 64f);
        }

        public void ExecuteAnimation(double deltaTime)
        {
            foreach (var anim in Animations)
            {
                Transform = anim.Invoke(Transform, deltaTime);
            }
        }

        public void SetAnimations(params Func<Transform, double, Transform>[] animations)
            => Animations = animations;
    }
}
