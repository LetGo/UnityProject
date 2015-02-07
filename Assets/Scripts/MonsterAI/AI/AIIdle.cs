using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAI.AI
{
    class AIIdle : AIBaseState
    {
        public AIIdle() :base(AIStateId.Idle) { 
            
        }

        public override void OnEnter()
        {
           
        }

        public override void OnExit()
        {
            
        }

        protected override AIStateId CheckTransImp(float _dt) 
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player"); //玩家
            if (p != null)
            {
                var dir = p.transform.position - m_roleFSM.Owner.Node.transform.position;
                if (dir.magnitude <= MonsterAISetting.SightDistance)
                {
                    return AIStateId.Chase;
                }
                else { Debug.Log("dir.magnitude :" + dir.magnitude); }
            }
            else { Debug.Log("no player "); }
           
            return m_AIStateId; 
        }
    }
}
