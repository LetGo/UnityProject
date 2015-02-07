using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAI.AI
{
    class AIController : BaseActionController
    {
        FSMMonster m_fsm;
        AIFSM m_aiFSM;
        public AIController(FSMMonster fsm) {
            m_fsm = fsm;
            m_aiFSM = new AIFSM();
        }

        public override void Initialize()
        {
            base.Initialize();
            m_aiFSM.AddState<AIIdle>();
            m_aiFSM.AddState<AIPatrol>();
            m_aiFSM.SetDefaultStateId(AIStateId.Idle);
            m_aiFSM.Init( m_fsm );
        }

        public override bool Updtae(float _dt)
        {
            m_aiFSM.Update(_dt);
            return base.Updtae(_dt);
        }
    }
}
