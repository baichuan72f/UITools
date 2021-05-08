using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextGroupComponent))]
public class TextGroupComponentEditor : Editor
{
    private TextGroupComponent m_GroupIui;

    private void OnEnable()
    {
        m_GroupIui = target as TextGroupComponent;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.PrefixLabel("template:");
        m_GroupIui.template = EditorGUILayout.TextArea(m_GroupIui.template);
        EditorGUILayout.EndVertical();
    }
}