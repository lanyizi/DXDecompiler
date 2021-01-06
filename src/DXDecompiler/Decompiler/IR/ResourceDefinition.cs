using DXDecompiler.Chunks.Rdef;

namespace DXDecompiler.Decompiler.IR
{
	public class ResourceDefinition
	{
		internal ResourceDefinitionChunk Chunk;
		public ResourceDefinition(ResourceDefinitionChunk chunk)
		{
			Chunk = chunk;
		}
	}
}
