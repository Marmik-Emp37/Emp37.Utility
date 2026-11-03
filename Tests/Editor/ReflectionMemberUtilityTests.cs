using System;
using System.Reflection;
using NUnit.Framework;

namespace Emp37.Utility.Tests
{
	public class ReflectionMemberUtilityTests
	{
		private class Dummy
		{
			public int Number = 5;
			public string Name { get; set; } = "Hello";
			public int Add(int a, int b) => a + b;
		}


		[SetUp]
		public void ClearCaches() => ReflectionUtility.ClearMemberCache();

		[Test]
		public void FindField_ResolvesDeclaredField()
		{
			FieldInfo field = ReflectionUtility.FindField(nameof(Dummy.Number), typeof(Dummy));
			Assert.IsNotNull(field);
		}
		[Test]
		public void FindMember_ResolvesField_WhenOnlyFieldMatches()
		{
			MemberInfo member = ReflectionUtility.FindMember(nameof(Dummy.Number), typeof(Dummy));
			Assert.IsInstanceOf<FieldInfo>(member);
		}
		[Test]
		public void FindMember_ReturnsNull_WhenNameUnknown()
		{
			MemberInfo member = ReflectionUtility.FindMember("DoesNotExist", typeof(Dummy));
			Assert.IsNull(member);
		}
		[Test]
		public void ReadField_ReturnsCurrentValue()
		{
			var dummy = new Dummy();
			Assert.AreEqual(5, ReflectionUtility.ReadField(nameof(Dummy.Number), dummy));
		}
		[Test]
		public void ReadProperty_ReturnsCurrentValue()
		{
			var dummy = new Dummy();
			Assert.AreEqual("Hello", ReflectionUtility.ReadProperty(nameof(Dummy.Name), dummy));
		}
		[Test]
		public void InvokeMethod_ReturnsResult()
		{
			var dummy = new Dummy();
			var result = ReflectionUtility.InvokeMethod(nameof(Dummy.Add), dummy, new object[] { 2, 3 });
			Assert.AreEqual(5, result);
		}
		[Test]
		public void TryReadMember_ReturnsFalse_WhenMemberMissing()
		{
			var dummy = new Dummy();
			bool found = ReflectionUtility.TryReadMember("DoesNotExist", dummy, out object value);
			Assert.IsFalse(found);
			Assert.IsNull(value);
		}
		[Test]
		public void TryReadMember_ReturnsTrue_AndReadsField()
		{
			var dummy = new Dummy();
			bool found = ReflectionUtility.TryReadMember(nameof(Dummy.Number), dummy, out object value);
			Assert.IsTrue(found);
			Assert.AreEqual(5, value);
		}
		[Test]
		public void AutoInvokeMethod_ResolvesArgumentsByName()
		{
			var dummy = new Dummy();
			MethodInfo method = typeof(Dummy).GetMethod(nameof(Dummy.Add));

			// reuses Number (5) for both parameters: Add(5, 5) == 10
			object result = ReflectionUtility.AutoInvokeMethod(method, dummy, new[] { nameof(Dummy.Number), nameof(Dummy.Number) });
			Assert.AreEqual(10, result);
		}
		[Test]
		public void AutoInvokeMethod_Throws_WhenArgumentCountMismatches()
		{
			var dummy = new Dummy();
			MethodInfo method = typeof(Dummy).GetMethod(nameof(Dummy.Add));
			Assert.Throws<ArgumentException>(() => ReflectionUtility.AutoInvokeMethod(method, dummy, new[] { nameof(Dummy.Number) }));
		}

		[Test]
		public void AutoInvokeMethod_Throws_WhenArgumentMemberUnresolvable()
		{
			var dummy = new Dummy();
			MethodInfo method = typeof(Dummy).GetMethod(nameof(Dummy.Add));
			Assert.Throws<ArgumentException>(() => ReflectionUtility.AutoInvokeMethod(method, dummy, new[] { "DoesNotExist", nameof(Dummy.Number) }));
		}
	}
}