using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 用于保存到文件记录中的
/// </summary>
public class BundleDataInfo{
	public AssetType assetType = AssetType.Default;
	public string assetPath = string.Empty;
	public string hashCode = string.Empty;

	public bool IsEqual(BundleDataInfo other){
		if(assetPath == other.assetPath && assetType == other.assetType)
			return true;
		return false;
	}

	public bool IsSame(BundleDataInfo o){
		return hashCode == o.hashCode;
	}
}

public enum BundleFileStatus{
	None,
	New,
	Update,
	Same,
	Count
}

public class BundleFile  {
	static System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider ();

	public BundleDataInfo bundleDataInfo;

	/// <summary>
	/// 文件名
	/// </summary>
	public string m_strFileName = string.Empty;

	/// <summary>
    /// 相对于该资源类型根目录的相对目录
	/// </summary>
	public string relativePath = string.Empty;

	/// <summary>
	/// 要打包文件的全路径
	/// </summary>
	string m_fileFullPath = string.Empty;

	/// <summary>
    /// 打包成bundle的全路径
	/// </summary>
	string m_bundelFileSavePath = string.Empty;

	//准备加入打包列表
	public bool readyToPack = false;

    private BundleFileStatus m_state = BundleFileStatus.None;
	public BundleFileStatus State{
		get{return m_state;}
	}
	public bool buildSuccess = false;

	public BundleFile(DirectoryInfo relativeDirectoryInfo,FileInfo fileInfo,AssetType assetype,bool needFoldClassfy){
		buildSuccess = false;

		bundleDataInfo = new BundleDataInfo ();

		bundleDataInfo.assetType = assetype;

		m_strFileName = Path.GetFileNameWithoutExtension (fileInfo.FullName);

		m_fileFullPath = fileInfo.FullName;

		relativePath = fileInfo.FullName.Replace(relativeDirectoryInfo.FullName,"").Replace(fileInfo.Name,"").Replace("\\","/");

		if (needFoldClassfy) {
			m_bundelFileSavePath = HotUpdateMrg.GetBundleRoot() + GetTypePath(assetype) + relativePath + m_strFileName + HotUpdateMrg.GetBundleExtensionName();
			bundleDataInfo.assetPath = relativePath + m_strFileName;
		}else{ 
			m_bundelFileSavePath = HotUpdateMrg.GetBundleRoot() + GetTypePath(assetype) + m_strFileName + HotUpdateMrg.GetBundleExtensionName();
			bundleDataInfo.assetPath = m_strFileName;
		}

		bundleDataInfo.hashCode = GetFileHashCode (m_fileFullPath);
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

	public void SetBundleState(BundleFileStatus state){
		m_state = state;
	}

	public string GetBundleStateName(){
		switch (m_state) {
		case BundleFileStatus.New:
			return "新增";
		case BundleFileStatus.Update:
			return "更新";
		case BundleFileStatus.Same:
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

		switch (bundleDataInfo.assetType) {
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
		Debug.Log("create bundle :" + m_fileFullPath + "  to " + m_bundelFileSavePath);

		FileInfo file = null;
		FileInfo newFile = null;
		
		string timeStr = "";
		
		if(File.Exists(m_bundelFileSavePath)){
			file = new FileInfo(m_bundelFileSavePath);
			timeStr = file.LastWriteTime.ToShortTimeString();
		}
		
		BuildPipeline.BuildStreamedSceneAssetBundle (res, m_bundelFileSavePath, EditorUserBuildSettings.activeBuildTarget);
		
		if(File.Exists(m_bundelFileSavePath)){
			newFile = new FileInfo(m_bundelFileSavePath);
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

		Debug.Log("create bundle :" + relativeAssetPath + "  to " + m_bundelFileSavePath);

		FileInfo file = null;
		FileInfo newFile = null;

		string timeStr = "";

		if(File.Exists(m_bundelFileSavePath)){
			file = new FileInfo(m_bundelFileSavePath);
			timeStr = file.LastWriteTime.ToShortTimeString();
		}

		BuildPipeline.BuildAssetBundle(obj,null,m_bundelFileSavePath,options,EditorUserBuildSettings.activeBuildTarget);

		if(File.Exists(m_bundelFileSavePath)){
			newFile = new FileInfo(m_bundelFileSavePath);
		}
		if(newFile != null){
			if(!timeStr.Equals(newFile.LastWriteTime.ToShortTimeString())){
				buildSuccess = true;
			}
		}
	}

    /// <summary>
    /// 若果没有保存的目录就创建该目录
    /// </summary>
	void CheckBundleDataPath(){
		if (!Directory.Exists (Path.GetDirectoryName (m_bundelFileSavePath))) {
			Directory.CreateDirectory(Path.GetDirectoryName (m_bundelFileSavePath));
		}
	}
}
