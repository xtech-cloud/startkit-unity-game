using System;
using System.Collections.Generic;
using XTC.oelMVCS;

public class BootloaderModel : Model
{
    public class Step
    {
        public int length = 1;
        public string tip = "";
        public int finish = 0;

        public Action onExecute = null;
    }

    public const string NAME = "BootloaderModel";
    public delegate void OnFinishDelegate();

    private BootloaderBatchController controller
    {
        get;
        set;
    }

    private BootloaderStatus status
    {
        get
        {
            return status_ as BootloaderStatus;
        }
    }


    public class BootloaderStatus : Model.Status
    {
        public const string NAME = "BootloaderStatus";
        public List<Step> steps = new List<Step>();
        public int index = 0;
        public OnFinishDelegate onFinish = null;
    }

    protected override void preSetup()
    {
        Error err;
        status_ = spawnStatus<BootloaderStatus>(BootloaderStatus.NAME, out err);
        if (0 != err.getCode())
        {
            getLogger().Error(err.getMessage());
        }
    }

    protected override void setup()
    {
        getLogger().Info("setup BootloaderModel");
        controller = findController(BootloaderBatchController.NAME) as BootloaderBatchController;
    }

    protected override void dismantle()
    {
        Error err;
        killStatus(BootloaderStatus.NAME, out err);
        if (0 != err.getCode())
        {
            getLogger().Error(err.getMessage());
        }
    }

    public void SaveSteps(List<Step> _steps)
    {
        status.steps.Clear();
        status.steps.AddRange(_steps);
    }

    public void UpdateExecute()
    {
        controller.Execute(status);
    }

    public void UpdateFinishCurrentStep()
    {
        controller.FinishCurrentStep(status);
    }

    public void SaveOnFinishCallback(OnFinishDelegate _onFinish)
    {
        status.onFinish = _onFinish;
    }
}
