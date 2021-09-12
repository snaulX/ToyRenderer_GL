using Silk.NET.OpenGL;
using System.Numerics;

namespace ToyRendererGL
{
    public struct Material
    {
        public Material(GL gl, string texturePath, Vector3 specular, float shiness) : this()
        {
            DiffuseTexture = new Texture(gl, texturePath);
            Specular = specular;
            Shiness = shiness;
        }

        public Texture DiffuseTexture { get; set; }
        public Vector3 Specular { get; set; }
        public float Shiness { get; set; }
    }
}
