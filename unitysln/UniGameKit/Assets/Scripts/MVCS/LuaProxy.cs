using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class LuaProxy
{
    private LuaEnv rootLuaEnv_ { get; set; }

    private bool hasAwake = false;
    private bool hasEnable = false;
    private bool hasStart = false;
    private bool hasUpdate = false;
    private bool hasDisable = false;
    private bool hasDestroy = false;

    private List<string> searchPaths = new List<string>();
    private string rootCode = "";
    public void AddSearchPath(string _path)
    {
        searchPaths.Add(_path);
    }

    public void UseRootCode(string _code)
    {
        rootCode = _code;
        hasAwake = rootCode.Contains("function uniAwake()");
        hasEnable = rootCode.Contains("function uniEnable()");
        hasStart = rootCode.Contains("function uniStart()");
        hasUpdate = rootCode.Contains("function uniUpdate()");
        hasDisable = rootCode.Contains("function uniDisable()");
        hasDestroy = rootCode.Contains("function uniDestroy()");
    }

    public void Execute(string _code)
    {
        hasAwake = _code.Contains("function uniAwake()");
        hasEnable = _code.Contains("function uniEnable()");
        hasStart = _code.Contains("function uniStart()");
        hasUpdate = _code.Contains("function uniUpdate()");
        hasDisable = _code.Contains("function uniDisable()");
        hasDestroy = _code.Contains("function uniDestroy()");
        rootLuaEnv_.DoString(_code);
    }


    public void DoAwake()
    {
        rootLuaEnv_ = new LuaEnv();
        rootLuaEnv_.DoString(rootCode);

        if(!hasAwake)
            return;
        rootLuaEnv_.DoString("uniAwake()");
    }

    public void DoOnEnable()
    {
        if(!hasEnable)
            return;
        rootLuaEnv_.DoString("uniEnable()");
    }

    public void DoStart()
    {
        if(!hasStart)
            return;
        rootLuaEnv_.DoString("uniStart()");
    }

    public void DoUpdate()
    {
        rootLuaEnv_.Tick();
        if(!hasUpdate)
            return;
        rootLuaEnv_.DoString("uniUpdate()");
    }

    public void DoOnDisable()
    {
        if(!hasDisable)
            return;
        rootLuaEnv_.DoString("uniDisable()");
    }

    public void DoOnDestroy()
    {
        if(hasDisable)
        {
            rootLuaEnv_.DoString("uniDestroy()");
        }
        rootLuaEnv_.Dispose();
    }

    public string ReadFile(string _filepath)
    {
        byte[] bytes = new byte[0];
        try
        {
            var filepath = _filepath;
#if UNITY_ANDROID && !UNITY_EDITOR
                UnityEngine.WWW www = new UnityEngine.WWW(filepath);
                while (true)
                {
                    if(!string.IsNullOrEmpty(www.error))
                    {
                        System.Threading.Thread.Sleep(50);
                        break;
                    }
                    if (www.isDone)
                    {
                        bytes = www.bytes;
                        break;
                    }
                }
#else
            if (File.Exists(filepath))
            {
                bytes = File.ReadAllBytes(filepath);
            }
#endif
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
        return Encoding.UTF8.GetString(bytes);
    }

    private void addLoader()
    {
        foreach (string path in searchPaths)
        {
            string searchPath = path;
            rootLuaEnv_.AddLoader((ref string _filename) =>
            {
                byte[] bytes = new byte[0];
                try
                {
                    var filepath = searchPath + "/" + _filename;
                    #if UNITY_ANDROID && !UNITY_EDITOR
                        UnityEngine.WWW www = new UnityEngine.WWW(filepath);
                        while (true)
                        {
                            if(!string.IsNullOrEmpty(www.error))
                            {
                                System.Threading.Thread.Sleep(50);
                                break;
                            }
                            if (www.isDone)
                            {
                                bytes = www.bytes;
                                break;
                            }
                        }
                    #else
                        if (File.Exists(filepath))
                        {
                            bytes = File.ReadAllBytes(filepath);
                        }
                    #endif
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
                return bytes;
            });
        }
    }

}
