using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(Optional<>))]
    public class OptionalPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var valueProperty = property.FindPropertyRelative("value");
            var enabledProperty = property.FindPropertyRelative("enabled");
            var positionProperty = property.FindPropertyRelative("position");

            float totalHeight = EditorGUI.GetPropertyHeight(enabledProperty);

            if (enabledProperty.boolValue)
            {
                totalHeight += EditorGUI.GetPropertyHeight(valueProperty) + EditorGUI.GetPropertyHeight(positionProperty) + EditorGUIUtility.standardVerticalSpacing * 2;
            }

            return totalHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var valueProperty = property.FindPropertyRelative("value");
            var enabledProperty = property.FindPropertyRelative("enabled");
            var positionProperty = property.FindPropertyRelative("position");

            EditorGUI.BeginProperty(position, label, property);
            position.width -= 24;

            EditorGUI.PropertyField(position, enabledProperty, label, true);
            position.y += EditorGUI.GetPropertyHeight(enabledProperty) + EditorGUIUtility.standardVerticalSpacing;

            if (enabledProperty.boolValue)
            {
                EditorGUI.indentLevel++;

                EditorGUI.PropertyField(position, valueProperty, true);
                position.y += EditorGUI.GetPropertyHeight(valueProperty) + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.PropertyField(position, positionProperty, true);

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }
    }


}
