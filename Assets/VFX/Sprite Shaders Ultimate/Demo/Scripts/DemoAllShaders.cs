using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEditor;

namespace SpriteShadersUltimate
{
    public class DemoAllShaders : MonoBehaviour
    {
        public static DemoAllShaders c;

        [Header("Normal Mapping:")]
        public List<DemoNormalMapping> normalMappings;

        Dictionary<Sprite, Texture> normalDictionary;
        List<SpriteRenderer> groundSprites;
        GameObject lightSource;
        Transform shadersParent;

        List<SpriteRenderer> sprites;

        Slider fadeSlider;
        Slider radiusSlider;
        Graphic[] typeButtons;

        Transform currentShaderParent;

        string lastPath;
        string currentCategory;
        string currentShader;
        string currentType;

        void Awake()
        {
            c = this;

            lightSource = GameObject.Find("Environment/Light");
            shadersParent = GameObject.Find("All Shaders").transform;

            fadeSlider = transform.Find("Fade/Slider").GetComponent<Slider>();
            radiusSlider = transform.Find("Radius/Slider").GetComponent<Slider>();

            typeButtons = new Graphic[]
            {
            transform.Find("Standard").GetComponent<Graphic>(),
            transform.Find("Lit").GetComponent<Graphic>()
            };

            normalDictionary = new Dictionary<Sprite, Texture>();
            foreach (DemoNormalMapping dnm in normalMappings)
            {
                normalDictionary.Add(dnm.sourceSprite, dnm.targetNormal);
            }
        }

        void Start()
        {
            groundSprites = new List<SpriteRenderer>();
            Transform groundParent = GameObject.Find("Environment/Ground").transform;
            for (int n = 0; n < groundParent.childCount; n++)
            {
                SpriteRenderer sr = groundParent.GetChild(n).GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    groundSprites.Add(sr);
                }
            }

            int shaderCount = 0;

            //Generate Categories:
            GameObject categoryBlank = transform.Find("Categories/Blank").gameObject;
            for (int n = 0; n < shadersParent.childCount; n++)
            {
                Transform categoryChild = shadersParent.GetChild(n);
                int categoryShaderCount = categoryChild.childCount;

                if(categoryChild.name != "Uber")
                {
                    shaderCount += categoryShaderCount;
                }

                string categoryName = categoryChild.name;

                GameObject newCategory = Instantiate<GameObject>(categoryBlank);
                newCategory.name = categoryName;
                newCategory.GetComponent<Text>().text = categoryName + "<size=18><color=#5555> (" + categoryShaderCount + ")</color></size>";
                newCategory.GetComponent<DemoShaderButton>().category = categoryName;
                newCategory.GetComponent<DemoShaderButton>().Invoke("StartFadeIn", 0.02f);
                newCategory.transform.SetParent(categoryBlank.transform.parent, true);
                newCategory.transform.localScale = Vector3.one;
                newCategory.GetComponent<RectTransform>().anchoredPosition = new Vector2(40, -38f * n);
                newCategory.SetActive(true);
            }

            transform.Find("Shader Count").GetComponent<Text>().text = shaderCount + " Shaders";

            SelectShader("Color", "Strong Tint");
            SwitchShaders("Standard");
        }

        void Update()
        {
            UpdateTypeButtons();
        }

        public void UpdateRadius()
        {
            SetRadius(radiusSlider.value);
        }
        public void SetRadius(float amount)
        {
            bool couldFade = false;

            if (currentShader == "Burn")
            {
                amount = 2.5f + amount * 4.3f;
            }else if(currentShader == "Ink Spread")
            {
                amount = 0.5f + amount * 5.5f;
            }

            if (currentCategory != "Interactive" && currentCategory != "Generated" && currentCategory != "Uber")
            {
                foreach (SpriteRenderer sr in sprites)
                {
                    Shader targetShader = sr.material.shader;

                    for (int n = 0; n < targetShader.GetPropertyCount(); n++)
                    {
                        ShaderPropertyType type = targetShader.GetPropertyType(n);
                        if (type == ShaderPropertyType.Range || type == ShaderPropertyType.Float)
                        {
                            string propertyName = targetShader.GetPropertyName(n);
                            if ((propertyName.EndsWith("Radius") || propertyName.EndsWith("Distance") || propertyName.EndsWith("Spread")) && propertyName.StartsWith("_Enable") == false)
                            {
                                sr.material.SetFloat(propertyName, amount);
                                couldFade = true;
                            }
                        }
                    }
                }
            }

            radiusSlider.transform.parent.Find("Hide").gameObject.SetActive(!couldFade);
        }

        public void UpdateFade()
        {
            SetFade(fadeSlider.value);
        }
        public void SetFade(float amount)
        {
            bool couldFade = false;

            if(currentCategory == "Fading")
            {
                if (currentShader.StartsWith("Source"))
                {
                    amount *= 5f;
                }

                if(currentShader == "Halftone")
                {
                    amount = 5 + amount * 18f;
                }

                if (currentShader.StartsWith("Directional"))
                {
                    amount = -2 + amount * 6f;
                }
            }

            if(currentCategory != "Interactive")
            {
                foreach (SpriteRenderer sr in sprites)
                {
                    Shader targetShader = sr.material.shader;

                    for (int n = 0; n < targetShader.GetPropertyCount(); n++)
                    {
                        ShaderPropertyType type = targetShader.GetPropertyType(n);
                        if (type == ShaderPropertyType.Range || type == ShaderPropertyType.Float)
                        {
                            string propertyName = targetShader.GetPropertyName(n);
                            if (propertyName.EndsWith("Fade") && propertyName.StartsWith("_Enable") == false)
                            {
                                sr.material.SetFloat(propertyName, amount);
                                couldFade = true;
                            }else if(propertyName == "_Hue")
                            {
                                sr.material.SetFloat(propertyName, amount - 0.5f);
                                couldFade = true;
                            } else if (propertyName == "_Brightness")
                            {
                                sr.material.SetFloat(propertyName, 1 + amount * 2f);
                                couldFade = true;
                            } else if (propertyName == "_Contrast")
                            {
                                sr.material.SetFloat(propertyName, 1 + amount * 1.5f);
                                couldFade = true;
                            } else if (propertyName == "_Saturation")
                            {
                                sr.material.SetFloat(propertyName, 1 - amount);
                                couldFade = true;
                            }
                        }
                    }
                }
            }

            fadeSlider.transform.parent.Find("Hide").gameObject.SetActive(!couldFade);
        }

        void UpdateTypeButtons()
        {
            foreach (Graphic g in typeButtons)
            {
                if (currentType == g.name)
                {
                    g.color = new Color(1, 1, 1, Mathf.Lerp(g.color.a, 1f, Time.deltaTime * 5f));
                    g.transform.localScale = Vector3.Lerp(g.transform.localScale, Vector3.one * 1.07f, Time.deltaTime * 5f);
                }
                else
                {
                    g.color = new Color(1, 1, 1, Mathf.Lerp(g.color.a, 0.3f, Time.deltaTime * 5f));
                    g.transform.localScale = Vector3.Lerp(g.transform.localScale, Vector3.one, Time.deltaTime * 5f);
                }
            }
        }

        public string GetCurrentType()
        {
            return currentType;
        }
        public string GetCurrentCategory()
        {
            return currentCategory;
        }
        public string GetCurrentShader()
        {
            return currentShader;
        }
        public void SelectShader(string category, string shader)
        {
            Transform shadersUI = transform.Find("Shaders");

            //Fade Current:
            if (category != "" && currentCategory != category)
            {
                //Clear Shaders:
                float delay = 0f;
                float delayIncrease = 0.012f;

                for (int n = 0; n < shadersUI.childCount; n++)
                {
                    Transform child = shadersUI.GetChild(n);
                    if (child.gameObject.activeSelf)
                    {
                        child.GetComponent<DemoShaderButton>().Invoke("FadeAway", delay);
                        delay += delayIncrease;
                    }
                }

                currentCategory = category;
                delay = 0.05f;

                //Create Shaders:
                Transform shaderCategory = shadersParent.Find(category);
                GameObject shaderBlank = shadersUI.Find("Blank").gameObject;
                for (int n = 0; n < shaderCategory.childCount; n++)
                {
                    string shaderName = shaderCategory.GetChild(n).name;

                    GameObject newShader = Instantiate<GameObject>(shaderBlank);
                    newShader.name = shaderName;
                    newShader.GetComponent<Text>().text = shaderName;
                    newShader.GetComponent<DemoShaderButton>().shader = shaderName;
                    newShader.transform.SetParent(shaderBlank.transform.parent, true);
                    newShader.transform.localScale = Vector3.one;

                    RectTransform rect = newShader.GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(0, -27f * n);

                    newShader.SetActive(true);

                    newShader.GetComponent<DemoShaderButton>().Invoke("StartFadeIn", delay);
                    delay += delayIncrease;
                }
            }

            if (shader != "")
            {
                currentShader = shader;
                lastPath = currentCategory + "/" + shader;
            }

            if (shader != "")
            {
                for (int n = 0; n < shadersParent.childCount; n++)
                {
                    Transform categoryChild = shadersParent.GetChild(n);

                    if (categoryChild.name == currentCategory)
                    {
                        categoryChild.gameObject.SetActive(true);

                        for (int m = 0; m < categoryChild.childCount; m++)
                        {
                            Transform shaderChild = categoryChild.GetChild(m);

                            if (shaderChild.name == shader)
                            {
                                shaderChild.gameObject.SetActive(true);
                                currentShaderParent = shaderChild;

                                string shaderName = shader;
                                if (shaderName.StartsWith("Example"))
                                {
                                    shaderName = "Uber";
                                }

                                sprites = new List<SpriteRenderer>();
                                foreach(SpriteRenderer sr in shaderChild.GetComponentsInChildren<SpriteRenderer>())
                                {
                                    if (sr.material.shader.name.Contains(shaderName))
                                    {
                                        sprites.Add(sr);
                                    }
                                }

                                SwitchShaders(currentType);
                            }
                            else
                            {
                                shaderChild.gameObject.SetActive(false);
                            }
                        }
                    }
                    else
                    {
                        categoryChild.gameObject.SetActive(false);
                    }
                }

                float targetAlpha = 1f;

                if (currentCategory == "Fading")
                {
                    targetAlpha = 0.5f;
                }

                SetFade(targetAlpha);
                SetRadius(0.5f);
                radiusSlider.value = 0.5f;
                fadeSlider.value = targetAlpha;

                #if UNITY_EDITOR
                if (sprites != null && sprites.Count > 0)
                {
                    Material[] materials = new Material[sprites.Count];
                    for(int n = 0; n < sprites.Count; n++)
                    {
                        materials[n] = sprites[n].material;
                    }
                    Selection.objects = materials;
                }else
                {
                    ParticleSystem[] particles = currentShaderParent.GetComponentsInChildren<ParticleSystem>();

                    if(particles.Length > 0)
                    {
                        Material[] mats = new Material[particles.Length];

                        for(int n = 0; n < particles.Length; n++)
                        {
                            mats[n] = particles[n].GetComponent<ParticleSystemRenderer>().material;
                        }

                        Selection.objects = mats;
                    }
                }
                #endif
            }
        }

        public void SwitchShaders(string newType)
        {
            currentType = newType;

            foreach (SpriteRenderer sr in groundSprites)
            {
                SwitchMaterial(sr, newType);
            }

            foreach (SpriteRenderer sr in shadersParent.Find(lastPath).GetComponentsInChildren<SpriteRenderer>())
            {
                SwitchMaterial(sr, newType);
            }

            foreach (ParticleSystem ps in shadersParent.Find(lastPath).GetComponentsInChildren<ParticleSystem>())
            {
                SwitchMaterial(ps.GetComponent<ParticleSystemRenderer>(), newType);
            }

            lightSource.SetActive(newType == "Lit");
        }
        public void SwitchMaterial(Renderer sr, string newType)
        {
            if (sr == null) return;

            string shaderPath = sr.material.shader.name;

            if (shaderPath.StartsWith("Sprite Shaders Ultimate") == false) return;

            string[] shaderPathArray = shaderPath.Split('/');

            if (shaderPathArray.Length < 3) return;

            string shaderName = shaderPathArray[shaderPathArray.Length - 2] + "/" + shaderPathArray[shaderPathArray.Length - 1];

            Shader newShader = null;

            if (shaderName.StartsWith("Uber"))
            {
                if (newType == "Standard")
                {
                    newShader = Shader.Find("Sprite Shaders Ultimate/Uber/Standard Uber");
                }
                else if (newType == "Lit")
                {
                    newShader = Shader.Find("Sprite Shaders Ultimate/Uber/URP Lit Uber");
                }
            }
            else
            {
                shaderName = shaderName.Replace(" URP", "").Replace(" Lit", "");

                if (newType == "Standard")
                {
                    newShader = Shader.Find("Sprite Shaders Ultimate/Standard/" + shaderName);
                }
                else if (newType == "Lit")
                {
                    newShader = Shader.Find("Sprite Shaders Ultimate/Uber/URP Lit Uber");

                    sr.material.shader = newShader;

                    string shaderRealName = shaderName.Split('/')[1];
                    if (shaderRealName != "Default" && shaderRealName != "Additive" && shaderRealName != "Multiplicative")
                    {
                        string variable = "_Enable" + shaderRealName.Replace(" ", "");
                        sr.material.SetFloat(variable, 1f);
                        sr.material.EnableKeyword(variable.ToUpper() + "_ON");
                    }

                }
            }

            if (newShader != null)
            {
                sr.material.shader = newShader;

                if (newType == "Lit")
                {
                    sr.material.SetFloat("_NormalIntensity", 0.5f);

                    SpriteRenderer sprite = sr.GetComponent<SpriteRenderer>();

                    if(sprite != null)
                    {
                        if (normalDictionary.ContainsKey(sprite.sprite))
                        {
                            sr.material.SetTexture("_NormalMap", normalDictionary[sprite.sprite]);
                        }
                    }
                }
            }
        }

        [System.Serializable]
        public class DemoNormalMapping
        {
            public Sprite sourceSprite;
            public Texture targetNormal;
        }
    }
}
