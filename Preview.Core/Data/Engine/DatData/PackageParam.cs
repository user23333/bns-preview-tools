using System.Security.Cryptography;
using CUE4Parse.Compression;

namespace Xylia.Preview.Data.Engine.DatData;
public class PackageParam(string path, bool? bit64 = null)
{
	public string FolderPath;
	public string PackagePath = path;
	public bool Bit64 = bit64 ?? Path.GetFileNameWithoutExtension(path).Contains("64");
	public CompressionLevel CompressionLevel = CompressionLevel.Normal;
	public CompressionMethod CompressionMethod = CompressionMethod.Zlib;
	public BinaryXmlVersion BinaryXmlVersion = BinaryXmlVersion.Version4;
	public byte[] AES_KEY = Constants.AES_2020_05;
	public byte[] XOR_KEY = Constants.XOR_KEY_2021;
	public RSAParameters RSA_KEY = Constants.RSA3;
}

public enum CompressionLevel
{
	Fastest,
	Fast,
	Normal,
	Maximum
}

public enum BinaryXmlVersion
{
	None = -1,
	Version3,
	Version4
}