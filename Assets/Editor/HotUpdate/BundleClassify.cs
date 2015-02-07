using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public delegate void DelegateBuildBundle();

public class BundleClassify
{
	/// <summary>
	/// 资源相对目录的根目录
	/// </summary>
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
	
	/// <summary>
	/// 过滤命令格式 
	/// </summary>
	Dictionary<string ,string > m_dicFilterCmd = new Dictionary<string, string>();
	
	/// <summary>
	/// 每一个Bundle文件资源
	/// </summary>
	List<BundleFile> m_bundleFiles = new List<BundleFile>();
	
	/// <summary>
	/// 资源类型中文名
	/// </summary>
	string m_bundleClassifyName = string.Empty;
	
	/// <summary>
	/// 资源类型
	/// </summary>
	AssetType m_assetType = AssetType.Default;
	
	/// <summary>
	/// 是否需要已目录路径在分类
	/// </summary>
	bool m_needFoldClassify = false;
	
	//需要分局牡蛎显示的话，在进行bundle file 的路径在分类
	Dictionary<string, List<BundleFile>> m_dicSortPathBundleFile = new Dictionary<string, List<BundleFile>>();
	
	Dictionary<string,bool> m_dicIfBundleFileListing = new Dictionary<string, bool>();
	/// <summary>
	/// 打包完成委托
	/// </summary>
	DelegateBuildBundle m_DelegateBuildBundleOver;
	/// <summary>
	/// 上次打包的记录信息
	/// </summary>
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
		GUILayout.TextField ("资源目录：" + m_resourceRootPath);
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
			foreach(KeyValuePair<string,List<BundleFile>> data in m_dicSortPathBundleFile){
				foreach(BundleFile bundleFile in data.Value){
					bundleFile.readyToPack = true;
				}	
			}
		}else{ 
			foreach(BundleFile bundleFile in m_bundleFiles){
				bundleFile.readyToPack = true;
			}
		}
	}
	
	void ClearAll(){
		if (m_needFoldClassify) {
			foreach(KeyValuePair<string,List<BundleFile>> data in m_dicSortPathBundleFile){
				foreach(BundleFile bundleFile in data.Value){
					bundleFile.readyToPack = false;
				}	
			}
		}else{ 
			foreach(BundleFile bundleFile in m_bundleFiles){
				bundleFile.readyToPack = false;
			}
		}
	}

	void SelectUpdateBundle(){
		if (m_needFoldClassify) {
			foreach(KeyValuePair<string,List<BundleFile>> data in m_dicSortPathBundleFile){
				foreach(BundleFile bundleFile in data.Value){
					if(bundleFile.State == BundleFileStatus.New || bundleFile.State == BundleFileStatus.Update){
						bundleFile.readyToPack = true;
					}
				}	
			}
		}else{ 
			foreach(BundleFile bundleFile in m_bundleFiles){
				if(bundleFile.State == BundleFileStatus.New || bundleFile.State == BundleFileStatus.Update){
					bundleFile.readyToPack = true;
				}
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
				}else
				{
					Debug.LogError("打包出错 :" + bundleFile.m_strFileName);
				}
			}
			
		}
		SaveBundleRecordFile (lstPackBundle);
	}

	/// <summary>
	/// 显示资源目录下可以打包的列表.
	/// </summary>
	void ShowBundleFileList(){
		EditorGUILayout.BeginVertical ();
		if (!m_needFoldClassify) {
			foreach(BundleFile bundleFile in m_bundleFiles){
				EditorGUILayout.BeginHorizontal ();
				bundleFile.readyToPack = GUILayout.Toggle(bundleFile.readyToPack,bundleFile.GetBundleStateName() + bundleFile.m_strFileName);
				EditorGUILayout.EndHorizontal ();
			}		
		}else{ 
			foreach(KeyValuePair<string,List<BundleFile>> data in m_dicSortPathBundleFile){

				m_dicIfBundleFileListing[data.Key] = EditorGUILayout.Foldout(m_dicIfBundleFileListing[data.Key],data.Key);

				if(m_dicIfBundleFileListing[data.Key] == true){
					foreach(BundleFile bundleFile in data.Value){
						EditorGUILayout.BeginHorizontal ();
						bundleFile.readyToPack = GUILayout.Toggle(bundleFile.readyToPack,bundleFile.GetBundleStateName() + bundleFile.m_strFileName);
						EditorGUILayout.EndHorizontal ();
					}	
				}
			}
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
		if(f != null)
			return f.IsSame (bundleInfo) == true ? BundleFileStatus.Same : BundleFileStatus.Update;
		return BundleFileStatus.New;
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
	/// 加载bundle记录文件，反序列化得到先前的bundle记录m_lstLastBundleDataInfo
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

	/// <summary>
	/// 根据资源类型获取保存的文件名
	/// </summary>
	/// <returns>The recod file name.</returns>
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

	/// <summary>
	/// 保存生成bundle的记录
	/// </summary>
	/// <param name="lstPackBundle">Lst pack bundle.</param>
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

	/// <summary>
	/// 添加之前已经保存的记录
	/// </summary>
	/// <param name="lstPackBundle">Lst pack bundle.</param>
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
				//如果当前打包的文件不存在于之前的记录中添加到当前的记录
				if(!exist){
					lstPackBundle.Add(info);
				}
				
			}
		}
	}
}