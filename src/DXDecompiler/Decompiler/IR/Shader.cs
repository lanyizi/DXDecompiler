using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Fx10;
using DXDecompiler.Chunks.Libf;
using DXDecompiler.Chunks.Shex;
using DXDecompiler.Chunks.Shex.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.Decompiler.IR
{
	public class Shader
	{
		internal BytecodeContainer container;
		public List<Pass> Passes = new List<Pass>();
		public Effect Effect;
		public ResourceDefinition ResourceDefinition;
		public InterfaceManger InterfaceManger;
		public List<Signature> Signatures = new List<Signature>();
		public Shader()
		{
		}
		public Shader(BytecodeContainer container)
		{
			this.container = container;
			if(container.Version.ProgramType == ProgramType.LibraryShader)
			{
				ParseLibrary();
				return;
			}
			if(container.Version.ProgramType == ProgramType.EffectsShader)
			{
				ParseEffect();
				return;
			}
			ResourceDefinition = new ResourceDefinition(container.ResourceDefinition);
			if(container.Interfaces != null)
			{
				InterfaceManger = new InterfaceManger(container.Interfaces);
			}
			if(container.InputSignature != null)
			{
				Signatures.Add(new Signature(
					container.InputSignature,
					$"{container.Version.ProgramType}Input",
					SignatureType.Input));
			}
			if(container.OutputSignature != null)
			{
				Signatures.Add(new Signature(
					container.OutputSignature,
					$"{container.Version.ProgramType}Output",
					SignatureType.Output));
			}
			ParseShaderChunk(container.Shader);
		}
		void ParseLibrary()
		{
			var libraryHeader = container.Chunks.OfType<LibHeaderChunk>().Single();
			var libraryFunctions = container.Chunks.OfType<LibfChunk>().ToArray();
			for(int i = 0; i < libraryFunctions.Length; i++)
			{
				Passes.Add(new Pass(libraryFunctions[0], libraryHeader.FunctionDescs[i].Name));
			}
		}
		void ParseEffect()
		{
			var effect = container.Chunks.OfType<EffectChunk>().Single();
			Effect = new Effect(effect);
			foreach(var variable in effect.LocalVariables)
			{
				foreach(var shader in variable.ShaderData)
				{
					var chunk = shader.Shader?.Shader;
					if(chunk == null) continue;
					Passes.Add(new Pass(chunk));
				}
				foreach(var shader in variable.ShaderData5)
				{
					var chunk = shader.Shader.Shader;
					Passes.Add(new Pass(chunk));
				}
			}
		}
		/// <summary>
		/// Split instructions into seperate phases.
		/// </summary>
		/// <param name="programChunk"></param>
		void ParseShaderChunk(ShaderProgramChunk programChunk)
		{
			Func<OpcodeToken, int, bool> NotSplitToken = (OpcodeToken token, int index) =>
				index == 0 ||
				token.Header.OpcodeType != OpcodeType.HsForkPhase &&
				token.Header.OpcodeType != OpcodeType.HsJoinPhase &&
				token.Header.OpcodeType != OpcodeType.Label;
			int i = 0;
			while(i < programChunk.Tokens.Count)
			{
				var tokens = programChunk.Tokens.Skip(i).TakeWhile(NotSplitToken).ToList();
				if(i == 0)
				{
					var type = programChunk.Version.ProgramType.GetPassType();
					var pass = new Pass(tokens, programChunk.Version.ProgramType.ToString(), type);
					pass.InputSignature = Signatures.First(s => s.SignatureType == SignatureType.Input);
					pass.OutputSignature = Signatures.First(s => s.SignatureType == SignatureType.Output);
					Passes.Add(pass);
				}
				else if(tokens[0].Header.OpcodeType == OpcodeType.Label)
				{
					var index = (tokens[0] as InstructionToken).Operands[0].Indices[0].Value;
					Passes.Add(new Pass(tokens, $"Label{index}", Pass.PassType.FunctionBody));
				}
				else if(tokens[0].Header.OpcodeType == OpcodeType.HsForkPhase)
				{
					var index = Passes.Select(p => p.Type == Pass.PassType.HullShaderForkPhase).Count();
					Passes.Add(new Pass(tokens, $"HullForkPhase{index}", Pass.PassType.HullShaderForkPhase));
				}
				else
				{
					var index = Passes.Select(p => p.Type == Pass.PassType.HullShaderJoinPhase).Count();
					Passes.Add(new Pass(tokens, $"HullJoinPhase{index}", Pass.PassType.HullShaderJoinPhase));
				}
				i += tokens.Count;
			}
			InterfaceManger?.Parse(this);
			if(programChunk.Version.ProgramType == ProgramType.HullShader)
			{
				string patchConstantPassName = "HullPatchConstant";
				Passes.Add(new Pass("HullPatchConstant", Pass.PassType.HullShaderPatchConstantPhase));
				var ctrlPointPass = Passes.First(p => p.Type == Pass.PassType.HullShaderControlPointPhase);
				ctrlPointPass.Attributes.Add(Attribute.Create("patchconstantfunc", patchConstantPassName));
			}
		}
	}
}
