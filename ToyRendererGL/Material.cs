using Silk.NET.OpenGL;
using System.Numerics;

namespace ToyRendererGL
{
    public struct Material
    {
        public Material(GL gl, string texturePath, string specularTexturePath, float shiness) : this()
        {
            DiffuseTexture = new Texture(gl, texturePath);
            SpecularTexture = new Texture(gl, specularTexturePath);
            Shiness = shiness;
        }

        public Texture DiffuseTexture { get; set; }
        public Texture SpecularTexture { get; set; }
        public float Shiness { get; set; }
    }
}
