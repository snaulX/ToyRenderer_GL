using System;
using System.Collections.Generic;
using System.Numerics;

namespace ToyRendererGL
{
    public struct DirectionLight
    {
        public DirectionLight(Vector3 direction, Vector3 ambient, Vector3 diffuse, Vector3 specular)
        {
            Direction = direction;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
        }

        public Vector3 Direction { get; set; }
        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }
    }
}
