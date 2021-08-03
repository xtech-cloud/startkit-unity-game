using System.Collections.Generic;
using XTC.oelMVCS;

public static class FacadeStore
{
    public static Dictionary<string, View.Facade> facades = new Dictionary<string, View.Facade>();

    public static void Register(string _name, View.Facade _facade)
    {
        facades[_name] = _facade;
    }

    public static void Cancel(string _name)
    {
        facades.Remove(_name);
    }

    public static View.Facade Find(string _name)
    {
        View.Facade facade;
        facades.TryGetValue(_name, out facade);
        return facade;
    }
}
