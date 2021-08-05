using System.Numerics;

namespace ToyRendererGL
{
    public class Camera
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Up { get; set; } = Vector3.UnitY;
        public Vector3 LookDirection 
        { 
            get => Vector3.Transform(Vector3.UnitZ, Rotation);
            set => LookAt(value);
        }
        public Quaternion Rotation { get; set; } = Quaternion.Identity;
        public float FieldOfView { get; set; } = 1f;
        public float Near { get; set; } = 0.1f;
        public float Far { get; set; } = 100f;
        public float AspectRatio { get; set; }
        public Matrix4x4 ViewMatrix { get; private set; }
        public Matrix4x4 PerspectiveMatrix { get; private set; }

        public Camera(int width, int height)
        {
            OnResized(width, height);
        }

        public void OnResized(int width, int height)
        {
            AspectRatio = width / height;
        }

        public void LookAt(Vector3 target)
        {
            ViewMatrix = Matrix4x4.CreateLookAt(Position, target, Up);
            Rotation = Quaternion.CreateFromRotationMatrix(ViewMatrix);
        }

        public void UpdateViewMatrix()
        {
            ViewMatrix = Matrix4x4.CreateLookAt(Position, Position + LookDirection, Up);
        }

        public void UpdatePerspectiveMatrix()
        {
            PerspectiveMatrix = Matrix4x4.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Near, Far);
        }
    }
}
