using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant
{
	public const string Domain = "http://api.meemake.net";

	public static string Platform{
		get{
			if(RuntimePlatform.WindowsPlayer == Application.platform ||
				RuntimePlatform.WindowsEditor == Application.platform)
				return "win32";
			if(RuntimePlatform.Android == Application.platform)
				return "android";
			return "";
		}
	}

	public static class BootloaderStep
	{
		public const string Fetch = "Fetch";
		public const string Download = "Downlaod";
		public const string Decompress = "Decompress";
		public const string Parse = "Parse";
		public const string Load = "Load";
		public const string Run = "Run";
	}

	public static class CustomSettings
	{
		public const string Language = "settings.language";
	}
}
