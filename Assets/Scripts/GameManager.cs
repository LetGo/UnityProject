using UnityEngine;
using System.Collections;
using GameState;

public class GameManager : Singleton<GameManager> {

    public ClientProxy ClientProxy { get; set; }
    GameSceneManager mGameSceneManager;

    public override void Initialize()
    {
        Debug.Log("GameManager Initialize");
        mGameSceneManager = GameSceneManager.Instance;
        DictMgr.Instance.Initialize();
        UIManager.Instance.Initialize();
       // mGameSceneManager.Initialize();
    }

    public override void UnInitialize()
    {
        Debug.Log("GameManager UnInitialize");
        UIManager.Instance.UnInitialize();
        DictMgr.Instance.UnInitialize();
        base.UnInitialize();
    }

    public void UnLoadUnuseAsset()
    {
        ClientProxy.UnLoadUnuseAsset();
    }

    public void MainUpdate()
    {
        mGameSceneManager.Update(Time.deltaTime);
    }
}
