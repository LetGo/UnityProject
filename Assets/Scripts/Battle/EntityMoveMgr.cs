using UnityEngine;
using System.Collections;
using UniCommon;

public class EntityMoveMgr {
	public delegate void MoveEndCallback();

	float moveTime;

	const float moveDistance = 10f;

	public void BeginMoveToPostion(BattleTeamMgr team){
		team.EntityList.ApplyAll (C => Move(C,1.5f) );
		team.SetAllRun ();
	}

	void Move(BattleEntity entity,float duration){
		Vector3 from = Vector3.zero;
		Vector3 to = entity.entityGo.transform.localPosition;
		from = to;
		from.z = to.z - moveDistance;

		TweenPosition tp = TweenPosition.Begin (entity.entityGo,duration,to);
		tp.from = from;
		tp.to = to;
		tp.delay = 0.1f;
		tp.duration = duration;
		tp.eventReceiver = entity.entityGo;
        tp.callWhenFinished = "OnSkillPlayEnd";
	}
}
