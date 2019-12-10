using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Byjus.CarGame
{
    public class WheelRotate : MonoBehaviour
    {
        public Vector3 _RotationSpeed = Vector3.zero;

        void LateUpdate()
        {
            transform.Rotate(_RotationSpeed);
        }
    }
}
