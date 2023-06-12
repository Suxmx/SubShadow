using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

namespace SpriteShadersUltimate
{
    [ExecuteAlways]
    public class MaterialInstancer : MonoBehaviour
    {
        private Renderer render;
        private Graphic graphic;
        private Material material;
        private float nextSave;

        //Serialized Stuff:
        public List<ShaderPropertyData> properties;
        public Shader shader;
        public Material original;

        //Events:
        private void Awake()
        {
            if(properties == null || properties.Count == 0)
            {
                Save();
            }
            else
            {
                Load();
            }
        }
        void Update()
        {
            #if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
            {
                if(UnityEditor.Selection.activeGameObject == gameObject && Time.realtimeSinceStartup > nextSave)
                {
                    if (Save())
                    {
                        UnityEditor.EditorUtility.SetDirty(this);
                    }

                    nextSave = Time.realtimeSinceStartup + 0.01f;
                }
            }
            #endif
        }

        //Functions:
        public void Initialize()
        {
            render = GetComponent<Renderer>();
            graphic = GetComponent<Graphic>();

            if (!HasMaterial())
            {
                return;
            }

            if (original == null && HasMaterial())
            {
                original = GetMaterial();
            }

            if(shader == null)
            {
                shader = original.shader;
            }

            if (properties != null && properties.Count > 0)
            {
                Load();
            }
            else
            {
                material = new Material(GetMaterial());
                material.name = "Instanced Shader";
                SetMaterial(material);
            }
        }

        public bool HasMaterial()
        {
            return render != null || graphic != null;
        }
        public Material GetMaterial()
        {
            if(render != null)
            {
                return render.sharedMaterial;
            }
            else
            {
                return graphic.material;
            }
        }
        public void SetMaterial(Material mat)
        {
            if (render != null)
            {
                render.sharedMaterial = mat;
            }
            else
            {
                graphic.material = mat;
            }
        }

        public bool Save()
        {
            bool changes = false;

            if (!HasMaterial())
            {
                Initialize();
            }

            if (!HasMaterial())
            {
                return false; //No Material located.
            }

            shader = material.shader;
            int propertyCount = shader.GetPropertyCount();

            if (properties == null || properties.Count != propertyCount)
            {
                changes = true;
                properties = new List<ShaderPropertyData>();

                for (int n = 0; n < shader.GetPropertyCount(); n++)
                {
                    string propertyName = shader.GetPropertyName(n);
                    ShaderPropertyType propertyType = shader.GetPropertyType(n);
                    if (propertyType == ShaderPropertyType.Range) propertyType = ShaderPropertyType.Float;

                    ShaderPropertyData propertyData = new ShaderPropertyData();
                    propertyData.Set(ref material, propertyName, propertyType);
                    properties.Add(propertyData);
                }
            }
            else
            {
                for (int n = 0; n < shader.GetPropertyCount(); n++)
                {
                    string propertyName = shader.GetPropertyName(n);
                    ShaderPropertyType propertyType = shader.GetPropertyType(n);
                    if (propertyType == ShaderPropertyType.Range) propertyType = ShaderPropertyType.Float;

                    ShaderPropertyData propertyData = properties[n];
                    if(propertyData.propName != propertyName || propertyData.type != propertyType)
                    {
                        changes = true;
                    }
                    if(propertyData.Set(ref material, propertyName, propertyType))
                    {
                        changes = true;
                    }
                }
            }

            return changes;
        }

        public void Load()
        {
            if(!HasMaterial())
            {
                Initialize();
            }

            if (!HasMaterial())
            {
                return; //No Material located.
            }

            if (properties != null && properties.Count > 0)
            {
                material = new Material(shader);
                foreach (ShaderPropertyData propertyData in properties)
                {
                    propertyData.Get(ref material);
                }
                material.name = "Instanced Shader";
                SetMaterial(material);
            }
        }

        public void Remove()
        {
            if(!HasMaterial())
            {
                Initialize();
            }

            if(original != null)
            {
                SetMaterial(original);
            }

            DestroyImmediate(this);
        }
    }

    [System.Serializable]
    public class ShaderPropertyData
    {
        [Header("Property:")]
        public ShaderPropertyType type;
        public string propName;

        [Header("Data:")]
        public Color colorValue;
        public float floatValue;
        public Texture textureValue;
        public Vector4 vectorValue;

        public bool Set(ref Material mat, string propertyName, ShaderPropertyType propertyType)
        {
            type = propertyType;
            propName = propertyName;

            switch (type)
            {
                case (ShaderPropertyType.Color):
                    Color col = mat.GetColor(propertyName);
                    if(colorValue != col)
                    {
                        colorValue = col;
                        return true;
                    }
                    break;
                case (ShaderPropertyType.Float):
                    float flo = mat.GetFloat(propertyName);
                    if(floatValue != flo)
                    {
                        floatValue = flo;
                        return true;
                    }
                    break;
                case (ShaderPropertyType.Texture):
                    Texture tex = mat.GetTexture(propertyName);
                    if(textureValue != tex)
                    {
                        textureValue = tex;
                        return true;
                    }
                    break;
                case (ShaderPropertyType.Vector):
                    Vector4 vec = mat.GetVector(propertyName);
                    if(vectorValue != vec)
                    {
                        vectorValue = vec;
                        return true;
                    }
                    break;
            }

            return false;
        }

        public void Get(ref Material mat)
        {
            switch (type)
            {
                case (ShaderPropertyType.Color):
                    mat.SetColor(propName,colorValue);
                    break;
                case (ShaderPropertyType.Float):
                    if(floatValue > 0.5f && propName.StartsWith("_Enable"))
                    {
                        mat.EnableKeyword(propName.ToUpper() + "_ON");
                    }
                    if(floatValue > 0.5f && propName == "_ShaderSpace")
                    {
                        mat.DisableKeyword("_SHADERSPACE_UV");

                        if(floatValue < 1.5f)
                        {
                            mat.EnableKeyword("_SHADERSPACE_UV_RAW");
                        }else if(floatValue < 2.5f)
                        {
                            mat.EnableKeyword("_SHADERSPACE_OBJECT");
                        }
                        else if (floatValue < 3.5f)
                        {
                            mat.EnableKeyword("_SHADERSPACE_OBJECT_SCALED");
                        }
                        else if (floatValue < 4.5f)
                        {
                            mat.EnableKeyword("_SHADERSPACE_WORLD");
                        }
                        else if (floatValue < 5.5f)
                        {
                            mat.EnableKeyword("_SHADERSPACE_UI_ELEMENT");
                        }
                        else if (floatValue < 6.5f)
                        {
                            mat.EnableKeyword("_SHADERSPACE_SCREEN");
                        }
                    }
                    mat.SetFloat(propName, floatValue);
                    break;
                case (ShaderPropertyType.Texture):
                    mat.SetTexture(propName, textureValue);
                    break;
                case (ShaderPropertyType.Vector):
                    mat.SetVector(propName, vectorValue);
                    break;
            }
        }
    }
}