using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WEngine {
    /// <summary>
    /// For displaying a pretty note in your inspectors.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class InspectorNoteAttribute : PropertyAttribute {
        public InspectorNoteAttribute(string header, string message = "") {
            Header = header;
            Message = message;
        }

        public readonly string Header;
        public readonly string Message;
    }

    
    /// <summary>
    /// For referencing interface types in your editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class InterfaceFieldAttribute : PropertyAttribute {
        public InterfaceFieldAttribute(Type type) {
            Type = type;
        }

        public readonly Type Type;
    }


    /// <summary>
    /// Mostly for a prettier pop-ups, refer to PopUpAttributeDrawer to see clearer
    /// usage.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class PopUpAttribute : PropertyAttribute {
        public PopUpAttribute() { }
        public PopUpAttribute(object obj) {
            Obj = obj;
        }
        public PopUpAttribute(object obj, object obj2) {
            Obj = obj;
            Obj2 = obj2;
        }
        public PopUpAttribute(object obj, object obj2, object obj3) {
            Obj = obj;
            Obj2 = obj2;
            Obj3 = obj3;
        }

        public readonly object Obj;
        public readonly object Obj2;
        public readonly object Obj3;
    }


    /// <summary>
    /// Use for easily referencing objects such as SceneAsset in your editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ObjectFieldAttribute : PropertyAttribute {
        public ObjectFieldType Type;

        public ObjectFieldAttribute(ObjectFieldType type) {
            Type = type;
        }
    }

    public enum ObjectFieldType {
        SceneAsset,
    }
}