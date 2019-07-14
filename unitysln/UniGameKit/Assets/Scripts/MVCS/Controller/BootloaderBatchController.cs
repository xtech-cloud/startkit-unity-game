using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XTC.MVCS;

public class BootloaderBatchController : Controller
{
    public const string NAME = "BootloaderBatchController";
    public delegate void OnFinishDelegate();
    public OnFinishDelegate onFinish;

    private BootloaderView view{
        get{
            return viewCenter_.FindView(BootloaderView.NAME) as BootloaderView;
        }
    }

    private BootloaderModel model{
        get{
            return modelCenter_.FindModel(BootloaderModel.NAME) as BootloaderModel;
        }
    }

    protected override void setup()
    {
    }

    protected override void dismantle()
    {
    }


    public void Execute()
    {
        List<BootloaderModel.Step> steps = (List<BootloaderModel.Step>)model.property["steps"];
        int index =  (int)model.property["index"];

        if(index >= steps.Count)
        {
            view.SetActive(false);
            if(null != onFinish)
                onFinish();
            return;
        }

        BootloaderModel.Step step = steps[index];
        view.RefreshTip(step.tip);
        if(null != step.onExecute)
            step.onExecute();
        
    }

    public void FinishCurrentStep()
    {
        List<BootloaderModel.Step> steps = (List<BootloaderModel.Step>)model.property["steps"];
        int index =  (int)model.property["index"];
        if(index >= steps.Count)
        {
            return;
        }

        BootloaderModel.Step now = steps[index];
        now.finish = now.length;

        float totalLength = 0;
        float totalFinish = 0;
        foreach(BootloaderModel.Step step in steps)
        {
            totalLength += step.length;
            totalFinish += step.finish;
        }
        view.RefreshProgress(totalFinish/totalLength);

        index += 1;
        model.property["index"] = index;

        Execute();
    }
}
