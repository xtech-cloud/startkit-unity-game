using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XTC.oelMVCS;


public class BootloaderFacade :  View.Facade
{
	public const string NAME = "BootloaderFacade";
    public BootloaderUI ui {get;set;}
}
