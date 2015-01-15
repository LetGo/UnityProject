using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class  AssetBundleManager
{
	private static Dictionary<string, AssetBundleRef> dictAssetBundleRefs;
	
	static AssetBundleManager(){
		dictAssetBundleRefs = new Dictionary<string, AssetBundleRef>();
	}

	public static IEnumerator DownloadAssetBundle(string url, string hashCode)
	{
		string key = url + hashCode;
		if ( dictAssetBundleRefs.ContainsKey (key) ) {
			yield return null;	
		}else {
			Debug.Log("没有缓存到内存");
			AssetBundleRef abRef = new AssetBundleRef(url,hashCode);
			string savePath = AssetDownFileRecord.Instance.GetCacheDataSavePath(url);
			if(AssetDownFileRecord.Instance.HasLocalCache(url,hashCode)){
				Debug.Log(string.Format("本地有文件缓存URL:{0} CODE :",url,hashCode));
				string loadPath = string.Empty;
				if(Application.platform == RuntimePlatform.Android){
					loadPath = "file://" + savePath;
				}else{
					loadPath = "file://" + savePath;
				}
				abRef.www = new WWW(loadPath);
				while(!abRef.www.isDone){
					yield return new WaitForSeconds(1.0f);
				}
				if(abRef.www.error != null){
					Debug.LogError("load url" + abRef.www.error);
				}
				abRef.assetBundle = abRef.www.assetBundle;
				dictAssetBundleRefs.Add(key,abRef);
			}else{
				Debug.Log(string.Format("本地有文件没缓存开始下载URL:{0} CODE :",url,hashCode));
				abRef.www = new WWW(url);
				while(!abRef.www.isDone){
					yield return new WaitForSeconds(1.0f);
				}
				if(abRef.www.error != null){
					Debug.LogError("load url" + abRef.www.error);
				}

				yield return abRef.www;
				abRef.assetBundle = abRef.www.assetBundle;
				dictAssetBundleRefs.Add(key,abRef);
			}
		}
	}
	
	public static  AssetBundle GetAssetBundle(string url, string hashCode)
	{
		string key = url + hashCode;
		AssetBundleRef abRef;
		if (dictAssetBundleRefs.TryGetValue (key, out abRef)) {
			return abRef.assetBundle;		
		}
		return null;
	}
	
	public static void Unload(string url, string hashCode, bool allObjects)
	{
		string str = url + hashCode;;
		AssetBundleRef abRef;
		if (dictAssetBundleRefs.TryGetValue(str, out abRef))
		{
			if (null != abRef.assetBundle )
			{
				abRef.assetBundle.Unload(allObjects);
				abRef.assetBundle = null;
			}
			dictAssetBundleRefs.Remove(str);
		}
	}

	public static void UnloadAllMirroring(string hashCode)
	{
		while(dictAssetBundleRefs.Count > 0){
			foreach(string key in dictAssetBundleRefs.Keys){
				if(dictAssetBundleRefs[key].assetBundle)dictAssetBundleRefs[key].assetBundle.Unload(true);
				dictAssetBundleRefs.Remove(key);
				break;
			}
		}
	}

	public static float GetDownloadProgress(string url, string hashCode)
	{
		string str;
		AssetBundleRef ref2;
		str = url + hashCode;
		if (dictAssetBundleRefs.TryGetValue(str, out ref2) )
		{
			return ref2.www.progress;
		}
		return 0f;
	}
	
	public static void StopDownload(string url, string hashCode)
	{
		string str;
		AssetBundleRef ref2;
		str = url + hashCode;
		if (dictAssetBundleRefs.TryGetValue(str, out ref2))
		{
			ref2.www.Dispose();
		}
	}
	


	private class AssetBundleRef
	{
		public AssetBundle assetBundle;
		public string hashCode;
		public string url;
		public WWW www;
		
		public AssetBundleRef(string strUrlIn, string strHashCode){
			this.url = strUrlIn;
			this.hashCode = strHashCode;
		}
	}

}
