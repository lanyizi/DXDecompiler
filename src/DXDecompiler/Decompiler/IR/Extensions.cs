using DXDecompiler.Chunks.Common;
using System;
using static DXDecompiler.Decompiler.IR.Pass;

namespace DXDecompiler.Decompiler.IR
{
	public static class Extensions
	{
		public static PassType GetPassType(this ProgramType programType)
		{
			switch(programType)
			{
				case ProgramType.VertexShader:
					return PassType.VertexShader;
				case ProgramType.PixelShader:
					return PassType.PixelShader;
				case ProgramType.GeometryShader:
					return PassType.GeometryShader;
				case ProgramType.DomainShader:
					return PassType.DomainShader;
				case ProgramType.HullShader:
					return PassType.HullShaderControlPointPhase;
				case ProgramType.ComputeShader:
					return PassType.ComputeShader;
				default:
					throw new InvalidOperationException($"Could not get PassType for ProgramType {programType}");
			}
		}
	}
}
