using UnityEngine;
using System.Collections;
public enum State
{
    None = -1,
    Idle = 0, // idle
    Move, // move stop
    BattleIdle, // battleready
    DoAction, // skillcast, baseattack1, baseattack2
    Dead, // dead
    Hurt, // hurt
    Num
}

public class FSMMonster : FSMComponent, IActionCmdTranslator
{
    private State m_preState;
    private State m_activeState;
    private BaseState[] m_states;
    private MonsterAI.AI.BaseActionController m_actionCtrl = null;
    public override void Initialize()
    {
        m_states = new BaseState[(int)State.Num];
        m_states[(int)State.Idle] = new MonsterIdleState(this);
        m_states[(int)State.Move] = new MonsterMoveState(this);
        m_states[(int)State.DoAction] = new MonsterDoActionState(this);
        m_states[(int)State.Dead] = new MonsterDeadState(this);
        m_activeState = State.Idle;

        m_actionCtrl = new MonsterAI.AI.AIController(this);
        m_actionCtrl.Initialize();
       // m_actionCtrl.GetActionCtrl().SetActionCmdTranslator(this);
    }

    public override void UnInitialize()
    {
        for (int i = 0; i < (int)State.Num; ++i)
        {
            m_states[i] = null;
        }
        m_states = null;
    }

    public override void UpdateComponent(float deltaTime)
    {
        base.UpdateComponent(deltaTime);
        if (m_actionCtrl != null)
        {
            m_actionCtrl.Updtae(deltaTime);
        }
        m_states[(int)m_activeState].Update(deltaTime);
    }
    public override void OnGameEvent(GameEvent gameEvent)
    {
        base.OnGameEvent(gameEvent);
        m_states[(int)m_activeState].OnGameEvent(gameEvent);
    }

    public void ChangeToState(State state)
    {
        if (m_activeState != State.None)
            m_states[(int)m_activeState].Exit();
        m_activeState = state;
        m_states[(int)m_activeState].Active();
    }

    public bool TryChangeToState(State state)
    {
        if (m_activeState != State.None &&
            m_states[(int)m_activeState].IfCanChangeToState(state) == false)
        {
            return false;
        }
        if (m_activeState != State.None)
        {
            m_states[(int)m_activeState].Exit();
        }
        m_preState = m_activeState;
        m_activeState = state;
        m_states[(int)m_activeState].Active();
        return true;
    }

    public  bool MoveCmd_Translating(SubActionCmd movSCmd, GameEvent dirCmd) 
    {
        return true;
    }
    public bool ActionCmd_translating(SubActionCmd actSCmd, GameEvent actCmd) { return true; }
    public bool AuxActionCmd_translating(SubActionCmd actSCmd, GameEvent auxActCmd) { return true; }
}
