using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class HotUpdateMrg : EditorWindow
{
	
	Vector2 m_vectorScorllPos = new Vector2();
	
	BundleClassify m_bundleScenePart = null;
	
	BundleClassify m_bundlePrefabPart = null;
	
	string m_currVesionCode = "";
	
	[MenuItem("GameTool/BundelManager")]
	static void CreateWindow(){
		HotUpdateMrg window = EditorWindow.GetWindow<HotUpdateMrg> (false, "BundelManager", true);
		window.position = new Rect ((Screen.currentResolution.width - 640) / 2, (Screen.currentResolution.height - 480) / 2, 640, 480);
		window.Show ();
	}
	
	void Awake(){

		LoadVersion ();
		
		Dictionary<string,string> sceneFilte = new Dictionary<string, string> ();
		sceneFilte.Add("extensionName",".unity");
		sceneFilte.Add("notContain","(A)");
		m_bundleScenePart = new BundleClassify ("打包场景文件","Assets/ArtResource/Scene/",AssetType.Scene,sceneFilte,false,OnBuildBundleOver);
		
		Dictionary<string,string> prefabFilte = new Dictionary<string, string> ();
		prefabFilte.Add("extensionName",".prefab");
		m_bundlePrefabPart = new BundleClassify ("打包prefab文件","Assets/ArtResource/prefab/",AssetType.Prefab,prefabFilte,true,OnBuildBundleOver);
		
		//TODO 配置文件打包
	}
	
	void LoadVersion(){
		m_currVesionCode = "0";
		string path = GetVersionCodePath ();
		if (!File.Exists (path)) {
			return;		
		}
		m_currVesionCode = File.ReadAllText (path);
	}

	void SaveVersionCode(){
		int preversion = int.Parse (m_currVesionCode);
		System.DateTime now = System.DateTime.Now;
		m_currVesionCode = "" + now.Year + now.Month + now.Day + now.Hour;
		int curversion = int.Parse (m_currVesionCode);

		if (curversion == preversion) {
			curversion += 1;
		}else if(curversion < preversion){ 
			curversion = preversion + 1;
		}
		m_currVesionCode = curversion.ToString ();

		File.WriteAllText (GetVersionCodePath (), m_currVesionCode);
	}

	void OnBuildBundleOver(){
		SaveVersionCode ();
		SaveConfigFile ();
	}

	void SaveConfigFile(){
		Hashtable data = new Hashtable ();
		if (m_bundlePrefabPart != null) {
			string txt = ReadFile(m_bundlePrefabPart.GetSaveConfigFilePath());
			if(!string.IsNullOrEmpty(txt)){
				data.Add("prefabs",txt);
			}
		}
		if (m_bundleScenePart != null) {
			string txt = ReadFile(m_bundleScenePart.GetSaveConfigFilePath());
			if(!string.IsNullOrEmpty(txt)){
				data.Add("scenes",txt);
			}
		}

		if (data.Count > 0) {
			string jsondata = JsonFx.Json.JsonWriter.Serialize(data);
			File.WriteAllText(GetBundleRoot() +"Assets.json",jsondata);
		}
	}

	void OnGUI(){
		m_vectorScorllPos = EditorGUILayout.BeginScrollView (m_vectorScorllPos, GUILayout.Width (640), GUILayout.Height (480));
		
		if (m_bundleScenePart != null) {
			m_bundleScenePart.ShowEditorGUI();
		}
		
		if (m_bundlePrefabPart != null) {
			m_bundlePrefabPart.ShowEditorGUI();		
		}
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("版本号:", GUILayout.Width (45));
		EditorGUILayout.LabelField (m_currVesionCode, GUILayout.Width (100));
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.EndScrollView();
	}
	
	string GetVersionCodePath(){
		return GetBundleRoot () + "version_code.json";
	}
	
	string ReadFile(string path){
		if (File.Exists (path)) {
			return File.ReadAllText(path);		
		}
		return "";
	}
	
	public static string GetBundleExtensionName(){
		return ".unity3d";
	}
	/// <summary>
	/// 获取当前平台的assentbund的根目录
	/// </summary>
	/// <returns>The bundle root.</returns>
	public static string GetBundleRoot(){
		string path = "";
		switch (EditorUserBuildSettings.activeBuildTarget) {
		case BuildTarget.StandaloneWindows:
			path = Application.streamingAssetsPath +"/AssetBundles/Windows/";	
			break;
		case BuildTarget.Android:
			path = Application.streamingAssetsPath +"/AssetBundles/Android/";	
			break;
		case BuildTarget.iPhone:
			path = Application.streamingAssetsPath +"/AssetBundles/IOS/";	
			break;
		}
		
		return path;
	}
}

