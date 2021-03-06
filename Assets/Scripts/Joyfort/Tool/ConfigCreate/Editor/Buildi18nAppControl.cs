//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using static Game.PublishManager;
//using System.IO;
//using System;

//public class Buildi18nAppControl : EditorWindow
//{
//    bool groupEnabled = true;     // 选项组是否可用
//    static bool isTestModel = false;    // 复选框状态
//    static string version = string.Empty;//版本号
//    static int versionCode = 1;//build

//    static EPublishChannel channel = EPublishChannel.TD_ANDROID;
    
//    [MenuItem("Joyfort/Tools/国际版打包")]
//    public static void ShowBuildUI()
//    {
//        version = Config.bath_globalConfig.getInstace().Version;
//        CreateVersionCode();
//        EditorWindow.GetWindow(typeof(Buildi18nAppControl));
//    }

//    private void OnGUI()
//    {
//        GUILayout.Label("模式：");
//        isTestModel = EditorGUILayout.Toggle("测试模式", isTestModel);

//        version = EditorGUILayout.TextField("当前版本：", version);
//        CreateVersionCode();
//        EditorGUILayout.BeginHorizontal();
//        GUILayout.Label("versionCode:", GUILayout.Width(80));
//        GUILayout.Label(versionCode.ToString(), GUILayout.Width(120));
//        EditorGUILayout.EndHorizontal();

//		// 开启一组选项
//		groupEnabled = EditorGUILayout.BeginToggleGroup("发布渠道", groupEnabled);
//		channel = EditorGUILayout.Toggle(EPublishChannel.i18n_IOS.ToString(), channel == EPublishChannel.i18n_IOS) ? EPublishChannel.i18n_IOS : EPublishChannel.i18n_Android;
//		channel = EditorGUILayout.Toggle(EPublishChannel.i18n_Android.ToString(), channel == EPublishChannel.i18n_Android) ? EPublishChannel.i18n_Android : EPublishChannel.i18n_IOS;
//		//channel = EditorGUILayout.Toggle("TD", channel == EPublishChannel.TD_ANDROID) ? EPublishChannel.TD_ANDROID : (channel == EPublishChannel.TAPTAP_ANDROID) ? EPublishChannel.TAPTAP_ANDROID : EPublishChannel.CLOUDTEST_ANDROID;
//		//channel = EditorGUILayout.Toggle("TAP", channel == EPublishChannel.TAPTAP_ANDROID) ? EPublishChannel.TAPTAP_ANDROID : (channel == EPublishChannel.TD_ANDROID) ? EPublishChannel.TD_ANDROID : EPublishChannel.CLOUDTEST_ANDROID;
//		//channel = EditorGUILayout.Toggle("CloudTest", channel == EPublishChannel.CLOUDTEST_ANDROID) ? EPublishChannel.CLOUDTEST_ANDROID : (channel == EPublishChannel.TD_ANDROID) ? EPublishChannel.TD_ANDROID : EPublishChannel.TAPTAP_ANDROID;
//		EditorGUILayout.EndToggleGroup();

//		if (GUILayout.Button("BuildAPP"))
//        {
//            CreateAPP();

//            this.Close();
//        }
//    }
//    /// <summary>
//    /// 生成versionCode
//    /// </summary>
//    static void CreateVersionCode()
//    {
//        int startVersionCode = 1;
//        int startDotPos = version.IndexOf(".");
//        if (startDotPos <= 0)
//        {
//            return;
//        }
//        if (int.TryParse(version.Substring(0, startDotPos), out startVersionCode))
//        {
//            versionCode = startVersionCode * 1000000;
//        }
//        int lastDotPos = version.LastIndexOf('.');
//        if (lastDotPos <= startDotPos)
//        {
//            return;
//        }
//        string midVersionCodeStr = version.Substring(startDotPos + 1, lastDotPos - startDotPos - 1);
//        int midVersionCode = 0;
//        if (int.TryParse(midVersionCodeStr, out midVersionCode))
//        {
//            midVersionCode = midVersionCode * (int)Mathf.Pow(10, 6 - midVersionCode.ToString().Length);
//            versionCode += midVersionCode;
//        }

//        string endVersionCodeStr = version.Substring(lastDotPos + 1);
//        int endVersionCode = 1;
//        if (int.TryParse(endVersionCodeStr, out endVersionCode))
//        {
//            versionCode += endVersionCode;
//        }
//    }
//    /// <summary>
//    /// 创建打包数据
//    /// </summary>
//    void CreateAPP()
//    {
//		CreateConfig();
//		switch (channel)
//		{
//			case EPublishChannel.i18n_IOS:
//				PlayerSettings.applicationIdentifier = "com.joyfort.hotspringtycoon.os";
//				BuildIOS();
//				break;
//			case EPublishChannel.i18n_Android:
//				PlayerSettings.applicationIdentifier = "com.minigame.bathtycoon.gp";
//				BuildAPK("i18n");
//				break;
//		}
//    }

//    /// <summary>
//    /// 创建ScriptableObject
//    /// </summary>
//    static void CreateConfig()
//    {
//        PublishTable publishTable = new PublishTable();

//        publishTable.PublishChannel = (int)channel;
//        publishTable.TestModel = isTestModel;

//        AssetDatabase.CreateAsset(publishTable, "Assets/Resources/publishTable.asset");
        
//    }


//    /// <summary>
//    /// APK包
//    /// </summary>
//    static void BuildAPK(string channelName)
//    {
//        // 签名文件配置，若不配置，则使用Unity默认签名
//        PlayerSettings.Android.keystoreName = "./userkey/user.keystore";
//        PlayerSettings.Android.keystorePass = "123456";
//        PlayerSettings.Android.keyaliasName = "123456";
//        PlayerSettings.Android.keyaliasPass = "123456";
//        string versionName = version;
//        PlayerSettings.bundleVersion = version;
//        PlayerSettings.Android.bundleVersionCode = versionCode;
//        versionName += string.Format("({0})", versionCode);
//        // APK路径、名字配置
//        if (isTestModel)
//        {
//            versionName += "_Debug";
//        }
//        string apkName = string.Format("BathCenter_{0}_{1}_{2}", channelName, versionName, DateTime.Now.ToString("yyyyMMdd_HHmmss"));
//        //string path = Application.dataPath.Replace("/UnityBath/Assets", "") + "/APK/" + apkName + ".apk";
//		string path = "../../trunk/APK/" + apkName + ".apk";
//		BuildPipeline.BuildPlayer(GetBuildScenes(), path, BuildTarget.Android, BuildOptions.None);
//    }

//	static void BuildIOS()
//	{
//		string versionName = version;
//		PlayerSettings.bundleVersion = version;
//		PlayerSettings.iOS.buildNumber = "0";
//		string path = "../XCode";
//		BuildPipeline.BuildPlayer(GetBuildScenes(), path, BuildTarget.iOS, BuildOptions.None);
//	}

//	/// <summary>
//	/// 在这里找出你当前工程所有的场景文件
//	/// </summary>
//	/// <returns></returns>
//	static string[] GetBuildScenes()
//    {
//        List<string> sceneNames = new List<string>();
//        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
//        {
//            if (e == null)
//                continue;
//            if (e.enabled)
//                sceneNames.Add(e.path);
//        }
//        return sceneNames.ToArray();
//    }
//}


