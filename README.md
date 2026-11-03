# About
A collection of general-purpose Unity utilities: a rich inspector-attribute system, singleton base classes, cached reflection helpers, a curated color palette, and a couple of editor tools - all built on a single shared custom editor so everything works out of the box on any `MonoBehaviour` or `ScriptableObject`.


# Features
+ Inspector attribute system - buttons, conditional show/enable, titles, separators, help boxes, comments, inline buttons, toggle buttons, and more.
+ Zero setup - one shared custom editor (`Emp37Editor`) is automatically applied to every `MonoBehaviour` and `ScriptableObject`. No `[CustomEditor]` boilerplate required.
+ Singleton base classes - `Dynamic<T>` (auto-creating, persistent-capable) and `Lightweight<T>` (manual placement, Awake-registered).
+ Cached reflection utilities - attribute lookup and member resolution (fields/properties/methods), including invoking a method by resolving its arguments from matching members by name.
+ Curated `Shade` color palette - 60+ named colors with hex and rich-text conversion helpers.
+ Lightweight MonoBehaviour registry - type-keyed singleton lookup, independent of the `Singleton` base classes.
+ Editor tools - a Color Palette window and "Extract Component" context menu actions.
+ General-purpose extensions for `Transform`, `Rect`, and `string`.


# Installation
### Via Package Manager (recommended)
1. Open **Window > Package Manager**.
2. Click **+ > Add package from git URL...**
3. Paste:
   ```
   https://github.com/Marmik-Emp37/Emp37.Utility.git
   ```

### Manual
Alternatively, clone or download this repository and copy the **`Emp37.Utility`** folder into your project's `Assets` folder.

**Requirements:** Unity with UIElements-era `PropertyDrawer`/`DecoratorDrawer` support (2019.4+).


# Quick Start
```csharp
using UnityEngine;
using Emp37.Utility;

[Note("This component controls the player's core stats.", Shade.SkyBlue)]
public class PlayerStats : MonoBehaviour
{
    [Title("Health")]
    [SerializeField, Max(999F)] private float currentHealth = 100F;

    [Separator]
    [Title("Movement")]
    [SerializeField] private bool canSprint = true;
    [ShowIf(nameof(canSprint))]
    [SerializeField] private float sprintSpeed = 8F;

    [Comment("Assign the weapon this character starts with.", Shade.Amber)]
    [RequireObject]
    [SerializeField] private Weapon startingWeapon;

    [ToggleButton]
    [SerializeField] private bool isInvincible;

    [Button(Size.Large)]
    private void ResetHealth() => currentHealth = 100F;
}
```
No `[CustomEditor]` needed - every attribute above is picked up automatically by `MonoBehaviourEditor` / `ScriptableObjectEditor`.


# Inspector Attributes
All attributes live in `Emp37.Utility` and are consumed by the shared `Emp37Editor`.

| Attribute | Target | Description |
| --- | --- | --- |
| **`Title(text, shade?)`** | Field | Bold section title with an underline drawn above the field. |
| **`StylizedTitle(text, shade?)`** | Field | Same as `Title`, but renders the text letter-spaced and upper-cased. |
| **`Note(text, shade?)`** | Class | Colored `HelpBox` shown at the top of the inspector. Repeatable. |
| **`Comment(text, shade?)`** | Field | Tinted rich-text comment block above a field. Repeatable. |
| **`HelpBox(text, type?)`** | Field | Unity-style help box (`Info`/`Warning`/`Error`) above a field. |
| **`Separator(shade?, alpha?)`** | Field | Horizontal rule / divider. Repeatable. |
| **`Indent(level)`** | Field | Indents the field by the given indent level. |
| **`Label(text?, icon?)`** | Field | Overrides a field's displayed label and/or icon. |
| **`Horizontal(value = true)`** | Field / Method | Groups consecutive members onto a single inspector row (`true` opens the group, `false` closes it). |
| **`ShowIf(condition)`** | Field / Method | Shows the member only when the named bool field/property/method returns `true` (supports `Invert`). |
| **`EnableIf(condition)`** | Field / Method | Enables the member only when the named bool field/property/method returns `true` (supports `Invert`). |
| **`Disable(exclusiveToPlaymode?)`** | Field / Method | Greys out / disables the member, optionally only while in Play mode. |
| **`Max(value)`** | Field | Clamps a numeric or vector field (`float`, `int`, `Vector2/3`, `Vector2Int/3Int`) to a maximum. |
| **`TabGroup(name?)`** | Field | Groups consecutive fields into tabs under a toolbar. A named `[TabGroup("Name")]` opens a tab that includes every following field until the next one. A bare `[TabGroup]` closes the toolbar. Fields only. |
| **`PreviewField(size?)`** | Field | Draws an object reference (e.g. `Sprite`, `Texture`, `Prefab`) as a square thumbnail. Default size is 64. |
| **`Unit(text)`** | Field | Draws a short text (e.g. a unit like `"m/s"`) inside the right edge of a numeric, vector or `string` field. |
| **`RequireObject(message?)`** | Field | Shows an error `HelpBox` under an object-reference field while it's unassigned. |
| **`ExpandableObject`** | Field | Makes an assigned object-reference field expandable, nesting its own editor inline. |
| **`ToggleButton(size?)`** | Field (`bool`) | Renders the bool as a toggle styled as a button, labeled `"Name : True/False"`. |
| **`Button(size?, params)`** | Method | Renders the method as a clickable inspector button, with optional size and arguments. |
| **`InlineButton(method, size?, params)`** | Field | Draws a button next to the field that invokes a named method. Repeatable. |
| **`HideDefaultProperty`** | Class | Hides the default "Script" field at the top of the inspector. |

> [!TIP]
> `ShowIf` / `EnableIf` conditions reference any parameterless `bool` field, property, or method on the same object by name (`nameof(...)` recommended).


# Singletons
Namespace: `Emp37.Utility.Singleton`

### Dynamic\<T> - auto-creating runtime singleton
Resolves or creates its instance on first access to `Instance` (searches the scene, including inactive objects, then spawns a new `GameObject` if none is found).
```csharp
public class GameManager : Dynamic<GameManager>
{
    private void Awake() => Initialize(persistent: true);
}

// access anywhere
GameManager.Instance.DoSomething();
```
- `Initialize(persistent, actionOnDuplicate)` must be called (typically from `Awake`) to register the instance.
- `actionOnDuplicate` (`DuplicateAction`) controls what happens to a second instance: `DestroyGameObject`, `DestroyComponent`, or `KeepAndWarn`.
- `Instance` returns `null` once the application is quitting - no extra null checks needed during shutdown.

### Lightweight\<T> - manually placed singleton
Registers itself in `Awake()`. Must be placed in the scene by hand.
```csharp
public class AudioManager : Lightweight<AudioManager>
{
    protected override void Awake()
    {
        base.Awake(); // required
    }
}
```
> [!IMPORTANT]
> `Instance` is lost across a mid-play domain reload (`Awake` doesn't re-run), and may still be `null` if read from another script's own `Awake`. Read it from `Start`, or give the singleton a negative `DefaultExecutionOrder`.


# Reflection Utilities
`ReflectionUtility` (namespace `Emp37.Utility`) wraps `System.Reflection` with caching for both attribute lookups and member resolution.
```csharp
// attributes - backed by a ConcurrentDictionary cache
ReflectionUtility.HasAttribute<TitleAttribute>(fieldInfo);
ReflectionUtility.TryGetAttribute(fieldInfo, out TitleAttribute title);
var all = ReflectionUtility.GetAttributes<SeparatorAttribute>(fieldInfo);

// members - searches field -> property -> method, also cached
var member = ReflectionUtility.FindMember("health", targetType);
bool found = ReflectionUtility.TryReadMember("health", target, out object value);

// invoke a method, resolving its parameters from same-named fields/properties on the target
ReflectionUtility.AutoInvokeMethod(methodInfo, target, argNames: new[] { "damageAmount" });
```
Call `ReflectionUtility.ClearAttributeCache()` / `ClearMemberCache()` if you need to invalidate the caches (e.g. after hot-reloading reflected types).


# Shade Color Palette
`Shade` (namespace `Emp37.Utility`) is a curated enum of 60+ named colors, backed by `ShadePalette`.
```csharp
Color color   = Shade.Amaranth.ToColor();          // optional alpha
string hex    = Shade.Cinnamon.ToHexRGB();         // "RRGGBB", no '#'
string rich   = Shade.SkyBlue.ToRichText("Online"); // "<color=#RRGGBB>Online</color>"
Color accent  = ShadePalette.Random;                // random color from the palette
```


# Editor Tools
### Color Palette window
Open **Tools > Emp37 > Color Palette** to create, edit, and save reusable `.json` color palettes (swatches, hue/saturation/vibrance sorting, adjustable swatch size).

### Extract Component
Right-click any component (except `Transform`) in the inspector for:
- **Extract** - moves the component onto a new, detached `GameObject`.
- **Extract To Child** - moves the component onto a new child of the source object.

Both are undo-friendly (single undo step) and remove the component from its original object.


# Extensions & Misc Utilities
```csharp
using Emp37.Utility;

// Extensions
transform.Reset();                              // zero position/rotation, scale = Vector3.one
string title    = "myFieldName".ToTitleCase();          // "My Field Name"
string stylized = "settings".ToStylizedTitleCase();      // letter-spaced, upper-cased
string clipped  = longDescription.Truncate(20);          // cuts to length + "..."
Rect inner      = fullRect.Indent(4F);

// Utility
float mapped   = Utility.Remap(value, 0F, 1F, 0F, 100F);
string hexRGBA = Utility.ToHex(new Color(1F, 0F, 0F, 0.5F)); // "#FF00007F"

// MonoBehaviourRegistry - type-keyed singleton lookup
private void Awake()  => this.Register();
private void OnDestroy() => this.Unregister();

var player = MonoBehaviourRegistry.Get<PlayerController>();
if (MonoBehaviourRegistry.TryGet<AudioManager>(out var audio)) audio.Play();
```
# License
Licensed under the [MIT License](LICENSE).
