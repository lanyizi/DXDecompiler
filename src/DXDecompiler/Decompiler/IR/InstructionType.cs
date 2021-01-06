namespace DXDecompiler.Decompiler.IR
{
	public enum InstructionType
	{
		IntrinsicCall,
		IntrinsicCallNoDest,
		MemberCall,
		BinaryOp,
		ObjectCall,
		ObjectCallNoDest,
		SampleCall,
		ControlFlow,
		Misc
	}
}
