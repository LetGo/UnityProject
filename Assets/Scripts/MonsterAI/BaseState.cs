using UnityEngine;
using System.Collections;

public abstract class BaseState
{
    protected FSMMonster m_fsm;

    public BaseState(FSMMonster fsm)
    {
        m_fsm = fsm;
    }
    /// <summary>
    /// 各个状态的执行入口
    /// </summary>
    public abstract void Active();
    /// <summary>
    /// 各个状态的执行完毕后的处理
    /// </summary>
    public virtual void Exit() { }
    /// <summary>
    /// 更新该状态 处理事件 转换状态
    /// </summary>
    /// <param name="deltaTime"></param>
    public abstract void Update(float deltaTime);
    /// <summary>
    /// 相应事件。比如被攻击死亡
    /// </summary>
    public virtual void OnGameEvent(GameEvent gameEvent) { }

    public abstract bool IfCanChangeToState(State state);

    public virtual bool IfCanPlayAnimation() { return true; }

    protected void ThrowActiveByWrongGameEventExcpetion()
    {
        string message = string.Format("{0} state is actived by wrong game event : game event id = {1}", this.ToString());
        throw new System.Exception(message);
    }
}