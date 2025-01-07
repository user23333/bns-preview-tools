using System.Diagnostics;
using System.IO;
using CSCore;

namespace Xylia.Preview.UI.Audio;
[DebuggerDisplay("{Name}")]
public class AudioFile(byte[] data, string extension)
{
	#region Fields
	public byte[] Data { get; } = data;
	public string Extension { get; } = extension;

	public long Length => Data.Length;
	public string Path { get; set; }
	public string Name { get; set; }
	#endregion

	#region Constructors
	public AudioFile(FileInfo fileInfo) : this(
		File.ReadAllBytes(fileInfo.FullName),
		fileInfo.Extension[1..])
	{
		Path = fileInfo.FullName.Replace('\\', '/');
		Name = fileInfo.Name;
	}

	public AudioFile(AudioFile file, IAudioSource wave) : this(file.Data, file.Extension)
	{
		Path = file.Path;
		Name = file.Name;
		//Encoding = wave.WaveFormat.WaveFormatTag;
		//BytesPerSecond = wave.WaveFormat.BytesPerSecond;
	}
	#endregion
}