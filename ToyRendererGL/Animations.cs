using System.Numerics;

namespace ToyRendererGL
{
    public static class Animations
    {
        private const float MinScale = 1;
        private const float MaxScale = 1.5f;
        private const float ScaleTime = 1.5f;
        private const float DegreesPerSecond = 90;

        private static float scale = MinScale;
        private static bool scaleDirection = true;

        public static Transform ScaleAnimation(Transform transform, double deltaTime)
        {
            switch (scale)
            {
                case >= MaxScale:
                    scaleDirection = false;
                    break;
                case <= MinScale:
                    scaleDirection = true;
                    break;
            }
            if (scaleDirection) scale += (float)deltaTime / ScaleTime;
            else scale -= (float)deltaTime / ScaleTime;
            transform.Scale = new Vector3(scale);
            return transform;
        }
        public static Transform RotationAnimation(Transform transform, double deltaTime)
        {
            transform.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, -((float)(DegreesPerSecond * deltaTime)).ToRadians());
            return transform;
        }

        public static Transform PositionAnimation(Transform transform, double deltaTime)
        {
            return transform;
        }
    }
}
