using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


public static class EditorUtils {
    public static IEnumerable<FieldInfo> GetPublicConstants(Type type) {
        return type
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
            .Concat(type.GetNestedTypes(BindingFlags.Public).SelectMany(GetPublicConstants));
    }
}
