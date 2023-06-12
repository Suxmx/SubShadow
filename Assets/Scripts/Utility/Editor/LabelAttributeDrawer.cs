using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

/// <summary>
/// 绘制Label特性
/// </summary>
[CustomPropertyDrawer(typeof(LabelAttribute))]
public class LabelAttributeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float baseHeight = base.GetPropertyHeight(property, label);
        if (property.isExpanded && property.propertyType == SerializedPropertyType.Generic)
            baseHeight += EditorGUIUtility.singleLineHeight * (property.CountInProperty() + 0.6f);
        return baseHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        LabelAttribute attr = attribute as LabelAttribute;
        label.text = attr.Name;

        if (property.propertyType == SerializedPropertyType.Enum)
        {
            DrawEnum(position, property, label);
        }
        else if (property.propertyType == SerializedPropertyType.Generic)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }

    }

    // 绘制枚举类型
    private void DrawEnum(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginChangeCheck();
        // 获取枚举相关属性
        Type type = fieldInfo.FieldType;
        string[] names = property.enumNames;

        // 获取枚举所对应的RenameAttribute
        for (int i = 0; i < names.Length; i++)
        {
            FieldInfo info = type.GetField(names[i]);
            //LabelAttribute[] atts = (LabelAttribute[])info.GetCustomAttributes(typeof(LabelAttribute), true);
            //if (atts.Length != 0) names[i] = atts[0].Name;
            LabelAttribute att = (LabelAttribute)info.GetCustomAttribute(typeof(LabelAttribute), true);
            if (att != null) names[i] = att.Name;
        }

        // 重绘GUI
        //FlagsAttribute[] flags = (FlagsAttribute[])type.GetCustomAttributes(typeof(FlagsAttribute), true);
        //if (flags.Length == 0)
        FlagsAttribute flag = (FlagsAttribute)type.GetCustomAttribute(typeof(FlagsAttribute), true);
        if (flag == null)
        {
            int index = EditorGUI.Popup(position, label.text, property.enumValueIndex, names);
            if (EditorGUI.EndChangeCheck() && index != -1) property.enumValueIndex = index;
        }
        else
        {
            int index = EditorGUI.MaskField(position, label.text, property.intValue, names);
            if (EditorGUI.EndChangeCheck()) property.intValue = index;
        }
    }
}
