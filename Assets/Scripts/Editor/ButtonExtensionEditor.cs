using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using Extension;

[CanEditMultipleObjects]
[CustomEditor(typeof(ButtonExtension))]
public class ButtonExtensionEditor : ButtonEditor
{
	private SerializedProperty _onDownProp;
	private SerializedProperty _onUpProp;
	private SerializedProperty _longPressProp;
	private SerializedProperty _pressedTimeForLongPressProp;
	private SerializedProperty _onEnterLongPressProp;
	private SerializedProperty _onLongPressProp;
	private SerializedProperty _onExitLongPressProp;
	private SerializedProperty _doubleClickProp;
	private SerializedProperty _clickIntervalForDoubleClickProp;
	private SerializedProperty _onDoubleClickProp;

	protected override void OnEnable()
	{
		base.OnEnable();

		_onDownProp = serializedObject.FindProperty("onDown");
		_onUpProp = serializedObject.FindProperty("onUp");
		_longPressProp = serializedObject.FindProperty("_longPress");
		_pressedTimeForLongPressProp = serializedObject.FindProperty("_pressedTimeForLongPress");
		_onEnterLongPressProp = serializedObject.FindProperty("onEnterLongPress");
		_onLongPressProp = serializedObject.FindProperty("onLongPress");
		_onExitLongPressProp = serializedObject.FindProperty("onExitLongPress");
		_doubleClickProp = serializedObject.FindProperty("_doubleClick");
		_clickIntervalForDoubleClickProp = serializedObject.FindProperty("_clickIntervalForDoubleClick");
		_onDoubleClickProp = serializedObject.FindProperty("onDoubleClick");
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		serializedObject.Update();

		EditorGUILayout.PropertyField(_onDownProp);
		EditorGUILayout.PropertyField(_onUpProp);
		EditorGUILayout.PropertyField(_longPressProp);
		if (_longPressProp.boolValue)
		{
			EditorGUILayout.PropertyField(_pressedTimeForLongPressProp);
			EditorGUILayout.Space(10);
			EditorGUILayout.PropertyField(_onEnterLongPressProp);
			EditorGUILayout.PropertyField(_onLongPressProp);
			EditorGUILayout.PropertyField(_onExitLongPressProp);
		}
		EditorGUILayout.PropertyField(_doubleClickProp);
		if (_doubleClickProp.boolValue)
		{
			EditorGUILayout.PropertyField(_clickIntervalForDoubleClickProp);
			EditorGUILayout.Space(10);
			EditorGUILayout.PropertyField(_onDoubleClickProp);
		}

		serializedObject.ApplyModifiedProperties();
	}
}
