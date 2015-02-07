using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAI.AI
{
    class AIPatrol : AIBaseState
    {
        public AIPatrol()
            : base(AIStateId.Patrol)
        { 
        }

        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
        }

        public override void OnUpdate(float _dt)
        {
            base.OnUpdate(_dt);
        }
    }
}
