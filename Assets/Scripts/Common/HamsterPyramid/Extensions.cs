using UnityEngine;

namespace Common.HamsterPyramid
{
    public static class Extensions
    {
        public static Vector3 Multiply(this Vector3 m_vector, Vector3 multiplyVector)
        {
            return new Vector3(m_vector.x * multiplyVector.x, 
                               m_vector.y * multiplyVector.y,
                               m_vector.z * multiplyVector.z);
        }

        public static float InverseLerp(float from, float to, float value)
        {
            return (value - from) / (to - from);
        }
    }
}