using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEngine.UI;
using System;

namespace SpriteShadersUltimate
{
    public class SingleShaderGUI : ShaderGUI
    {
        public static bool enableUtilities;

        //Shader Info:
        ShaderInformation infoAsset;
        string infoAssetName;

        //Other:
        string shaderName; //Name of Shader (ex: Strong Tint)
        string genericShaderPath; //No Type
        int shaderType; //Type (0=Standard,1=URP,2=Lit)

        static int mainTexId;
        static bool hintPropertyID;
        static bool hintTroubleshooting;
        static bool troubleGlowing;
        static bool troublePixelClipping;
        static bool troubleDemoVisuals;
        static bool troubleOther;

        static bool showMainTexture;
        static bool showSpaceSettings;
        static bool showTimeSettings;
        static bool showBonus;
        static bool showFading;
        static bool showNoise;
        static bool showLitSettings;

        Shader standardShader;
        Shader litShader;

        static Shader currentDefaultShader;
        static float[] defaultFloats;
        static Vector4[] defaultVectors;
        static Color[] defaultColors;

        static float uberFadingType;
        static float spaceType;
        static float timeType;
        static float hasEmission;

        public static bool isDeveloper = false; //Will check for certain consistencies and output Debug.Log() to help me keep variable names clean and stuff. 

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            //Shader Information:
            Material mat = ((Material)materialEditor.target);
            if (infoAsset == null || infoAssetName != mat.shader.name)
            {
                genericShaderPath = mat.shader.name;
                string[] strings = genericShaderPath.Split('/');

                if (strings.Length > 2)
                {
                    genericShaderPath = "";
                    for(int n = 2; n < strings.Length; n++)
                    {
                        genericShaderPath += (n > 2 ? "/" : "") + strings[n];
                    }
                }

                if (CloudProjectSettings.userName != null)
                {
                    isDeveloper = CloudProjectSettings.userName.StartsWith("ekincant");
                }

                //Type:
                shaderType = 0;
                if (genericShaderPath.EndsWith(" Lit"))
                {
                    genericShaderPath = genericShaderPath.Replace(" Lit", "");
                    shaderType = 1;
                }

                string[] splitAgain = genericShaderPath.Split('/');
                shaderName = splitAgain[splitAgain.Length - 1];

                standardShader = Shader.Find("Sprite Shaders Ultimate/Standard/" + genericShaderPath);
                litShader = Shader.Find("Sprite Shaders Ultimate/URP Lit/" + genericShaderPath + " Lit");

                infoAsset = Resources.Load<ShaderInformation>("SSU/" + genericShaderPath);
                infoAssetName = mat.shader.name;
            }

            mainTexId = Shader.PropertyToID("_MainTex");

            //Top:
            EditorGUILayout.BeginVertical("Helpbox");
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("<size=17><b>" + shaderName + "</b></size>", style);
            GUIStyle tabStyle = new GUIStyle(GUI.skin.button);
            tabStyle.richText = true;

            string standardButton = "Standard";
            if (standardShader == null)
            {
                standardButton = "- - -";
            }

            string litButton = "URP Lit";
            if (litShader == null)
            {
                litButton = "- - -";
            }

            int newShaderType = GUILayout.Toolbar(shaderType, new string[] { standardButton, litButton });
            if(newShaderType != shaderType)
            {
                switch (newShaderType)
                {
                    case (0):
                        if(standardShader != null)
                        {
                            mat.shader = standardShader;
                            EditorUtility.SetDirty(mat);
                        }
                        break;
                    case (1):
                        if (litShader != null)
                        {
                            mat.shader = litShader;
                            EditorUtility.SetDirty(mat);
                        }
                        break;
                }
            }
            EditorGUILayout.EndHorizontal();

            if (infoAsset != null && infoAsset.shaderDescription != null && infoAsset.shaderDescription != "")
            {
                GUI.color = new Color(1, 1, 1, 0.7f);
                string[] descriptionLines = infoAsset.shaderDescription.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.None);
                for (int n = 0; n < descriptionLines.Length; n++)
                {
                    EditorGUILayout.LabelField(descriptionLines[n], style);
                }
                GUI.color = Color.white;
            }
            else
            {
                EditorGUILayout.LabelField("- No information found.", style);
            }

            if(infoAsset != null && infoAsset.requiredComponents != null && infoAsset.requiredComponents.Count > 0 && Selection.gameObjects != null && Selection.gameObjects.Length == 1)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("<b>Components:</b>", style);
                EditorGUILayout.BeginHorizontal();
                foreach (string comp in infoAsset.requiredComponents)
                {
                    Type type = default;
                    switch(comp)
                    {
                        case ("InteractiveWind"):
                            type = typeof(InteractiveWind);
                            break;
                        case ("InteractiveSquish"):
                            type = typeof(InteractiveSquish);
                            break;
                        case ("BoxCollider2D"):
                            type = typeof(BoxCollider2D);
                            break;
                    }

                    Component component = Selection.activeGameObject.GetComponent(type);
                    if(component == null)
                    {
                        if (GUILayout.Button("Add " + comp))
                        {
                            Component addedComponent = Selection.activeGameObject.AddComponent(type);

                            if (comp == "BoxCollider2D")
                            {
                                BoxCollider2D box = (BoxCollider2D) addedComponent;

                                if (shaderName == "Grass")
                                {
                                    InteractiveWind.DefaultCollider(box);
                                }
                                else
                                {
                                    box.isTrigger = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        GUILayout.Toolbar(0, new string[] { "Add " + comp});
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            DisplayTroubleshooting();
            DisplayPropertyHint();
            DisplayUtility(materialEditor);

            //Switch to Uber:
            EditorGUILayout.LabelField(" ", GUILayout.Height(1));
            GUI.color = new Color(1, 1, 1, 0.4f);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUI.color = Color.white;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<size=13><b>Want to stack shaders ?</b></size>", style);
            if(GUILayout.Button("Switch to Uber"))
            {
                Undo.RecordObject(mat, "Switched to Uber Shader.");

                if (shaderName == "Additive")
                {
                    mat.shader = Shader.Find("Sprite Shaders Ultimate/Uber/Additive Uber");
                }
                else if (shaderName == "Multiplicative")
                {
                    mat.shader = Shader.Find("Sprite Shaders Ultimate/Uber/Multiplicative Uber");
                }
                else if (newShaderType == 0)
                {
                    mat.shader = Shader.Find("Sprite Shaders Ultimate/Uber/Standard Uber");
                }
                else
                {
                    mat.shader = Shader.Find("Sprite Shaders Ultimate/Uber/URP Lit Uber");
                }

                EditorUtility.SetDirty(mat);

                if(shaderName != "Default" && shaderName != "Additive" && shaderName != "Multiplicative")
                {
                    string variable = "_Enable" + shaderName.Replace(" ", "");
                    mat.SetFloat(variable, 1f);
                    mat.EnableKeyword(variable.ToUpper() + "_ON");

                    EditorUtility.SetDirty(mat);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();


            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            //Properties:
            DisplayProperties(infoAsset, materialEditor, properties,0);

            DisplayFinalInformation();
        }

        public static void Lines()
        {
            GUI.color = new Color(1, 1, 1, 0.5f);
            EditorGUILayout.LabelField("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ",GUILayout.Height(9));
            GUI.color = new Color(1, 1, 1, 1);
        }

        public static void DisplayProperties(ShaderInformation info, MaterialEditor materialEditor, MaterialProperty[] properties, int startIndex)
        {
            HashSet<string> toggledVariables = new HashSet<string>();

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;

            Material material = (Material)materialEditor.target;
            Shader shader = material.shader;

            if(currentDefaultShader != shader || defaultFloats == null)
            {
                currentDefaultShader = shader;

                int propCount = ShaderUtil.GetPropertyCount(shader);

                defaultFloats = new float[propCount];
                defaultVectors = new Vector4[propCount];
                defaultColors = new Color[propCount];

                Material defaultMaterial = new Material(shader);

                for(int n = 0; n < propCount; n++)
                {
                    string propName = ShaderUtil.GetPropertyName(shader, n);

                    switch (ShaderUtil.GetPropertyType(shader, n))
                    {
                        case (ShaderUtil.ShaderPropertyType.Float):
                            defaultFloats[n] = defaultMaterial.GetFloat(propName);
                            break;
                        case (ShaderUtil.ShaderPropertyType.Range):
                            defaultFloats[n] = defaultMaterial.GetFloat(propName);
                            break;
                        case (ShaderUtil.ShaderPropertyType.Vector):
                            defaultVectors[n] = defaultMaterial.GetVector(propName);
                            break;
                        case (ShaderUtil.ShaderPropertyType.Color):
                            defaultColors[n] = defaultMaterial.GetColor(propName);
                            break;
                        default:
                            break;
                    }
                }
            }

            int verticals = 0;

            for(int n = 0; n < properties.Length; n++)
            {
                MaterialProperty prop = properties[n];

                //Hidden:
                if(prop.name == "_texcoord" || prop.name == "_AlphaTex" || prop.name == "_AlphaCutoff" || prop.name == "_UseUIAlphaClip" || prop.name == "_ColorMask" || prop.name.StartsWith("_Stencil"))
                {
                    continue;
                }

                //Create Display Name:
                GUIContent displayName = new GUIContent();
                displayName.text = prop.displayName;

                if (prop.name == "_Color")
                {
                    displayName.text = "Sprite Color";
                }

                //Remove Prefix:
                string trueName = displayName.text;

                string[] prefixSplit = displayName.text.Split(':');
                if (prefixSplit.Length > 1)
                {
                    displayName.text = trueName = prefixSplit[1].Substring(1);

                    if (info != null && isDeveloper)
                    {
                        if (prefixSplit[0] != info.name)
                        {
                            Debug.Log("Expected Name [" + info.name + "]" + " but got [" + prefixSplit[0] + "].");
                        }

                        string propertyBegin = "_" + info.name.Replace(" ", "");
                        if (prop.name.StartsWith(propertyBegin) == false)
                        {
                            Debug.Log("Expected Variable Start [" + propertyBegin + "]" + " but false [" + prop.name + "].");
                        }
                    }
                }
                else if (isDeveloper)
                {
                    if (prop.name != "_MainTex" && prop.name != "_IsText" && prop.name != "PixelSnap" && prop.name != "_UberNoiseTexture" && prop.name != "_Color" && prop.name != "_NormalMap" && prop.name != "_MaskMap" && prop.name != "_NormalIntensity" && prop.name != "Uber Mask" && prop.name != "_Brightness" && prop.name != "_FullFade" && prop.name != "_UberFading" && prop.name != "_Metallic" && prop.name != "_Smoothness" && prop.name != "_HasEmission" && prop.name != "_EmissionMap" && prop.name != "_EmissionColor" && prop.name != "_SmoothnessMap" && prop.name != "_Contrast" && prop.name != "_Hue" && prop.name != "_UberMask" && prop.name != "_UberSpace" && prop.name != "_UberPosition" && prop.name != "_UberNoiseScale" && prop.name != "_UberNoiseFactor" && prop.name != "_UberWidth" && prop.name != "_Saturation" && prop.name != "_ShaderSpace" && prop.name != "_PixelsPerUnit" && prop.name != "_ScreenWidthUnits" && prop.name != "_RectWidth" && prop.name != "_RectHeight" && prop.name != "_TimeSettings" && prop.name != "_TimeScale" && prop.name != "_TimeFrequency" && prop.name != "_TimeValue" && prop.name != "_TimeRange" && prop.name != "_TimeFPS")
                    {
                        Debug.Log("Property name without shader prefix: [" + prop.name + "] & [" + displayName.text + "].");
                    }
                }

                //Show If:
                bool denied = false;
                if(info != null && info.showIfs != null)
                {
                    foreach (ShowIfToggled toggle in info.showIfs)
                    {
                        if (trueName == toggle.toggleVariable && prop.floatValue > 0.4f)
                        {
                            toggledVariables.Add(toggle.toggleVariable);
                        }

                        foreach (string displayNames in toggle.shownVariables)
                        {
                            if (displayNames == trueName)
                            {
                                if (toggledVariables.Contains(toggle.toggleVariable) == toggle.reverse)
                                {
                                    denied = true;
                                    continue;
                                }
                            }
                        }
                    }
                }
                if (denied) continue;

                //Title
                string title = GetTitle(trueName, info);

                //Seperation:
                if (verticals == 0 || title != "")
                {
                    if(verticals == 1)
                    {
                        if(startIndex <= 0)
                        {
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.Space();
                        }
                        verticals--;
                    }

                    verticals++;
                    if (startIndex <= 0)
                    {
                        EditorGUILayout.BeginVertical("Helpbox");
                    }
                    else
                    {
                        Lines();
                    }
                }

                if (title != "")
                {
                    if (startIndex <= 0)
                    {
                        switch(prop.name)
                        {
                            case ("_MainTex"):
                                ToggleCategory(title, ref showMainTexture, ref style);
                                if (showMainTexture == false) continue;
                                break;
                            case ("_Color"):
                                ToggleCategory(title, ref showBonus, ref style);
                                if (showBonus == false) continue;
                                break;
                            case ("_UberNoiseTexture"):
                                ToggleCategory(title, ref showNoise, ref style);
                                if (showNoise == false) continue;
                                break;
                            case ("_UberFading"):
                                ToggleCategory(title, ref showFading, ref style);
                                if (showFading == false) continue;
                                break;
                            case ("_MaskMap"):
                                ToggleCategory(title, ref showLitSettings, ref style);
                                if (showLitSettings == false) continue;
                                break;
                            case ("_Metallic"):
                                ToggleCategory(title, ref showLitSettings, ref style);
                                if (showLitSettings == false) continue;
                                break;
                            case ("_ShaderSpace"):
                                ToggleCategory(title, ref showSpaceSettings, ref style);
                                spaceType = prop.floatValue;

                                if (showSpaceSettings == false)
                                {
                                    continue;
                                }
                                
                                break;
                            case ("_TimeSettings"):
                                ToggleCategory(title, ref showTimeSettings, ref style);
                                timeType = prop.floatValue;

                                if(showTimeSettings == false)
                                {
                                    continue;
                                }
                                break;
                            default:
                                EditorGUILayout.LabelField("<b><size=13>" + title + "</size></b>", style);
                                break;
                        }
                    }
                }

                //Specific:
                switch (trueName)
                {
                    case ("Fade"):
                        displayName.text = "Fade " + Mathf.RoundToInt(prop.floatValue * 100f) + "%";
                        break;
                    case ("Uber Fading"):
                        displayName.text = "Type";
                        uberFadingType = prop.floatValue;
                        break;
                    case ("Full Fade"):
                        displayName.text = "Full Fade " + Mathf.RoundToInt(prop.floatValue * 100f) + "%";

                        if (prop.floatValue < 0) prop.floatValue = 0;

                        if (uberFadingType < 0.9f || !showFading)
                        {
                            continue;
                        } else if(uberFadingType < 3.9f && prop.floatValue > 1)
                        {
                            prop.floatValue = 1f;
                        }
                        break;
                    case ("Uber Mask"):
                        displayName.text = "Mask";

                        if (uberFadingType < 1.9f || uberFadingType > 2.9f || !showFading)
                        {
                            continue;
                        }
                        break;
                    case ("Uber Noise Scale"):
                        displayName.text = "Noise Scale";

                        if (uberFadingType < 2.9f || !showFading)
                        {
                            continue;
                        }
                        break;
                    case ("Uber Space"):
                        displayName.text = "Space";

                        if (uberFadingType < 2.9f || !showFading)
                        {
                            continue;
                        }
                        break;
                    case ("Uber Width"):
                        displayName.text = "Width";

                        if (uberFadingType < 2.9f || !showFading)
                        {
                            continue;
                        }
                        break;
                    case ("Uber Noise Factor"):
                        displayName.text = "Noise Factor";

                        if (uberFadingType < 3.9f || !showFading)
                        {
                            continue;
                        }
                        break;
                    case ("Uber Position"):
                        displayName.text = "Position";

                        if (uberFadingType < 3.9f || !showFading)
                        {
                            continue;
                        }
                        break;
                    case ("Pixel snap"):
                        if(showBonus == false)
                        {
                            continue;
                        }
                        break;
                    case ("Is Text"):
                        if (showBonus == false)
                        {
                            continue;
                        }
                        break;
                    case ("Normal Map"):
                        if (showLitSettings == false)
                        {
                            continue;
                        }
                        break;
                    case ("Normal Intensity"):
                        if (showLitSettings == false)
                        {
                            continue;
                        }
                        break;
                    case ("Smoothness"):
                        if (showLitSettings == false && prop.name == "_Smoothness")
                        {
                            continue;
                        }
                        break;
                    case ("Smoothness Map"):
                        if (showLitSettings == false)
                        {
                            continue;
                        }
                        break;
                    case ("Has Emission"):
                        if (showLitSettings == false)
                        {
                            continue;
                        }
                        else
                        {
                            hasEmission = prop.floatValue;
                        }
                        break;
                    case ("Emission Color"):
                        if (showLitSettings == false || hasEmission < 0.5f)
                        {
                            continue;
                        }
                        break;
                    case ("Emission Map"):
                        if (showLitSettings == false || hasEmission < 0.5f)
                        {
                            continue;
                        }
                        break;
                    case ("Pixels Per Unit"):
                        if (showSpaceSettings == false || (spaceType > 0.9f && spaceType < 4.9f) || spaceType > 5.9f)
                        {
                            continue;
                        }
                        break;
                    case ("Rect Width"):
                        if (showSpaceSettings == false || spaceType < 4.9f || spaceType > 5.9f)
                        {
                            continue;
                        }
                        break;
                    case ("Rect Height"):
                        if (showSpaceSettings == false || spaceType < 4.9f || spaceType > 5.9f)
                        {
                            continue;
                        }
                        break;
                    case ("Screen Width Units"):
                        if (showSpaceSettings == false || spaceType < 5.9f)
                        {
                            continue;
                        }
                        break;
                    case ("Time Scale"):
                        if(showTimeSettings == false || timeType < 0.5f || timeType > 2.5f)
                        {
                            continue;
                        }
                        break;
                    case ("Time Frequency"):
                        if (showTimeSettings == false || timeType < 2.5f || timeType > 4.5f)
                        {
                            continue;
                        }
                        break;
                    case ("Time Range"):
                        if (showTimeSettings == false || timeType < 2.5f || timeType > 4.5f)
                        {
                            continue;
                        }
                        break;
                    case ("Time FPS"):
                        if (showTimeSettings == false || timeType < 1.5f || timeType > 4.5f || (timeType > 2.5f && timeType < 3.5f))
                        {
                            continue;
                        }
                        break;
                    case ("Time Value"):
                        if (showTimeSettings == false || timeType < 4.5f)
                        {
                            continue;
                        }
                        break;
                }
                if (prop.name == "")
                {
                    displayName.text = "Texture";
                }

                //Min 0:
                if (trueName.EndsWith("ontrast") || trueName.EndsWith("rightness") || trueName.EndsWith("Saturation"))
                {
                    if (prop.floatValue < 0) prop.floatValue = 0;
                    displayName.text = trueName + " " + Mathf.RoundToInt(prop.floatValue * 100f) + "%";
                }
                if (trueName.EndsWith("Width"))
                {
                    if (prop.floatValue < 0) prop.floatValue = 0;
                }

                //Display Property:
                displayName.tooltip = prop.name + "  (C#)";
                EditorGUILayout.BeginHorizontal();

                ShaderUtil.ShaderPropertyType propType = ShaderUtil.ShaderPropertyType.TexEnv;
                if(ShaderUtil.GetPropertyCount(shader) >= properties.Length && prop == properties[n])
                {
                    propType = ShaderUtil.GetPropertyType(shader, n + startIndex);
                }

                //Special Display:
                if (trueName == "Space" || trueName == "Uber Space")
                {
                    EditorGUILayout.PrefixLabel("Space");
                    prop.floatValue = EditorGUILayout.IntPopup((int) (prop.floatValue), new string[] { "UV Space", "Local Space", "World Space" }, new int[] {0,1,2});
                }else if(trueName == "Flip")
                {
                    EditorGUILayout.PrefixLabel("Flip");
                    prop.floatValue = EditorGUILayout.IntPopup((int)(prop.floatValue), new string[] { "Default", "Flipped" }, new int[] { 0, -1});
                }else if(propType == ShaderUtil.ShaderPropertyType.Vector)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel(displayName);

                    Vector4 value = prop.vectorValue;
                    float oldWidth = EditorGUIUtility.labelWidth;
                    EditorGUIUtility.labelWidth = 15f;
                    value.x = EditorGUILayout.FloatField(" X", value.x);
                    value.y = EditorGUILayout.FloatField(" Y", value.y);
                    value.z = value.w = 0;
                    EditorGUILayout.EndHorizontal();
                    EditorGUIUtility.labelWidth = oldWidth;

                    prop.vectorValue = value;
                } else if (propType == ShaderUtil.ShaderPropertyType.TexEnv)
                {
                    prop.textureValue = (Texture) EditorGUILayout.ObjectField(displayName, prop.textureValue, typeof(Texture), false);
                }else if(trueName == "Full Fade" && uberFadingType > 3.9f)
                {
                    displayName.text = "Spread Distance";
                    prop.floatValue = EditorGUILayout.FloatField(displayName, prop.floatValue);
                }
                else
                {
                    materialEditor.ShaderProperty(prop, displayName);
                }

                #region Reset
                if(propType != ShaderUtil.ShaderPropertyType.TexEnv && trueName.EndsWith("Toggle") == false && trueName != "Shader Space" && trueName != "Has Emission" && trueName != "Uber Fading" && trueName != "Time Settings" && prop.name != "_WindIsParallax")
                {
                    GUIContent resetButton = new GUIContent();
                    resetButton.text = "R";
                    resetButton.tooltip = "Resets the property.";

                    if (propType == ShaderUtil.ShaderPropertyType.Vector) //Vector:
                    {
                        Vector4 defaultValue = defaultVectors[n + startIndex];

                        if (prop.vectorValue == defaultValue)
                        {
                            GUI.color = new Color(1, 1, 1, 0.5f);
                            GUILayout.Toolbar(0, new GUIContent[] { resetButton }, GUILayout.Width(20));
                        }
                        else
                        {
                            if (GUILayout.Button(resetButton, GUILayout.Width(20)))
                            {
                                prop.vectorValue = defaultValue;
                            }
                        }
                    }
                    else if (propType == ShaderUtil.ShaderPropertyType.Color) //Color:
                    {
                        Color defaultValue = defaultColors[n + startIndex];

                        if (prop.colorValue == defaultValue)
                        {
                            GUI.color = new Color(1, 1, 1, 0.5f);
                            GUILayout.Toolbar(0, new GUIContent[] { resetButton }, GUILayout.Width(20));
                        }
                        else
                        {
                            if (GUILayout.Button(resetButton, GUILayout.Width(20)))
                            {
                                prop.colorValue = defaultValue;
                            }
                        }
                    }
                    else
                    {
                        float defaultValue = defaultFloats[n + startIndex];

                        if (prop.floatValue == defaultValue)
                        {
                            GUI.color = new Color(1, 1, 1, 0.5f);
                            GUILayout.Toolbar(0, new GUIContent[] { resetButton }, GUILayout.Width(20));
                        }
                        else
                        {
                            if (GUILayout.Button(resetButton, GUILayout.Width(20)))
                            {
                                prop.floatValue = defaultValue;
                            }
                        }
                    }
                }
                GUI.color = new Color(1, 1, 1, 1f);
                #endregion

                EditorGUILayout.EndHorizontal();

                //Information:
                string suffix = GetInformation(trueName, info);

                if(uberFadingType > 0)
                {
                    if(uberFadingType < 1.1f && trueName == "Full Fade")
                    {
                        suffix = "- Fades all shaders <b>uniformly</b>.";
                    }else if(uberFadingType < 2.1f && trueName == "Uber Mask")
                    {
                        suffix = "- Fades all shaders using a <b>grayscale mask</b>.";
                    }else if(uberFadingType < 3.1f && trueName == "Uber Width")
                    {
                        suffix = "- Fades all shaders using a <b>dissolve</b> pattern.";
                    }
                    else if (uberFadingType < 4.1f && trueName == "Uber Width")
                    {
                        suffix = "- Fades all shaders by <b>spreading</b> from a source position.";
                    }
                }else if(trueName == "Uber Fading")
                {
                    suffix = "- Feature is <b>disabled</b>.";
                }

                if(timeType > 0)
                {
                    if(timeType < 1.5f && trueName == "Time Scale")
                    {
                        suffix = "- Sets the <b>speed</b> of all shaders.";
                    }else if(timeType < 2.5f && trueName == "Time FPS")
                    {
                        suffix = "- Sets the <b>frames per second</b> of all shaders.";
                    }
                    else if (timeType < 3.5f && trueName == "Time Range")
                    {
                        suffix = "- Animates shaders back and forth using a <b>sinus wave</b>.";
                    }
                    else if (timeType < 4.5f && trueName == "Time FPS")
                    {
                        suffix = "- Animates shaders using a <b>sinus wave</b> and sets <b>FPS</b>.";
                    }
                    else if (timeType < 5.5f && trueName == "Time Value")
                    {
                        suffix = "- Allows you to set a <b>custom</b> time value.";
                    }
                }
                else if(trueName == "Time Settings")
                {
                    suffix = "- Feature is <b>disabled</b>.";
                }

                bool linesAfterSuffix = false;
                if(prop.name == "_ShaderSpace")
                {
                    if(spaceType < 0.9f)
                    {
                        suffix = "- The position within the sprite's texture." + "\n" +
                                 "- May not work properly with <b>texture atlases</b>." + "\n" +
                                 "- Adjusts to the texture's pixel-size.";
                        linesAfterSuffix = true;
                    }
                    else if (spaceType < 1.9f)
                    {
                        suffix = "- The raw position within the sprite's texture." + "\n" +
                                 "- May not work properly with <b>texture atlases</b>.";
                    }
                    else if (spaceType < 2.9f)
                    {
                        suffix = "- The local position relative to the object." + "\n" +
                                 "- Requires a unique material instance per object.";
                    }
                    else if (spaceType < 3.9f)
                    {
                        suffix = "- The local position relative to the object." + "\n" +
                                 "- Requires a unique material instance per object." + "\n" +
                                 "- Adjusts to the object's scale.";
                    }
                    else if (spaceType < 4.9f)
                    {
                        suffix = "- The position in world-space.";
                    }
                    else if (spaceType < 5.9f)
                    {
                        suffix = "- Use this for <b>UI</b> elements." + "\n" + 
                                 "- Please <b>fill in</b> the RectTransform's <b>Width</b> and <b>Height</b>.";
                        linesAfterSuffix = true;
                    }
                    else if (spaceType < 6.9f)
                    {
                        suffix = "- Use this for fading images accross the entire screen." + "\n" +
                                 "- This will look a bit weird in the scene view.";
                        linesAfterSuffix = true;
                    }
                }

                if (suffix != "" && startIndex <= 0)
                {
                    GUI.color = new Color(1, 1, 1, 0.7f);

                    string[] descriptionLines = suffix.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.None);
                    for (int x = 0; x < descriptionLines.Length; x++)
                    {
                        EditorGUILayout.LabelField(descriptionLines[x], style);
                    }

                    GUI.color = Color.white;
                }

                //Suffix Lines:
                if (linesAfterSuffix)
                {
                    Lines();
                }
            }

            if (verticals == 1)
            {
                if(startIndex <= 0)
                {
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }
                verticals--;
            }
        }

        public static void DisplayFinalInformation()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("Helpbox");

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;
            style.alignment = TextAnchor.MiddleLeft;

            GUIStyle linkStyle = new GUIStyle(GUI.skin.label);
            linkStyle.richText = true;
            linkStyle.alignment = TextAnchor.MiddleLeft;
            linkStyle.normal.textColor = linkStyle.focused.textColor = linkStyle.hover.textColor = EditorGUIUtility.isProSkin ? new Color(0.8f, 0.9f, 1f, 1) : new Color(0.1f, 0.2f, 0.4f, 1);
            linkStyle.active.textColor = EditorGUIUtility.isProSkin ? new Color(0.6f, 0.8f, 1f, 1) : new Color(0.15f, 0.4f, 0.6f, 1);

            EditorGUILayout.BeginHorizontal();
            GUI.color = new Color(1, 1, 1, 1);
            EditorGUILayout.LabelField("<b>Contact:</b>",style,GUILayout.Width(65));
            EditorGUILayout.SelectableLabel("<b>ekincantascontact@gmail.com</b>", linkStyle, GUILayout.Height(16));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUI.color = new Color(1, 1, 1, 1);
            EditorGUILayout.LabelField("<b>Website:</b>", style, GUILayout.Width(65));
            if(GUILayout.Button("<b>https://ekincantas.com/sprite-shaders-ultimate/</b>", linkStyle))
            {
                Application.OpenURL("https://ekincantas.com/sprite-shaders-ultimate");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            GUI.color = new Color(1, 1, 1, 0.75f);
            EditorGUILayout.LabelField("<b>Thank you for using my asset.</b>",style);
            EditorGUILayout.LabelField("<b>Reviews are appreciated.</b>", style);
            EditorGUILayout.EndVertical();
            GUI.color = new Color(1, 1, 1, 1);
        }

        public static string GetInformation(string displayName, ShaderInformation information)
        {
            if (displayName == "Sprite Texture" || displayName == "MainTex")
            {
                return "- Ignore this if you are using a <b>SpriteRenderer</b>.";
            }
            if (displayName == "Fade")
            {
                return "- Fades the shader effect.";
            }
            if (displayName == "Shader Mask")
            {
                return "- Grayscale texture to <b>mask</b> the <b>shader</b> effect.";
            }
            if (displayName == "Space")
            {
                return "<b>UV Space: </b> Position on the sprite's texture.\n" +
                       "<b>Local Space: </b> Relative to gameobject (local position).\n" +
                       "<b>World Space: </b> Position in world space.";
            }
            if(displayName == "Flip")
            {
                return "Flips the shader vertically.";
            }
            if (displayName == "Uber Noise Texture")
            {
                return "- Shared noise texture for various shaders.";
            }

            if (information != null && information.suffixInformation != null)
            {
                for (int n = 0; n < information.suffixInformation.Count; n++) {
                    if(information.suffixInformation[n].property == displayName)
                    {
                        return information.suffixInformation[n].information;
                    }
                }
            }

            return "";
        }

        public static string GetTitle(string displayName, ShaderInformation information)
        {
            switch(displayName)
            {
                case ("Sprite Texture"):
                    return "Main Texture";
                case ("MainTex"):
                    return "Main Texture";
                case ("Sprite Color"):
                    return "Bonus";
                case ("Mask Map"):
                    return "2D Lit Settings";
                case ("Fade"):
                    return "Shader Fading";
                case ("Shader Mask"):
                    return "Shader Masking";
                case ("Space"):
                    return "Shader Space";
                case ("Flip"):
                    return "Shader Flipping";
                case ("Uber Noise Texture"):
                    return "Uber Noise";
                case ("Uber Fading"):
                    return "Uber Fading";
                case ("Metallic"):
                    return "3D Lit Settings";
                case ("Shader Space"):
                    return "Shader Space";
                case ("Time Settings"):
                    return "Shader Time";
            }

            if (information != null && information.titles != null)
            {
                for(int n = 0; n< information.titles.Count; n++)
                {
                    if(information.titles[n].property == displayName)
                    {
                        return information.titles[n].title;
                    }
                }
            }

            return "";
        }

        public static void ToggleCategory(string title, ref bool toggleVariable, ref GUIStyle style)
        {
            GUIStyle button = new GUIStyle(GUI.skin.button);
            button.richText = true;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<b><size=13>" + title + "</size></b>", style);
            if (GUILayout.Button("<size=10>" + (toggleVariable ? "▼" : "▲") + "</size>", button, GUILayout.Width(20)))
            {
                toggleVariable = !toggleVariable;
            }
            EditorGUILayout.EndHorizontal();
        }

        public static string GetShaderName(string shaderName)
        {
            string[] strings = shaderName.Split('/');
            if (strings.Length > 1)
            {
                shaderName = strings[strings.Length - 1];
            }

            if (shaderName.EndsWith(" Lit"))
            {
                shaderName = shaderName.Replace(" Lit", "");
            }

            return shaderName;
        }

        public static void DisplayTroubleshooting()
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;

            GUIStyle button = new GUIStyle(GUI.skin.button);
            button.richText = true;

            EditorGUILayout.Space();

            GUI.color = new Color(1, 1, 1, EditorGUIUtility.isProSkin ? 1f : 0.4f);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUI.color = Color.white;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<size=13><b>Troubleshooting</b></size>", style);

            if (GUILayout.Button("<size=10>" + (hintTroubleshooting ? "▼" : "▲") + "</size>", button, GUILayout.Width(20)))
            {
                hintTroubleshooting = !hintTroubleshooting;
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();

            if (hintTroubleshooting)
            {
                //Glow:
                GUI.color = new Color(1, 1, 1, 0.5f);
                string label = "- Shaders do not glow.";
                if (troubleGlowing)
                {
                    GUI.color = new Color(1, 1, 1, 1f);
                    label = "<b>" + label.Substring(2) + "</b>";
                }
                EditorGUILayout.BeginVertical(GUI.skin.box);
                GUI.color = Color.white;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(label,style);
                if (GUILayout.Button("<size=10>" + (troubleGlowing ? "▼" : "▲") + "</size>", button, GUILayout.Width(20)))
                {
                    troubleGlowing = !troubleGlowing;
                }
                EditorGUILayout.EndHorizontal();
                if (troubleGlowing)
                {
                    GUI.color = new Color(1, 1, 1, 0.7f);
                    EditorGUILayout.LabelField("1. You need to enable <b>HDR</b>.",style);
                    EditorGUILayout.LabelField("<b>Standard:</b> Enable HDR in the graphics settings.", style);
                    EditorGUILayout.LabelField("<b>URP:</b> Enable HDR in the universal render pipeline asset.", style);
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("2. You need <b>Bloom</b> post processing.", style);
                    EditorGUILayout.LabelField("<b>Standard:</b> Get the post processing package (package manager).", style);
                    EditorGUILayout.LabelField("<b>URP/HDRP:</b> Use the volume component.", style);
                    GUI.color = Color.white;
                }
                EditorGUILayout.EndVertical();

                //Clipping:
                GUI.color = new Color(1, 1, 1, 0.5f);
                string label2 = "- Pixels clip out.";
                if (troublePixelClipping)
                {
                    GUI.color = new Color(1, 1, 1, 1f);
                    label2 = "<b>" + label2.Substring(2) + "</b>";
                }
                EditorGUILayout.BeginVertical(GUI.skin.box);
                GUI.color = Color.white;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(label2,style);
                if (GUILayout.Button("<size=10>" + (troublePixelClipping ? "▼" : "▲") + "</size>", button, GUILayout.Width(20)))
                {
                    troublePixelClipping = !troublePixelClipping;
                }
                EditorGUILayout.EndHorizontal();
                if (troublePixelClipping)
                {
                    GUI.color = new Color(1, 1, 1, 0.7f);
                    EditorGUILayout.LabelField("Some shaders move pixels in the UV map.", style);
                    EditorGUILayout.LabelField("Those pixels can move out of the sprites mesh.", style);
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("1. Set the <b>Mesh Type</b> to <b>Full Rect</b> in the textures import settings.", style);
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("2. If step 1 is not enough add empty <b>transparent space</b> to the texture.", style);
                    EditorGUILayout.LabelField("In a larger texture the pixels have more space to move.", style);
                    GUI.color = Color.white;
                }
                EditorGUILayout.EndVertical();

                //Demo Visuals:
                GUI.color = new Color(1, 1, 1, 0.5f);
                string label3 = "- Shaders don't look as showcased online.";
                if (troubleDemoVisuals)
                {
                    GUI.color = new Color(1, 1, 1, 1f);
                    label3 = "<b>" + label3.Substring(2) + "</b>";
                }
                EditorGUILayout.BeginVertical(GUI.skin.box);
                GUI.color = Color.white;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(label3, style);
                if (GUILayout.Button("<size=10>" + (troubleDemoVisuals ? "▼" : "▲") + "</size>", button, GUILayout.Width(20)))
                {
                    troubleDemoVisuals = !troubleDemoVisuals;
                }
                EditorGUILayout.EndHorizontal();
                if (troubleDemoVisuals)
                {
                    GUI.color = new Color(1, 1, 1, 0.7f);
                    EditorGUILayout.LabelField("I used <b>Linear</b> color space for my videos and images.", style);
                    EditorGUILayout.LabelField("In <b>Gamma</b> the shader's default settings can be too bright.", style);
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Either <b>tune down</b> the intensity of color properties.", style);
                    EditorGUILayout.LabelField("Or switch your project to <b>Linear</b> color space.", style);
                    GUI.color = Color.white;
                }
                EditorGUILayout.EndVertical();

                //Other:
                GUI.color = new Color(1, 1, 1, 0.5f);
                string label4 = "- Other issues that you might get.";
                if (troubleOther)
                {
                    GUI.color = new Color(1, 1, 1, 1f);
                    label4 = "<b>" + label4.Substring(2) + "</b>";
                }
                EditorGUILayout.BeginVertical(GUI.skin.box);
                GUI.color = Color.white;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(label4, style);
                if (GUILayout.Button("<size=10>" + (troubleOther ? "▼" : "▲") + "</size>", button, GUILayout.Width(20)))
                {
                    troubleOther = !troubleOther;
                }
                EditorGUILayout.EndHorizontal();
                if (troubleOther)
                {
                    GUI.color = new Color(1, 1, 1, 0.7f);
                    EditorGUILayout.LabelField("<b>1. Some shaders dont work well with sprite sheets.</b>", style);
                    EditorGUILayout.LabelField("Change <b>Shader Space</b> to <b>Local</b> if you have that option.", style);
                    EditorGUILayout.LabelField("Otherwise you may need to export your sprites in seperate files.", style);
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("<b>2. Some shaders require a transparent edge.</b>", style);
                    EditorGUILayout.LabelField("A tile texture that fills the entire sprite may not work with some shaders.", style);
                    EditorGUILayout.LabelField("Adding a 1 pixel transparent edge can fix the issue.", style);
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("<b>3. Some shaders require a unique material instance.</b>", style);
                    EditorGUILayout.LabelField("Change <b>Shader Space</b> from <b>Local</b> to <b>UV</b> if you can.", style);
                    EditorGUILayout.LabelField("Or have scripts which reference materials at runtime (to instance materials).", style);
                    EditorGUILayout.LabelField("Any runtime material references/changes will fix this issue.", style);
                    GUI.color = Color.white;
                }
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
        }

        //Hint:
        public static void DisplayPropertyHint()
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.richText = true;

            EditorGUILayout.LabelField(" ", GUILayout.Height(1));

            GUI.color = new Color(1, 1, 1, EditorGUIUtility.isProSkin ? 1f : 0.4f);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUI.color = Color.white;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<size=13><b>Changing Values with Code</b></size>", style);

            if (GUILayout.Button("<size=10>" + (hintPropertyID ? "▼" : "▲") + "</size>", buttonStyle, GUILayout.Width(20)))
            {
                hintPropertyID = !hintPropertyID;
            }

            EditorGUILayout.EndHorizontal();
            if (hintPropertyID)
            {
                GUI.color = new Color(1, 1, 1, 0.7f);
                EditorGUILayout.LabelField("Hover over properties to see their <b>internal name</b>.", style);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Save the property id using <b>Shader.PropertyToID(internalName);</b>.", style);
                EditorGUILayout.LabelField("Change values like this: <b>material.SetFloat(propertyID, float);</b>", style);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("You can also directly use the <b>internal name</b> but its less performant.", style);
                GUI.color = Color.white;
            }

            EditorGUILayout.EndVertical();
        }

        //Utility:
        public static void DisplayUtility(MaterialEditor mat)
        {
            EditorGUILayout.LabelField(" ", GUILayout.Height(1));

            GUI.color = new Color(1, 1, 1, EditorGUIUtility.isProSkin ? 1f : 0.4f);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUI.color = Color.white;

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.richText = true;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("<size=13><b>Shader Utilities</b></size>", style);

            if (GUILayout.Button("<size=10>" + (enableUtilities ? "▼" : "▲") + "</size>", buttonStyle, GUILayout.Width(20)))
            {
                enableUtilities = !enableUtilities;
            }

            EditorGUILayout.EndHorizontal();
            if (enableUtilities)
            {
                string plural = "s";
                if (mat.targets.Length <= 1)
                {
                    plural = "";
                }

                if (GUILayout.Button("Export as PNG"))
                {
                    ExportSprite(mat);
                }

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Reset Material" + plural))
                {
                    ResetMaterials(mat);
                }
                if (GUILayout.Button("Load Material" + plural))
                {
                    LoadMaterials(mat);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("New Clean Material"))
                {
                    CreateMaterial(mat, true);
                }
                if (GUILayout.Button("New Clone Material"))
                {
                    CreateMaterial(mat, false);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        public static void CreateMaterial(MaterialEditor materialEditor, bool clean)
        {
            Material mat = (Material)materialEditor.target;

            if(mat != null)
            {
                //New Material:
                Material newMaterial = clean ? new Material(mat.shader) : new Material(mat);

                //Get Paths:
                string oldPath = AssetDatabase.GetAssetPath(mat);
                if(oldPath == "")
                {
                    oldPath = Application.dataPath + "/";
                }

                string folderPath = "";
                string newPath = oldPath.Replace(".mat", "");
                string[] folders = oldPath.Split('/');
                for (int n = 0; n < folders.Length - 1; n++)
                {
                    folderPath += folders[n] + "/";
                }

                newPath = EditorUtility.SaveFilePanel("Save Texture", folderPath, "New Material", "mat");
                if(newPath != "")
                {
                    newPath = newPath.Substring(Application.dataPath.Length - 9);

                    while (newPath.Length > 1 && newPath.StartsWith("Assets") == false)
                    {
                        newPath = newPath.Substring(1);
                    }

                    if (newPath != "")
                    {
                        AssetDatabase.CreateAsset(newMaterial, newPath);
                    }
                }
            }
        }

        public static void ResetMaterials(MaterialEditor materialEditor)
        {
            Undo.RecordObjects(materialEditor.targets, "Reset Material(s)");

            foreach (Material mat in materialEditor.targets)
            {
                Material newMaterial = new Material(mat.shader);
                mat.CopyPropertiesFromMaterial(newMaterial);
                UnityEngine.Object.DestroyImmediate(newMaterial);
            }
        }

        public static void LoadMaterials(MaterialEditor materialEditor)
        {
            Undo.RecordObjects(materialEditor.targets, "Load Material(s)");

            //Get Paths:
            string oldPath = AssetDatabase.GetAssetPath((Material) materialEditor.target);
            if (oldPath == "")
            {
                oldPath = Application.dataPath + "/";
            }

            string folderPath = "";
            string newPath = oldPath.Replace(".mat", "");
            string[] folders = oldPath.Split('/');
            for (int n = 0; n < folders.Length - 1; n++)
            {
                folderPath += folders[n] + "/";
            }
            newPath = EditorUtility.OpenFilePanel("Save Texture", folderPath, "mat");
            if (newPath != "")
            {
                newPath = newPath.Substring(Application.dataPath.Length - 9);
                while (newPath.Length > 1 && newPath.StartsWith("Assets") == false)
                {
                    newPath = newPath.Substring(1);
                }

                if (newPath != "")
                {
                    Material loadMat = AssetDatabase.LoadAssetAtPath<Material>(newPath);

                    foreach (Material mat in materialEditor.targets)
                    {
                        if (loadMat != null && loadMat != mat)
                        {
                            if (mat.shader != loadMat.shader)
                            {
                                mat.shader = loadMat.shader;
                            }

                            mat.CopyPropertiesFromMaterial(loadMat);
                        }
                    }
                }
            }
        }

        public static void ExportSprite(MaterialEditor materialEditor)
        {
            Material material = (Material)materialEditor.target;
            Texture texture = null;
            if (Selection.activeGameObject != null)
            {
                SpriteRenderer sr = Selection.activeGameObject.GetComponent<SpriteRenderer>();
                if(sr != null)
                {
                    texture = sr.sprite.texture;
                }
                else
                {
                    Graphic graphic = Selection.activeGameObject.GetComponent<Graphic>();

                    if(graphic != null)
                    {
                        texture = graphic.mainTexture;
                    }
                }
            }

            if(texture == null)
            {
                texture = material.GetTexture("_MainTex");
            }

            if(material != null && texture != null)
            {
                SaveShadedTexture(material, texture);
            }
            else
            {
                EditorUtility.DisplayDialog("No texture found.", "Select a SpriteRenderer or Image with this material.\nOr assign a main texture to this material.", "Alright");
            }
        }
        public static void SaveShadedTexture(Material spriteMaterial, Texture spriteTexture)
        {
            //Blit to Render Texture:
            RenderTexture rt = new RenderTexture(spriteTexture.width, spriteTexture.height, 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(spriteTexture, rt, spriteMaterial);

            //Read RenderTexture to Texture:
            Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            tex.Apply();

            //Get Paths:
            string oldPath = AssetDatabase.GetAssetPath(spriteTexture);
            if (oldPath == "")
            {
                oldPath = Application.dataPath + "/";
            }

            string newPath = oldPath.Replace(".png", "");
            string folderPath = "";
            string[] folders = oldPath.Split('/');
            for (int n = 0; n < folders.Length - 1; n++)
            {
                folderPath += folders[n] + "/";
            }
            string fileName = spriteTexture.name;
            int number = 1;
            while (AssetDatabase.LoadAssetAtPath(newPath + " S" + number + ".png", typeof(Texture)) != null)
            {
                number++;
            }
            newPath = EditorUtility.SaveFilePanel("Save Texture", folderPath, spriteTexture.name + " S" + number, "png");

            if(newPath != "")
            {
                newPath = newPath.Substring(Application.dataPath.Length - 9);
                while (newPath.Length > 1 && newPath.StartsWith("Assets") == false)
                {
                    newPath = newPath.Substring(1);
                }

                //Save Texture:
                byte[] bytes = tex.EncodeToPNG();
                bool newFile = AssetDatabase.LoadAssetAtPath(newPath, typeof(Texture)) == null;
                if (newFile)
                {
                    AssetDatabase.CopyAsset(oldPath, newPath);
                }
                System.IO.File.WriteAllBytes(newPath, bytes);
                if (newFile)
                {
                    AssetDatabase.ImportAsset(newPath);
                }

                //Import new Texture:
                AssetDatabase.Refresh();

                //Ping:
                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(newPath, typeof(Texture)));
            }

            //Clean Up:
            UnityEngine.Object.DestroyImmediate(tex);
        }
    }

    public enum SpaceType
    {
        UV = 0
        ,
        LOCAL = 1
        ,
        WORLD = 2
    }
}
