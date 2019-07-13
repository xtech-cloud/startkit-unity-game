using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;

public class PostBuildActions
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget _target, string _targetPath)
    {
		Debug.Log(string.Format("OnPostProcessBuild {0} {1}",_target, _targetPath));
        if (BuildTarget.WebGL == _target)
        {
            var path = Path.Combine(_targetPath, "Build/UnityLoader.js");
            var text = File.ReadAllText(path);
            text = Regex.Replace(text, @"compatibilityCheck:function\(e,t,r\)\{.+,Blobs:\{\},loadCode",
                "compatibilityCheck:function(e,t,r){t()},Blobs:{},loadCode");
            File.WriteAllText(path, text);
        }

    }
}