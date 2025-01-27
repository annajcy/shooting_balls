using System;
using UnityEngine;

namespace Utilities
{
    public static class MathUtility
    {
        public static Tuple<float, float> GetYawAndPitchRotation(Vector3 a, Vector3 b)
        {
            a = a.normalized;
            b = b.normalized;

            Vector3 a_ = new Vector3(a.x, 0, a.z).normalized;
            Vector3 b_ = new Vector3(b.x, 0, b.z).normalized;

            // Calculate yaw
            float yaw = Mathf.Atan2(b_.z, b_.x) - Mathf.Atan2(a_.z, a_.x);

            // Calculate pitch
            float pitch = Mathf.Asin(b.y) - Mathf.Asin(a.y);

            yaw = Mathf.Rad2Deg * yaw;
            pitch = Mathf.Rad2Deg * pitch;

            if (yaw >= 180.0f) yaw -= 360.0f;
            return new Tuple<float, float>(yaw, pitch);
        }
    }
}