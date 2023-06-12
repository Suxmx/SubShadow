using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpriteShadersUltimate {

    public class DemoShaderButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public string category = "";
        public string shader = "";

        RectTransform rect;
        Graphic myGraphic;
        bool isHovered;
        bool isFadingOut;
        bool isFadingIn;

        void Start()
        {
            myGraphic = GetComponent<Graphic>();
            rect = GetComponent<RectTransform>();

            isFadingIn = true;
            isFadingOut = false;
            isHovered = false;

            myGraphic.color = new Color(1, 1, 1, 0);
        }

        void Update()
        {
            if (isFadingOut)
            {
                rect.anchoredPosition += Vector2.down * 100f * Time.deltaTime;
                myGraphic.color = new Color(1, 1, 1, Mathf.Max(0, Mathf.Lerp(myGraphic.color.a, -0.1f, Time.deltaTime * 35f * (0.05f + myGraphic.color.a))));

                if (myGraphic.color.a <= 0.0001f)
                {
                    Destroy(gameObject);
                }

                return;
            }

            if (isFadingIn)
            {
                transform.localScale = new Vector3(1, 1.3f, 1);
                return;
            }

            float targetAlpha = default;
            float targetScale = default;

            if ((category == "" || category == DemoAllShaders.c.GetCurrentCategory()) && (shader == "" || shader == DemoAllShaders.c.GetCurrentShader()))
            {
                targetAlpha = 1f;
                targetScale = 1.05f;
            }
            else
            {
                if (isHovered)
                {
                    targetScale = 1.12f;
                    targetAlpha = 0.6f;
                }
                else
                {
                    targetScale = 1f;
                    targetAlpha = 0.2f;
                }
            }

            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, targetScale, 1), Time.deltaTime * 14f);
            myGraphic.color = new Color(1, 1, 1, Mathf.Lerp(myGraphic.color.a, targetAlpha, Time.deltaTime * 7f));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isFadingOut) return;

            if ((category != "" && DemoAllShaders.c.GetCurrentCategory() != category) || (shader != "" && DemoAllShaders.c.GetCurrentShader() != shader))
            {
                DemoAllShaders.c.SelectShader(category, shader);
                transform.localScale = new Vector3(1f, 1.5f, 1);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovered = false;
        }

        public void StartFadeIn()
        {
            isFadingIn = false;
        }

        public void FadeAway()
        {
            isFadingOut = true;
        }
    }

}