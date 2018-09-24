using UnityEngine;
using UnityEditor;
using WEngine;


namespace WEditor {
    [CustomPropertyDrawer(typeof(InterfaceFieldAttribute))]
    public class InterfaceFieldAttributeDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // For referencing interfaces
            WEngine.InterfaceFieldAttribute attr = (InterfaceFieldAttribute)attribute;
            Component comp = (Component)EditorGUI.ObjectField(position, attr.Type.Name + "." + property.name, property.objectReferenceValue, typeof(Component), true);
            if(comp) {
                comp = comp.GetComponent(attr.Type);
                if(!comp) {
                    Debug.Log("Not a valid " + attr.Type.Name);
                }
            }
            property.objectReferenceValue = comp;
        }
    }
}