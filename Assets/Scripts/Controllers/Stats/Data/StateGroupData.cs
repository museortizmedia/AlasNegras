using System;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum StatType
{
    Damage,
    Health,
    Stamina,
    Speed,
    Defense,
}

[CreateAssetMenu(fileName = "New StatGroup", menuName = "Stats/Stats Asset")]
public class StatGroupData : ScriptableObject
{
    public StatsGroup[] statsGroup;
}

[Serializable]
public struct StatsGroup
{
    public StatType statType;
    public float initialValue;

    // Campos adicionales
    public float maxValue; // Para Health y Stamina
    public float regenRate; // Solo para Stamina
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(StatsGroup))]
public class StatsGroupDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Retrieve SerializedProperties
        SerializedProperty statType = property.FindPropertyRelative("statType");
        SerializedProperty initialValue = property.FindPropertyRelative("initialValue");
        SerializedProperty maxValue = property.FindPropertyRelative("maxValue");
        SerializedProperty regenRate = property.FindPropertyRelative("regenRate");

        EditorGUI.BeginProperty(position, label, property);

        float singleLineHeight = EditorGUIUtility.singleLineHeight;

        // Draw StatType field
        if (statType != null)
        {
            DrawField(ref position, statType, label, singleLineHeight);
        }

        // Draw InitialValue field
        if (initialValue != null)
        {
            DrawField(ref position, initialValue, new GUIContent("Initial Value"), singleLineHeight);
        }

        // Draw additional fields based on StatType
        if (statType != null)
        {
            StatType type = (StatType)statType.enumValueIndex;

            if (type == StatType.Health || type == StatType.Stamina)
            {
                if (maxValue != null)
                {
                    DrawField(ref position, maxValue, new GUIContent("Max Value"), singleLineHeight);
                }
            }

            if (type == StatType.Stamina)
            {
                if (regenRate != null)
                {
                    DrawField(ref position, regenRate, new GUIContent("Regen Rate"), singleLineHeight);
                }
            }
        }

        EditorGUI.EndProperty();
    }

    private void DrawField(ref Rect position, SerializedProperty property, GUIContent label, float fieldHeight = 0)
    {
        if (property != null)
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, fieldHeight), property, label);
            position.y += fieldHeight; // Move position for the next field
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Retrieve SerializedProperties
        SerializedProperty statType = property.FindPropertyRelative("statType");

        float singleLineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = 4f; // Spacing between fields
        float height = 0f;

        if (statType != null)
        {
            height += singleLineHeight + spacing; // For StatType field
            height += singleLineHeight + spacing; // For Initial Value field

            StatType type = (StatType)statType.enumValueIndex;

            if (type == StatType.Health || type == StatType.Stamina)
            {
                height += singleLineHeight + spacing; // For Max Value
            }

            if (type == StatType.Stamina)
            {
                height += singleLineHeight + spacing; // For Regen Rate
            }
        }

        return height;
    }
}
#endif
