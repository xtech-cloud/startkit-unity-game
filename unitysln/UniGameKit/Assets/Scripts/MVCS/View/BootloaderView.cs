using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XTC.MVCS;
using XTC.Text;

public class BootloaderView : View
{
	public const string NAME = "BootloaderView";

	private BootloaderUI uiBootloader{
		get{
			return (UIFacade.Find(BootloaderFacade.NAME) as BootloaderFacade).uiBootloader;
		}
	}

	protected override void setup()
	{
		uiBootloader.root.gameObject.SetActive(true);
		uiBootloader.txtTip.text = "";
	}

	protected override void bindEvents()
	{
	}

	protected override void unbindEvents()
	{
	}

	protected override void dismantle()
	{

	}

	public void SetActive(bool _active)
	{
		uiBootloader.root.SetActive(_active);
	}

	public void RefreshTip(string _tip)
    {
		uiBootloader.txtTip.text = Translator.Translate(_tip);
    }

	public void RefreshProgress(float _value)
    {
		uiBootloader.txtProgress.text = ((int)(_value *100)).ToString();
    }
}
