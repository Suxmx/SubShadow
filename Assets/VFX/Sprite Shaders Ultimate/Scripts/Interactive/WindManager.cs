using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteShadersUltimate
{
    public class WindManager : MonoBehaviour
    {
        [Header("Settings:")]

        [Tooltip("Smaller values will result in more frequent changes in wind.")]
        public float windNoiseScale = 0.1f;
        private float lastWindNoiseScale;

        [Tooltip("Speed at which the wind pattern moves horizontally.")]
        public float windNoiseSpeed = 2;
        private float lastWindNoiseSpeed;

        [Tooltip("The wind intensity will be between this value and 'Wind Intensity To'.")]
        public float windIntensityFrom = -0.5f;
        private float lastWindIntensityFrom;

        [Tooltip("The wind intensity will be between this value and 'Wind Intensity From'.")]
        public float windIntensityTo = 0.5f;
        private float lastWindIntensityTo;

        //Will update global shader variables as changes are made.
        void FixedUpdate()
        {
            ModifyIfChanged(ref windNoiseScale,ref lastWindNoiseScale, "WindNoiseScale");
            ModifyIfChanged(ref windNoiseSpeed, ref lastWindNoiseSpeed, "WindNoiseSpeed");
            ModifyIfChanged(ref windIntensityFrom, ref lastWindIntensityFrom, "WindMinIntensity");
            ModifyIfChanged(ref windIntensityTo, ref lastWindIntensityTo, "WindMaxIntensity");
        }

        //Modifies the global shader value on change.
        public void ModifyIfChanged(ref float currentValue, ref float oldValue, string globalShaderName)
        {
            if(oldValue != currentValue)
            {
                oldValue = currentValue;
                Shader.SetGlobalFloat(globalShaderName, currentValue);
            }
        }
    }
}