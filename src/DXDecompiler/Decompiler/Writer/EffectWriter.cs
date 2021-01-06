using DXDecompiler.Decompiler.IR;

namespace DXDecompiler.Decompiler.Writer
{
	public class EffectWriter : BaseWriter
	{
		public EffectWriter(DecompileContext context) : base(context)
		{

		}

		public void WriteEffect(Effect effect)
		{
			foreach(var group in effect.EffectChunk.Groups)
			{
				Write(group.ToString());
			}
		}
	}
}
