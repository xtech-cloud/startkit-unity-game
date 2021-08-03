using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XTC.oelMVCS;

public class BootloaderBatchController : Controller
{
    public const string NAME = "BootloaderBatchController";

    private BootloaderView view
    {
        get
        {
            return findView(BootloaderView.NAME) as BootloaderView;
        }
    }

    private BootloaderModel model
    {
        get
        {
            return findModel(BootloaderModel.NAME) as BootloaderModel;
        }
    }

    protected override void setup()
    {
        getLogger().Info("setup BootloaderBatchController");
    }

    protected override void dismantle()
    {
    }


    public void Execute(BootloaderModel.BootloaderStatus _status)
    {
        _status.index = 0;
        if (0 == _status.steps.Count)
        {
            view.SetActive(false);
            if (null != _status.onFinish)
                _status.onFinish();
            return;
        }

        executeStep(_status);
    }

    public void FinishCurrentStep(BootloaderModel.BootloaderStatus _status)
    {
        if (_status.index >= _status.steps.Count - 1)
        {
            getLogger().Info("all steps are finished");
            view.SetActive(false);
            if (null != _status.onFinish)
                _status.onFinish();
            return;
        }

        BootloaderModel.Step current = _status.steps[_status.index];
        current.finish = current.length;

        float totalLength = 0;
        float totalFinish = 0;
        foreach (BootloaderModel.Step step in _status.steps)
        {
            totalLength += step.length;
            totalFinish += step.finish;
        }
        view.RefreshProgress(totalFinish / totalLength);

        _status.index += 1;

        executeStep(_status);
    }

    private void executeStep(BootloaderModel.BootloaderStatus _status)
    {
        BootloaderModel.Step step = _status.steps[_status.index];
        getLogger().Info("execute step({0}/{1} : {2})", _status.index, _status.steps.Count, step.tip);
        view.RefreshTip(step.tip);
        if (null != step.onExecute)
            step.onExecute();
    }
}
