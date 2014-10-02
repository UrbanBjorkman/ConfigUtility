using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace ConfigUtility
{
	/// <summary>
	/// Merges two config files into one
	/// </summary>
	public class MergeConfig : Task
	{
		private string _sourceConfigFilename;

		/// <summary>
		/// The file that will be used to merge, this file overrides any duplicate keys
		/// </summary>
		[Required]
		public string SourceConfigFilename
		{
			get { return _sourceConfigFilename; }
			set { _sourceConfigFilename = value; }
		}

		private string _targetConfigFilename;


		/// <summary>
		/// The destination file of the merge
		/// </summary>
		[Required]
		public string TargetConfigFilename
		{
			get { return _targetConfigFilename; }
			set { _targetConfigFilename = value; }
		}


		public override bool Execute()
		{
			try
			{
				DoExecute();
			}
			catch (Exception ex)
			{
				Log.LogErrorFromException(ex);
				return false;
			}
			return true;
		}

		private void DoExecute()
		{
			var config = Load(SourceConfigFilename);

			config = Merge(TargetConfigFilename, config);

			Generate(config).Save(TargetConfigFilename);

			Console.WriteLine("Completed merging XML documents");

		}

		/// <summary>
		/// Generates XML Document
		/// </summary>
		/// <param name="keyValues"></param>
		/// <returns></returns>
		private XmlDocument Generate(Dictionary<string, string> keyValues)
		{
			var sb = new StringBuilder();
			sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("  <appSettings>");

			foreach (var kvp in keyValues)
			{
				sb.AppendLine(string.Format("    <add key=\"{0}\" value=\"{1}\" />", kvp.Key, kvp.Value));
			}

			sb.AppendLine("  </appSettings>");

			var outDocument = new XmlDocument();
			outDocument.LoadXml(sb.ToString());
			return outDocument;

		}


		private Dictionary<string, string> Load(string file)
		{
			var configKeys = new Dictionary<string, string>();
			var document = new XmlDocument();
			if (!System.IO.File.Exists(SourceConfigFilename)) return configKeys;

			document.Load(SourceConfigFilename);
			var keys = document.SelectNodes("/appSettings/add");

			if (keys != null)
			{
				configKeys =
					keys.Cast<XmlNode>()
						.Where(node => node.Attributes != null)
						.ToDictionary(node => node.Attributes["key"].Value, node => node.Attributes["value"].Value);
			}

			return configKeys;
		}

		private Dictionary<string, string> Merge(string file, Dictionary<string, string> configKeys)
		{
			var document = new XmlDocument();

			if (!System.IO.File.Exists(TargetConfigFilename)) return configKeys;

			document.Load(TargetConfigFilename);
			var keys = document.SelectNodes("/appSettings/add");
			if (keys == null) return configKeys;
			foreach (
				var node in
					keys.Cast<XmlNode>()
						.Where(node => node.Attributes != null && !configKeys.ContainsKey(node.Attributes["key"].Value)))
			{
				configKeys.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
			}
			return configKeys;
		}
	}
}
