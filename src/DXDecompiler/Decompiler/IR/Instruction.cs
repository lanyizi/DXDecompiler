using DXDecompiler.Chunks.Shex.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.Decompiler.IR
{
	public class Instruction
	{
		internal InstructionToken Token;
		public InstructionOpcode Opcode;
		public List<Operand> Operands;
		public string AsmDebug;
		public Instruction()
		{
			Operands = new List<Operand>();
		}
		public Instruction(InstructionToken token)
		{
			Token = token;
			AsmDebug = Token.ToString();
			Operands = new List<Operand>(token.Operands.Select(o => new Operand(o)));
			Opcode = (InstructionOpcode)Token.Header.OpcodeType;
		}
	}
}
