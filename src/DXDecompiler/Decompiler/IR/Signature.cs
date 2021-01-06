using DXDecompiler.Chunks.Xsgn;

namespace DXDecompiler.Decompiler.IR
{
	public class Signature
	{

		internal InputOutputSignatureChunk Chunk;
		public string Name;
		public SignatureType SignatureType;
		public Signature(InputOutputSignatureChunk chunk, string name, SignatureType signatureType)
		{
			Name = name;
			Chunk = chunk;
			SignatureType = signatureType;
		}
	}
}
