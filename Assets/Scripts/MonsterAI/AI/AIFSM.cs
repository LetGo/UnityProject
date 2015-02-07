using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAI.AI
{
    public class AIFSM
    {
        protected AIBaseState m_lastState = null;
        protected AIBaseState m_currentState = null;
        protected AIBaseState m_goalState = null;
        protected AIBaseState m_defaultState = null;
        protected object m_goalStateParam = null;

       
        protected Dictionary<AIStateId, AIBaseState> m_states = new Dictionary<AIStateId, AIBaseState>();
        
        public bool AddState<StateType>() where StateType : AIBaseState, new()
        {
            StateType state = new StateType();
            AIBaseState v = null;
            if (m_states.TryGetValue(state.StateId, out v))
            {
                return false;
            }
            m_states[state.StateId] = state;
            return true;
        }

        public bool SetDefaultStateId(AIStateId _id)
        {
            AIBaseState v = null;
            if (m_states.TryGetValue(_id, out v))
            {
                m_defaultState = v;
                return true;
            }
            return false;
        }

        public bool Update(float _dt) {
            if (m_states.Count <= 0) return false;

            if (m_goalState != null) {
                if (m_currentState != null)
                    m_currentState.OnExit();
                m_lastState = m_currentState;
                m_currentState = m_goalState;
                m_goalState = null;
                m_currentState.OnEnter();
                m_currentState.OnUpdate(_dt);
                //NGUIDebug.Log("m_goalState");
            }
            else if (m_currentState != null)
            {
                m_currentState.OnUpdate(_dt);
                //NGUIDebug.Log("m_currentState");
            }
            else
            {
                //NGUIDebug.Log("else");
                m_currentState = m_defaultState;
                m_currentState.OnEnter();
                m_goalStateParam = null;
                m_currentState.OnUpdate(_dt);
            }

            AIStateId goalId = m_currentState.CheckTrans(_dt);
            if (!goalId.Equals(m_currentState.StateId))
            {
                TransTo(goalId, m_goalStateParam);
            }
            return true;
        }

        public bool TransTo(AIStateId goalId, object arg = null)
        {
            AIBaseState v = null;
            if (m_states.TryGetValue(goalId, out v))
            {
                m_goalState = v;
                m_goalStateParam = arg;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 传入FSM
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool Init(FSMMonster param) { return OnInit(param); }

        protected bool OnInit(FSMMonster param)
        {
            foreach (var it in m_states)
            {
                if (!it.Value.Init(this,param))
                {
                    return false;
                }
            }
            if (m_defaultState != null && m_states.Count > 0)
            {
                m_defaultState = m_states.First().Value;
            }
            return true;
        }
    }
}
