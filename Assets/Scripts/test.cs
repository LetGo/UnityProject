using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JsonText{
	public string name;
	public int id;
}

public class Test : MonoBehaviour {

	public GameObject btn1;
	public GameObject btn2;

	void OnClick(){
		StartCoroutine (down ());
	}
	IEnumerator down(){
		string path = "file://" + Application.streamingAssetsPath + "/AssetBundles/Windows/Prefab/LoadingPanel.unity3d";
		path = "http://119.147.215.92/ServerList_bs.xml";
		Debug.Log (path);
		WWW www = new WWW ( path );

		yield return www;

		//AssetBundle bundle = www.assetBundle;

		if (www.isDone) {
			Debug.Log (www.error);
			if(www.error == null)
				Debug.Log (www.text);

			//GameObject go = GameObject.Instantiate (bundle.mainAsset ) as GameObject;
		}

	}

	void OnClik1(GameObject go){
		BattleManager.Instance.Initialize();
//		JsonText jt = new JsonText ();
//		jt.id = 1;
//		jt.name = "nnd";
//		string str = JsonFx.Json.JsonWriter.Serialize (jt);
//		System.IO.File.WriteAllText (Application.streamingAssetsPath + "/test.json", str);
	}
	bool update = false;
	void OnClik2(GameObject go){
		BattleManager.Instance.InitBattleEntitys ();
		update = true;
	}

	void Start(){
	//	UIEventListener.Get (btn1).onClick = OnClik1;
	//	UIEventListener.Get (btn2).onClick = OnClik2;
	//	//OnClik ();
	//	Des ();
	}

	void Des(){
		if (System.IO.File.Exists (Application.streamingAssetsPath + "/test.json")) {
			string str = System.IO.File.ReadAllText(Application.streamingAssetsPath + "/test.json");
			JsonText jt =JsonFx.Json.JsonReader.Deserialize<JsonText>(str);
			Debug.Log(jt.name);
			Debug.Log(jt.id);
		}
	}

	void Update(){
		if(update)
			BattleManager.Instance.Update (Time.deltaTime);
	}
}
 