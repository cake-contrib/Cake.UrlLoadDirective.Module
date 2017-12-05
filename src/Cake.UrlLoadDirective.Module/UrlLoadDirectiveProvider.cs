using System;
using System.Security.Cryptography;
using System.Text;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.IO;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Scripting.Processors.Loading;

namespace Cake.UrlLoadDirective.Module
{
	/// <summary>
	/// A URL Source Provider for #load directives
	/// </summary>
	public sealed class UrlLoadDirectiveProvider : ILoadDirectiveProvider
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Cake.UrlLoadDirective.Module.UrlLoadDirectiveProvider"/> class.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		/// <param name="environment">The environment.</param>
		public UrlLoadDirectiveProvider(ICakeConfiguration configuration, ICakeEnvironment environment)
		{
			_configuration = configuration;
			_environment = environment;
		}

		private ICakeEnvironment _environment;
		private ICakeConfiguration _configuration;

		/// <summary>
		/// Can the URL Provider load a given reference
		/// </summary>
		/// <returns><c>true</c>, if reference can be loaded, <c>false</c> otherwise.</returns>
		/// <param name="context">The context.</param>
		/// <param name="reference">The load reference.</param>
		public bool CanLoad(IScriptAnalyzerContext context, LoadReference reference)
		{
			return reference.Scheme != null && reference.Scheme.Equals("url", StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Load the given reference
		/// </summary>
		/// <returns>The load.</returns>
		/// <param name="context">The context.</param>
		/// <param name="reference">The load reference.</param>
		public void Load(IScriptAnalyzerContext context, LoadReference reference)
		{
			var urlsPath = GetUrlsPath(context.Root.GetDirectory());

			var actualUrl = reference.Address;

			var referenceHash = HashMd5(actualUrl.ToString());

			var urlFile = urlsPath.CombineWithFilePath(referenceHash + ".cake").MakeAbsolute(_environment);

			if (!System.IO.File.Exists(urlFile.FullPath))
			{
				var urlFileDir = urlFile.GetDirectory().FullPath;

				// Make sure the directory exists we want to save to
				if (!System.IO.Directory.Exists(urlFileDir))
					System.IO.Directory.CreateDirectory(urlFileDir);
				
				var http = new System.Net.Http.HttpClient();

				using (var httpStream = http.GetStreamAsync(actualUrl).Result)
				using (var cacheStream = System.IO.File.Create(urlFile.MakeAbsolute(_environment).FullPath))
				{
					httpStream.CopyTo(cacheStream);
				}
			}

			context.Analyze(urlFile);
		}

		private DirectoryPath GetToolPath(DirectoryPath root)
		{
			var toolPath = _configuration.GetValue(Constants.Paths.Tools);
			if (!string.IsNullOrWhiteSpace(toolPath))
			{
				return new DirectoryPath(toolPath).MakeAbsolute(_environment);
			}

			return root.Combine("tools");
		}

		private DirectoryPath GetUrlsPath(DirectoryPath root)
		{
			var modulePath = _configuration.GetValue(Constants.Paths.Urls);
			if (!string.IsNullOrWhiteSpace(modulePath))
			{
				return new DirectoryPath(modulePath).MakeAbsolute(_environment);
			}

			var toolPath = GetToolPath(root);
			return toolPath.Combine("Urls").Collapse();
		}

		private string HashMd5(string value)
		{
			var data = Encoding.UTF8.GetBytes(value);

			using (var md5 = MD5.Create())
			{
				var hash = md5.ComputeHash(data);

				return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
			}
		}
	}
}
