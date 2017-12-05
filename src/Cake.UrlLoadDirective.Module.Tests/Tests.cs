using System;
using Cake.Core.Scripting.Processors.Loading;
using Xunit;

namespace Cake.UrlLoadDirective.Module.Tests
{
	public class Tests
	{
		[Theory]
		[InlineData("url:https://raw.githubusercontent.com/Redth/Cake.UrlLoadDirective.Module/master/src/Cake.UrlLoadDirective.Module.Tests/test.cake")]
		[InlineData("url:http://raw.githubusercontent.com/Redth/Cake.UrlLoadDirective.Module/master/src/Cake.UrlLoadDirective.Module.Tests/test.cake")]
		public void Should_CanLoad (string url)
		{
			var f = new UrlLoadDirectiveProviderFixture();

			var lr = new LoadReference(new Uri(url));

			var r = f.CanLoad(lr);

			Assert.True(r);
		}


		[Theory]
		[InlineData("https://raw.githubusercontent.com/Redth/Cake.UrlLoadDirective.Module/master/src/Cake.UrlLoadDirective.Module.Tests/test.cake")]
		[InlineData("nuget:?package=Cake.FileHelpers")]
		public void Should_Not_CanLoad(string url)
		{
			var f = new UrlLoadDirectiveProviderFixture();

			var lr = new LoadReference(new Uri(url));

			var r = f.CanLoad(lr);

			Assert.False(r);
		}



		[Theory]
		[InlineData("url:https://raw.githubusercontent.com/Redth/Cake.UrlLoadDirective.Module/master/src/Cake.UrlLoadDirective.Module.Tests/test.cake")]
		[InlineData("url:http://raw.githubusercontent.com/Redth/Cake.UrlLoadDirective.Module/master/src/Cake.UrlLoadDirective.Module.Tests/test.cake")]
		public void Load_Url_Reference(string url)
		{
			var f = new UrlLoadDirectiveProviderFixture();

			var lr = new LoadReference(new Uri(url));

			f.Load(lr);

			Assert.NotEmpty(f.LoadedReferences);

			foreach (var k in f.LoadedReferences.Keys)
				Console.WriteLine(k);
			
		}
	}
}
