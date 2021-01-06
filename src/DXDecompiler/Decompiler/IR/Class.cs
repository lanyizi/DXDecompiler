using System.Collections.Generic;

namespace DXDecompiler.Decompiler.IR
{
	public class Class
	{
		public List<Pass> Passes = new List<Pass>();
		public string Name;
	}
}
