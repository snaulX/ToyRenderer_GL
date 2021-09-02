using System.Numerics;

namespace ToyRendererGL
{
    public class Camera
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Up { get; set; } = Vector3.UnitY;
        public Vector3 LookTarget { get; set; } = Vector3.Zero;//-Vector3.UnitZ;
        public Vector3 LookDirection => Vector3.Normalize(Vector3.Cross(Position, LookTarget));
        public float FieldOfView { get; set; } = System.MathF.PI / 2;
        public float Near { get; set; } = 0.1f;
        public float Far { get; set; } = 100f;
        public float AspectRatio { get; set; }
        public Matrix4x4 ViewMatrix { get; private set; }
        public Matrix4x4 PerspectiveMatrix { get; private set; }

        public Camera(int width, int height)
        {
            OnResized(width, height);
            UpdateViewMatrix();
        }

        public void OnResized(int width, int height)
        {
            if (width != 0 || height != 0)
            {
                AspectRatio = width / height;
                UpdatePerspectiveMatrix();
            }
        }

        public void UpdateViewMatrix()
        {
            ViewMatrix = Matrix4x4.CreateLookAt(Position, LookTarget, Up);
        }

        public void UpdatePerspectiveMatrix()
        {
            PerspectiveMatrix = Matrix4x4.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Near, Far);
        }
    }
}
