using System;
namespace Cake.UrlLoadDirective.Module
{
	/// <summary>
	/// Constants Class
	/// </summary>
	internal static class Constants
	{
		/// <summary>
		/// UrlLoadDirective Constants
		/// </summary>
		public static class Paths
		{
			/// <summary>
			/// The config key name for overriding the default download path for urls
			/// </summary>
			public const string Urls = "Paths_Urls";

			/// <summary>
			/// The config key name for overriding the default download path for tools
			/// </summary>
			public const string Tools = "Paths_Tools";
		}
	}
}
