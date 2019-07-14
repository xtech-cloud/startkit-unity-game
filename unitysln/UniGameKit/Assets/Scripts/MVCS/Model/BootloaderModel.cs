using System;
using System.Collections.Generic;
using XTC.MVCS;

public class BootloaderModel : Model
{
    public class Step
    {
        public string name = "";
        public int length = 1;
        public string tip = "";
        public int finish = 0;

        public Action onExecute = null;
    }

    public const string NAME = "BootloaderModel";


    public class BootloaderStatus : Model.Status
    {
    }

    protected override void setup()
    {
        property["steps"] = new List<Step>();
        property["index"] = 0;
    }

    protected override void dismantle()
    {
    }

    public void SaveSteps(List<Step> _steps)
    {
        List<Step> steps = (List<Step>)property["steps"];
        steps.Clear();
        steps.AddRange(_steps);
    }
}