using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteShadersUltimate
{
    public class DemoVerticalBorder : MonoBehaviour
    {
        RectTransform rect;
        public Transform shaders;

        void Start()
        {
            rect = GetComponent<RectTransform>();
        }

        void Update()
        {
            if (shaders == null) return;

            float targetY = -shaders.GetChild(shaders.childCount - 1).GetComponent<RectTransform>().anchoredPosition.y + 27;
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, Mathf.Lerp(rect.sizeDelta.y, targetY, Time.deltaTime * 10f));
        }
    }

}