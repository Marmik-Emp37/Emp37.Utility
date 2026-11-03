using System;
using System.Reflection;
using NUnit.Framework;

namespace Emp37.Utility.Tests
{
	public class ReflectionAttributeUtilityTests
	{
		[AttributeUsage(AttributeTargets.Field)]
		private sealed class MarkerAttribute : Attribute { }

		private class Dummy
		{
			[Marker] public int Tagged;
			public int Untagged;
		}

		[SetUp]
		public void ClearCaches() => ReflectionUtility.ClearAttributeCache();

		[Test]
		public void HasAttribute_ReturnsTrue_WhenPresent()
		{
			FieldInfo field = typeof(Dummy).GetField(nameof(Dummy.Tagged));
			Assert.IsTrue(ReflectionUtility.HasAttribute<MarkerAttribute>(field));
		}
		[Test]
		public void HasAttribute_ReturnsFalse_WhenAbsent()
		{
			FieldInfo field = typeof(Dummy).GetField(nameof(Dummy.Untagged));
			Assert.IsFalse(ReflectionUtility.HasAttribute<MarkerAttribute>(field));
		}
		[Test]
		public void TryGetAttribute_ReturnsInstance_WhenPresent()
		{
			FieldInfo field = typeof(Dummy).GetField(nameof(Dummy.Tagged));
			bool found = ReflectionUtility.TryGetAttribute(field, out MarkerAttribute attribute);

			Assert.IsTrue(found);
			Assert.IsNotNull(attribute);
		}
		[Test]
		public void TryGetAttribute_ReturnsFalse_WhenAbsent()
		{
			FieldInfo field = typeof(Dummy).GetField(nameof(Dummy.Untagged));
			bool found = ReflectionUtility.TryGetAttribute(field, out MarkerAttribute attribute);

			Assert.IsFalse(found);
			Assert.IsNull(attribute);
		}
		[Test]
		public void GetAttribute_ReturnsNull_WhenAbsent()
		{
			FieldInfo field = typeof(Dummy).GetField(nameof(Dummy.Untagged));
			Assert.IsNull(ReflectionUtility.GetAttribute<MarkerAttribute>(field));
		}
		[Test]
		public void GetAttributes_ReturnsEmptyArray_WhenAbsent()
		{
			FieldInfo field = typeof(Dummy).GetField(nameof(Dummy.Untagged));
			Assert.IsEmpty(ReflectionUtility.GetAttributes<MarkerAttribute>(field));
		}
		[Test]
		public void ClearAttributeCache_DoesNotChangeResult()
		{
			FieldInfo field = typeof(Dummy).GetField(nameof(Dummy.Tagged));
			Assert.IsTrue(ReflectionUtility.HasAttribute<MarkerAttribute>(field)); // populates cache

			ReflectionUtility.ClearAttributeCache();
			Assert.IsTrue(ReflectionUtility.HasAttribute<MarkerAttribute>(field)); // resolved fresh, same result
		}
	}
}