using DXDecompiler.Decompiler.IR;

namespace DXDecompiler.Decompiler.Writer
{
	public class OperandWriter : BaseWriter
	{
		public OperandWriter(DecompileContext context) : base(context)
		{

		}

		public void WriteOperand(Operand operand)
		{
			Write(operand.DXBCOperand.ToString());
		}
	}
}
