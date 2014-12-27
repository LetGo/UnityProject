using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JsonText{
	public string name;
	public int id;
}

public class test : MonoBehaviour {

	void OnClik(){
		JsonText jt = new JsonText ();
		jt.id = 1;
		jt.name = "nnd";
		string str = JsonFx.Json.JsonWriter.Serialize (jt);
		System.IO.File.WriteAllText (Application.streamingAssetsPath + "/test.json", str);
	}

	void Start(){
		//OnClik ();
		Des ();
	}

	void Des(){
		if (System.IO.File.Exists (Application.streamingAssetsPath + "/test.json")) {
			string str = System.IO.File.ReadAllText(Application.streamingAssetsPath + "/test.json");
			JsonText jt =JsonFx.Json.JsonReader.Deserialize<JsonText>(str);
			Debug.Log(jt.name);
			Debug.Log(jt.id);
		}
	}
}
