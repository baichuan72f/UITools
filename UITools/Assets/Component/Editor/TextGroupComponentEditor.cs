using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextGroupComponent))]
public class TextGroupComponentEditor : Editor
{
    private TextGroupComponent Group;

    private void OnEnable()
    {
        Group = target as TextGroupComponent;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.PrefixLabel("template:");
        Group.template = EditorGUILayout.TextArea(Group.template);
        EditorGUILayout.EndVertical();
    }
}