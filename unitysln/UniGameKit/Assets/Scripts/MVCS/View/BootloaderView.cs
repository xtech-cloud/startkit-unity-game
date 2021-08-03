using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XTC.oelMVCS;
using XTC.Text;

public class BootloaderView : View
{
	public const string NAME = "BootloaderView";

    private BootloaderFacade facade_ {get;set;}

    protected override void preSetup()
    {
        facade_ = FacadeStore.Find(BootloaderFacade.NAME) as BootloaderFacade; 
        Debug.Log(facade_);
    }

	protected override void setup()
	{
        getLogger().Info("setup BootloaderView");
		facade_.ui.root.gameObject.SetActive(true);
		facade_.ui.txtTip.text = "";
	}

	protected override void dismantle()
	{

	}

	public void SetActive(bool _active)
	{
		facade_.ui.root.SetActive(_active);
	}

	public void RefreshTip(string _tip)
    {
		facade_.ui.txtTip.text = Translator.Translate(_tip);
    }

	public void RefreshProgress(float _value)
    {
		facade_.ui.txtProgress.text = ((int)(_value *100)).ToString();
    }
}
