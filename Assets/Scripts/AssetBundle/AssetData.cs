using UnityEngine;
using System.Collections;

public class AssetData  {

	// Fields
	public AssetType m_AssetType;
	public AssetBundle m_bundleLoadedAsset;
	public AssetStatus m_LoadedStatus;
	public string m_strAssetPath;
	public string m_strCode;
	
	// Methods
	public AssetData()
	{
		this.m_strCode = string.Empty;
		this.m_AssetType = AssetType.Default;
		this.m_strAssetPath = string.Empty;
		this.m_bundleLoadedAsset = null;
		this.m_LoadedStatus = AssetStatus.NotReady;
		return;
	}
	
	public string GetSaveKey()
	{
		//return AssetMgr.GetAssetDataKey(this.m_AssetType, this.m_strAssetPath);
		return "";
	}
	
	public bool IsSameVersion(AssetData data)
	{
		return (this.m_strCode == data.m_strCode);
	}

}
