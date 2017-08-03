//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2016 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
#if UNITY_3_5
[CustomEditor(typeof(UIProgressBar))]
#else
[CustomEditor(typeof(UIProgressBar), true)]
#endif
public class UIProgressBarEditor : UIWidgetContainerEditor
{
	public override void OnInspectorGUI ()
	{
		NGUIEditorTools.SetLabelWidth(80f);

		serializedObject.Update();

		GUILayout.Space(3f);

		DrawLegacyFields();

		GUILayout.BeginHorizontal();
		var sp = NGUIEditorTools.DrawProperty("Steps", serializedObject, "numberOfSteps", GUILayout.Width(110f));
		if (sp.intValue == 0) GUILayout.Label("= unlimited");
		GUILayout.EndHorizontal();

		OnDrawExtraFields();

		if (NGUIEditorTools.DrawHeader("Appearance", "Appearance", false, true))
		{
			NGUIEditorTools.BeginContents(true);
			NGUIEditorTools.DrawProperty("Foreground", serializedObject, "mFG");
			NGUIEditorTools.DrawProperty("Background", serializedObject, "mBG");
			NGUIEditorTools.DrawProperty("Thumb", serializedObject, "thumb");

			GUILayout.BeginHorizontal();
			NGUIEditorTools.DrawProperty("Direction", serializedObject, "mFill");
			NGUIEditorTools.DrawPadding();
			GUILayout.EndHorizontal();

			OnDrawAppearance();
			NGUIEditorTools.EndContents();
		}

		var sb = target as UIProgressBar;
		NGUIEditorTools.DrawEvents("On Value Change", sb, sb.onChange);
		serializedObject.ApplyModifiedProperties();
	}

	protected virtual void DrawLegacyFields()
	{
		var sb = target as UIProgressBar;
		var val = EditorGUILayout.Slider("Value", sb.value, 0f, 1f);
		var alpha = EditorGUILayout.Slider("Alpha", sb.alpha, 0f, 1f);

		if (sb.value != val ||
			sb.alpha != alpha)
		{
			NGUIEditorTools.RegisterUndo("Progress Bar Change", sb);
			sb.value = val;
			sb.alpha = alpha;
			NGUITools.SetDirty(sb);

			for (var i = 0; i < UIScrollView.list.size; ++i)
			{
				var sv = UIScrollView.list[i];

				if (sv.horizontalScrollBar == sb || sv.verticalScrollBar == sb)
				{
					NGUIEditorTools.RegisterUndo("Progress Bar Change", sv);
					sv.UpdatePosition();
				}
			}
		}
	}

	protected virtual void OnDrawExtraFields () { }
	protected virtual void OnDrawAppearance () { }
}
