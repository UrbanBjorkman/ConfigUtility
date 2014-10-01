using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace ConfigUtility
{
	public class ClearConfig : Task
	{

		private string _configFilename;
		[Required]
		public string ConfigFilename
		{
			get { return _configFilename; }
			set { _configFilename = value; }
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
			XmlDocument xmlDoc = GenerateEmpty();

			xmlDoc.Save(ConfigFilename);

		}



		private XmlDocument GenerateEmpty()
		{
			var sb = new StringBuilder();
			sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("  <appSettings>");

			//foreach (var kvp in keyValues)
			//{
			//	sb.AppendLine(string.Format("    <add key=\"{0}\" value=\"{1}\" />", kvp.Key, kvp.Value));
			//}

			sb.AppendLine("  </appSettings>");

			var outDocument = new XmlDocument();
			outDocument.LoadXml(sb.ToString());
			return outDocument;

		}
	}
}
