using System.Numerics;
using Silk.NET.OpenGL;

namespace ToyRendererGL
{
    public class Camera
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public float FieldOfView { get; set; }
        public float Near { get; set; }
        public float Far { get; set; }
    }
}
