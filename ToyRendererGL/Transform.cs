﻿using System;
using System.Numerics;

namespace ToyRendererGL
{
    public class Transform
    {
        public Vector3 Position { get; set; } = new Vector3(0, 0, 0);

        public Vector3 Scale { get; set; } = Vector3.One;

        public Quaternion Rotation { get; set; } = Quaternion.Identity;

        //Note: The order here does matter.
        public Matrix4x4 ViewMatrix 
            => Matrix4x4.Identity * Matrix4x4.CreateFromQuaternion(Rotation) * Matrix4x4.CreateScale(Scale) * Matrix4x4.CreateTranslation(Position);
    }
}
