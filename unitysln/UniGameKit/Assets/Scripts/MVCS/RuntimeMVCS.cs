using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.MVCS;
using XTC.Types;
using XTC.Logger;

public class RuntimeMVCS : RootMono
{
    private RuntimeMT runtimetMT {get;set;}

    void Awake()
    {
        Debug.Log("---------------  Awake ------------------------");

        GameObject objMT = new GameObject("_MT_");
        runtimetMT = objMT.AddComponent<RuntimeMT>();
        runtimetMT.mvcs = this;

        foreach(Transform child in this.transform.Find("UIFacades"))
        {
            UIFacade facade = child.GetComponent<UIFacade>();
            facade.Register();
        }

        initialize();

        //SampleModel model = new SampleModel();
        //SampleView view = new SampleView();
        //SampleController controller = new SampleController();
        //SampleService service = new SampleService();

        //service.domain = "http://127.0.0.1";
        //service.MockProcessor = this.mockProcessor;
        //service.useMock = true;

        ///framework.modelCenter.Register(SampleModel.NAME, model);
        //framework.viewCenter.Register(SampleView.NAME, view);
        //framework.controllerCenter.Register(SampleController.NAME, controller);
        //framework.serviceCenter.Register(SampleService.NAME, service);
    }

    void OnEnable()
    {
        Debug.Log("---------------  OnEnable ------------------------");
        setup();
    }

    void Start()
    {
        Debug.Log("---------------  Start ------------------------");
        runtimetMT.FecthUUID();
    }

    void OnDisable()
    {
        Debug.Log("---------------  OnDisable ------------------------");
        dismantle();
    }

    void OnDestroy()
    {
        Debug.Log("---------------  OnDestroy ------------------------");

        //framework.modelCenter.Cancel(SampleModel.NAME);
        //framework.viewCenter.Cancel(SampleView.NAME);
        //framework.controllerCenter.Cancel(SampleController.NAME);
        //framework.serviceCenter.Cancel(SampleService.NAME);

        foreach(Transform child in this.transform.Find("UIFacades"))
        {
            UIFacade facade = child.GetComponent<UIFacade>();
            facade.Cancel();
        }

        release();
    }

    public void HandleUUID(string _uuid)
    {
        this.LogDebug("handle uuid: [{0}]", _uuid);
        RuntimeUIFacade facade = (UIFacade.Find(RuntimeUIFacade.NAME) as RuntimeUIFacade);
    }
   
}
