using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Emp37.Utility.Editor
{
	/// <summary>
	/// An ordered collection of colors, serializable to and from JSON via <see cref="JsonUtility"/>.
	/// </summary>
	[Serializable]
	internal class ColorPalette
	{
		[SerializeField] private Color[] colors = Array.Empty<Color>();

		public ReadOnlySpan<Color> Colors => colors;
		public int Count => colors?.Length ?? 0;

		public Color this[int index] => colors[index];

		public ColorPalette(Color[] colors) => this.colors = colors ?? Array.Empty<Color>();
	}

	public class ColorPaletteWindow : EditorWindow
	{
		private const string Extension = "json", LastPathKey = "Emp37.ColorPalette.LastPath", DefaultPath = "Assets/Editor/Color Palettes";

		private const float Spacing = 4F, ButtonSize = 60F;
		private static readonly Vector2 SwatchSizeRange = new(48F, 128F);

		[SerializeField] private string currentPath = string.Empty;
		[SerializeField] private List<Color> colors = new();

		private float swatchSize = 64F;
		private Vector2 scroll;
		private int pendingRemoval = -1;

		private string DisplayName => string.IsNullOrEmpty(currentPath) ? "Untitled" : Path.GetFileNameWithoutExtension(currentPath);
		private static string EnsureDefaultDirectory
		{
			get
			{
				if (!Directory.Exists(DefaultPath))
				{
					Directory.CreateDirectory(DefaultPath);
					AssetDatabase.Refresh();
				}
				return DefaultPath;
			}
		}
		private bool ConfirmDiscard => !hasUnsavedChanges || EditorUtility.DisplayDialog("Unsaved Changes", $"'{DisplayName}' has unsaved changes. Discard them?", "Discard", "Cancel");


		[MenuItem("Tools/Emp37/Color Palette")]
		private static void OpenWindow()
		{
			ColorPaletteWindow window = GetWindow<ColorPaletteWindow>("Color Palette");
			window.minSize = new Vector2(320F, 240F);
		}

		private void OnEnable()
		{
			titleContent = new GUIContent("Color Palette");

			if (colors.Count == 0)
			{
				string last = EditorPrefs.GetString(LastPathKey, string.Empty);
				if (!string.IsNullOrEmpty(last) && File.Exists(last)) LoadPaletteData(last);
			}
		}
		private void OnGUI()
		{
			using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
			{
				if (Button("New")) CreateNewPalette(); if (Button("Open")) OpenPalette(); if (Button("Save")) SaveChanges(); if (Button("Save As")) SaveChangesAs();

				GUILayout.FlexibleSpace();

				swatchSize = GUILayout.HorizontalSlider(swatchSize, SwatchSizeRange.x, SwatchSizeRange.y, GUILayout.Width(80F));

				static bool Button(string name) => GUILayout.Button(name, EditorStyles.toolbarButton, GUILayout.Width(ButtonSize));
			}

			using (EditorGUILayout.ScrollViewScope scope = new(scroll))
			{
				scroll = scope.scrollPosition;

				int count = colors.Count, total = count + 1;
				Vector2 cellSize = new(x: swatchSize + Spacing, y: swatchSize + EditorStyles.miniLabel.lineHeight + (2F * Spacing));

				int columns = Mathf.Max(1, Mathf.FloorToInt((EditorGUIUtility.currentViewWidth - Spacing) / cellSize.x)), rows = Mathf.CeilToInt((float) total / columns);
				Rect gridRect = GUILayoutUtility.GetRect(columns * cellSize.x, rows * cellSize.y);
				Event e = Event.current;

				for (int i = 0; i < total; i++)
				{
					int row = i / columns, column = i % columns;

					Rect cellRect = new(gridRect)
					{
						position = gridRect.position + new Vector2((column * cellSize.x) + Spacing, (row * cellSize.y) + Spacing),
						size = cellSize
					};
					Rect swatchRect = new(cellRect)
					{
						size = swatchSize * Vector2.one
					};

					if (i < count)
					{
						if (e.type == EventType.MouseDown && e.button == 2 && cellRect.Contains(e.mousePosition))
						{
							pendingRemoval = i;
							e.Use();
							continue;
						}

						using (EditorGUI.ChangeCheckScope checkScope = new())
						{
							Color edited = EditorGUI.ColorField(swatchRect, GUIContent.none, colors[i], false, false, false);
							if (checkScope.changed)
							{
								colors[i] = edited;
								hasUnsavedChanges = true;
							}
						}

						Rect hexRect = new(cellRect)
						{
							y = cellRect.y + swatchRect.height,
							width = swatchRect.width,
							height = EditorStyles.miniLabel.lineHeight
						};
						using (EditorGUI.ChangeCheckScope checkScope = new())
						{
							string text = EditorGUI.DelayedTextField(hexRect, "#" + ColorUtility.ToHtmlStringRGB(colors[i]), EditorStyles.miniLabel);
							if (checkScope.changed)
							{
								if (!text.StartsWith("#")) text = "#" + text;
								if (ColorUtility.TryParseHtmlString(text, out Color color))
								{
									colors[i] = color;
									hasUnsavedChanges = true;
								}
							}
						}
					}
					else if (GUI.Button(swatchRect, "+"))
					{
						colors.Add(Color.white);
						hasUnsavedChanges = true;
					}
				}
			}
			if (pendingRemoval >= 0)
			{
				colors.RemoveAt(pendingRemoval);
				pendingRemoval = -1;
				hasUnsavedChanges = true;
				GUI.FocusControl(null);
				Repaint();
			}

			using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
			{
				GUILayout.Label($"{DisplayName}: {colors.Count} color{(colors.Count == 1 ? "" : "s")}", EditorStyles.boldLabel, GUILayout.MinWidth(ButtonSize));
				GUILayout.FlexibleSpace();
				using (new EditorGUI.DisabledScope(colors.Count < 2))
				{
					if (GUILayout.Button("Sort", EditorStyles.toolbarDropDown))
					{
						GenericMenu menu = new();
						menu.AddItem(new GUIContent("Hue"), false, () => SortColorsBy(0));
						menu.AddItem(new GUIContent("Saturation"), false, () => SortColorsBy(1));
						menu.AddItem(new GUIContent("Vibrance"), false, () => SortColorsBy(2));
						menu.ShowAsContext();
					}
					if (GUILayout.Button("Reverse", EditorStyles.toolbarButton))
					{
						colors.Reverse();
						hasUnsavedChanges = true;
					}
				}
			}
		}

		private void SortColorsBy(int hsvComponent)
		{
			colors.Sort((a, b) =>
			{
				Color.RGBToHSV(a, out float hA, out float sA, out float vA); Color.RGBToHSV(b, out float hB, out float sB, out float vB);

				return hsvComponent switch
				{ 0 => hA.CompareTo(hB), 1 => sA.CompareTo(sB), _ => vA.CompareTo(vB) };
			});
			hasUnsavedChanges = true;
		}
		private void UpdateEditorMessages() => saveChangesMessage = $"'{DisplayName}' has unsaved changes. Would you like to save them before closing?";

		#region F I L E
		private void CreateNewPalette()
		{
			if (!ConfirmDiscard) return;
			colors.Clear();
			currentPath = string.Empty;
			hasUnsavedChanges = false;
			UpdateEditorMessages();
		}
		private void OpenPalette()
		{
			if (!ConfirmDiscard) return;
			string path = EditorUtility.OpenFilePanel("Open Color Palette", Path.GetFullPath(EnsureDefaultDirectory), Extension);
			if (!string.IsNullOrEmpty(path)) LoadPaletteData(path);
		}
		public override void SaveChanges()
		{
			if (string.IsNullOrEmpty(currentPath)) SaveChangesAs();
			else WritePaletteToDisk(currentPath);
		}
		private void SaveChangesAs()
		{
			string path = EditorUtility.SaveFilePanelInProject("Save Color Palette", DisplayName, Extension, "Choose a location.", EnsureDefaultDirectory);
			if (!string.IsNullOrEmpty(path)) WritePaletteToDisk(path);
		}
		private void LoadPaletteData(string path)
		{
			try
			{
				ColorPalette palette = JsonUtility.FromJson<ColorPalette>(File.ReadAllText(path));
				if (palette == null)
				{
					Debug.LogError($"Color Palette at '{path}' could not be parsed.");
					return;
				}
				colors.Clear();
				colors.AddRange(palette.Colors.ToArray());
				currentPath = path;
				hasUnsavedChanges = false;
				EditorPrefs.SetString(LastPathKey, path);
				UpdateEditorMessages();
			}
			catch (Exception e)
			{
				Debug.LogError($"Failed to load Color Palette at '{path}': {e.Message}");
			}
		}
		private void WritePaletteToDisk(string path)
		{
			try
			{
				ColorPalette palette = new(colors.ToArray());
				File.WriteAllText(path, JsonUtility.ToJson(palette, true));
				currentPath = path;
				hasUnsavedChanges = false;
				EditorPrefs.SetString(LastPathKey, path);
				AssetDatabase.Refresh();
				UpdateEditorMessages();
			}
			catch (Exception e)
			{
				Debug.LogError($"Failed to save Color Palette to '{path}': {e.Message}");
			}
		}
		#endregion
	}
}