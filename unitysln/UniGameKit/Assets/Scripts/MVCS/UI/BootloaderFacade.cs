using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XTC.MVCS;

[System.Serializable]
public class BootloaderUI
{
	public GameObject root;
	public Text txtProgress;
	public Text txtTip;
}

public class BootloaderFacade :  UIFacade
{
	public const string NAME = "BootloaderFacade";

	public BootloaderUI uiBootloader;
}
