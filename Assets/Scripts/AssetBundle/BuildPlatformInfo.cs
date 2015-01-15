using UnityEngine;
using System.Collections;

public class BuildPlatformInfo  {
	public string androidForderName;
	public bool bLoadFromLocal;
	//public ClientMachineType buildTarget;
	public string bundleIdentifier;
	public string bundleVersion;
	public string cpaAppKey;
	public string cpaChannelID;
	public string defineSymbols;
	public string gameVersion;
	public string httpResourcesIP;
	public string httpSDKRootPath;
	public string httpServerListPath;
	public string httpServerNoticePath;
	public string iconFolderName;
	public string prefabPath;
	public string splashForderName;
	public int VersionCode;
	
	public BuildPlatformInfo()
	{
		//this.buildTarget = 1;
		this.bundleIdentifier = "com.product.company";
		this.prefabPath = "Prefabs/Base/";
		this.bundleVersion = "1.0.0";
		this.cpaChannelID = "AppStore";
		this.cpaAppKey = "b3e0263473494ecc84dcb45a571756a1";
		this.VersionCode = 1;
		this.defineSymbols = string.Empty;
		this.bLoadFromLocal = true;
		this.httpResourcesIP = "172.16.10.111";
		this.httpServerListPath = "ServerList.xml";
		this.gameVersion = "0.1.2.14530";
		this.httpServerNoticePath = "Notice/notice.xml";
		this.httpSDKRootPath = "platform/pc/";
		return;
	}
	
}
