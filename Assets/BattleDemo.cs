using UnityEngine;
using System.Collections;

public class BattleDemo : MonoBehaviour {

    public GameObject btn1;
    public GameObject btn2;

	// Use this for initialization
	void Start () {
        UIEventListener.Get(btn1).onClick = OnClick1;
        UIEventListener.Get(btn2).onClick = OnClick2;
	}

    void OnClick1(GameObject go)
    {
        BattleManager.Instance.Initialize();
    }

    void OnClick2(GameObject go)
    {
        BattleManager.Instance.InitBattleEntitys();
    }
	// Update is called once per frame
	void Update () {
        BattleManager.Instance.Update(Time.deltaTime);
	}
}
