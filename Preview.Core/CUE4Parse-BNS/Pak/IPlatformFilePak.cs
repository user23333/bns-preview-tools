using System.ComponentModel;

namespace CUE4Parse.UE4.Pak;
#pragma warning disable CA2211
public class IPlatformFilePak
{
	public static byte[] Signature;

	public static void DoSignatureCheck()
	{
#if DEVELOP
	    return;
#endif

		// ComputeHash
		if (Signature == null) throw new WarningException("Invalid signature, please try reopen the application.");
	}
}
#pragma warning restore CA2211