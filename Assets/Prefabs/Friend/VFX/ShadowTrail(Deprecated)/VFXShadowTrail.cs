using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace CustomizedVFX
{
    public class VFXShadowTrail : MonoBehaviour
    {
        [SerializeField] private VisualEffect vfx;
        [SerializeField] private float duration = 1f;

        private void OnEnable()
        {
            Debug.Log("Shadow VFX Enable!");
        }

        public Vector2 velocity
        {
            get => vfx.GetVector2("Velocity");
            set
            {
                vfx.SetVector2("Velocity", value);
            }
        }

        private void AutoDisable()
        {
            StartCoroutine(Shutdown(duration));
        }

        IEnumerator Shutdown(float sec)
        {
            yield return new WaitForSeconds(sec);
            gameObject.SetActive(false);
        }
    }
}