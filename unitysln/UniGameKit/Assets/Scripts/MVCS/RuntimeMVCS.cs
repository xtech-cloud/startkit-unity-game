using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.oelMVCS;
using XTC.Logger;
using XTC.Text;

public class RuntimeMVCS : MonoBehaviour
{
    public XRProxy proxyXR;
    private LuaProxy proxyLua;

    private RuntimeMT runtimetMT
    {
        get;
        set;
    }
    private BootloaderModel modelBootloader
    {
        get;
        set;
    }

    private Framework framework_
    {
        get;
        set;
    }

    void Awake()
    {
        Debug.Log("---------------  Awake ------------------------");

        GameObject objMT = new GameObject("_MT_");
        runtimetMT = objMT.AddComponent<RuntimeMT>();
        runtimetMT.mvcs = this;

        Debug.Log("############# XR-Proxy");
        proxyXR.DoAwake();

        Debug.Log("############# MVCS");
        UnityLogger logger = new UnityLogger();
        logger.setLevel(LogLevel.ALL);
        framework_ = new Framework();
        framework_.setLogger(logger);
        framework_.Initialize();
        registerMVCS();


        Debug.Log("############# Lua-Proxy");
        proxyLua = new LuaProxy();
        proxyLua.AddSearchPath(Application.persistentDataPath);
        string lua = proxyLua.ReadFile(System.IO.Path.Combine(Application.streamingAssetsPath, "root.lua"));
        proxyLua.UseRootCode(lua);
        proxyLua.DoAwake();
    }

    void OnEnable()
    {
        Debug.Log("---------------  OnEnable ------------------------");
        proxyXR.DoOnEnable();
        proxyLua.DoOnEnable();
    }

    void Start()
    {
        Debug.Log("---------------  Start ------------------------");
        proxyXR.DoStart();
        mergeLanguageFiles();
        proxyLua.DoStart();
        framework_.Setup();
        modelBootloader.SaveOnFinishCallback(runRom);
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
    }

    void OnDestroy()
    {
        Debug.Log("---------------  OnDestroy ------------------------");
        proxyLua.DoOnDestroy();
        proxyXR.DoOnDestroy();

        framework_.Dismantle();
        cancelMVCS();
        framework_.Release();
    }

    public void HandleUUID(string _uuid)
    {
        this.LogDebug("handle uuid: [{0}]", _uuid);
        modelBootloader.UpdateFinishCurrentStep();
    }

    private void executeBootloader()
    {
        List<BootloaderModel.Step> steps = new List<BootloaderModel.Step>();
        // fetch uuid
        {
            BootloaderModel.Step step = new BootloaderModel.Step();
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
            step.length = 10;
            step.tip = "bootloader_step_load";
            step.onExecute = () =>
            {
                asyncLoad();
            };
            steps.Add(step);
        }

        // run rom
        {
            BootloaderModel.Step step = new BootloaderModel.Step();
            step.length = 1;
            step.tip = "bootloader_step_run";
            step.onExecute = () =>
            {
                if (XRProxy.VRMode.OFF == proxyXR.modeVR)
                    Camera.main.clearFlags = CameraClearFlags.Skybox;
                else
                    proxyXR.ResetCameraClearFlags();
                modelBootloader.UpdateFinishCurrentStep();
            };
            steps.Add(step);
        }

        modelBootloader.SaveSteps(steps);
        modelBootloader.UpdateExecute();
    }

    private async void asyncLoad()
    {
        var operation = Task.Run(() =>
        {
            Thread.Sleep(3000);
        });
        await operation;
        modelBootloader.UpdateFinishCurrentStep();
    }

    private void runRom()
    {
        proxyLua.Execute("print('run rom')");
    }

    private void mergeLanguageFiles()
    {
        SystemLanguage systemLanguage =  Application.systemLanguage;
        string defaultLanguage = "zh_CN";
        if (systemLanguage == SystemLanguage.English)
            defaultLanguage = "en_US";

        string language = PlayerPrefs.GetString(Constant.CustomSettings.Language, defaultLanguage);
        Translator.language = language;

        Translator.MergeFromResource("Translator/UI", true);
    }

    private void registerMVCS()
    {
        // bootloader
        var viewBootloader = new BootloaderView();
        framework_.getStaticPipe().RegisterView(BootloaderView.NAME, viewBootloader);
        var controllerBootloader = new BootloaderBatchController();
        framework_.getStaticPipe().RegisterController(BootloaderBatchController.NAME, controllerBootloader);
        modelBootloader = new BootloaderModel();
        framework_.getStaticPipe().RegisterModel(BootloaderModel.NAME, modelBootloader);

        //framework_.getStaticPipe().RegisterModel(SampleModel.NAME, new SampleModel());
        //framework_.getStaticPipe().RegisterView(SampleView.NAME, new SampleView());
        //framework_.getStaticPipe().RegisterController(SampleController.NAME, new SampleController());
        //framework_.getStaticPipe().RegisterService(SampleService.NAME, new SampleService());
    }

    private void cancelMVCS()
    {
        framework_.getStaticPipe().CancelController(BootloaderBatchController.NAME);
        framework_.getStaticPipe().CancelView(BootloaderView.NAME);
        framework_.getStaticPipe().CancelModel(BootloaderModel.NAME);
        //framework_.getStaticPipe().CancelModel(SampleModel.NAME);
        //framework_.getStaticPipe().CancelView(SampleView.NAME);
        //framework_.getStaticPipe().CancelController(SampleController.NAME);
        //framework_.getStaticPipe().CancelService(SampleService.NAME);
    }

}
