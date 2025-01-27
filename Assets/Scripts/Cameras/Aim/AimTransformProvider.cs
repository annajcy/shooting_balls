using UnityEngine;

namespace Cameras.Aim
{
    public class AimTransformProvider : MonoBehaviour
    {
        public Vector3 Forward()
        {
            return this.transform.forward;
        }

        public Vector3 Position()
        {
            return this.transform.position;
        }

        public Quaternion Rotation()
        {
            return this.transform.rotation;
        }
    }
}