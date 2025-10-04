using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StatValuePair))]
public class StatValuePairDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var categoryProp = property.FindPropertyRelative("category");
        var valueProp = property.FindPropertyRelative("value");

        string newLabel = $"{categoryProp.enumDisplayNames[categoryProp.enumValueIndex]}: {valueProp.intValue}";
        label.text = newLabel;

        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
