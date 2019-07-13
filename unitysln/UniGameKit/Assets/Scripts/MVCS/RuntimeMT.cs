using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using XTC.Logger;

/// Message Translation
public class RuntimeMT : MonoBehaviour
{
	public RuntimeMVCS mvcs {get;set;}

	[DllImport("__Internal")]
    public static extern string QueryUUID();

	public void FecthUUID()
	{
		#if UNITY_WEBGL
		string uuid = QueryUUID();
		this.LogDebug("uuid is {0}", uuid);
		HandleUUID(uuid);
		#endif
	}

    public void HandleUUID(string _uuid)
	{
		mvcs.HandleUUID(_uuid);
	}
}
