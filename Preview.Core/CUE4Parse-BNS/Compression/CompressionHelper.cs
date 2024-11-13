using System.IO.Compression;
using CUE4Parse.UE4.Exceptions;
using K4os.Compression.LZ4;
using OodleDotNet;
using Xylia.Preview.Properties;
using ZlibngDotNet;

namespace CUE4Parse.Compression;
public static partial class CompressionHelper
{
	public static byte[] Compress(byte[] uncompressed, int uncompressedSize, CompressionMethod method, int compressionLevel)
	{
		switch (method)
		{
			case CompressionMethod.None:
				return uncompressed;

			case CompressionMethod.Zlib:
			{
				//var srcStream = new MemoryStream();
				//var zlib = new ZlibStream(srcStream, CompressionMode.Compress, (CompressionLevel)compressionLevel, true);
				//zlib.Write(uncompressed, 0, uncompressedSize);
				//zlib.Flush();
				//zlib.Close();
				//return srcStream.ToArray();

				return ZlibCompress.Compress(uncompressed, uncompressedSize, (ZlibngCompressionLevel)compressionLevel);
			}

			case CompressionMethod.Gzip:
			{
				var srcStream = new MemoryStream();
				var gzip = new GZipStream(srcStream, CompressionMode.Compress);
				gzip.Write(uncompressed, 0, uncompressedSize);
				gzip.Flush();
				gzip.Dispose();

				return srcStream.ToArray();
			}

			case CompressionMethod.Oodle:
				return OodleCompress.Compress(uncompressed, uncompressedSize, OodleCompressor.Kraken, (OodleCompressionLevel)compressionLevel);

			case CompressionMethod.LZ4:
			{
				var compressedBuffer = new byte[0];

				LZ4Codec.Encode(uncompressed, compressedBuffer, (LZ4Level)compressionLevel);
				return compressedBuffer;
			}

			default: throw new UnknownCompressionMethodException($"Compression method \"{method}\" is unknown");
		}
	}


	public static void InitOodle()
	{
		var oodlePath = Path.Combine(Settings.ApplicationData, OodleHelper.OODLE_DLL_NAME);
		if (File.Exists(OodleHelper.OODLE_DLL_NAME))
		{
			File.Move(OodleHelper.OODLE_DLL_NAME, oodlePath, true);
		}
		else if (!File.Exists(oodlePath))
		{
			OodleHelper.DownloadOodleDll(oodlePath);
		}

		OodleHelper.Initialize(oodlePath);
	}

	public static void InitZlib()
	{
		var zlibPath = Path.Combine(Settings.ApplicationData, ZlibHelper.DLL_NAME);
		if (!File.Exists(zlibPath))
		{
			ZlibHelper.DownloadDll(zlibPath);
		}

		ZlibHelper.Initialize(zlibPath);
	}
}