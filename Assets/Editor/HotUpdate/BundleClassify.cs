using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public delegate void DelegateBuildBundle();

public class BundleClassify
{
	string m_resourceRootPath = string.Empty;
	
    /// <summary>
    /// 将要打包的资源目录
    /// </summary>
	DirectoryInfo m_ioResourcesDirInfo;
	/// <summary>
	/// 将要打包的资源文件
	/// </summary>
	List<FileInfo> m_ioResourcesFileInfos = new List<FileInfo>();
	
	bool m_guiListing = false;
	
	//过滤命令格式
	Dictionary<string ,string > m_dicFilterCmd = new Dictionary<string, string>();
	
	//每一个文件资源
	List<BundleFile> m_bundleFiles = new List<BundleFile>();
	
	//资源类型中文名
	string m_bundleClassifyName = string.Empty;
	
	//资源类型
	AssetType m_assetType = AssetType.Default;
	
	//是否需要已目录路径在分类
	bool m_needFoldClassify = false;
	
	//需要分局牡蛎显示的话，在进行bundle file 的路径在分类
	Dictionary<string, List<BundleFile>> m_dicSortPathBundleFile = new Dictionary<string, List<BundleFile>>();
	
	Dictionary<string,bool> m_dicIfBundleFileListing = new Dictionary<string, bool>();
	
	DelegateBuildBundle m_DelegateBuildBundleOver;
	
	BundleDataInfo[] m_lstLastBundleDataInfo = null;
	
	public BundleClassify (string bundleClassifyName,string resourceRootPath,AssetType assetType,Dictionary<string ,string > dicFilterCmd,bool needFoldClassify = false,DelegateBuildBundle callback = null)
	{
		m_bundleClassifyName = bundleClassifyName;
		
		m_resourceRootPath = resourceRootPath;
		
		m_assetType = assetType;
		
		m_needFoldClassify = needFoldClassify;
		
		m_dicFilterCmd = dicFilterCmd;
		
		m_DelegateBuildBundleOver = callback;
		
		InitDirInfo();
	}
	
	
	
	/// <summary>
	/// 初始化文件数据列表
	/// </summary>
	void InitDirInfo(){
		LoadBundleRecordFile ();
		
		m_ioResourcesDirInfo = new DirectoryInfo(m_resourceRootPath);

		m_ioResourcesFileInfos.Clear ();
		
		m_bundleFiles.Clear ();
		
		m_dicSortPathBundleFile.Clear ();
		
		m_dicIfBundleFileListing.Clear ();
		
		GetFiles (m_ioResourcesDirInfo, ref m_ioResourcesFileInfos);
		
		for (int i = 0; i < m_ioResourcesFileInfos.Count; ++i) {
			BundleFile bundleFile = new BundleFile(m_ioResourcesDirInfo,m_ioResourcesFileInfos[i],m_assetType,m_needFoldClassify);
			
			BundleFileStatus state = GetBundleFileState(bundleFile.bundleDataInfo);
			bundleFile.SetBundleState(state);
			
			if(m_needFoldClassify){
				string relativePath = bundleFile.relativePath.Replace("/","");
				
				if(!m_dicSortPathBundleFile.ContainsKey(relativePath)){
					
					m_dicIfBundleFileListing[relativePath] = false;
					m_dicSortPathBundleFile[relativePath] = new List<BundleFile>();
				}
				m_dicSortPathBundleFile[relativePath].Add(bundleFile);
			}
			m_bundleFiles.Add(bundleFile);
		}
	}
	
	public void ShowEditorGUI(){
		EditorGUILayout.BeginHorizontal ();
		m_guiListing = EditorGUILayout.Foldout (m_guiListing, m_bundleClassifyName);
		
		if ( GUILayout.Button ( "刷新", GUILayout.Width (70) ) ) {
			InitDirInfo();		
		}
		EditorGUILayout.EndHorizontal ();
		
		if (m_guiListing) {
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Space(50f);
			ShowBundleFileList();
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("选中所有")) {
				SelectAll();
			}
			if (GUILayout.Button ("清除所有")) {
				ClearAll();
			}

			if (GUILayout.Button ("选中有更新和新增")) {
				
			}
			if (GUILayout.Button ("打包以上选中文件")) {
				PackSelectBundle();
			}
			EditorGUILayout.EndHorizontal ();
		}
	}
	
	void SelectAll(){
		if (m_needFoldClassify) {
			
		}else{ 
			foreach(BundleFile bundleFile in m_bundleFiles){
				bundleFile.readyToPack = true;
			}
		}
	}
	
	void ClearAll(){
		if (m_needFoldClassify) {
			
		}else{ 
			foreach(BundleFile bundleFile in m_bundleFiles){
				bundleFile.readyToPack = false;
			}
		}
	}
	
	/// <summary>
	/// 打包选中的文件.
	/// </summary>
	void PackSelectBundle(){
		List<BundleDataInfo> lstPackBundle = new List<BundleDataInfo> ();
		
		foreach (BundleFile bundleFile in m_bundleFiles) {
			
			if(bundleFile.readyToPack){
				
				bundleFile.CreateAssetBundle();
				
				bool success = bundleFile.buildSuccess;
				if(success)
				{
					lstPackBundle.Add(bundleFile.bundleDataInfo);
					bundleFile.buildSuccess = false;
				}else
				{
					Debug.LogError("打包出错");
				}
			}
			
		}
		SaveBundleRecordFile (lstPackBundle);
	}
	
	void ShowBundleFileList(){
		EditorGUILayout.BeginVertical ();
		if (!m_needFoldClassify) {
			foreach(BundleFile bundleFile in m_bundleFiles){
				EditorGUILayout.BeginHorizontal ();
				bundleFile.readyToPack = GUILayout.Toggle(bundleFile.readyToPack,bundleFile.GetBundleStateName() + bundleFile.fileName);
				EditorGUILayout.EndHorizontal ();
			}		
		}else{ 
			
		}
		EditorGUILayout.EndVertical ();
	}
	
	/// <summary>
	/// 递归获取指定目录的所有文件
	/// </summary>
	/// <param name="dir">Dir.</param>
	/// <param name="result">Result.</param>
	void GetFiles(DirectoryInfo dir,ref List<FileInfo> result){
		foreach(FileInfo file in dir.GetFiles()){
			if(IfCanAddToClassify(file)){
				result.Add(file);
			}
		}
		
		DirectoryInfo[] dirs = dir.GetDirectories ();
		foreach (DirectoryInfo di in dirs) {
			GetFiles(di,ref result);		
		}
	}
	
	BundleFileStatus GetBundleFileState(BundleDataInfo bundleInfo){
		if(m_lstLastBundleDataInfo == null)
			return BundleFileStatus.New;
		BundleDataInfo f = null;
		foreach (BundleDataInfo info in m_lstLastBundleDataInfo) {
			if(info.IsEqual(bundleInfo)){
				f = info;
				break;
			}		
		}
		
		return f.IsSame (bundleInfo) == true ? BundleFileStatus.Same : BundleFileStatus.Update;
	}
	
	bool IfCanAddToClassify(FileInfo file){
		foreach (KeyValuePair<string,string> filter in m_dicFilterCmd) {
			if(filter.Key == "extensionName"){
				if(filter.Value != Path.GetExtension(file.FullName)){
					return false;
				}
			}
			else if(filter.Key == "notCaontain"){
				if(file.FullName.Contains(filter.Value)){
					return false;
				}
			}	
		}
		return true;
	}
	
	/// <summary>
	/// 加载配置文件，反序列化
	/// </summary>
	void LoadBundleRecordFile(){
		string savePath = GetSaveConfigFilePath ();
		if (savePath == "")
			return;
		if(!File.Exists(savePath))
			return;
		string jsonData = File.ReadAllText (savePath);
		m_lstLastBundleDataInfo = JsonFx.Json.JsonReader.Deserialize<BundleDataInfo[]> (jsonData);
	}
	
	/// <summary>
	/// 获取保存的配置文件路径
	/// </summary>
	/// <returns>The save config file path.</returns>
	public string GetSaveConfigFilePath(){
		string fileName = GetRecodFileName ();
		if (string.IsNullOrEmpty (fileName)) {
			Debug.LogError("GetRecodFileName error :" + m_assetType);	
			return "";
		}
		return HotUpdateMrg.GetBundleRoot () + fileName;
	}
	
	string GetRecodFileName(){
		string path = "";
		switch (m_assetType) {
		case AssetType.Scene:
			path = "scene_version.json";
			break;
		case AssetType.Prefab:
			path = "preafab_version.json";
			break;
		}
		return path;
	}
	
	void SaveBundleRecordFile(List<BundleDataInfo> lstPackBundle){
		if (lstPackBundle.Count > 0) {
			AddLastBundles(ref lstPackBundle);
			string savePath = GetSaveConfigFilePath();
			string jsonData = JsonFx.Json.JsonWriter.Serialize(lstPackBundle);
			File.WriteAllText(savePath,jsonData);
			
			InitDirInfo();
			
			if(m_DelegateBuildBundleOver != null){
				m_DelegateBuildBundleOver();
			}
		}
	}
	
	void AddLastBundles(ref List<BundleDataInfo> lstPackBundle){
		if (m_lstLastBundleDataInfo != null) {
			foreach(BundleDataInfo info in m_lstLastBundleDataInfo){
				int count = lstPackBundle.Count;
				bool exist = false;
				for(int i=0;i< count; ++i){
					if(info.IsEqual(lstPackBundle[i])){
						exist = true;break;
					}
				}
				if(!exist){
					lstPackBundle.Add(info);
				}
				
			}
		}
	}
}