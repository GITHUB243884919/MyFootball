using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateUISpritePrefab : MonoBehaviour
{
	/// <summary>
	/// 静态图集目录
	/// </summary>
	public static string staticAtlasPath = "Assets/Res/UIAtlas_Static/";

	/// <summary>
	/// 动态图集目录
	/// </summary>
	public static string dynamicAtlasPath = "Assets/Res/UIAtlas/";

	/// <summary>
	/// 动态图集prefab目录
	/// </summary>
	public static string atlasResourcesPath = "Assets/Resources/UIAtlas/";
	
	[MenuItem("Joyfort/Tools/图集/生成图集的prefab文件")]
	static void CreateData()
	{
		if (Directory.Exists(dynamicAtlasPath))
		{  
			FileUtil.DeleteDir(atlasResourcesPath);
			DirectoryInfo direction = new DirectoryInfo(dynamicAtlasPath);  
			FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
			for (int i = 0, iMax = files.Length; i < iMax; i++)
			{  
				if (!files[i].Name.EndsWith(".meta"))
				{  
					string fullName =  Path.GetFullPath(files[i].FullName);
					fullName = FormatAssetPath(fullName);
					var spr = AssetDatabase.LoadAssetAtPath(fullName,typeof(UnityEngine.Sprite))as UnityEngine.Sprite;
					var name = Path.GetFileNameWithoutExtension (files[i].FullName);
					AddAtlasPrefab (spr, name, fullName);
					EditorUtility.DisplayProgressBar("生成图集的prefab文件", name, i / (float)iMax);  
				}
			}
			EditorUtility.ClearProgressBar();
			AssetDatabase.Refresh();
			EditorUtility.DisplayDialog("标题", "所有的图集的prefab文件生成成功", "确定");
		}

	}

	private static void AddAtlasPrefab(Sprite spr,string name, string SprAssetPath)
	{
		var go = new GameObject(name);
		go.AddComponent<SpriteRenderer> ().sprite = spr;
		string outPaht = SprAssetPath.Replace ("/Res/", "/Resources/");
		var str = outPaht.Split ('.') [0];
		outPaht = str +".prefab";
		FileUtil.CreateDir (str.Replace("/" + name, ""), false);
		//PrefabUtility.CreatePrefab (outPaht, go1);
		bool ret;
		PrefabUtility.SaveAsPrefabAsset(go, outPaht, out ret);
		DestroyImmediate(go);
	}

	public static string FormatAssetPath(string filePath)
	{
		var formatFilePath = filePath.Replace("\\", "/");
		formatFilePath = formatFilePath.Replace("//", "/").Trim();
		formatFilePath = formatFilePath.Replace("///", "/").Trim();
		formatFilePath = formatFilePath.Replace("\\\\", "/").Trim();
		var n = formatFilePath.IndexOf("Assets/");
		formatFilePath = formatFilePath.Substring(n);
		return formatFilePath;
	}
}
