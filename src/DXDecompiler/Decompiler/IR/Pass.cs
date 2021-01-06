using DXDecompiler.Chunks.Libf;
using DXDecompiler.Chunks.Shex;
using DXDecompiler.Chunks.Shex.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.Decompiler.IR
{
	public class Pass
	{
		public enum PassType
		{
			PixelShader,
			VertexShader,
			GeometryShader,
			DomainShader,
			ComputeShader,
			FunctionBody,
			HullShaderControlPointPhase,
			HullShaderPatchConstantPhase,
			HullShaderForkPhase,
			HullShaderJoinPhase,
		}
		internal List<DeclarationToken> Declarations = new List<DeclarationToken>();
		public List<Instruction> Instructions = new List<Instruction>();
		public List<Attribute> Attributes = new List<Attribute>();
		public string Name;
		public PassType Type;
		public Signature InputSignature;
		public Signature OutputSignature;
		public Pass(string name, PassType type)
		{
			Name = name;
			Type = type;
		}
		public Pass(LibfChunk libraryChunk, string name)
		{
			Name = name;
			ParseInstructions(libraryChunk.LibraryContainer.Shader.InstructionTokens);
		}
		public Pass(ShaderProgramChunk programChunk)
		{
			Name = programChunk.Version.ProgramType.ToString();
			ParseInstructions(programChunk.InstructionTokens);
		}
		public Pass(List<OpcodeToken> instructions, string name, PassType type)
		{
			Name = name;
			Type = type;
			ParseDeclarations(instructions.OfType<DeclarationToken>());
			ParseInstructions(instructions.OfType<InstructionToken>());
		}
		private void ParseDeclarations(IEnumerable<DeclarationToken> declarations)
		{
			Declarations = new List<DeclarationToken>(declarations);
			foreach(var token in declarations)
			{
				switch(token.Header.OpcodeType)
				{
					case OpcodeType.DclTessDomain:
						{
							var decl = token as TessellatorDomainDeclarationToken;
							Attributes.Add(Attribute.Create("domain", decl.Domain.GetAttributeName()));
						}
						break;
					case OpcodeType.DclMaxOutputVertexCount:
						{
							var decl = token as GeometryShaderMaxOutputVertexCountDeclarationToken;
							Attributes.Add(Attribute.Create("maxvertexcount", decl.MaxPrimitives));
						}
						break;
					case OpcodeType.DclOutputControlPointCount:
						{
							var decl = token as ControlPointCountDeclarationToken;
							Attributes.Add(Attribute.Create("outputcontrolpoints", decl.ControlPointCount));
						}
						break;
					case OpcodeType.DclTessPartitioning:
						{
							var decl = token as TessellatorPartitioningDeclarationToken;
							Attributes.Add(Attribute.Create("partitioning", decl.Partitioning.ToString().ToLower()));
						}
						break;
					case OpcodeType.DclHsMaxTessFactor:
						{
							var decl = token as HullShaderMaxTessFactorDeclarationToken;
							Attributes.Add(Attribute.Create("maxtessfactor", decl.MaxTessFactor));
						}
						break;
					case OpcodeType.DclTessOutputPrimitive:
						{
							var decl = token as TessellatorOutputPrimitiveDeclarationToken;
							Attributes.Add(Attribute.Create("outputtopology", decl.OutputPrimitive.ToString().ToLower()));
						}
						break;
					case OpcodeType.DclThreadGroup:
						{
							var decl = token as ThreadGroupDeclarationToken;
							Attributes.Add(Attribute.Create("numthreads",
								decl.Dimensions[0],
								decl.Dimensions[1],
								decl.Dimensions[2]));
						}
						break;
				}
			}
		}
		private void ParseInstructions(IEnumerable<InstructionToken> instructions)
		{
			foreach(var token in instructions)
			{
				Instructions.Add(new Instruction(token));
			}
		}
	}
}
