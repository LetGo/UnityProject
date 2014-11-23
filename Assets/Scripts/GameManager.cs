using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

    public ClientProxy ClientProxy { get; set; }

    public override void Initialize()
    {
        Debug.Log("GameManager Initialize");
        DictMgr.Instance.Initialize();
        UIManager.Instance.Initialize();
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

    }
}
