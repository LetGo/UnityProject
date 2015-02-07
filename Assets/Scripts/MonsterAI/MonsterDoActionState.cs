using UnityEngine;
using System.Collections;

public class MonsterDoActionState : BaseState
{
    string attackAnim = string.Empty;
    float timeOut = 0;
    public MonsterDoActionState(FSMMonster fsm)
        : base(fsm)
    { 
    }
    public override void Active()
    {
        //TODO 调用动作播放器 播放攻击动画 获取动作时长timeOut
        m_fsm.Owner.AnimationCp.wrapMode = WrapMode.Once;
        m_fsm.Owner.AnimationCp.Play("baseattack1");
        timeOut = m_fsm.Owner.AnimationCp["baseattack1"].length;
    }

    public override void Update(float deltaTime)
    {
        //throw new System.NotImplementedException();
        //TODO 处理攻击是否成功等。。

        if (timeOut <= 0 )
        {
            m_fsm.TryChangeToState(State.Idle);
        }
        timeOut -= deltaTime;
    }

    public override bool IfCanChangeToState(State state)
    {
        if (timeOut <= 0) {
            return true;
        }

        return false;
    }

    public override void OnGameEvent(GameEvent gameEvent)
    {
        base.OnGameEvent(gameEvent);
        //TODO 判断是否死亡事件 执行死亡状态
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override bool IfCanPlayAnimation()
    {
        return base.IfCanPlayAnimation();
    }
}
