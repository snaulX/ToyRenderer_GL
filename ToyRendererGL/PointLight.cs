using System;
using System.Collections.Generic;
using System.Numerics;

namespace ToyRendererGL
{
    public struct PointLight
    {
        public PointLight(Vector3 position, Vector3 ambient, Vector3 diffuse, Vector3 specular, 
            float constant = 1.0f, float linear = 0.09f, float quadratic = 0.032f)
        {
            Position = position;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Constant = constant;
            Linear = linear;
            Quadratic = quadratic;
        }

        public Vector3 Position { get; set; }
        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }

        public float Constant { get; set; }
        public float Linear { get; set; }
        public float Quadratic { get; set; }
    }
}
