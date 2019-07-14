using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.MVCS;
using XTC.Logger;
using XTC.Text;

public class RuntimeMVCS : RootMono
{
    public XRProxy proxyXR;
    private LuaProxy proxyLua;

    private RuntimeMT runtimetMT { get; set; }
    private BootloaderBatchController controllerBootloader {get;set;}
    private BootloaderModel modelBootloader {get;set;}

    void Awake()
    {
        Debug.Log("---------------  Awake ------------------------");
        
        proxyLua = new LuaProxy();
        proxyLua.AddSearchPath(Application.persistentDataPath);
        string lua = proxyLua.ReadFile(System.IO.Path.Combine(Application.streamingAssetsPath, "root.lua"));
        proxyLua.UseRootCode(lua);

        proxyXR.DoAwake();

        GameObject objMT = new GameObject("_MT_");
        runtimetMT = objMT.AddComponent<RuntimeMT>();
        runtimetMT.mvcs = this;

        foreach (Transform child in this.transform.Find("UIFacades"))
        {
            UIFacade facade = child.GetComponent<UIFacade>();
            facade.Register();
        }

        initialize();

        // bootloader
        BootloaderView viewBootloader = new BootloaderView();
        framework.viewCenter.Register(BootloaderView.NAME, viewBootloader);
        controllerBootloader = new BootloaderBatchController();
        controllerBootloader.onFinish = runRom;
        framework.controllerCenter.Register(BootloaderBatchController.NAME, controllerBootloader);
        modelBootloader = new BootloaderModel();
        framework.modelCenter.Register(BootloaderModel.NAME, modelBootloader);

        proxyLua.DoAwake();
    }

    void OnEnable()
    {
        Debug.Log("---------------  OnEnable ------------------------");
        proxyXR.DoOnEnable();
        setup();
        proxyLua.DoOnEnable();
    }

    void Start()
    {
        Debug.Log("---------------  Start ------------------------");
        proxyXR.DoStart();

        mergeLanguageFiles();

        proxyLua.DoStart();
        executeBootloader();
    }

    void Update()
    {
        proxyXR.DoUpdate();
        proxyLua.DoUpdate();
    }

    void OnDisable()
    {
        Debug.Log("---------------  OnDisable ------------------------");
        proxyLua.DoOnDisable();
        proxyXR.DoOnDisable();
        dismantle();
    }

    void OnDestroy()
    {
        Debug.Log("---------------  OnDestroy ------------------------");
        proxyLua.DoOnDestroy();
        proxyXR.DoOnDestroy();

        //framework.modelCenter.Cancel(SampleModel.NAME);
        //framework.viewCenter.Cancel(SampleView.NAME);
        //framework.controllerCenter.Cancel(SampleController.NAME);
        //framework.serviceCenter.Cancel(SampleService.NAME);

        foreach (Transform child in this.transform.Find("UIFacades"))
        {
            UIFacade facade = child.GetComponent<UIFacade>();
            facade.Cancel();
        }

        release();
    }

    public void HandleUUID(string _uuid)
    {
        this.LogDebug("handle uuid: [{0}]", _uuid);
        controllerBootloader.FinishCurrentStep();
    }

    private void executeBootloader()
    {
        List<BootloaderModel.Step> steps = new List<BootloaderModel.Step>();
        // fetch uuid
        {
            BootloaderModel.Step step = new BootloaderModel.Step();
            step.name = Constant.BootloaderStep.Fetch;
            step.length = 1;
            step.tip = "bootloader_step_fetch";
            step.onExecute = () =>
            {
                runtimetMT.FecthUUID();
            };
            steps.Add(step);
        }

        // load rom
        {
            BootloaderModel.Step step = new BootloaderModel.Step();
            step.name = Constant.BootloaderStep.Load;
            step.length = 10;
            step.tip = "bootloader_step_load";
            step.onExecute = () =>
            {
                this.StartCoroutine(load());
            };
            steps.Add(step);
        }

        // run rom
        {
            BootloaderModel.Step step = new BootloaderModel.Step();
            step.name = Constant.BootloaderStep.Run;
            step.length = 1;
            step.tip = "bootloader_step_run";
            step.onExecute = () =>
            {
                if (XRProxy.VRMode.OFF == proxyXR.modeVR)
                    Camera.main.clearFlags = CameraClearFlags.Skybox;
                else
                    proxyXR.ResetCameraClearFlags();
                controllerBootloader.FinishCurrentStep();
            };
            steps.Add(step);
        }

        modelBootloader.SaveSteps(steps);
        controllerBootloader.Execute();
    }

    private IEnumerator load()
    {
        yield return new WaitForSeconds(3);
        controllerBootloader.FinishCurrentStep();
    }

    private void runRom()
    {
        proxyLua.Execute("print('run rom')");
    }

    private void mergeLanguageFiles()
    {
        SystemLanguage systemLanguage =  Application.systemLanguage;
        string defaultLanguage = "zh_CN";
        if(systemLanguage == SystemLanguage.English)
            defaultLanguage = "en_US";

        string language = PlayerPrefs.GetString(Constant.CustomSettings.Language, defaultLanguage);
		Translator.language = language;

        Translator.MergeFromResource("Translator/UI", true);
    }


}
