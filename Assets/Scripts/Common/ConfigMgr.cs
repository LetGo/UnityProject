using UnityEngine;
using System.Collections;

public class ConfigMgr : Singleton<ConfigMgr> {
	public delegate void OnFinishLoadPlatformData();

	private BuildPlatformInfo buildPlatformInfo;
//	public Quality CurrentQuality;
	public string DeviceName;
	private static ConfigMgr m_instance;
	public OnFinishLoadPlatformData OnFinishLoadPlatformDataCallBack;
	public string strFileRootPath;
	public string strPlatformFileContent;
	
	public override void Initialize ()
	{
		base.Initialize ();
		this.DeviceName = SystemInfo.deviceName;
	//	this.SettingQuality(this.DeviceName);
	//	this.Config(this.CurrentQuality);
	//	DictMgr.GetInstance().DeviceTable = null;

	}
	public BuildPlatformInfo GetPlatformInfo()
	{
		if (this.buildPlatformInfo == null)
		{
		//	Debug.Log("---->buildPlatformInfo = " + this.buildPlatformInfo);
		//	this.buildPlatformInfo = JsonReader.Deserialize<BuildPlatformInfo>(this.strPlatformFileContent);
		}
		return this.buildPlatformInfo;
	}
	

}
