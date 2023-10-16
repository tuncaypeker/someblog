using CookComputing.XmlRpc;
using System;

namespace SomeBlog.Wordpress.XmlRpc
{
    /// <summary>
    /// Stores the configuration for the WordPressClient. Use Default to construct a configuration based on the following app settings: 
    /// WordPressUserName, WordPressPassword, WordPressBaseUrl and WordPressBlogId (optional).
    /// </summary>
    [XmlRpcMissingMapping(MappingAction.Ignore)]
	public class WordPressSiteConfig
	{
		/// <summary>
		/// Gets the default configuration. This configuration looks for the following app settings: 
		/// WordPressUserName, WordPressPassword, WordPressBaseUrl and WordPressBlogId (optional).
		/// </summary>
		/// <value>
		/// The default.
		/// </value>
		[XmlRpcMember("blog_id")]
		public int BlogId { get; set; }

		[XmlRpcMember("username")]
		public string Username { get; set; }

		[XmlRpcMember("password")]
		public string Password { get; set; }

		public string BaseUrl { get; set; }

		public string FullUrl { get { return string.Concat(BaseUrl, BaseUrl.EndsWith("/") ? "xmlrpc.php" : "/xmlrpc.php"); } }

		
	}
}