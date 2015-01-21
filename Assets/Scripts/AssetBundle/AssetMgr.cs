using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;

public class AssetMgr : MonoBehaviour {
	private Dictionary<string, AssetBundle> _assetBundleMap;
	private Dictionary<string, AssetBundle> _loadingBundles;
	private bool m_bDataInit;
	public Dictionary<string, AssetData> m_dicAssetData;
	private Dictionary<string, getAssetCompleteCallBack> m_dicCacheLoadingCallBackFunc;
	private static AssetMgr m_instance;
	private List<string> m_lstCallBackDeleteKey;
	public const string PlatfromResPath = "/JsonData/PlatformInfo/PlatformInfo.txt";
	private string strAssetBundlePath;
	private string strLocalAssetBundlePath;
	
	// Methods
	public AssetMgr()
	{
		this.m_dicAssetData = new Dictionary<string, AssetData>();
		this.m_dicCacheLoadingCallBackFunc = new Dictionary<string, getAssetCompleteCallBack>();
		this.m_lstCallBackDeleteKey = new List<string>();
		this._assetBundleMap = new Dictionary<string, AssetBundle>();
		this._loadingBundles = new Dictionary<string, AssetBundle>();

		return;
	}
	
	public static Dictionary<string, AssetData> AnalysisAssetJsonData(string data)
	{
		Dictionary<string, AssetData> dictionary;
//		Hashtable hashtable;
//		AssetData[] dataArray;
//		AssetData data2;
//		AssetData[] dataArray2;
//		int num;
//		string str;
//		AssetData[] dataArray3;
//		AssetData data3;
//		AssetData[] dataArray4;
//		int num2;
//		string str2;
//		AssetData data4;
//		string str3;
		dictionary = new Dictionary<string, AssetData>();
//		hashtable = JsonReader.Deserialize<Hashtable>(data);
//		if (hashtable != null)
//		{
//			goto Label_0025;
//		}
//		Debug.LogError("load json data error! : " + data);
		return dictionary;
//	Label_0025:
//			if (hashtable.ContainsKey("scenes") == null)
//		{
//			goto Label_007E;
//		}
//		dataArray2 = JsonReader.Deserialize<AssetData[]>(hashtable["scenes"].ToString());
//		num = 0;
//		goto Label_0073;
//	Label_0056:
//			data2 = dataArray2[num];
//		str = data2.GetSaveKey();
//		dictionary.Add(str, data2);
//		num += 1;
//	Label_0073:
//			if (num < ((int) dataArray2.Length))
//		{
//			goto Label_0056;
//		}
//	Label_007E:
//			if (hashtable.ContainsKey("prefabs") == null)
//		{
//			goto Label_00DC;
//		}
//		dataArray4 = JsonReader.Deserialize<AssetData[]>(hashtable["prefabs"].ToString());
//		num2 = 0;
//		goto Label_00D1;
//	Label_00B1:
//			data3 = dataArray4[num2];
//		str2 = data3.GetSaveKey();
//		dictionary.Add(str2, data3);
//		num2 += 1;
//	Label_00D1:
//			if (num2 < ((int) dataArray4.Length))
//		{
//			goto Label_00B1;
//		}
//	Label_00DC:
//			if (hashtable.ContainsKey("csv") == null)
//		{
//			goto Label_0116;
//		}
//		data4 = JsonReader.Deserialize<AssetData>(hashtable["csv"].ToString());
//		str3 = data4.GetSaveKey();
//		dictionary.Add(str3, data4);
//	Label_0116:
//			return dictionary;
	}
	
//	[DebuggerHidden]
//	private IEnumerator GetAsset(AssetData assetData, getAssetCompleteCallBack callback)
//	{
//		<GetAsset>c__Iterator8 iterator;
//		iterator = new <GetAsset>c__Iterator8();
//		iterator.assetData = assetData;
//		iterator.callback = callback;
//		iterator.<$>assetData = assetData;
//		iterator.<$>callback = callback;
//		iterator.<>f__this = this;
//		return iterator;
//	}
	
	public void GetAssetData(AssetType type, string name, getAssetCompleteCallBack callback)
	{
//		string str;
//		str = GetAssetDataKey(type, name);
//		if (this.m_dicAssetData.ContainsKey(str) == null)
//		{
//			goto Label_006A;
//		}
//		if (this.m_dicAssetData[str].m_LoadedStatus != 2)
//		{
//			goto Label_0047;
//		}
//		callback(this.m_dicAssetData[str]);
//		goto Label_0065;
//	Label_0047:
//			//GameMain.Instance.StartCoroutine(this.GetAsset(this.m_dicAssetData[str], callback));
//	Label_0065:
//			goto Label_007F;
//	Label_006A:
//			Debug.LogError("NO THIS ASSET:" + str.ToString());
//	Label_007F:
//			return;
	}
	
	public static string GetAssetDataKey(AssetType type, string _name)
	{
		return (((int) type) + "_" + _name);
	}
	
	public string GetAssetPath(AssetType assetType, string assetName = "", string relativePath = "")
	{
		string str;
//		AssetType type;
		str = string.Empty;
//		type = assetType;
//		switch ((type - 1))
//		{
//		case 0:
//			goto Label_0021;
//			
//		case 1:
//			goto Label_0054;
//			
//		case 2:
//			goto Label_0087;
//		}
//		goto Label_00B0;
//	Label_0021:
//			str = this.strLocalAssetBundlePath + "Scene/";
//		if ((assetName != string.Empty) == null)
//		{
//			goto Label_00B0;
//		}
//		str = str + assetName + ".unity3d";
//		goto Label_00B0;
//	Label_0054:
//			str = this.strLocalAssetBundlePath + "Prefab/";
//		if ((assetName != string.Empty) == null)
//		{
//			goto Label_00B0;
//		}
//		str = str + assetName + ".unity3d";
//		goto Label_00B0;
//	Label_0087:
//			str = this.strAssetBundlePath;
//		if ((assetName != string.Empty) == null)
//		{
//			goto Label_00B0;
//		}
//		str = str + assetName + ".unity3d";
//	Label_00B0:
			return str;
//	}
//	
//	public string GetCsvData(string name)
//	{
//		TextAsset asset;
//		string str;
//		TextAsset asset2;
//		if (Application.platform == 7)
//		{
//			goto Label_0015;
//		}
//		if (Application.platform != null)
//		{
//			goto Label_0043;
//		}
//	Label_0015:
//			asset = Resources.Load("CSV/" + name) as TextAsset;
//		if (asset == null)
//		{
//			goto Label_003D;
//		}
//		return asset.text;
//	Label_003D:
//			return string.Empty;
//	Label_0043:
//			str = GetAssetDataKey(3, "CSV");
//		if (this.m_dicAssetData.ContainsKey(str) != null)
//		{
//			goto Label_0076;
//		}
//		Debug.LogError("Load CSV data error!!! : " + str);
//		return string.Empty;
//	Label_0076:
//			asset2 = this.m_dicAssetData[str].m_bundleLoadedAsset.Load(name) as TextAsset;
//		if ((asset2 == null) == null)
//		{
//			goto Label_00B5;
//		}
//		Debug.LogError("no find data: " + name);
//		return string.Empty;
//	Label_00B5:
//			return asset2.text;
	}
	
	public float GetDownloadProgress(AssetData assetData)
	{
		string str;
		return AssetBundleManager.GetDownloadProgress(this.GetAssetPath(assetData.m_AssetType, assetData.m_strAssetPath, string.Empty), assetData.m_strCode);
	}
	
	public string GetFbJsonData(string name)
	{
		//TextAsset asset;
//		string str;
//		string str2;
//		str = "JsonData/FbEvent/" + name;
//		if (Resources.Load(str) == null)
//		{
//			goto Label_0037;
//		}
//		asset = Resources.Load(str, typeof(TextAsset)) as TextAsset;
//		goto Label_003D;
//	Label_0037:
		return string.Empty;
//	Label_003D:
	//return asset.text;
	}
	
	public void InitAssetBundlePath()
	{
//		string[] textArray1;
//		if (ConfigMgr.GetInstance().GetPlatformInfo().bLoadFromLocal == null)
//		{
//			goto Label_0033;
//		}
//		this.strAssetBundlePath = "file://" + Application.streamingAssetsPath + "/Assetbundles/Windows/";
//		goto Label_0057;
//	Label_0033:
//			this.strAssetBundlePath = "http://" + ConfigMgr.GetInstance().GetPlatformInfo().httpResourcesIP + "/Resources/Windows/";
//	Label_0057:
//			this.strLocalAssetBundlePath = "file://" + Application.streamingAssetsPath + "/Assetbundles/Windows/";
//		textArray1 = new string[] { "AssetMgr Init Done ! strAssetBundlePath:", this.strAssetBundlePath, "  strStreamAssetPath : ", StreamAssetPath, ", strLocalAssetBundlePath:", this.strLocalAssetBundlePath };
//		Logger.Info(0, string.Concat(textArray1), new object[0]);
//		return;
	}
	
	public void LoadAssetData()
	{
//		string str;
//		if (this.m_bDataInit != null)
//		{
//			goto Label_0033;
//		}
//		str = this.strAssetBundlePath + "Assets.txt";
//		TXTFileLoader.GetInstance().LoadTXTFileByName(new TXTFileLoader.OnFinishLoadCallBack(this.LoadAssetDataByWWWResource), str);
//	Label_0033:
//			return;
	}
	
	public void LoadAssetDataByWWWResource(string assetVersionData)
	{
//		if (this.m_bDataInit != null)
//		{
//			goto Label_003C;
//		}
//		this.m_dicAssetData = AnalysisAssetJsonData(assetVersionData);
//		this.m_bDataInit = 1;
//		if (ConfigMgr.GetInstance().GetPlatformInfo().bLoadFromLocal == null)
//		{
//			goto Label_003C;
//		}
//		AssetVersionMgr.Instance.ToLoginPreparePage();
//	Label_003C:
//			return;
	}
	
	public void ReleaseAssetMemory(AssetType type, string name)
	{
		string str;
//		AssetData data;
//		string str2;
//		str = GetAssetDataKey(type, name);
//		if (this.m_dicAssetData.ContainsKey(str) == null)
//		{
//			goto Label_0059;
//		}
//		data = this.m_dicAssetData[str];
//		data.m_bundleLoadedAsset = null;
//		data.m_LoadedStatus = 0;
//		AssetBundleManager.Unload(this.GetAssetPath(data.m_AssetType, data.m_strAssetPath, string.Empty), data.m_strCode, 1);
//	Label_0059:
			return;
	}
	
	public void RemoveAssetBundle(string bundleName)
	{
		if (this._assetBundleMap.ContainsKey(bundleName) == null)
		{
			goto Label_001E;
		}
		this._assetBundleMap.Remove(bundleName);
	Label_001E:
			return;
	}
	
	public void Run()
	{
//		KeyValuePair<string, getAssetCompleteCallBack> pair;
//		Dictionary<string, getAssetCompleteCallBack>.Enumerator enumerator;
//		string str;
//		List<string>.Enumerator enumerator2;
//		enumerator = this.m_dicCacheLoadingCallBackFunc.GetEnumerator();
//	Label_000C:
//			try
//		{
//			goto Label_0066;
//		Label_0011:
//				pair = &enumerator.Current;
//			if (this.m_dicAssetData[&pair.Key].m_LoadedStatus != 2)
//			{
//				goto Label_0066;
//			}
//			this.m_lstCallBackDeleteKey.Add(&pair.Key);
//			&pair.Value(this.m_dicAssetData[&pair.Key]);
//		Label_0066:
//				if (&enumerator.MoveNext() != null)
//			{
//				goto Label_0011;
//			}
//			goto Label_0083;
//		}
//		finally
//		{
//		Label_0077:
//				((Dictionary<string, getAssetCompleteCallBack>.Enumerator) enumerator).Dispose();
//		}
//	Label_0083:
//			if (this.m_lstCallBackDeleteKey.Count <= 0)
//		{
//			goto Label_00E2;
//		}
//		enumerator2 = this.m_lstCallBackDeleteKey.GetEnumerator();
//	Label_00A0:
//			try
//		{
//			goto Label_00BA;
//		Label_00A5:
//				str = &enumerator2.Current;
//			this.m_dicCacheLoadingCallBackFunc.Remove(str);
//		Label_00BA:
//				if (&enumerator2.MoveNext() != null)
//			{
//				goto Label_00A5;
//			}
//			goto Label_00D7;
//		}
//		finally
//		{
//		Label_00CB:
//				((List<string>.Enumerator) enumerator2).Dispose();
//		}
//	Label_00D7:
//			this.m_lstCallBackDeleteKey.Clear();
//	Label_00E2:
//			return;
	}
	
	// Properties
	public static AssetMgr Instance
	{
		get
		{
			if (m_instance != null)
			{
				goto Label_0014;
			}
			m_instance = new AssetMgr();
		Label_0014:
				return m_instance;
		}
	}
	
	public static string StreamAssetPath
	{
		get
		{
			return ("file://" + Application.streamingAssetsPath);
		}
	}

	
	public delegate void getAssetCompleteCallBack(AssetData data);
	
	public delegate void InitAssetLoadCompleteCallBack();
}

