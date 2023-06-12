using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SpriteShadersUltimate
{
    [CustomEditor(typeof(MaterialInstancer))]
    public class MaterialInstancerEditor : Editor
    {
        MaterialInstancer materialInstancer;

        [ExecuteAlways]
        private void Awake()
        {
            materialInstancer = (MaterialInstancer)target;
        }

        public override void OnInspectorGUI()
        {
            GUIStyle style = new GUIStyle();
            style.richText = true;
            EditorGUILayout.LabelField("<size=15><b>Material Instancer (Experimental)</b></size>", style);

            EditorGUILayout.Space();

            if (materialInstancer != null && materialInstancer.HasMaterial() == false)
            {
                GUI.color = new Color(1, 0.7f, 0.7f, 1f);
                EditorGUILayout.BeginVertical("Helpbox");
                GUI.color = new Color(1, 1, 1, 1);
                EditorGUILayout.LabelField("Requires a gameobject with a <b>Renderer</b> or <b>Graphic</b> component.", style);
                EditorGUILayout.LabelField("And the gameobject should be <b>active</b> in the hierarchy.", style);
                EditorGUILayout.EndVertical();
                return;
            }

            //Info:
            EditorGUILayout.BeginVertical("Helpbox");
            EditorGUILayout.LabelField("<b>I do not recommend using this script.</b>", style);
            EditorGUILayout.LabelField("<b>It can cause bugs and I just use it for my assets demo scene.</b>", style);
            GUI.color = new Color(1, 1, 1, 0.7f);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("This script <b>saves</b> and <b>loads</b> materials.", style);
            EditorGUILayout.LabelField("While the gameobject is <b>active</b> in the hierarchy.", style);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("This will not work well with Uber Shaders.", style);
            EditorGUILayout.LabelField("And prefabs will appear purple in the project folder.", style);
            EditorGUILayout.LabelField("Use this only in scenes, avoid using this on prefabs.", style);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("<b>For Programmers:</b>", style);
            EditorGUILayout.LabelField("<b>Loading</b> happens in <b>void Awake()</b>.", style);
            EditorGUILayout.LabelField("Use <b>void Start()</b> when referencing the material.", style);
            EditorGUILayout.EndVertical();
            GUI.color = new Color(1, 1, 1, 1f);

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("Helpbox");
            EditorGUILayout.LabelField("<b>Saved Data:</b>", style);
            GUI.color = new Color(1, 1, 1, 0.7f);

            //Properties:
            int savedProperties = 0;
            if(materialInstancer.properties != null)
            {
                savedProperties = materialInstancer.properties.Count;
            }
            EditorGUILayout.LabelField("Properties: <b>[" + savedProperties + "]</b>", style);

            //Shader:
            string shaderName = "<i>missing</i>";
            if(materialInstancer.shader != null)
            {
                shaderName = materialInstancer.shader.name;
            }
            EditorGUILayout.LabelField("Shader: <b>[" + shaderName + "]</b>", style);

            //Original Material:
            string originalMaterial = "<i>missing</i>";
            if(materialInstancer.original != null)
            {
                originalMaterial = materialInstancer.original.name;
            }

            EditorGUILayout.LabelField("Original Material: <b>[" + originalMaterial + "]</b>", style);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            GUI.color = new Color(1, 1, 1, 1f);
            EditorGUILayout.BeginVertical("Helpbox");
            EditorGUILayout.LabelField("<b>Tools:</b>", style);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Manual Load"))
            {
                materialInstancer.Load();
            }
            if (GUILayout.Button("Remove"))
            {
                materialInstancer.Remove();
            }
            if (materialInstancer.original != null && GUILayout.Button("Original Material"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = materialInstancer.original;
                EditorGUIUtility.PingObject(materialInstancer.original);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            GUI.color = new Color(1, 1, 1, 1f);
        }
    }
}