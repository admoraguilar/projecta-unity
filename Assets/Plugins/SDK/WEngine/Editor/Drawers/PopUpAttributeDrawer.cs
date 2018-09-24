using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using WEngine;


namespace WEditor {
    // For enums
    [CustomPropertyDrawer(typeof(PopUpAttribute))]
    public class PopUpAttributeDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            PopUpAttribute attr = (PopUpAttribute)attribute;
            switch(property.type) {
                case "Enum":
                    string[] enumNames = property.enumNames;
                    for(int i = 0; i < enumNames.Length; ++i)
                        enumNames[i] = enumNames[i].Replace('_', '/');
                    property.enumValueIndex = EditorGUI.Popup(position, property.displayName, property.enumValueIndex, enumNames);
                    break;
                //case "string":
                //    if(property.stringValue == "") {
                //        // Get string names
                //        string[] strArray = ((string)attr.Obj).Split((char)attr.Obj2);

                //        // Make editor representation
                //        string[] strListEditor = attr.Obj3 != null ? strArray.Select(str => str.Replace(((char)attr.Obj3), '/')).ToArray() : strArray;
                //        int index = -1;
                //        int popUp = EditorGUI.Popup(position, property.name, index, strListEditor);
                //        property.stringValue = popUp >= 0 ? strArray[popUp] : "";
                //    } else {
                //        if(GUI.Button(position, property.name + ": " + property.stringValue)) {
                //            property.stringValue = "";
                //        }
                //    }
                //    break;
                case "string":
                    if(property.stringValue == "") {
                        Type type = (Type)attr.Obj;

                        // Get string names
                        List<string> ids = new List<string>();
                        foreach(FieldInfo fi in EditorUtils.GetPublicConstants(type)) {
                            if(fi.Name.Contains("Id")) {
                                ids.Add((string)fi.GetValue(null));
                            }
                        }

                        // Make editor representation
                        string[] idsEditor = ids.Select(id => id.Replace('.', '/')).ToArray();
                        int index = -1;
                        int popUp = EditorGUI.Popup(position, property.name, index, idsEditor);
                        property.stringValue = popUp >= 0 ? ids[popUp] : "";
                    } else {
                        if(GUI.Button(position, property.name + ": " + property.stringValue)) {
                            property.stringValue = "";
                        }
                    }
                    break;
            }
        }
    }
}