using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteShadersUltimate
{
    public class DemoLight : MonoBehaviour
    {
        [Header("Automatic Movement:")]
        public Vector3 positionA;
        public Vector3 positionB;
        public float frequency = 1f;

        float currentTime;

        //Internal:
        float isAutomatic;
        bool mouseOver;
        bool isDragging;
        float shaderHighlight;

        //References:
        SpriteRenderer sprite;
        Material mat;

        void Awake()
        {
            mouseOver = false;
            isAutomatic = 1f;
        }

        private void Start()
        {
            sprite = GetComponent<SpriteRenderer>();
            mat = sprite.material;

            shaderHighlight = 0;
            UpdateVisuals();
        }

        void OnMouseEnter()
        {
            mouseOver = true;
        }
        void OnMouseExit()
        {
            mouseOver = false;
        }

        void LateUpdate()
        {
            float oldHighlight = shaderHighlight;

            if (mouseOver || isDragging)
            {
                isAutomatic += (0f - isAutomatic) * Time.deltaTime * 3f;

                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    isDragging = true;
                    isAutomatic = 0;
                }
                else
                {
                    if (isDragging == true)
                    {
                        isAutomatic = 1f;
                    }

                    isDragging = false;
                }

                if (isDragging)
                {
                    Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    targetPosition.z = 0;
                    transform.position = targetPosition;

                    shaderHighlight = 1f;
                }
                else
                {
                    shaderHighlight = 0.5f;
                }
            }
            else
            {
                isAutomatic += Time.deltaTime * 2f;
                shaderHighlight = 0f;
            }

            isAutomatic = Mathf.Clamp01(isAutomatic);
            transform.localScale = Vector3.one * (0.5f - isAutomatic * 0.1f);

            currentTime += Time.deltaTime * frequency * isAutomatic;
            Vector3 desiredPosition = Vector3.Lerp(positionA, positionB, (1f + Mathf.Sin(currentTime)) / 2f);
            desiredPosition.y -= Mathf.Abs((1f + Mathf.Sin(currentTime)) / 2f - 0.5f) * 6f;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * isAutomatic * 2.5f);

            //Update Visuals:
            if (shaderHighlight != oldHighlight)
            {
                UpdateVisuals();
            }
        }

        void UpdateVisuals()
        {
            mat.SetFloat("_OutlineFade", shaderHighlight);
            sprite.color = new Color(1, 1, 1, 0.25f + 0.75f * shaderHighlight);
        }
    }
}