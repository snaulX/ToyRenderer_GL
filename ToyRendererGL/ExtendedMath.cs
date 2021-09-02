using System;
using System.Numerics;

namespace ToyRendererGL
{
    public static class ExtendedMath
    {
        public static Matrix4x4 Multiply(Matrix4x4 mat4x4, Vector4 vec4)
        {
            Matrix4x4 ret = new Matrix4x4();
            {
                ret.M11 = mat4x4.M11 * vec4.X;
                ret.M12 = mat4x4.M12 * vec4.Y;
                ret.M13 = mat4x4.M13 * vec4.Z;
                ret.M14 = mat4x4.M14 * vec4.W;
            }
            {
                ret.M21 = mat4x4.M21 * vec4.X;
                ret.M22 = mat4x4.M22 * vec4.Y;
                ret.M23 = mat4x4.M23 * vec4.Z;
                ret.M24 = mat4x4.M24 * vec4.W;
            }
            {
                ret.M31 = mat4x4.M31 * vec4.X;
                ret.M32 = mat4x4.M32 * vec4.Y;
                ret.M33 = mat4x4.M33 * vec4.Z;
                ret.M34 = mat4x4.M34 * vec4.W;
            }
            {
                ret.M41 = mat4x4.M41 * vec4.X;
                ret.M42 = mat4x4.M42 * vec4.Y;
                ret.M43 = mat4x4.M43 * vec4.Z;
                ret.M44 = mat4x4.M44 * vec4.W;
            }
            return ret;
        }

        public static float ToRadians(this float degrees) => MathF.PI / 180 * degrees;
    }
}
