//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenVolume))]
public class TweenVolumeEditor : UITweenerEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
		NGUIEditorTools.SetLabelWidth(120f);

		var tw = target as TweenVolume;
		GUI.changed = false;

		var from = EditorGUILayout.Slider("From", tw.from, 0f, 1f);
		var to = EditorGUILayout.Slider("To", tw.to, 0f, 1f);

		if (GUI.changed)
		{
			NGUIEditorTools.RegisterUndo("Tween Change", tw);
			tw.from = from;
			tw.to = to;
			NGUITools.SetDirty(tw);
		}

		DrawCommonProperties();
	}
}
