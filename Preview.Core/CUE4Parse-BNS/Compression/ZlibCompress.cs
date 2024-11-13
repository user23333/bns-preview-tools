using ZlibngDotNet;
using static CUE4Parse.Compression.ZlibHelper;

namespace CUE4Parse.Compression;
public static partial class ZlibCompress
{
	public static byte[] Compress(byte[] uncompressed, int uncompressedSize, ZlibngCompressionLevel level = ZlibngCompressionLevel.BestSpeed)
	{
		if (Instance is null) throw new OodleException("Zlib compression failed: not initialized");

		int compressedSize = uncompressedSize;
		byte[] compressed = new byte[compressedSize];

		var encodedSize = Instance.Compress2(uncompressed.AsSpan(0, uncompressedSize),
		   compressed.AsSpan(0, compressedSize), level);

		byte[] outputBuffer = new byte[encodedSize];
		Buffer.BlockCopy(compressed, 0, outputBuffer, 0, encodedSize);

		return outputBuffer;
	}
}