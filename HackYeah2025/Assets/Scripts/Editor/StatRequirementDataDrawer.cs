using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StatRequirementData))]
public class StatRequirementDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var categoryProp = property.FindPropertyRelative("category");
        var minValueProp = property.FindPropertyRelative("minValue");
        var maxValueProp = property.FindPropertyRelative("maxValue");

        string newLabel = $"{categoryProp.enumDisplayNames[categoryProp.enumValueIndex]}: {minValueProp.intValue} - {maxValueProp.intValue}";
        label.text = newLabel;

        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
