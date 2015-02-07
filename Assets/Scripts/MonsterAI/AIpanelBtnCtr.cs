using UnityEngine;
using System.Collections;

public class AIpanelBtnCtr : MonoBehaviour {
    public Vector3 moveTarget;

    public GameObject BtnIdle;
    public GameObject BtnMove;
    public GameObject BtnAttack;
    public GameObject BtnStart;
    Monster mMonster;
	// Use this for initialization
	void Start () {
        UIEventListener.Get(BtnStart).onClick = OnStart;
        UIEventListener.Get(BtnIdle).onClick = OnIdle;
        UIEventListener.Get(BtnMove).onClick = OnMove;
        UIEventListener.Get(BtnAttack).onClick = OnAttack;
	}

    void OnStart(GameObject go) {
        mMonster = new Monster();
        mMonster.Initialize();
    }

    void OnIdle(GameObject go)
    {

    }
    void OnMove(GameObject go)
    {

    }
    void OnAttack(GameObject go)
    {
        mMonster.OnGameEvent(new GameEvent(GameEventID.GEAttack, null));
    }
	// Update is called once per frame
	void Update () {
        if(mMonster != null)
            mMonster.Update(Time.deltaTime);
	}
}
