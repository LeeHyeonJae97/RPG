using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(ToggleButton))]
public class ToggleButtonEditor : SelectableEditor
{
    private SerializedProperty _toggleTransitionProp;
    private SerializedProperty _onColorProp;
    private SerializedProperty _offColorProp;
    private SerializedProperty _onSpriteProp;
    private SerializedProperty _offSpriteProp;
    private SerializedProperty _onStateChangedProp;
    private SerializedProperty _isOnOnStartProp;

    protected override void OnEnable()
    {
        base.OnEnable();

        _toggleTransitionProp = serializedObject.FindProperty("_toggleTransition");
        _onColorProp = serializedObject.FindProperty("_onColor");
        _offColorProp = serializedObject.FindProperty("_offColor");
        _onSpriteProp = serializedObject.FindProperty("_onSprite");
        _offSpriteProp = serializedObject.FindProperty("_offSprite");
        _onStateChangedProp = serializedObject.FindProperty("onStateChanged");
        _isOnOnStartProp = serializedObject.FindProperty("_isOnOnStart");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        GUILayout.Space(5);
        EditorGUILayout.PropertyField(_toggleTransitionProp);

        switch (_toggleTransitionProp.enumValueIndex)
        {
            
            // ColorTint
            case 0:
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_onColorProp);
                EditorGUILayout.PropertyField(_offColorProp);
                EditorGUI.indentLevel--;
                break;

            // SpriteSwap
            case 1:
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_onSpriteProp);
                EditorGUILayout.PropertyField(_offSpriteProp);
                EditorGUI.indentLevel--;
                break;
        }

        GUILayout.Space(5);
        EditorGUILayout.PropertyField(_onStateChangedProp);
        EditorGUILayout.PropertyField(_isOnOnStartProp);

        serializedObject.ApplyModifiedProperties();
    }
}
