﻿using DXDecompiler.Chunks;
using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Rdef;
using DXDecompiler.Decompiler.IR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.Decompiler.Writer
{
	public class ResourceDefinitionWriter : BaseWriter
	{
		bool EmitRegisterDeclarations => true;
		public bool EmitPackingOffset => true;

		public ResourceDefinitionWriter(DecompileContext context) : base(context)
		{
		}

		public void WriteResourceDefinition(ResourceDefinition resourceDefinition)
		{
			var rdef = resourceDefinition.Chunk;
			var seen = new HashSet<string>();
			WriteLine("// ConstantBuffers");
			WriteCBInnerStructs(rdef, ref seen);
			foreach(var constantBuffer in rdef.ConstantBuffers)
			{
				WriteConstantBuffer(rdef, constantBuffer, ref seen);
			}

			WriteLine("// ResourceBindings");
			foreach(var resourceBinding in rdef.ResourceBindings)
			{
				WriteResourceBinding(rdef, resourceBinding);
			}
			WriteLine();
		}

		private void WriteCBInnerStructs(ResourceDefinitionChunk rdef, ref HashSet<string> seen)
		{
			foreach(var cb in rdef.ConstantBuffers)
			{
				foreach(var variable in cb.Variables)
				{
					WriteCBInnerStructs(variable.Member, ref seen);
				}
			}
		}
		private void WriteCBInnerStructs(ShaderTypeMember member, ref HashSet<string> seen)
		{
			if(member.Type.VariableClass != ShaderVariableClass.Struct) return;
			if(seen.Contains(member.Type.BaseTypeName)) return;
			foreach(var child in member.Type.Members)
			{
				WriteCBInnerStructs(child, ref seen);
			}
			if(member.Type.BaseTypeName == null || member.Type.BaseTypeName.EndsWith("<unnamed>")) return;
			seen.Add(member.Type.BaseTypeName);
			WriteLineFormat("{0};", GetShaderTypeDeclaration(member.Type));
		}
		void WriteConstantBuffer(ResourceDefinitionChunk rdef, ConstantBuffer constantBuffer, ref HashSet<string> seen)
		{
			if(constantBuffer.BufferType == ConstantBufferType.ConstantBuffer ||
				constantBuffer.BufferType == ConstantBufferType.TextureBuffer)
			{
				if(constantBuffer.Name == "$Globals")
				{
					WriteLine("// Globals");
					foreach(var variable in constantBuffer.Variables)
					{
						WriteVariable(variable, 0, packOffset: false);
					}
					WriteLine();
				}
				else if(constantBuffer.Name == "$Params")
				{
				}
				else
				{
					var resourceBinding = rdef.ResourceBindings.FirstOrDefault(rb =>
						(rb.Type == ShaderInputType.CBuffer || rb.Type == ShaderInputType.TBuffer) &&
						rb.Name == constantBuffer.Name);
					var bufferName = resourceBinding.Type == ShaderInputType.CBuffer ?
						"cbuffer" :
						"tbuffer";
					Write($"{bufferName} {constantBuffer.Name}");
					if(EmitRegisterDeclarations)
					{
						var registerName = resourceBinding.Type == ShaderInputType.CBuffer ?
							"b" :
							"t";
						Write($" : register({registerName}{resourceBinding.BindPoint})");
					}
					WriteLine();
					WriteLine("{");
					foreach(var variable in constantBuffer.Variables)
					{
						WriteVariable(variable);
					}
					WriteLine("}");
					WriteLine("");
				}
			}
			else if(constantBuffer.BufferType == ConstantBufferType.ResourceBindInformation)
			{
				WriteLine("// ResourceBindInformation");
				var element = constantBuffer.Variables.Single();
				var baseType = element.ShaderType;
				if(baseType.VariableClass != ShaderVariableClass.Struct)
				{
					WriteLine(element.ToString());
					return;
				}
				var typeName = element.ShaderType.BaseTypeName;
				if(string.IsNullOrEmpty(typeName))
				{
					var index = rdef.ConstantBuffers
						.Where(cb => cb.BufferType == ConstantBufferType.ResourceBindInformation)
						.ToList()
						.IndexOf(constantBuffer);
					typeName = $"struct{index}";
				}
				if(seen.Contains(typeName))
				{
					WriteLine($"// {baseType} {typeName}");
				}
				else
				{
					seen.Add(typeName);
					WriteLine(GetShaderTypeDeclaration(baseType, overrideName: typeName));
				}
			}
		}
		string GetTextureTypeName(ResourceBinding resourceBinding)
		{
			switch(resourceBinding.Dimension)
			{
				case ShaderResourceViewDimension.Texture1D:
					return "Texture1D";
				case ShaderResourceViewDimension.Texture1DArray:
					return "Texture1DArray";
				case ShaderResourceViewDimension.Texture2D:
					return "Texture2D";
				case ShaderResourceViewDimension.Texture2DArray:
					return "Texture2DArray";
				case ShaderResourceViewDimension.Buffer:
					return "Buffer";
				case ShaderResourceViewDimension.Texture2DMultiSampled:
					return "Texture2DMS";
				case ShaderResourceViewDimension.Texture2DMultiSampledArray:
					return "Texture2DMSArray";
				case ShaderResourceViewDimension.TextureCube:
					return "TextureCube";
				case ShaderResourceViewDimension.TextureCubeArray:
					return "TextureCubeArray";
				case ShaderResourceViewDimension.Texture3D:
					return "Texture3D";
				default:
					throw new ArgumentException($"Unexpected resource binding  dimension type {resourceBinding.Dimension}");
			}
		}
		string GetUAVTypeName(ResourceBinding resourceBinding)
		{
			switch(resourceBinding.Dimension)
			{
				case ShaderResourceViewDimension.Buffer:
					return "RWBuffer";
				case ShaderResourceViewDimension.Texture1D:
					return "RWTexture1D";
				case ShaderResourceViewDimension.Texture1DArray:
					return "RWTexture1DArray";
				case ShaderResourceViewDimension.Texture2D:
					return "RWTexture2D";
				case ShaderResourceViewDimension.Texture2DArray:
					return "RWTexture2DArray";
				case ShaderResourceViewDimension.Texture3D:
					return "RWTexture3D";
				default:
					throw new ArgumentException($"Unexpected resource binding dimension type {resourceBinding.Dimension}");
			}
		}
		string GetBindingName(ResourceBinding resourceBinding)
		{
			switch(resourceBinding.Type)
			{
				case ShaderInputType.Texture:
					return GetTextureTypeName(resourceBinding);
				case ShaderInputType.Sampler:
					return resourceBinding.Flags.HasFlag(ShaderInputFlags.ComparisonSampler) ?
						"SamplerComparisonState" :
						"SamplerState";
				case ShaderInputType.ByteAddress:
					return "ByteAddressBuffer";
				case ShaderInputType.Structured:
					return "StructuredBuffer";
				case ShaderInputType.UavRwByteAddress:
					return "RWByteAddressBuffer";
				case ShaderInputType.UavRwStructuredWithCounter:
				case ShaderInputType.UavRwStructured:
					return "RWStructuredBuffer";
				case ShaderInputType.UavRwTyped:
					return GetUAVTypeName(resourceBinding);
				case ShaderInputType.UavAppendStructured:
					return "AppendStructuredBuffer";
				case ShaderInputType.UavConsumeStructured:
					return "ConsumeStructuredBuffer";
				default:
					throw new ArgumentException($"Unexpected resource binding type {resourceBinding.Type}");
			}
		}
		void WriteResourceBinding(ResourceDefinitionChunk rdef, ResourceBinding resourceBinding)
		{
			WriteLine(resourceBinding.ToString());
			if(resourceBinding.Type == ShaderInputType.CBuffer || resourceBinding.Type == ShaderInputType.TBuffer)
			{
				return;
			}
			if(resourceBinding.Type.IsUav())
			{
				//TODO: Move this to IR phase
				/*var dcl = Container.Shader.DeclarationTokens
					.OfType<UnorderedAccessViewDeclarationTokenBase>()
					.Single(t => t.Operand.Indices[0].Value == resourceBinding.ID);
				if(dcl.Coherency == UnorderedAccessViewCoherency.GloballyCoherent)
				{
					Output.Append("globallycoherent ");
				}*/
			}
			var type = GetBindingName(resourceBinding);
			Write(type);
			if(resourceBinding.Type == ShaderInputType.Texture || resourceBinding.Type == ShaderInputType.UavRwTyped)
			{
				if(!(resourceBinding.ReturnType == ResourceReturnType.Float &&
					resourceBinding.Flags.HasFlag(ShaderInputFlags.TextureComponents) &&
					!resourceBinding.Dimension.IsMultiSampled()) || resourceBinding.Type == ShaderInputType.UavRwTyped)
				{
					string returnType = GetReturnTypeDescription(resourceBinding.ReturnType);
					if(resourceBinding.Flags.HasFlag(ShaderInputFlags.TextureComponent0) && !resourceBinding.Flags.HasFlag(ShaderInputFlags.TextureComponent1))
						returnType += "2";
					if(!resourceBinding.Flags.HasFlag(ShaderInputFlags.TextureComponent0) && resourceBinding.Flags.HasFlag(ShaderInputFlags.TextureComponent1))
						returnType += "3";
					if(resourceBinding.Flags.HasFlag(ShaderInputFlags.TextureComponent0) && resourceBinding.Flags.HasFlag(ShaderInputFlags.TextureComponent1))
						returnType += "4";
					if(resourceBinding.Dimension.IsMultiSampled() && resourceBinding.NumSamples > 0)
					{
						WriteFormat("<{0}, {1}>", returnType, resourceBinding.NumSamples);
					}
					else
					{
						WriteFormat("<{0}>", returnType);
					}
				}
			}
			if(resourceBinding.Type.IsStructured())
			{
				var typeName = GetStructuredTypeName(rdef, resourceBinding);
				WriteFormat("<{0}>", typeName);
			}
			Write($" {resourceBinding.Name}");
			if(EmitRegisterDeclarations)
			{
				Write($" : register({resourceBinding.GetBindPointDescription()})");
			}
			WriteLine($";");
		}
		string GetStructuredTypeName(ResourceDefinitionChunk rdef, ResourceBinding resourceBinding)
		{
			var buffer = rdef.ConstantBuffers.Single(cb =>
				cb.BufferType == ConstantBufferType.ResourceBindInformation
				&& cb.Name == resourceBinding.Name);
			var typeName = "";
			if(buffer.Variables[0].ShaderType.VariableClass == ShaderVariableClass.Struct)
			{
				typeName = buffer.Variables[0].ShaderType.BaseTypeName;
				if(string.IsNullOrEmpty(typeName))
				{
					var index = rdef.ConstantBuffers
						.Where(cb => cb.BufferType == ConstantBufferType.ResourceBindInformation)
						.ToList()
						.FindIndex(cb => cb == buffer);
					typeName = $"struct{index}";
				}
				typeName = $"struct {typeName}";
			}
			else
			{
				typeName = GetShaderTypeName(buffer.Variables[0].ShaderType);
			}
			return typeName;
		}
		string GetReturnTypeDescription(ResourceReturnType resourceReturnType)
		{
			switch(resourceReturnType)
			{
				case ResourceReturnType.SInt:
					return "int";
				case ResourceReturnType.UNorm:
					return "unorm float";
				case ResourceReturnType.SNorm:
					return "snorm float";
				default:
					return resourceReturnType.GetDescription();
			}
		}
		void WriteVariable(ShaderVariable variable, int indentLevel = 1, bool packOffset = true)
		{
			Write($"{GetShaderTypeDeclaration(variable.ShaderType, indentLevel, root: false)} {variable.Name}");
			if(variable.ShaderType.ElementCount > 0)
			{
				WriteFormat("[{0}]", variable.ShaderType.ElementCount);
			}
			if(variable.DefaultValue != null)
			{
				if(variable.DefaultValue.Count > 1)
				{
					WriteFormat(" = {0}({1})",
						GetShaderTypeDeclaration(variable.ShaderType),
						string.Join(", ", variable.DefaultValue));
				}
				else
				{
					WriteFormat(" = {0}", variable.DefaultValue[0]);
				}
			}
			// packoffset needs to be disabled for globals
			if(EmitPackingOffset && packOffset)
			{
				var componentOffset = variable.StartOffset % 16 / 4;
				string componentPacking = "";
				switch(componentOffset)
				{
					case 1:
						componentPacking = ".y";
						break;
					case 2:
						componentPacking = ".z";
						break;
					case 3:
						componentPacking = ".w";
						break;
				}
				WriteFormat(" : packoffset(c{0}{1})", variable.StartOffset / 16, componentPacking);
			}
			Write($"; // Offset {variable.StartOffset} Size {variable.Size} CBSize {variable.Member.GetCBVarSize(true)}");
			if(variable.Flags.HasFlag(ShaderVariableFlags.Used))
			{
				Write($" [unused]");
			}
			WriteLine();
		}
		internal string GetShaderTypeName(ShaderType variable)
		{
			switch(variable.VariableClass)
			{
				case ShaderVariableClass.InterfacePointer:
					{
						if(!string.IsNullOrEmpty(variable.BaseTypeName)) // BaseTypeName is only populated in SM 5.0
						{
							return variable.BaseTypeName;
						}
						else
						{
							return string.Format("{0}{1}",
								variable.VariableClass.GetDescription(),
								variable.VariableType.GetDescription());
						}
					}
			}
			return variable.ToString().Replace("//", "").Trim(); // TODO: Cleanup
		}
		string GetShaderTypeDeclaration(ShaderType type, int indent = 0, string overrideName = null, bool root = true)
		{
			var sb = new StringBuilder();
			string indentString = new string(' ', indent * 4);
			string baseTypeName = overrideName == null ? type.BaseTypeName : overrideName;
			switch(type.VariableClass)
			{
				case ShaderVariableClass.InterfacePointer:
				case ShaderVariableClass.MatrixColumns:
				case ShaderVariableClass.MatrixRows:
					{
						sb.Append(indentString);
						if(!string.IsNullOrEmpty(type.BaseTypeName)) // BaseTypeName is only populated in SM 5.0
						{
							sb.Append(string.Format("{0}{1}", type.VariableClass.GetDescription(), baseTypeName));
						}
						else
						{
							sb.Append(type.VariableClass.GetDescription());
							sb.Append(type.VariableType.GetDescription());
							if(type.Columns > 1)
							{
								sb.Append(type.Columns);
								if(type.Rows > 1)
									sb.Append("x" + type.Rows);
							}
						}
						break;
					}
				case ShaderVariableClass.Vector:
					{
						sb.Append(indentString + type.VariableType.GetDescription());
						sb.Append(type.Columns);
						break;
					}
				case ShaderVariableClass.Struct:
					{
						//SM4 doesn't include typenames, check by signature
						//TODO
						if(root || baseTypeName == null || baseTypeName.EndsWith("<unnamed>"))
						{
							sb.Append(indentString + "struct ");
							if(baseTypeName == null || baseTypeName.EndsWith("<unnamed>"))
							{
							}
							else
							{
								sb.Append(baseTypeName);
							}
							sb.AppendLine("");
							sb.AppendLine(indentString + "{");
							foreach(var member in type.Members)
								sb.AppendLine(GetShaderMemberDeclaration(member, indent + 1));
							sb.Append(indentString + "}");
						}
						else
						{
							sb.Append(indentString + "struct " + baseTypeName); //struct keyword optional
						}
						break;
					}
				case ShaderVariableClass.Scalar:
					{
						sb.Append(indentString + type.VariableType.GetDescription());
						break;
					}
				default:
					throw new InvalidOperationException(string.Format("Variable class '{0}' is not currently supported.", type.VariableClass));
			}
			return sb.ToString();
		}
		string GetShaderMemberDeclaration(ShaderTypeMember member, int indent)
		{
			string declaration = GetShaderTypeDeclaration(member.Type, indent, root: false) + " " + member.Name;
			if(member.Type.ElementCount > 0)
				declaration += string.Format("[{0}]", member.Type.ElementCount);
			declaration += ";";
			declaration += $" // Offset {member.Offset} CBSize {member.GetCBVarSize(true)}";
			return declaration;
		}
	}
}
