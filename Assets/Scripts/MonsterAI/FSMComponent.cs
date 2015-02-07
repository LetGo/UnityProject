using UnityEngine;
using System.Collections;

public abstract class FSMComponent : MonoBehaviour {

    Role m_owner = null;
    public Role Owner
    {
        get { return m_owner; }
    }

    private void OnSetOwner(Role role)
    {
        m_owner = role;
    }
    public abstract void Initialize();

    public abstract void UnInitialize();

    public virtual void UpdateComponent(float deltaTime) { }

    public virtual void OnGameEvent(GameEvent gameEvent) { }

}
