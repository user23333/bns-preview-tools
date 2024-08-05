﻿using System.IO;

using CSCore;

using NVorbis;

namespace Xylia.Preview.UI.Audio;
public class NVorbisSource : ISampleSource
{
	#region Variables
	private readonly Stream stream;
	private readonly VorbisReader vorbisReader;
	private readonly WaveFormat waveFormat;
	private bool disposed;
	#endregion

	#region Constructor
	public NVorbisSource(Stream stream)
	{
		ArgumentNullException.ThrowIfNull(stream);

		if (!stream.CanRead)
		{
			throw new ArgumentException("Stream is not readable.", "stream");
		}

		this.stream = stream;
		this.vorbisReader = new VorbisReader(stream, false);
		this.waveFormat = new WaveFormat(this.vorbisReader.SampleRate, 32, this.vorbisReader.Channels, AudioEncoding.IeeeFloat);
	}
	#endregion

	#region ISampleSource
	public bool CanSeek
	{
		get
		{
			return this.stream.CanSeek;
		}
	}

	public WaveFormat WaveFormat
	{
		get
		{
			return this.waveFormat;
		}
	}

	public long Position
	{
		get
		{
			return Convert.ToInt64(this.vorbisReader.TimePosition.TotalSeconds * this.vorbisReader.SampleRate * this.vorbisReader.Channels);
		}

		set
		{
			if (value < 0 || value > Length)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			this.vorbisReader.TimePosition = TimeSpan.FromSeconds(Convert.ToDouble(value) / this.vorbisReader.SampleRate / this.vorbisReader.Channels);
		}
	}

	public long Length
	{
		get
		{
			return Convert.ToInt64(this.vorbisReader.TotalTime.TotalSeconds * this.waveFormat.SampleRate * this.waveFormat.Channels);
		}
	}

	public int Read(float[] buffer, int offset, int count)
	{
		return this.vorbisReader.ReadSamples(buffer, offset, count);
	}

	public void Dispose()
	{
		if (!this.disposed)
		{
			this.vorbisReader.Dispose();
		}
		//else
		//{
		//    throw new ObjectDisposedException("NVorbisSource");
		//}

		this.disposed = true;
	}
	#endregion
}