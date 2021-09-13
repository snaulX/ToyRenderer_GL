using System;
using System.Numerics;

namespace ToyRendererGL
{
    public struct SpotLight
    {
        public SpotLight(Vector3 position, Vector3 direction, Vector3 ambient, Vector3 diffuse, Vector3 specular, 
            float cutOffDegrees, float outerCutOffDegrees, float constant = 1.0f, float linear = 0.09f, float quadratic = 0.032f)
        {
            Position = position;
            Direction = direction;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Constant = constant;
            Linear = linear;
            Quadratic = quadratic;
            CutOff = MathF.Cos(cutOffDegrees.ToRadians());
            OuterCutOff = MathF.Cos(outerCutOffDegrees.ToRadians());
        }

        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }
        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }

        public float Constant { get; set; }
        public float Linear { get; set; }
        public float Quadratic { get; set; }

        public float CutOff { get; set; }
        public float OuterCutOff { get; set; }
    }
}
