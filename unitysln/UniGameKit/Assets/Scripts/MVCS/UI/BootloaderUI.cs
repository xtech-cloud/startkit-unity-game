using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BootloaderUI: MonoBehaviour
{
	public GameObject root;
	public Text txtProgress;
	public Text txtTip;

    void Awake()
    {
        BootloaderFacade facade = new BootloaderFacade();
        facade.ui = this;
        FacadeStore.Register(BootloaderFacade.NAME, facade);
    }
}
