using DXDecompiler.Chunks;
using DXDecompiler.Chunks.Shex;
using DXDecompiler.Decompiler.IR;

namespace DXDecompiler.Decompiler.Writer
{
	public class InstructionWriter : BaseWriter
	{
		public InstructionWriter(DecompileContext context) : base(context)
		{
		}

		public void WriteInstruction(Instruction instruction)
		{
			switch(instruction.Opcode.GetInstructionType())
			{
				case InstructionType.IntrinsicCall:
					WriteCallInstruction(instruction);
					break;
				case InstructionType.IntrinsicCallNoDest:
					WriteCallInstructionNoDest(instruction);
					break;
				case InstructionType.BinaryOp:
					WriteBinaryOp(instruction);
					break;
				case InstructionType.SampleCall:
					WriteSampleCall(instruction);
					break;
				case InstructionType.ObjectCall:
					WriteObjectCall(instruction);
					break;
				case InstructionType.ObjectCallNoDest:
					WriteObjectCallNoDest(instruction);
					break;
				case InstructionType.ControlFlow:
					WriteControlFlow(instruction);
					break;
				default:
					switch(instruction.Opcode)
					{
						case InstructionOpcode.Dfma:
						case InstructionOpcode.IMad:
						case InstructionOpcode.Mad:
							WriteMad(instruction);
							break;
						case InstructionOpcode.Mov:
							WriteMov(instruction);
							break;
						default:
							WriteIndent();
							WriteLineFormat("// Not Implemented [{0}]: {1}", instruction.Opcode, instruction.AsmDebug);
							break;
					}
					break;
			}
		}
		void WriteCallInstruction(Instruction instruction)
		{
			var name = instruction.Opcode.GetDescription();
			WriteIndent();
			Context.OperandWriter.WriteOperand(instruction.Operands[0]);
			WriteFormat(" = {0}(", name);
			for(int i = 1; i < instruction.Operands.Count; i++)
			{
				Context.OperandWriter.WriteOperand(instruction.Operands[i]);
				if(i < instruction.Operands.Count - 1)
				{
					Write(", ");
				}
			}
			Write(");");
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
		void WriteCallInstructionNoDest(Instruction instruction)
		{
			var name = instruction.Opcode.GetDescription();
			WriteIndent();
			WriteFormat("{0}(", name);
			for(int i = 0; i < instruction.Operands.Count; i++)
			{
				Context.OperandWriter.WriteOperand(instruction.Operands[i]);
				if(i < instruction.Operands.Count - 1)
				{
					Write(", ");
				}
			}
			Write(");");
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
		void WriteBinaryOp(Instruction instruction)
		{
			var name = instruction.Opcode.GetDescription();
			WriteIndent();
			Context.OperandWriter.WriteOperand(instruction.Operands[0]);
			Write(" = ");
			Context.OperandWriter.WriteOperand(instruction.Operands[1]);
			WriteFormat(" {0} ", name);
			Context.OperandWriter.WriteOperand(instruction.Operands[2]);
			Write(";");
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
		void WriteObjectCall(Instruction instruction)
		{
			var name = instruction.Opcode.GetDescription();
			WriteIndent();
			Context.OperandWriter.WriteOperand(instruction.Operands[0]);
			Write(" = ");
			Context.OperandWriter.WriteOperand(instruction.Operands[1]);
			WriteFormat(".{0}(", name);
			for(int i = 2; i < instruction.Operands.Count; i++)
			{
				Context.OperandWriter.WriteOperand(instruction.Operands[i]);
				if(i < instruction.Operands.Count - 1)
				{
					Write(", ");
				}
			}
			Write(");");
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
		void WriteSampleCall(Instruction instruction)
		{
			var name = instruction.Opcode.GetDescription();
			WriteIndent();
			Context.OperandWriter.WriteOperand(instruction.Operands[0]);
			Write(" = ");
			Context.OperandWriter.WriteOperand(instruction.Operands[2]);
			WriteFormat(".{0}(", name);
			for(int i = 3; i < instruction.Operands.Count; i++)
			{
				Context.OperandWriter.WriteOperand(instruction.Operands[i]);
				Write(", ");
			}
			Context.OperandWriter.WriteOperand(instruction.Operands[1]);
			Write(");");
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
		void WriteObjectCallNoDest(Instruction instruction)
		{
			var name = instruction.Opcode.GetDescription();
			WriteIndent();
			Context.OperandWriter.WriteOperand(instruction.Operands[0]);
			WriteFormat(".{0}(", name);
			for(int i = 1; i < instruction.Operands.Count; i++)
			{
				Context.OperandWriter.WriteOperand(instruction.Operands[i]);
				if(i < instruction.Operands.Count - 1)
				{
					Write(", ");
				}
			}
			Write(");");
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
		void WriteControlFlow(Instruction instruction)
		{
			switch(instruction.Opcode)
			{
				case InstructionOpcode.EndIf:
				case InstructionOpcode.EndLoop:
				case InstructionOpcode.EndSwitch:
					DecreaseIndent();
					break;
			}
			WriteIndent();
			switch(instruction.Opcode)
			{
				case InstructionOpcode.MovC:
				case InstructionOpcode.BreakC:
					Write("if(");
					Context.OperandWriter.WriteOperand(instruction.Operands[0]);
					Write(") ");
					break;
			}
			switch(instruction.Opcode)
			{
				case InstructionOpcode.MovC:
					Context.OperandWriter.WriteOperand(instruction.Operands[2]);
					Write(" = ");
					Context.OperandWriter.WriteOperand(instruction.Operands[3]);
					Write(";");
					break;
				case InstructionOpcode.Loop:
					Write("while(true){");
					IncreaseIndent();
					break;
				case InstructionOpcode.If:
					Write("if(?){");
					IncreaseIndent();
					break;
				case InstructionOpcode.Switch:
					Write("switch(");
					Context.OperandWriter.WriteOperand(instruction.Operands[0]);
					Write("){");
					IncreaseIndent();
					break;
				case InstructionOpcode.Case:
					Write("case ");
					Context.OperandWriter.WriteOperand(instruction.Operands[0]);
					Write(":");
					break;
				case InstructionOpcode.Default:
					Write("default:");
					break;
				case InstructionOpcode.BreakC:
				case InstructionOpcode.Break:
					Write("break;");
					break;
				case InstructionOpcode.EndIf:
				case InstructionOpcode.EndLoop:
				case InstructionOpcode.EndSwitch:
					Write("}");
					break;
				case InstructionOpcode.Ret:
					Write("return;");
					break;
				default:
					WriteLineFormat("// Not Implemented {0}", instruction.AsmDebug);
					return;
			}
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
		void WriteMov(Instruction instruction)
		{
			WriteIndent();
			Context.OperandWriter.WriteOperand(instruction.Operands[0]);
			Write(" = ");
			Context.OperandWriter.WriteOperand(instruction.Operands[1]);
			Write(";");
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
		void WriteMad(Instruction instruction)
		{
			WriteIndent();
			Context.OperandWriter.WriteOperand(instruction.Operands[0]);
			Write(" = ");
			Context.OperandWriter.WriteOperand(instruction.Operands[1]);
			WriteFormat(" * ");
			Context.OperandWriter.WriteOperand(instruction.Operands[2]);
			WriteFormat(" + ");
			Context.OperandWriter.WriteOperand(instruction.Operands[3]);
			Write(";");
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
	}
}
