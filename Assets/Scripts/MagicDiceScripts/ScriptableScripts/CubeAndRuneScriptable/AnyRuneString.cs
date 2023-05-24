using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScriptableScripts
{
    [Serializable]
    public class AnyRuneString
    {
        [AnyRuneStringAttribute]
        public string RuneName;
    }
    
    
    
    public class AnyRuneStringAttribute : PropertyAttribute
    {
        
    }
    
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AnyRuneStringAttribute))]
    public class SkillTypeNameDrawer : PropertyDrawer
    {
        private readonly List<string> _stringList = Runes.GetRunesNamesList();
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_stringList.Count!= 0)
            {
                int selectedIndex= Mathf.Max(_stringList.IndexOf(property.stringValue), 0);
                selectedIndex = EditorGUI.Popup(position, selectedIndex, _stringList.ToArray());

                property.stringValue = _stringList[selectedIndex];
                
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
    #endif
}