using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace SpriteShadersUltimate
{
    public class UberShaderGUI : ShaderGUI
    {
        public static ShaderInformation[] shaderInfos;

        Shader standardShader;
        Shader litShader;
        Shader additiveShader;
        Shader multiplicativeShader;
        Shader uiShader;
        Shader additiveUiShader;
        Shader lit3DShader;
        Shader lit3DURPShader;

        bool hintUber;
        int enabledCount;
        int shaderCount;

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            if (shaderInfos == null)
            {
                shaderInfos = Resources.LoadAll<ShaderInformation>("SSU/");
            }

            Material mat = ((Material)materialEditor.target);
            Shader shader = mat.shader;

            string extraLine = "";
            string extraLine2 = "";

            int shaderType = 0;
            if(shader.name.EndsWith("Standard Uber"))
            {
                shaderType = 0;
            }
            else if (shader.name.EndsWith("/Additive Uber"))
            {
                shaderType = 1;
            }
            else if (shader.name.EndsWith("Multiplicative Uber"))
            {
                shaderType = 2;
            }
            else if (shader.name.EndsWith("URP Lit Uber"))
            {
                shaderType = 3;
                extraLine = "- Will only work in <b>URP</b> with a <b>2D Renderer</b>.";
            }
            else if (shader.name.EndsWith("UI Uber"))
            {
                shaderType = 4;
                extraLine = "- For maskable <b>Image</b> and <b>Text</b> components.";
                extraLine2 = "- When using <b>UI Masks</b> press [<b>Control + S</b>] to update changes.";
            }
            else if (shader.name.EndsWith("UI Additive Uber"))
            {
                shaderType = 5;
                extraLine = "- For maskable <b>Image</b> and <b>Text</b> components.";
                extraLine2 = "- When using <b>UI Masks</b> press [<b>Control + S</b>] to update changes.";
            }
            else if (shader.name.EndsWith("3D Lit Uber"))
            {
                shaderType = 6;
                extraLine = "- Will not work in <b>URP</b> or <b>HDRP</b>.";
            }
            else if (shader.name.EndsWith("3D Lit URP Uber"))
            {
                shaderType = 7;
                extraLine = "- Will only work in <b>URP</b> with a <b>Forward Renderer</b>.";
            }

            if (standardShader == null)
            {
                standardShader = Shader.Find("Sprite Shaders Ultimate/Uber/Standard Uber");
            }
            if (litShader == null)
            {
                litShader = Shader.Find("Sprite Shaders Ultimate/Uber/URP Lit Uber");
            }
            if (additiveShader == null)
            {
                additiveShader = Shader.Find("Sprite Shaders Ultimate/Uber/Additive Uber");
            }
            if (multiplicativeShader == null)
            {
                multiplicativeShader = Shader.Find("Sprite Shaders Ultimate/Uber/Multiplicative Uber");
            }
            if(uiShader == null)
            {
                uiShader = Shader.Find("Sprite Shaders Ultimate/Uber/UI Uber");
            }
            if (additiveUiShader == null)
            {
                additiveUiShader = Shader.Find("Sprite Shaders Ultimate/Uber/UI Additive Uber");
            }
            if (lit3DShader == null)
            {
                lit3DShader = Shader.Find("Sprite Shaders Ultimate/Uber/3D Lit Uber");
            }
            if (lit3DURPShader == null)
            {
                lit3DURPShader = Shader.Find("Sprite Shaders Ultimate/Uber/3D Lit URP Uber");
            }

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.richText = true;
            buttonStyle.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.BeginVertical("Helpbox");
            //Shader Selection:

            int newShaderType = shaderType;

            int first = GUILayout.Toolbar(shaderType, new string[] { "Default", "Additive", "Multiply", "2D Lit URP" });
            int second = 4 + GUILayout.Toolbar(shaderType - 4, new string[] { "UI Default", "UI Additive", "3D Lit", "3D Lit URP"});

            if(extraLine != "")
            {
                GUI.color = new Color(1, 1, 1, 0.7f);
                EditorGUILayout.LabelField(extraLine, style);

                if(extraLine2 != "")
                {
                    EditorGUILayout.LabelField(extraLine2, style);
                }

                GUI.color = Color.white;
            }

            if (second != newShaderType)
            {
                newShaderType = second;
            }
            else
            {
                newShaderType = first;
            }

            if (newShaderType != shaderType)
            {
                switch (newShaderType)
                {
                    case (0):
                        if (standardShader != null)
                        {
                            mat.shader = standardShader;
                            EditorUtility.SetDirty(mat);
                        }
                        break;
                    case (1):
                        if (additiveShader != null)
                        {
                            mat.shader = additiveShader;
                            EditorUtility.SetDirty(mat);
                        }
                        break;
                    case (2):
                        if (multiplicativeShader != null)
                        {
                            mat.shader = multiplicativeShader;
                            EditorUtility.SetDirty(mat);
                        }
                        break;
                    case (3):
                        if (litShader != null)
                        {
                            mat.shader = litShader;
                            EditorUtility.SetDirty(mat);
                        }
                        break;
                    case (4):
                        if (uiShader != null)
                        {
                            mat.shader = uiShader;
                            EditorUtility.SetDirty(mat);
                        }
                        break;
                    case (5):
                        if (additiveUiShader != null)
                        {
                            mat.shader = additiveUiShader;
                            EditorUtility.SetDirty(mat);
                        }
                        break;
                    case (6):
                        if (lit3DShader != null)
                        {
                            mat.shader = lit3DShader;
                            EditorUtility.SetDirty(mat);
                        }
                        break;
                    case (7):
                        if (lit3DURPShader != null)
                        {
                            mat.shader = lit3DURPShader;
                            EditorUtility.SetDirty(mat);
                        }
                        break;
                }
                
                shaderType = newShaderType;
            }

            SingleShaderGUI.DisplayTroubleshooting();

            SingleShaderGUI.DisplayPropertyHint();

            EditorGUILayout.LabelField(" ", GUILayout.Height(1));
            GUI.color = new Color(1, 1, 1, EditorGUIUtility.isProSkin ? 1f : 0.4f);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUI.color = Color.white;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<size=13><b>Uber Shader Hints</b></size>", style);

            if (GUILayout.Button("<size=10>" + (hintUber ? "▼" : "▲") + "</size>", buttonStyle, GUILayout.Width(20)))
            {
                hintUber = !hintUber;
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();
            if (hintUber)
            {
                GUI.color = GUI.color = new Color(1, 1, 1, 0.7f); ;
                EditorGUILayout.LabelField("<b>Uber Shaders</b> allow you to stack multiple effects.", style);
                EditorGUILayout.LabelField("Simply enable the shaders you need per material.", style);
                EditorGUILayout.LabelField("Disabled shaders do not cost any performance.", style);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Use the <b>Fade</b> properties to hide or show shaders.", style);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Runtime toggling of shaders through keywords should be avoided.", style);
                EditorGUILayout.LabelField("Because unity will only include compiled shader variants into builds.", style);
                GUI.color = Color.white;
            }
            EditorGUILayout.EndVertical();

            SingleShaderGUI.DisplayUtility(materialEditor);


            string numberColor = EditorGUIUtility.isProSkin ? "<color=#FFFFFFFF>" : "<color=#000000bb>";

            EditorGUILayout.LabelField(" ", GUILayout.Height(1));
            GUI.color = new Color(1, 1, 1, EditorGUIUtility.isProSkin ? 1f : 0.4f);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUI.color = Color.white;
            EditorGUILayout.LabelField("<b><size=13>Enabled " + numberColor + enabledCount + "</color> out of " + numberColor + shaderCount + "</color> shaders.</size></b>", style);
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            string previousShader = "";
            int startIndex = 0;
            float lastEnabled = 1f;
            bool vGroup = false;
            shaderCount = enabledCount = 0;
            List<MaterialProperty> currentProperties = new List<MaterialProperty>();
            for(int n = 0; n <= properties.Length; n++)
            {
                MaterialProperty prop = (n == properties.Length) ? null : properties[n];

                if (n == properties.Length || prop.name.StartsWith("_Enable"))
                {
                    //Previous Shader:
                    if(lastEnabled > 0.5f && lastEnabled != 1.1f)
                    {
                        MaterialProperty[] currentArray = new MaterialProperty[currentProperties.Count];
                        for (int i = 0; i < currentProperties.Count; i++)
                        {
                            currentArray[i] = currentProperties[i];
                        }
                        ShaderInformation si = null;
                        if (previousShader != "")
                        {
                            foreach (ShaderInformation shaderInfo in shaderInfos)
                            {
                                if (shaderInfo.name == previousShader)
                                {
                                    si = shaderInfo;
                                    break;
                                }
                            }

                            if (si != null && si.shaderDescription != null && si.shaderDescription != "")
                            {
                                GUI.color = new Color(1, 1, 1, 0.7f);
                                string[] descriptionLines = si.shaderDescription.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.None);
                                for (int l = 0; l < descriptionLines.Length; l++)
                                {
                                    EditorGUILayout.LabelField(descriptionLines[l], style);
                                }
                                GUI.color = Color.white;
                            }
                        }

                        SingleShaderGUI.DisplayProperties(si, materialEditor, currentArray, startIndex);
                    }

                    //Grouping:
                    if(vGroup)
                    {
                        EditorGUILayout.EndVertical();
                        vGroup = false;
                    }

                    //Next Shader:
                    currentProperties = new List<MaterialProperty>();
                    if(n < properties.Length - 1)
                    {
                        if(prop.name == "_EnableStrongTint")
                        {
                            NewCategory("COLOR");
                        }else if(prop.name == "_EnableHologram")
                        {
                            NewCategory("EFFECT");
                        }
                        else if (prop.name == "_EnableFullAlphaDissolve")
                        {
                            NewCategory("FADING");
                        }
                        else if (prop.name == "_EnablePixelate")
                        {
                            NewCategory("TRANSFORM");
                        }
                        else if (prop.name == "_EnableWind")
                        {
                            NewCategory("INTERACTIVE");
                        }
                        else if (prop.name == "_EnableCheckerboard")
                        {
                            NewCategory("GENERATED");
                        }

                        string keyword = prop.name.ToUpper() + "_ON";
                        bool oldValue = prop.floatValue > 0.5f;

                        vGroup = true;
                        float brightness =  oldValue ? (EditorGUIUtility.isProSkin ? 0f : (prop.floatValue == 1.1f ? 0.3f : 0.5f)) : 1f;
                        GUI.color = new Color(brightness, brightness, brightness, 1);
                        EditorGUILayout.BeginVertical("Helpbox");

                        previousShader = prop.displayName.Replace("Enable ", "");
                        startIndex = n+1;

                        GUI.color = new Color(1, 1, 1, oldValue ? 1f : 0.8f);
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PrefixLabel("<b><size=14>" + previousShader + "</size></b>",style,style);
                        materialEditor.ShaderProperty(prop, GUIContent.none);
                        bool currentValue = prop.floatValue > 0.5f;

                        shaderCount++;
                        if (currentValue)
                        {
                            bool expanded = prop.floatValue != 1.1f;

                            enabledCount++;

                            if(GUILayout.Button("<size=10>" + (expanded ? "▼" : "▲") + "</size>", buttonStyle, GUILayout.Width(20))) {
                                expanded = !expanded;

                                if (expanded)
                                {
                                    prop.floatValue = 1f;
                                }
                                else
                                {
                                    prop.floatValue = 1.1f;
                                }
                            }
                        }

                        EditorGUILayout.EndHorizontal();
                        GUI.color = Color.white;

                        lastEnabled = prop.floatValue;

                        if (oldValue != currentValue)
                        {
                            foreach (Material material in prop.targets)
                            {
                                if (currentValue)
                                {
                                    material.EnableKeyword(keyword);
                                }
                                else
                                {
                                    material.DisableKeyword(keyword);
                                }
                            }
                        }
                    }
                }
                else
                {
                    currentProperties.Add(prop);
                }
            }
            if (vGroup)
            {
                EditorGUILayout.EndVertical();
            }

            SingleShaderGUI.DisplayFinalInformation();
        }

        public static void NewCategory(string category)
        {
            GUIStyle newStyle = new GUIStyle(GUI.skin.label);
            newStyle.alignment = TextAnchor.MiddleCenter;
            newStyle.richText = true;
            newStyle.normal.textColor = EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f, 1) :  new Color(0.3f,0.3f,0.3f, 1);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUI.color = new Color(1, 1, 1, 1f);
            EditorGUILayout.LabelField("<b><size=20>──────────────────────────────────────────────────────────── " + category + " ────────────────────────────────────────────────────────────</size></b>", newStyle);
            GUI.color = Color.white;
        }
    }
}
