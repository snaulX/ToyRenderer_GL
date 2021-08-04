using System;
using System.Numerics;

namespace ToyRendererGL
{
    public struct EulerAngles
    {
        float Yaw { get; set; }
        float Pitch { get; set; }
        float Roll { get; set; }

        public static implicit operator Quaternion(EulerAngles ea) 
            => Quaternion.CreateFromYawPitchRoll(ea.Yaw, ea.Pitch, ea.Roll);

        // https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles#Quaternion_to_Euler_angles_conversion
        public static explicit operator EulerAngles(Quaternion q)
        {
            EulerAngles angles = new EulerAngles();

            // roll (x-axis rotation)
            float sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            float cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            angles.Roll = MathF.Atan2(sinr_cosp, cosr_cosp);

            // pitch (y-axis rotation)
            float sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (MathF.Abs(sinp) >= 1)
                angles.Pitch = MathF.CopySign(MathF.PI / 2, sinp); // use 90 degrees if out of range
            else
                angles.Pitch = MathF.Asin(sinp);

            // yaw (z-axis rotation)
            float siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            float cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            angles.Yaw = MathF.Atan2(siny_cosp, cosy_cosp);

            return angles;
        }
    }
}
