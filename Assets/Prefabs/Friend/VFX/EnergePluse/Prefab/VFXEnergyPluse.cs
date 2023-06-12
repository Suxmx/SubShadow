using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace CustomizedVFX
{
    public class VFXEnergyPluse : MonoBehaviour
    {
        [SerializeField] private VisualEffect EnergyPluseVFXLeft;
        [SerializeField] private VisualEffect EnergyPluseVFXMid;
        [SerializeField] private VisualEffect EnergyPluseVFXRight;
        [SerializeField, Range(0f, 180f)] private float fanAngle = 70f;

        public void SetAngle(float angleInDegree)
        {
            if (EnergyPluseVFXLeft != null)
            {
                EnergyPluseVFXLeft.SetFloat("Angle", angleInDegree + fanAngle);
            }
            if (EnergyPluseVFXMid != null)
            {
                EnergyPluseVFXMid.SetFloat("Angle", angleInDegree);
            }
            if (EnergyPluseVFXRight != null)
            {
                EnergyPluseVFXRight.SetFloat("Angle", angleInDegree - fanAngle);
            }
        }

        public Vector3 up
        {
            get => transform.up;
            set
            {
                transform.up = value;
                SetAngle(transform.rotation.eulerAngles.z);
            }
        }
    }
}