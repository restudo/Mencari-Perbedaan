using UnityEngine;
using UnityEditor;

public class ConditionalFieldAttribute : PropertyAttribute
{
    public string FieldToCheck;
    public object[] CompareValues;

    public ConditionalFieldAttribute(string fieldToCheck, params object[] compareValues)
    {
        FieldToCheck = fieldToCheck;
        CompareValues = compareValues;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
public class ConditionalFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;
        bool enabled = IsFieldVisible(property, conditional);

        if (enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;
        bool enabled = IsFieldVisible(property, conditional);

        if (!enabled)
        {
            return 0f;
        }

        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    private bool IsFieldVisible(SerializedProperty property, ConditionalFieldAttribute conditional)
    {
        SerializedProperty sourceProperty = property.serializedObject.FindProperty(conditional.FieldToCheck);

        if (sourceProperty == null)
        {
            Debug.LogWarning($"Cannot find property '{conditional.FieldToCheck}' in '{property.serializedObject.targetObject.GetType()}'.");
            return true;
        }

        foreach (var value in conditional.CompareValues)
        {
            switch (sourceProperty.propertyType)
            {
                case SerializedPropertyType.Integer:
                    if (sourceProperty.intValue.Equals(value))
                        return true;
                    break;
                case SerializedPropertyType.Boolean:
                    if (sourceProperty.boolValue.Equals(value))
                        return true;
                    break;
                case SerializedPropertyType.Enum:
                    if (sourceProperty.enumValueIndex.Equals((int)value))
                        return true;
                    break;
                // add other types as needed
            }
        }

        return false;
    }
}
#endif
