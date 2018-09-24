using System;
using System.Text.RegularExpressions;
using UnityEngine;


public static class Utilities {
    public static bool IsEmailValid(string email) {
        string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
          + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
          + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        return regex.IsMatch(email);
    }

    public static T CreateDummy<T>() {
        return (T)CreateDummy(typeof(T));
    }

    public static object CreateDummy(Type type) {
        if(typeof(MonoBehaviour).IsAssignableFrom(type)) {
            MonoBehaviour mono = (MonoBehaviour)new GameObject().AddComponent(type);
            mono.GetComponent<Transform>().position = new Vector3(10000f, 10000f, 10000f);
            return mono;
        }

        if(typeof(ScriptableObject).IsAssignableFrom(type)) {
            return ScriptableObject.CreateInstance(type);
        }

        if(typeof(object).IsAssignableFrom(type)) {
            return Activator.CreateInstance(type);
        }

        return null;
    }
}