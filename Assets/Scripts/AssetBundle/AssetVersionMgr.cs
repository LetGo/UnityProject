using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class AssetVersionMgr : Singleton<AssetVersionMgr> {

	private int itempServerVersion;
	private string strConfigPath;
	private string tempAssetVersionData;

	public string VersionConfigRoot
	{
		get
		{
			return "";
			//return (ConfigMgr.GetInstance().GetPlatformInfo().httpSDKRootPath + "Resources/Windows");
		}
	}
	
	public string strServerVersionPath
	{
		get
		{
			return (this.VersionConfigRoot + "/version_code.txt");
		}
	}
	
	public string strServerConfigPath
	{
		get
		{
			return (this.VersionConfigRoot + "/Assets.txt");
		}
	}

	public AssetVersionMgr()
	{
		this.strConfigPath = string.Empty;
		this.tempAssetVersionData = string.Empty;
		Caching.CleanCache();
		this.strConfigPath = Application.persistentDataPath + "/Assets.png";
	}

	public void StartCheckResouceVersion()
	{
		//从本地下载
		bool bLoadFromLocal = true;
		//bLoadFromLocal = ConfigMgr.GetInstance().GetPlatformInfo().bLoadFromLocal;
		if (bLoadFromLocal)
		{
		//	AssetMgr.Instance.LoadAssetData();
		}else
		{
		//	Logger.Info(0, "开始版本对比 : " + this.strServerVersionPath, new object[0]);
		//	HttpRequestMgr.GetInstance().StartRequest(this.strServerVersionPath, new HttpRequestMgr.DelResponse(this.analyzeResourceVersion));
		}
	}

	/// <summary>
	/// 分析总资源版本号.
	/// </summary>
	/// <param name="data">Data.</param>
	private void analyzeResourceVersion(string data)
	{
		string str;
		if (data.StartsWith ("Error")) {
			
			//Msgbox.Show(1, 1, "提示", "加载版本文件失败,是否重试?", new MessageBox.OnMsgBoxCallback(this.OnGetServiceListErrorClick), string.Empty, string.Empty);
			return;	
		}
		this.itempServerVersion = int.Parse(data);
		if (PlayerPrefsManager.ContainIntKey(enum_Int_PlayerPrefs.int_当前资源版本号))
		{
			if (PlayerPrefsManager.GetIntValue(enum_Int_PlayerPrefs.int_当前资源版本号) == this.itempServerVersion)
			{
				//Logger.Info(0, "版本号一致，不需要下载资源。可以进入游戏。以客户端资源为基准。", new object[0]);
				//this.setDownLoadInfoToUI("资源版本号一致，等待加载进入游戏。");
				if (!File.Exists(this.strConfigPath))
				{
				//	Logger.Info(0, "资源版本信息文件被删除了，重新下载资源。", new object[0]);
				//	HttpRequestMgr.GetInstance().StartRequest(this.strServerConfigPath, new HttpRequestMgr.DelResponse(this.firstDownloadAsset));
					return;
				}
				str = File.ReadAllText(this.strConfigPath);
			//	AssetMgr.Instance.LoadAssetDataByWWWResource(str);
			//	this.ToLoginPreparePage();

			}else{
				//Logger.Info(0, "版本号不一致，下载服务器资源版本信息。", new object[0]);
				//HttpRequestMgr.GetInstance().StartRequest(this.strServerConfigPath, new HttpRequestMgr.DelResponse(this.startCompareAndDownload));
			}
		}else{
			//	Logger.Info(0, "客户端暂时没有版本号，开始下载资源版本信息。", new object[0]);
			//HttpRequestMgr.GetInstance().StartRequest(this.strServerConfigPath, new HttpRequestMgr.DelResponse(this.firstDownloadAsset));
		}
	}

	/// <summary>
	/// 第一次进入游戏第一次下载资源
	/// </summary>
	/// <param name="data">Data.</param>
	private void firstDownloadAsset(string data)
	{
		this.tempAssetVersionData = data;
		//AssetMgr.Instance.LoadAssetDataByWWWResource(this.tempAssetVersionData);
		//GameMain.Instance.StartCoroutine(this.downloadAllAsset());
	}

	/// <summary>
	/// 飞第一次进入游戏，版本号不一致开始对比并下载 .
	/// </summary>
	/// <param name="data">Data.</param>
	private void startCompareAndDownload(string data)
	{
		this.tempAssetVersionData = data;
		//AssetMgr.Instance.LoadAssetDataByWWWResource(this.tempAssetVersionData);
		//GameMain.Instance.StartCoroutine(this.compareVersionAndDownload());
		return;
	}

	/// <summary>
	/// 第一次下载所有资源
	/// </summary>
	/// <returns>The all asset.</returns>
	private IEnumerator downloadAllAsset()
	{
		Dictionary<string,AssetData> assetVersionData = null; 
		foreach (AssetData asset in assetVersionData.Values) {
			//TODO获取链接并下载
			yield return null;//下载
		}
		assetAready ();
	}
	/// <summary>
	/// 对比版本信息后下载
	/// </summary>
	/// <returns>The version and download.</returns>
	private IEnumerator compareVersionAndDownload()
	{
		if (File.Exists (strConfigPath)) {
			string curVersionDada = File.ReadAllText(strConfigPath);
			Dictionary<string, AssetData> tempCurAssetData ; //= 从AssetMgr获取
			//TODO获取链接并下载
			yield return null;//下载
		}else
		{
			//没有配置文件从新下载所有downloadAllAsset
		}
	}

	private void onLoadCsvDataOver(AssetData assetData)
	{
//		GlobalDataMgr.instance.LoadConfigData();
//		UIManager.GetInstance().LoadResPathFromConfigData();
//		UILogicMgr.GetInstance().InitListener();
//		TipsMessageMgr.GetInstance().InitListenerData();
//		LoginMgr.GetInstance().PrepareLogin();
		return;
	}
	


	private void assetAready()
	{
		this.saveServerAssetsInfo();
		this.saveServerVersion();
		this.setDownLoadInfoToUI("对比下载完毕，准备开始游戏");
		this.ToLoginPreparePage();
		return;
	}
	
	private void saveServerAssetsInfo()
	{
		//Logger.Info(0, "所有资源下载完毕，将资源信息写入客户端。", new object[0]);
		File.WriteAllText(this.strConfigPath, this.tempAssetVersionData);
		return;
	}

	private void saveServerVersion()
	{
		//Logger.Info(0, "已经对比更新所有资源，将资源版本记录。", new object[0]);
		PlayerPrefsManager.SetIntValue(enum_Int_PlayerPrefs.int_当前资源版本号, this.itempServerVersion);
		return;
	}
	
	private void setDownLoadInfoToUI(string info)
	{
		//UIManager.GetInstance().SendMsgToWndUI(0, 8, info);
		return;
	}
	
	public void ToLoginPreparePage()
	{
		//AssetMgr.Instance.GetAssetData(3, "CSV", new AssetMgr.getAssetCompleteCallBack(this.onLoadCsvDataOver));
		return;
	}
	
	


	
	



}
