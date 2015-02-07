using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterAI.AI
{
	public class MoveData{
		public Vector3 start;
		public Vector3 endPos;
		public MoveData(Vector3 s,Vector3 e){
			start = s;
			endPos = e;
		}
	}

    public class AIChase : AIBaseState
    {
		Vector3 m_curTargetPos = Vector3.zero;
        public AIChase()
            : base(AIStateId.Chase)
        { 
        }

        public override void OnEnter()
        {
			GameObject p = GameObject.FindGameObjectWithTag("Player"); //玩家
			if (p != null) {
				if(( p.transform.position - m_aiFSM.initPosition ).magnitude >= MonsterAISetting.ChaseMaxDistance ){
					m_aiFSM.TransTo( AIStateId.GoHome );
				}else if ( !Action_MoveToPlayer( out m_curTargetPos ) ) {
					m_aiFSM.TransTo( AIStateId.GoHome );
				}		
			}
			base.OnEnter ();
        }

        public override void OnExit()
        {
			base.OnExit ();
        }

        public override void OnUpdate(float _dt)
        {
            base.OnUpdate(_dt);
        }

        protected override AIStateId CheckTransImp(float _dt)
        {
			GameObject p = GameObject.FindGameObjectWithTag("Player"); //玩家
			if ( p == null ) {
				return AIStateId.GoHome;
			} else {
				Vector3 _dis = p.transform.position - m_aiFSM.initPosition;
				float _distance = _dis.magnitude;
				if ( _distance > MonsterAISetting.ChaseMaxDistance ) {
					return AIStateId.GoHome;
				}

				Vector3 dis = p.transform.position  - m_roleFSM.Owner.Node.transform.position;
				float distance = dis.magnitude;
				if ( distance < MonsterAISetting.FightDistance ) {
					return AIStateId.Fight;
				}
//				Vector3 targetDiff = p.transform.position - m_curTargetPos;
//				if ( targetDiff.magnitude > MonsterAISetting.MaxPathReplanDistance ) {
//					Action_MoveToPlayer( out m_curTargetPos );
//					return StateId;
//				}
//				if ( !m_roleFSM.GetController().IsActionExecuting() ) {
//					if ( _distance >= MonsterAISetting.ChaseMaxDistance ) {
//						return AIStateId.GoHome;
//					} else if ( distance > MonsterAISetting.MaxFightDistance ) {
//						Action_MoveToPlayer( out m_curTargetPos );
//					}
//				}
			}
			return StateId;
        }
    }
}
