using System;

namespace Emp37.Utility
{
	/// <summary>
	/// Groups consecutive fields into tabs, displayed under a toolbar in the Inspector.
	/// </summary>
	/// <remarks>
	/// Apply with a name to <b>open</b> a tab. Every field drawn afterward, including fields with no attribute, belongs to that tab until another <c>[TabGroup("Name")]</c> starts the next tab of the same toolbar.
	/// <br>Apply with no name (<c>[TabGroup]</c>) to <b>close</b> the toolbar. That field and the ones after it are drawn outside the tabs, and a later named <c>[TabGroup]</c> starts a new toolbar.</br>
	/// <para>The toolbar is drawn above the first field of the group. Tabs apply to fields only, so the methods section is always drawn outside the tabs.</para>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class TabGroupAttribute : Attribute
	{
		public readonly string Name;

		public TabGroupAttribute(string name = null) => Name = name;
	}
}
