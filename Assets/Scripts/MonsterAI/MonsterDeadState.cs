using UnityEngine;
using System.Collections;

public class MonsterDeadState : BaseState
{
    public MonsterDeadState(FSMMonster fsm) : base(fsm) { 

    }

    public override void Active()
    {
        throw new System.NotImplementedException();
    }

    public override void Update(float deltaTime)
    {
        throw new System.NotImplementedException();
    }

    public override bool IfCanChangeToState(State state)
    {
        throw new System.NotImplementedException();
    }

    public override void OnGameEvent(GameEvent gameEvent)
    {
        base.OnGameEvent(gameEvent);
    }


}
