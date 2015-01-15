using UnityEngine;
using System.Collections;

public class BattleDemo : GameUIPanelBase {

    public GameObject btn1;
    public GameObject btn2;
    public Animation anim;
	// Use this for initialization
	void Start () {
        UIEventListener.Get(btn1).onClick = OnClick1;
        UIEventListener.Get(btn2).onClick = OnClick2;
	}

    void OnClick1(GameObject go)
    {
      // anim.Play("run");
        UIManager.Instance.Log("ceshi");
        HerosManager.Instance.Initialize();
//         HerosManager.Instance.AddNewHero(1001);
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

    public override bool OnMsg(PanelMsgID msgID, object obj = null)
    {
        throw new System.NotImplementedException();
    }
}
