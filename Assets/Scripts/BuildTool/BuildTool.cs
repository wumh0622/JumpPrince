#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class BuildTool
{
    [MenuItem("Build/Build All")]
    public static void BuildAll()
    {
        if(EditorPrefs.GetBool("BuildWindows"))
        {
            BuildExe();
        }

        if (EditorPrefs.GetBool("BuildMac"))
        {
            BuildMac();
        }

        if (EditorPrefs.GetBool("BuildWeb"))
        {
            BuildWeb();
        }
    }

    [MenuItem("Build/Build Test Windows")]
    public static void BuildExe()
    {
        bool isDev = true;
        string version = "0.0.0";
        if (!ParseCommandLine(out isDev, out version))
        {
            isDev = EditorPrefs.GetBool("DevelopmentBuild");
            version = EditorPrefs.GetString("Version");
        }
        PlayerSettings.bundleVersion = version;

        BuildPlayerOptions opt = new BuildPlayerOptions();
        string[] allScenePath = new string[EditorBuildSettings.scenes.Length];

        int idx = 0;
        foreach (EditorBuildSettingsScene item in EditorBuildSettings.scenes)
        {
            allScenePath[idx] = item.path;
            idx++;
        }

        string fileName = "JumpPrince_"  + (isDev ? "DevBuild" : "Shipping") + "_Windows" + "_" + PlayerSettings.bundleVersion;

        opt.scenes = allScenePath;
        opt.locationPathName = Application.dataPath + "/../Bin/" + fileName + "/JumpPrince.exe";
        opt.target = BuildTarget.StandaloneWindows;
        if(isDev)
        {
            opt.options = BuildOptions.Development;
        }

        BuildPipeline.BuildPlayer(opt);

        Debug.Log("Build App Done!");
    }

    [MenuItem("Build/Build Test Mac")]
    public static void BuildMac()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.Mono2x);

        bool isDev = true;
        string version = "0.0.0";
        if(!ParseCommandLine(out isDev, out version))
        {
            isDev = EditorPrefs.GetBool("DevelopmentBuild");
            version = EditorPrefs.GetString("Version");
        }
        PlayerSettings.bundleVersion = version;

        BuildPlayerOptions opt = new BuildPlayerOptions();
        string[] allScenePath = new string[EditorBuildSettings.scenes.Length];

        int idx = 0;
        foreach (EditorBuildSettingsScene item in EditorBuildSettings.scenes)
        {
            allScenePath[idx] = item.path;
            idx++;
        }

        string fileName = "JumpPrince_" + (isDev ? "DevBuild" : "Shipping") + "_Mac" + "_" + PlayerSettings.bundleVersion;

        opt.scenes = allScenePath;
        opt.locationPathName = Application.dataPath + "/../Bin/" + fileName + "/JumpPrince";
        opt.target = BuildTarget.StandaloneOSX;
        if (isDev)
        {
            opt.options = BuildOptions.Development;
        }

        BuildPipeline.BuildPlayer(opt);

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.IL2CPP);
        Debug.Log("Build App Done!");
    }

    [MenuItem("Build/Build Test Web")]
    public static void BuildWeb()
    {
        //PlayerSettings.SetScriptingBackend(BuildTargetGroup.WebGL, ScriptingImplementation.Mono2x);

        bool isDev = true;
        string version = "0.0.0";
        if (!ParseCommandLine(out isDev, out version))
        {
            isDev = EditorPrefs.GetBool("DevelopmentBuild");
            version = EditorPrefs.GetString("Version");
        }
        PlayerSettings.WebGL.memorySize = 64;
        PlayerSettings.bundleVersion = version;

        BuildPlayerOptions opt = new BuildPlayerOptions();
        string[] allScenePath = new string[EditorBuildSettings.scenes.Length];

        int idx = 0;
        foreach (EditorBuildSettingsScene item in EditorBuildSettings.scenes)
        {
            allScenePath[idx] = item.path;
            idx++;
        }

        string fileName = "JumpPrince_" + (isDev ? "DevBuild" : "Shipping") + "_WebGL" + "_" + PlayerSettings.bundleVersion;

        opt.scenes = allScenePath;
        opt.locationPathName = Application.dataPath + "/../Bin/" + fileName + "/JumpPrince";
        opt.target = BuildTarget.WebGL;
        if (isDev)
        {
            opt.options = BuildOptions.Development;
        }

        BuildPipeline.BuildPlayer(opt);

        //PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.IL2CPP);
        Debug.Log("Build App Done!");
    }

    public static bool ParseCommandLine(out bool isDev, out string version)
    {
        string[] args = System.Environment.GetCommandLineArgs();
        isDev = true;
        version = "0.0.0";

        bool commandIsPass = false;

        foreach (var s in args)
        {
            if (s.Contains("--DevVer"))
            {
                bool result = true;
                if (bool.TryParse(s.Split(':')[1], out result))
                {
                    commandIsPass = true;
                    isDev = result;
                }
            }

            if (s.Contains("--version:"))
            {
                commandIsPass = true;
                version = s.Split(':')[1];
            }
        }

        return commandIsPass;
    }
}

public class BuildToolEditor : EditorWindow
{
    [MenuItem("Build/BuildTool Settings")]
    static void Init()
    {
        BuildToolEditor window = (BuildToolEditor)EditorWindow.GetWindow(typeof(BuildToolEditor));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("BuildTool Settings", EditorStyles.boldLabel);

        bool isDevEditor = EditorPrefs.GetBool("DevelopmentBuild");
        bool isDev = EditorGUILayout.Toggle("Development Build", isDevEditor);
        EditorPrefs.SetBool("DevelopmentBuild", isDev);

        string versionEditor = EditorPrefs.GetString("Version");
        string version = EditorGUILayout.TextField("Version", versionEditor);
        EditorPrefs.SetString("Version", version);

        GUILayout.Label("Build all platform select");

        bool buildWindowsEditor = EditorPrefs.GetBool("BuildWindows");
        bool buildWindows = EditorGUILayout.Toggle("Windows", buildWindowsEditor);
        EditorPrefs.SetBool("BuildWindows", buildWindows);

        bool buildMacEditor = EditorPrefs.GetBool("BuildMac");
        bool buildMac = EditorGUILayout.Toggle("Mac", buildMacEditor);
        EditorPrefs.SetBool("BuildMac", buildMac);

        bool buildWebEditor = EditorPrefs.GetBool("BuildWeb");
        bool buildWeb = EditorGUILayout.Toggle("Web", buildWebEditor);
        EditorPrefs.SetBool("BuildWeb", buildWeb);
    }
}

#endif