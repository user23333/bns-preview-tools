using CSCore.CoreAudioAPI;

namespace Xylia.Preview.UI.Audio;
public class AudioDevice(MMDevice device , string name, string deviceId)
{
	public MMDevice Device { get; } = device;

	public string DeviceId { get; } = deviceId;

	public string Name { get; } = name;


	public override string ToString()
	{
		return this.Name;
	}

	public override bool Equals(object obj)
	{
		if (obj == null || !GetType().Equals(obj.GetType()))
		{
			return false;
		}

		return this.DeviceId.Equals(((AudioDevice)obj).DeviceId);
	}

	public override int GetHashCode()
	{
		return new { this.DeviceId }.GetHashCode();
	}
}