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
	internal sealed class UrlLoadDirectiveProvider : ILoadDirectiveProvider
	{
		public UrlLoadDirectiveProvider(ICakeConfiguration configuration, ICakeEnvironment environment)
		{
			_configuration = configuration;
			_environment = environment;
		}

		private ICakeEnvironment _environment;
		private ICakeConfiguration _configuration;

		public bool CanLoad(IScriptAnalyzerContext context, LoadReference reference)
		{
			return reference.Scheme != null && reference.Scheme.Equals("url", StringComparison.OrdinalIgnoreCase);
		}

		public void Load(IScriptAnalyzerContext context, LoadReference reference)
		{
			var urlsPath = GetUrlsPath(context.Root.GetDirectory());

			var referenceHash = HashMd5(reference.ToString());

			var urlFile = urlsPath.CombineWithFilePath(referenceHash + ".cake").MakeAbsolute(_environment);

			if (!System.IO.File.Exists(urlFile.FullPath))
			{
				var http = new System.Net.Http.HttpClient();

				using (var httpStream = http.GetStreamAsync(reference.ToString()).Result)
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
