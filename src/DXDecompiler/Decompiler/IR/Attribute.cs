using System.Collections.Generic;

namespace DXDecompiler.Decompiler.IR
{
	public class Attribute
	{
		public string Name;
		public List<string> Arguments = new List<string>();
		public static Attribute Create(string name, params object[] args)
		{
			var result = new Attribute();
			result.Name = name;
			foreach(var arg in args)
			{
				if(arg is string)
				{
					result.Arguments.Add($"\"{arg}\"");
				}
				else
				{
					result.Arguments.Add(arg.ToString());
				}
			}
			return result;
		}
	}
}
