using UnityEngine;
using System.Collections;

public class MonsterIdleState : BaseState
{
    public MonsterIdleState(FSMMonster fsm)
        : base(fsm) { 
    }

    public override void Active()
    {
        //TODO 调用动作播放器 播放idle动画

        m_fsm.Owner.AnimationCp.wrapMode = WrapMode.Loop;
        m_fsm.Owner.AnimationCp.Play("idle");
    }

    public override void OnGameEvent(GameEvent gameEvent)
    {
        base.OnGameEvent(gameEvent);
        //TODO 判断是否死亡事件 执行死亡状态
        if (gameEvent.m_GEID == GameEventID.GEAttack) {
            m_fsm.ChangeToState(State.DoAction);
        }
    }

    public override void Update(float deltaTime)
    {
        //throw new System.NotImplementedException();
        //TODO 调用控制器 判断是否近战攻击，或者移动

    }

    public override bool IfCanChangeToState(State state)
    {
        return true;
    }

    public override bool IfCanPlayAnimation()
    {
        return base.IfCanPlayAnimation();
    }
}
