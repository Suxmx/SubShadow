using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpriteShadersUltimate
{
    public class DemoTypeButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public string type = "Standard";

        bool isHovered;
        Graphic myGraphic;

        void Start()
        {
            myGraphic = GetComponent<Graphic>();
            isHovered = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (DemoAllShaders.c.GetCurrentType() != type)
            {
                DemoAllShaders.c.SwitchShaders(type);
                transform.localScale = new Vector3(1.1f, 1.2f, 1);
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

        void Update()
        {
            if (isHovered && DemoAllShaders.c.GetCurrentType() != type)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.1f, 1.2f, 1f), Time.deltaTime * 7f);
                myGraphic.color = Color.Lerp(myGraphic.color, Color.white, Time.deltaTime * 2f);
            }
        }
    }

}