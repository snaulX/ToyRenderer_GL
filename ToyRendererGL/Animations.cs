using System.Numerics;

namespace ToyRendererGL
{
    public static class Animations
    {
        private const float MinScale = 1;
        private const float MaxScale = 1.5f;
        private const float DegreesPerSecond = 90;

        private static float scale = MinScale;

        public static Transform ScaleAnimation(Transform transform, double deltaTime)
        {
            if (scale >= MaxScale)
            {
                scale -= (float)deltaTime;
            }
            else if (scale <= MinScale)
            {
                scale += (float)deltaTime;
            }
            transform.Scale = new Vector3(scale);
            return transform;
        }
        public static Transform ScaleRotation(Transform transform, double deltaTime)
        {
            transform.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, -((float)(DegreesPerSecond * deltaTime)).ToRadians());
            return transform;
        }
    }
}
