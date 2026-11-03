using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace Emp37.Utility.Editor
{
	using static ReflectionUtility;

	internal class Emp37Editor : UnityEditor.Editor
	{
		private Type targetType;

		private bool showDefaultProperty;
		private SerializedProperty defaultProperty;

		private NoteAttribute[] notes;
		private (SerializedProperty, FieldInfo field)[] properties;
		private (MethodInfo, ButtonAttribute)[] buttons;

		private bool isHorizontalLayoutActive;

		private int[] propertyGroup, propertyTab;
		private string[][] groupTabNames;
		private int[] groupSelection;


		private void OnEnable()
		{
			targetType = target.GetType();

			showDefaultProperty = !HasAttribute<HideDefaultPropertyAttribute>(targetType);
			notes = GetAttributes<NoteAttribute>(targetType, true);

			#region I N I T I A L I Z E   S E R I A L I Z E D   P R O P E R T I E S
			List<(SerializedProperty, FieldInfo)> propertyList = new();
			SerializedProperty iterator = serializedObject.GetIterator();

			if (iterator.NextVisible(true))
			{
				defaultProperty = iterator.Copy();
				while (iterator.NextVisible(false))
				{
					SerializedProperty property = iterator.Copy();
					if (property.GetField() is { } field) propertyList.Add((property, field));
				}
			}
			iterator.Dispose();

			properties = propertyList.ToArray();
			#endregion

			#region I N I T I A L I Z E   T A B S
			int length = properties.Length;
			propertyGroup = new int[length];
			propertyTab = new int[length];
			List<List<string>> groups = new();
			int group = -1, tab = -1;

			for (int i = 0; i < length; i++)
			{
				if (TryGetAttribute(properties[i].field, out TabGroupAttribute tabGroup))
				{
					if (string.IsNullOrEmpty(tabGroup.Name)) group = -1;
					else
					{
						if (group < 0)
						{
							groups.Add(new());
							group = groups.Count - 1;
						}
						tab = groups[group].IndexOf(tabGroup.Name);
						if (tab < 0)
						{
							groups[group].Add(tabGroup.Name);
							tab = groups[group].Count - 1;
						}
					}
				}
				propertyGroup[i] = group;
				propertyTab[i] = group < 0 ? -1 : tab;
			}
			groupTabNames = groups.ConvertAll(names => names.ToArray()).ToArray();
			groupSelection = new int[groups.Count];
			#endregion

			#region I N I T I A L I Z E   S E R I A L I Z E D   M E T H O D S
			List<(MethodInfo, ButtonAttribute)> buttonList = new();
			foreach (MethodInfo method in targetType.GetMethods(DEFAULT_FLAGS))
			{
				if (GetAttribute<ButtonAttribute>(method, true) is { } button) buttonList.Add((method, button));
			}
			buttons = buttonList.ToArray();
			#endregion
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			#region N O T E S
			if (notes.Length > 0)
			{
				foreach (NoteAttribute note in notes)
				{
					using (new EditorGUIHelper.BackgroundColorScope(note.Color))
					{
						EditorGUILayout.HelpBox(note.Content);
					}
				}
			}
			#endregion

			#region D R A W   D E F A U L T   P R O P E R T Y
			if (showDefaultProperty && defaultProperty != null)
			{
				using (new EditorGUI.DisabledScope(true))
				{
					EditorGUILayout.PropertyField(defaultProperty);
				}
			}
			#endregion

			#region D R A W   S E R I A L I Z E D   P R O P E R T I E S
			for (int i = 0; i < properties.Length; i++)
			{
				(SerializedProperty property, FieldInfo field) = properties[i];
				int groupIdx = propertyGroup[i];
				if (groupIdx >= 0)
				{
					if (i == 0 || propertyGroup[i - 1] != groupIdx)
					{
						EndActiveHorizontalGroup();
						groupSelection[groupIdx] = GUILayout.Toolbar(groupSelection[groupIdx], groupTabNames[groupIdx]);
					}
					if (propertyTab[i] != groupSelection[groupIdx]) continue; // skip fields in an unselected tab
				}
				if (!EvaluateVisibility(field)) continue;
				EvaluateGroup(field);
				using (new EditorGUI.DisabledScope(!EvaluateEnabled(field)))
				{
					EditorGUILayout.PropertyField(property, true);
				}
			}
			EndActiveHorizontalGroup();
			#endregion

			#region D R A W   S E R I A L I Z E D   M E TH O D S
			foreach ((MethodInfo method, ButtonAttribute button) in buttons)
			{
				if (!EvaluateVisibility(method)) continue;
				EvaluateGroup(method);
				using (new EditorGUI.DisabledScope(!EvaluateEnabled(method)))
				using (new EditorGUIHelper.BackgroundColorScope(button.BackgroundColor))
				{
					if (GUILayout.Button(button.Name ?? method.Name.ToTitleCase(), GUILayout.ExpandWidth(true), GUILayout.Height(button.Height))) AutoInvokeMethod(method, target, button.Parameters);
				}
			}
			EndActiveHorizontalGroup();
			#endregion

			serializedObject.ApplyModifiedProperties();
		}

		private void EvaluateGroup(ICustomAttributeProvider provider)
		{
			if (TryGetAttribute(provider, out HorizontalAttribute horizontal))
			{
				if (horizontal.Value)
				{
					if (isHorizontalLayoutActive) EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
					isHorizontalLayoutActive = true;
				}
				else
				{
					EndActiveHorizontalGroup();
				}
			}
		}
		private void EndActiveHorizontalGroup()
		{
			if (!isHorizontalLayoutActive) return;
			EditorGUILayout.EndHorizontal();
			isHorizontalLayoutActive = false;
		}
		private bool EvaluateVisibility(ICustomAttributeProvider provider)
		{
			bool output = true;
			if (TryGetAttribute(provider, out ShowIfAttribute a0, true))
				output &= TryReadMember(a0.Condition, target, out object obj) && obj is bool value && (value ^ a0.Invert);
			return output;
		}
		private bool EvaluateEnabled(ICustomAttributeProvider provider)
		{
			bool output = true;
			if (TryGetAttribute(provider, out DisableAttribute a0, true))
				output &= a0.ExclusiveToPlaymode && !EditorApplication.isPlaying;
			if (TryGetAttribute(provider, out EnableIfAttribute a1, true))
				output &= TryReadMember(a1.Condition, target, out object obj) && obj is bool value && (value ^ a1.Invert);
			return output;
		}
	}
}