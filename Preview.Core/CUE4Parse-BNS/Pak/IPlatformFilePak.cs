﻿using System.ComponentModel;

namespace CUE4Parse.UE4.Pak;
public class IPlatformFilePak
{
	public static byte[] Signature { get; set; }

	public static void DoSignatureCheck()
	{
#if DEVELOP
	    return;
#endif

		// ComputeHash
		if (Signature == null) throw new WarningException("Invalid signature, please try reopen the application.");
	}
}