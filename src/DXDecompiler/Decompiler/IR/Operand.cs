namespace DXDecompiler.Decompiler.IR
{
	public class Operand
	{
		internal Chunks.Shex.Tokens.Operand DXBCOperand;
		public Operand(Chunks.Shex.Tokens.Operand operand)
		{
			DXBCOperand = operand;
		}
	}
}
