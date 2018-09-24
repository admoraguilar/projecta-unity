using UnityEngine;
using System;


namespace WEngine {
    /// <summary>
    /// Makes a method run before the game loads.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RunBeforeGameLoadAttribute : Attribute {
        /// <summary>
        /// Used for manually ordering the call of the methods. Useful for having a method be called
        /// first than the others.
        /// </summary>
        public int Priority { get; set; }
    }


    /// <summary>
    /// Used for dependecy injection. Attach this attribute on public fields or properties,
    /// to get instances referenced in your game's Config object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | 
                    AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class InjectAttribute : Attribute {
        public bool IsInjectInstance { get; set; }
    }


    /// <summary>
    /// Fills the field with a dummy object if null.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field |
                    AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class NullFillableAttribute : Attribute {

    }
}
