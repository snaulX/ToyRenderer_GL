using System;
using System.Collections.Generic;
using System.Numerics;

namespace ToyRendererGL
{
    public struct Light
    {
        public Light(Vector3 position, Vector3 ambient, Vector3 diffuse, Vector3 specular)
        {
            Position = position;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
        }

        public Vector3 Position { get; set; }
        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }
    }
}
