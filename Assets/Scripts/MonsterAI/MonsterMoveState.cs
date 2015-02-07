using UnityEngine;
using System.Collections;

public class MonsterMoveState : BaseState
{
    public MonsterMoveState(FSMMonster fsm) : base(fsm) { 
    }

    public override void Active()
    {
        //TODO 调用动作播放器 播放run动画
    }

    public override void Update(float deltaTime)
    {
        //throw new System.NotImplementedException();
        //TODO 调用控制器 判断是否近战攻击，或者移动 否则转换为idle状态
    }

    public override bool IfCanChangeToState(State state)
    {
        throw new System.NotImplementedException();
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
