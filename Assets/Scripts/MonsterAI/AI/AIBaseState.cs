using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterAI.AI
{
    public enum AIStateId
    {
        None = -1,
        Idle = 0,
        Fight,
        Chase,          //追逐
        AdjustPose,
        Retreat,
        Patrol,
        GoHome,
        Num
    }

    public abstract class AIBaseState
    {
        protected FSMMonster m_roleFSM = null;
        protected AIFSM m_aiFSM = null;
        
        protected AIStateId m_AIStateId = AIStateId.None;
        public AIStateId StateId { get { return m_AIStateId; } }
        public AIBaseState(AIStateId state) {
            m_AIStateId = state;
        }
        /// <summary>
        /// FSMMonster
        /// </summary>
        /// <param name="o"> FSMMonster</param>
        /// <returns></returns>
        public bool Init(AIFSM aifsm, FSMMonster fsm) 
        {
            m_aiFSM = aifsm;
            m_roleFSM = fsm;
            return true; 
        }

        public virtual void OnEnter() { Debug.Log("OnEnter :" + m_AIStateId); }
        public virtual void OnExit() { Debug.Log("OnExit :" + m_AIStateId); }

        public void SetAIBaseFSM(AIFSM aifsm) {
            m_aiFSM = aifsm;
        }

		public bool Action_MoveTo(Vector3 target) 
        {
			//如果不能寻路返回false

			var ctrl = m_roleFSM.GetController();
			Vector3 startPos = m_roleFSM.Owner.Node.transform.position;
			Vector3 endPos = target;
			ctrl.BeginPushAction();
			GameEvent act = new GameEvent (GameEventID.GEMove,new MoveData(startPos,endPos));
			ctrl.PostAction( act );
			return true;
        }

        public virtual void OnUpdate(float _dt)
        {
            //NGUIDebug.Log("OnUpdate :" + m_AIStateId);
        }

		public bool Action_MoveToPlayer( out Vector3 pos ) {
			GameObject p = GameObject.FindGameObjectWithTag("Player"); //玩家
			if ( p == null ) {
				pos = Vector3.zero;
				return false;
			}
			pos = p.transform.position;
			return Action_MoveTo( pos );
		}

        public AIStateId CheckTrans(float _dt) { return CheckTransImp(_dt); }

        protected virtual AIStateId CheckTransImp(float _dt) { return m_AIStateId; }
    }
}
