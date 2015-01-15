using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Application.pers...
/// </summary>
public class AssetDownFileRecord :Singleton<AssetDownFileRecord>  {

	private object m_objLock = null;
	private List<RecordData> m_lstCacheData = null;

	public AssetDownFileRecord(){
		this.m_objLock = new object();
		this.m_lstCacheData = new List<RecordData>();
		this.loadCache();
	}

	private class RecordData
	{
		// Fields
		public string m_strCode = string.Empty;
		public string m_strName = string.Empty;

		public bool IsSameName(string _name){
			return (this.m_strName == _name);
		}

		public bool IsSameVersion(string code){
			return (this.m_strCode == code);

		}
	}

	private void loadCache()
	{
		string sPath = getSaveCachePath ();
		if (!File.Exists (sPath)) {
			return;		
		}

		string jsonData = File.ReadAllText (sPath);
		RecordData[] data = JsonFx.Json.JsonReader.Deserialize<RecordData[]> (jsonData);
		this.m_lstCacheData.Clear ();
		for (int i = 0; i< data.Length; ++i) {
			m_lstCacheData.Add(data[i]);
		}
	}

	string GetRecordName(string url){
		string floderName = string.Empty;
		string fileName = string.Empty;
		GetFloderFileName (url, ref floderName, ref fileName);
		return floderName + "/" + fileName;  
	}

	public bool HasLocalCache(string url,string hashCode){
		lock (m_objLock) {
			if(m_lstCacheData == null)
				return false;
			string savePath = GetCacheDataSavePath(url);
			if(!File.Exists(savePath)){
				return false;
			}
			string name = GetRecordName(url);
			bool bRet = false;
			RecordData data = null;
			for (int i = 0; i< m_lstCacheData.Count; ++i) {
				if( m_lstCacheData[i].IsSameName(name)){
					data = m_lstCacheData[i];
					if(data.IsSameVersion(hashCode)){
						bRet = true;
					}
					break;
				}
			}
			//版本不同
			if(data != null && !bRet){
				m_lstCacheData.Remove(data);
				File.Delete(savePath);
			}
			return bRet;
		}
	}

	public string GetCacheDataSavePath(string url){
		string floderName = string.Empty;
		string fileName = string.Empty;
		GetFloderFileName (url, ref floderName, ref fileName);
		string floderPath = Application.persistentDataPath + "/" + floderName;
		string filePath = floderPath + "/" + fileName;
		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
			floderPath = floderPath.Replace('/','\\');
			filePath = filePath.Replace('/','\\');
		}

		DirectoryInfo dirInfo = new DirectoryInfo (floderPath);
		if (!dirInfo.Exists) {
			dirInfo.Create();		
		}
		return filePath;
	}

	void GetFloderFileName(string url,ref string floderName,ref string fileName){
		int index = url.LastIndexOf("/");
		fileName = url.Substring (index + 1);
		url = url.Substring (0, index);
		index = url.LastIndexOf("/");
		floderName = url.Substring (index + 1);
	}

	public void SaveCacheData(string url, string code)
	{
		lock(m_objLock)
		{
			RecordData data = new RecordData();
			data.m_strName = this.GetRecordName(url);
			data.m_strCode = code;
			this.m_lstCacheData.Add(data);
			this.SaveCache();
		}
	}

	private void SaveCache()
	{
		string str;
		string str2;
		str = this.getSaveCachePath();
		str2 = JsonFx.Json.JsonWriter.Serialize(this.m_lstCacheData);
		File.WriteAllText(str, str2);
	}
	//反编译
//	public void SaveCacheData(string url, string code)
//	{
//		object obj2;
//		RecordData data;
//		obj2 = this.m_objLock;
//		Monitor.Enter(obj2);
//	Label_000D:
//			try
//		{
//			data = new RecordData();
//			data.m_strName = this.getRecordName(url);
//			data.m_strCode = code;
//			this.m_lstCacheData.Add(data);
//			this.saveCache();
//			goto Label_0045;
//		}
//		finally
//		{
//		Label_003E:
//				Monitor.Exit(obj2);
//		}
//	Label_0045:
//			return;
//	}
	
	

	


	private string getSaveCachePath()
	{
		return (Application.persistentDataPath + "/_Cachedata.txt");
	}
	
	

}
