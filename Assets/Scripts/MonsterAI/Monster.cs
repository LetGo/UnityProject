using UnityEngine;
using System.Collections;

public class Monster : Role
{
    private GameObject m_model;
    public void Initialize() {
        m_node.name = "Monster";
        m_node.layer = LayerMask.NameToLayer("Role");
        FsmComponent = m_node.AddComponent<FSMMonster>();
        
        string path = "Character/player_1/player_1_1";
        GameObject prefab = Resources.Load(path) as GameObject;
        if (prefab == null)
        {
            Debug.LogError("Prefab not found: " + path);
        }
        m_model = (GameObject)GameObject.Instantiate(prefab);
        m_model.transform.parent = m_node.transform;
        AnimationCp = m_model.animation;

        BroadcastMessage("OnSetOwner", this);
        FsmComponent.Initialize();
        ((FSMMonster)FsmComponent).ChangeToState(State.Idle);
    }

    public override void Update(float deltaTime)
    {
        FsmComponent.UpdateComponent(deltaTime);
    }

    public override void Destroy()
    {
        FsmComponent.UnInitialize();
        GameObject.Destroy(m_node);
    }

    /// <summary>
    /// 后面添加事件参数 
    /// </summary>
    public override void OnGameEvent(GameEvent gameEvent)
    {
        FsmComponent.OnGameEvent(gameEvent);
    }

    public override void Hide(bool hide)
    {
        m_model.SetActive(hide ? false : true);
    }
}
