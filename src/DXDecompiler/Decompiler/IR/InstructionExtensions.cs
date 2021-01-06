﻿namespace DXDecompiler.Decompiler.IR
{
	public static class InstructionExtensions
	{
		public static InstructionType GetInstructionType(this InstructionOpcode opcode)
		{
			switch(opcode)
			{
				case InstructionOpcode.FirstBitHi:
				case InstructionOpcode.FirstBitSHi:
				case InstructionOpcode.BfRev:
				case InstructionOpcode.Bfi:
				case InstructionOpcode.UBfe:
				case InstructionOpcode.IBfe:
				case InstructionOpcode.Dp2:
				case InstructionOpcode.Dp3:
				case InstructionOpcode.Dp4:
				case InstructionOpcode.Log:
				case InstructionOpcode.Rsq:
				case InstructionOpcode.Exp:
				case InstructionOpcode.Sqrt:
				case InstructionOpcode.RoundPi:
				case InstructionOpcode.RoundNi:
				case InstructionOpcode.RoundZ:
				case InstructionOpcode.RoundNe:
				case InstructionOpcode.Frc:
				case InstructionOpcode.IMax:
				case InstructionOpcode.UMax:
				case InstructionOpcode.DMax:
				case InstructionOpcode.Max:
				case InstructionOpcode.IMin:
				case InstructionOpcode.UMin:
				case InstructionOpcode.DMin:
				case InstructionOpcode.Min:
				case InstructionOpcode.EvalCentroid:
				case InstructionOpcode.EvalSampleIndex:
				case InstructionOpcode.EvalSnapped:
				case InstructionOpcode.Drcp:
				case InstructionOpcode.Rcp:
				case InstructionOpcode.F32ToF16:
				case InstructionOpcode.F16ToF32:
				case InstructionOpcode.INeg:
				case InstructionOpcode.DerivRtx:
				case InstructionOpcode.RtxCoarse:
				case InstructionOpcode.RtxFine:
				case InstructionOpcode.Msad:
				case InstructionOpcode.FirstBitLo:
				case InstructionOpcode.SampleInfo:
				case InstructionOpcode.CheckAccessFullyMapped:
					return InstructionType.IntrinsicCall;
				case InstructionOpcode.Abort:
					return InstructionType.IntrinsicCallNoDest;
				case InstructionOpcode.DAdd:
				case InstructionOpcode.IAdd:
				case InstructionOpcode.Add:
				case InstructionOpcode.DMul:
				case InstructionOpcode.IMul:
				case InstructionOpcode.Mul:
				case InstructionOpcode.UDiv:
				case InstructionOpcode.Ddiv:
				case InstructionOpcode.Div:
				case InstructionOpcode.Or:
				case InstructionOpcode.And:
				case InstructionOpcode.UShr:
				case InstructionOpcode.IShr:
				case InstructionOpcode.IShl:
				case InstructionOpcode.Xor:
				case InstructionOpcode.Ge:
				case InstructionOpcode.Lt:
				case InstructionOpcode.Eq:
				case InstructionOpcode.IGe:
				case InstructionOpcode.ILt:
				case InstructionOpcode.IEq:
				case InstructionOpcode.DLt:
				case InstructionOpcode.DGe:
				case InstructionOpcode.DEq:
					return InstructionType.BinaryOp;
				case InstructionOpcode.LdS:
				case InstructionOpcode.LdMsS:
				case InstructionOpcode.LdMs:
				case InstructionOpcode.LdUavTypedS:
				case InstructionOpcode.LdUavTyped:
				case InstructionOpcode.LdRawS:
				case InstructionOpcode.LdRaw:
					return InstructionType.ObjectCall;
				case InstructionOpcode.AtomicAnd:
				case InstructionOpcode.AtomicOr:
				case InstructionOpcode.AtomicXor:
				case InstructionOpcode.AtomicCmpStore:
				case InstructionOpcode.AtomicIAdd:
				case InstructionOpcode.AtomicIMax:
				case InstructionOpcode.AtomicIMin:
				case InstructionOpcode.AtomicUMax:
				case InstructionOpcode.AtomicUMin:
				case InstructionOpcode.ImmAtomicIAdd:
				case InstructionOpcode.ImmAtomicAnd:
				case InstructionOpcode.ImmAtomicOr:
				case InstructionOpcode.ImmAtomicXor:
				case InstructionOpcode.ImmAtomicExch:
				case InstructionOpcode.ImmAtomicCmpExch:
				case InstructionOpcode.ImmAtomicIMax:
				case InstructionOpcode.ImmAtomicIMin:
				case InstructionOpcode.ImmAtomicUMax:
				case InstructionOpcode.ImmAtomicUMin:
				case InstructionOpcode.ImmAtomicAlloc:
				case InstructionOpcode.ImmAtomicConsume:
				case InstructionOpcode.Bufinfo:
					return InstructionType.ObjectCallNoDest;
				case InstructionOpcode.Loop:
				case InstructionOpcode.EndLoop:
				case InstructionOpcode.Break:
				case InstructionOpcode.BreakC:
				case InstructionOpcode.ContinueC:
				case InstructionOpcode.If:
				case InstructionOpcode.RetC:
				case InstructionOpcode.Else:
				case InstructionOpcode.EndSwitch:
				case InstructionOpcode.EndIf:
				case InstructionOpcode.Continue:
				case InstructionOpcode.Switch:
				case InstructionOpcode.Case:
				case InstructionOpcode.Default:
				case InstructionOpcode.Ret:
				case InstructionOpcode.MovC:
					return InstructionType.ControlFlow;
				case InstructionOpcode.Sample:
				case InstructionOpcode.SampleBClS:
				case InstructionOpcode.SampleB:
				case InstructionOpcode.SampleClS:
				case InstructionOpcode.SampleC:
				case InstructionOpcode.SampleCLzS:
				case InstructionOpcode.SampleCLz:
				case InstructionOpcode.SampleDClS:
				case InstructionOpcode.SampleD:
				case InstructionOpcode.SampleLS:
				case InstructionOpcode.SampleL:
				case InstructionOpcode.SampleCClS:
				case InstructionOpcode.Gather4:
				case InstructionOpcode.Gather4C:
				case InstructionOpcode.Gather4CS:
				case InstructionOpcode.Gather4Po:
				case InstructionOpcode.Gather4PoC:
				case InstructionOpcode.Gather4PoCS:
				case InstructionOpcode.Gather4PoS:
				case InstructionOpcode.Gather4S:
				case InstructionOpcode.Lod:
				case InstructionOpcode.Resinfo:
					return InstructionType.SampleCall;
				default:
					return InstructionType.Misc;
			}
		}
	}
}
