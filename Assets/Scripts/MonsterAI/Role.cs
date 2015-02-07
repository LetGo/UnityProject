using UnityEngine;
using System.Collections;

public abstract class Role
{
    protected GameObject m_node;
    public GameObject Node
    {
        get { return m_node; }
        set { m_node = value; }
    }
    public Role()
    {
        m_node = new GameObject();
    }

    private FSMComponent m_fsmComponent = null;
    public FSMComponent FsmComponent
    {
        get { return m_fsmComponent; }
        set { m_fsmComponent = value; }
    }
    private Animation m_Animation = null;
    public Animation AnimationCp {
        get { return m_Animation; }
        set { m_Animation = value; }
    } 
    public abstract void Destroy();

    public abstract void Update(float deltaTime);

    public abstract void OnGameEvent(GameEvent gameEvent);

    public abstract void Hide(bool hide);
    protected void BroadcastMessage(string methodName, object parameter)
    {
        m_node.BroadcastMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
    }
}
