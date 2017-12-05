using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Testing;
using NSubstitute;
using System.Collections.Generic;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Scripting.Processors.Loading;

namespace Cake.UrlLoadDirective.Module.Tests
{
	internal sealed class UrlLoadDirectiveProviderFixture
	{
		public ICakeEnvironment Environment { get; set; }
		public IFileSystem FileSystem { get; set; }
		public IProcessRunner ProcessRunner { get; set; }
		public IScriptAnalyzerContext ScriptAnalyzerContext { get; set; }
		public ICakeLog Log { get; set; }
		public ICakeConfiguration Config { get; set; }
		public Dictionary<string, string> LoadedReferences { get; set; }

		internal UrlLoadDirectiveProviderFixture()
		{
			Environment = FakeEnvironment.CreateUnixEnvironment();
			FileSystem = new FakeFileSystem(Environment);
			ProcessRunner = Substitute.For<IProcessRunner>();

			LoadedReferences = new Dictionary<string, string>();

			ScriptAnalyzerContext = Substitute.For<IScriptAnalyzerContext>();
			ScriptAnalyzerContext.Root.Returns(new FilePath(System.IO.Directory.GetCurrentDirectory()));
			ScriptAnalyzerContext
				.WhenForAnyArgs(x => x.Analyze(Arg.Any<FilePath>()))
				.Do(c => {
					var fp = c.Arg<FilePath>();
					LoadedReferences[fp.FullPath] = System.IO.File.ReadAllText(fp.FullPath);
				});

			Log = new FakeLog();
			Config = new CakeConfiguration (new Dictionary<string, string> ());
		}

		internal UrlLoadDirectiveProvider CreateProvider()
		{
			return new UrlLoadDirectiveProvider(Config, Environment);
		}

		internal bool CanLoad(LoadReference loadRef)
		{
			var p = CreateProvider();
			return p.CanLoad(ScriptAnalyzerContext, loadRef);
		}

		internal void Load(LoadReference loadRef)
		{
			var p = CreateProvider();
			p.Load(ScriptAnalyzerContext, loadRef);
		}
	}
}
