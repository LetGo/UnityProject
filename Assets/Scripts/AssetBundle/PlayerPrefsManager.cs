using UnityEngine;
using System.Collections;

public static class PlayerPrefsManager  {
	public const string KEY_USER_FASHION = "user_fashion";
	public const string KYE_USER_WINGS = "user_wings";
	
	public static bool ContainIntKey(enum_Int_PlayerPrefs key)
	{
		return PlayerPrefs.HasKey(((enum_Int_PlayerPrefs) key).ToString());
	}
	

	public static int GetIntValue(enum_Int_PlayerPrefs intPref)
	{
		return PlayerPrefs.GetInt(((enum_Int_PlayerPrefs) intPref).ToString());
	}
	
	
	public static void SetIntValue(enum_Int_PlayerPrefs intPref, int value)
	{
		PlayerPrefs.SetInt(((enum_Int_PlayerPrefs) intPref).ToString(), value);
		PlayerPrefs.Save();
		return;
	}
	
	

}
