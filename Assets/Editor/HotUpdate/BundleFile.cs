﻿using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class BundleDataInfo{
	public string m_assetPath = string.Empty;
	public AssetType m_assetType = AssetType.Default;
	public string m_hashCode = string.Empty;

	public bool IsEqual(BundleDataInfo other){
		if(m_assetPath == other.m_assetPath && m_assetType == other.m_assetType)
			return true;
		return false;
	}

	public bool IsSame(BundleDataInfo o){
		return m_hashCode == o.m_hashCode;
	}
}

public class BundleFile  {
	static System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider ();

	public BundleDataInfo bundleDataInfo;

	//文件名
	public string fileName = string.Empty;

	//相对于根目录的相对目录
	public string relativePath = string.Empty;

	//全路径
	string m_fileFullPath = string.Empty;

	//打包成bundle的全路径
	string m_bundelFilePath = string.Empty;

	//准备加入打包列表
	public bool readyToPack = false;

	private int m_state = 0;
	/// <summary>
	/// 0  new  1 updat 2 same.
	/// </summary>
	/// <value>The state.</value>
	public int State{get{return m_state;}}

	public bool buildSuccess = false;

	public BundleFile(){

	}

	public BundleFile(DirectoryInfo relativeDirectoryInfo,FileInfo fileInfo,AssetType assetype,bool needFoldClassfy){
		buildSuccess = false;

		bundleDataInfo = new BundleDataInfo ();

		bundleDataInfo.m_assetType = assetype;

		fileName = Path.GetFileNameWithoutExtension (fileInfo.FullName);

		m_fileFullPath = fileInfo.FullName;

		relativePath = fileInfo.FullName.Replace(relativeDirectoryInfo.FullName,"").Replace(fileInfo.Name,"").Replace("\\","/");

		if (needFoldClassfy) {
				
		}else{ 
		
		}

		bundleDataInfo.m_hashCode = GetFileHashCode (m_fileFullPath);
	}

	string GetFileHashCode (string path){
		string code = "";
		using(Stream s = File.OpenRead(path)){
			var hash = sha1.ComputeHash(s);
			var shash = Convert.ToBase64String(hash) +"@" + s.Length;
			code = shash;
		}
		return code;
	}

	public void SetBundleState(int state){
		m_state = state;
	}

	public string GetBundleStateName(){
		switch (m_state) {
		case 0:
			return "新增";
		case 1:
			return "更新";
		case 2:
			return "无改变";
		}
		return "";
	}

	string GetTypePath(AssetType type){
		string path = "";
		switch (type) {
		case AssetType.Scene :
			path = "Scene/";
			break;
		case AssetType.Prefab :
			path = "Prefab/";
			break;
		}
		return path;
	}

	public void CreateAssetBundle(){
		CheckBundleDataPath ();

		switch (bundleDataInfo.m_assetType) {
		case AssetType.Scene :
			BuildSceneAssentBundle();
			break;
		case AssetType.Prefab :
			BuildNormalAssetBundle();
			break;		
		}
	}

	void BuildSceneAssentBundle(){
		string[] res = new string[1];
		res [0] = m_fileFullPath;
		Debug.Log("create bundle :" + m_fileFullPath + "  to " + m_bundelFilePath);

		FileInfo file = null;
		FileInfo newFile = null;
		
		string timeStr = "";
		
		if(File.Exists(m_bundelFilePath)){
			file = new FileInfo(m_bundelFilePath);
			timeStr = file.LastWriteTime.ToShortTimeString();
		}
		
		BuildPipeline.BuildStreamedSceneAssetBundle (res, m_bundelFilePath, EditorUserBuildSettings.activeBuildTarget);
		
		if(File.Exists(m_bundelFilePath)){
			newFile = new FileInfo(m_bundelFilePath);
		}
		if(newFile != null){
			if(!timeStr.Equals(newFile.LastWriteTime.ToShortTimeString())){
				buildSuccess = true;
			}
		}
	}

	void BuildNormalAssetBundle(){
		//资源包编译选项                     / 包含所有依赖关系                                   强制包括整个资源。
		BuildAssetBundleOptions options = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;

		string relativeAssetPath = m_fileFullPath.Substring(m_fileFullPath.LastIndexOf("Assets"));
		//指定路径加载主资源
		UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath (relativeAssetPath);

		Debug.Log("create bundle :" + relativeAssetPath + "  to " + m_bundelFilePath);

		FileInfo file = null;
		FileInfo newFile = null;

		string timeStr = "";

		if(File.Exists(m_bundelFilePath)){
			file = new FileInfo(m_bundelFilePath);
			timeStr = file.LastWriteTime.ToShortTimeString();
		}

		BuildPipeline.BuildAssetBundle(obj,null,m_bundelFilePath,options,EditorUserBuildSettings.activeBuildTarget);

		if(File.Exists(m_bundelFilePath)){
			newFile = new FileInfo(m_bundelFilePath);
		}
		if(newFile != null){
			if(!timeStr.Equals(newFile.LastWriteTime.ToShortTimeString())){
				buildSuccess = true;
			}
		}
	}

	void CheckBundleDataPath(){
		if (!Directory.Exists (Path.GetDirectoryName (m_bundelFilePath))) {
			Directory.CreateDirectory(Path.GetDirectoryName (m_bundelFilePath));
		}
	}
}
