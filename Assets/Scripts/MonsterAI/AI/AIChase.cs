using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAI.AI
{
    public class AIChase : AIBaseState
    {
        public AIChase()
            : base(AIStateId.Chase)
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

        protected override AIStateId CheckTransImp(float _dt)
        {
            return base.CheckTransImp(_dt);
        }
    }
}
