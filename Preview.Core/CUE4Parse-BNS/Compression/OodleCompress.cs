using OodleDotNet;
using static CUE4Parse.Compression.OodleHelper;

namespace CUE4Parse.Compression;
public static partial class OodleCompress
{
	public static byte[] Compress(byte[] uncompressed, int uncompressedSize, OodleCompressor format, OodleCompressionLevel level = OodleCompressionLevel.Fast)
	{
		if (Instance is null) throw new OodleException("Oodle compression failed: not initialized");

		int compressedSize = GetCompressionBound(uncompressedSize);
		byte[] compressed = new byte[compressedSize];

		var encodedSize = Instance.Compress(format, level, uncompressed.AsSpan(0, uncompressedSize),
			compressed.AsSpan(0, compressedSize));

		byte[] outputBuffer = new byte[encodedSize];
		Buffer.BlockCopy(compressed, 0, outputBuffer, 0, (int)encodedSize);

		return outputBuffer;
	}

	private static int GetCompressionBound(int bufferSize)
	{
		return bufferSize + 274 * ((bufferSize + 0x3FFFF) / 0x40000);
	}
}
