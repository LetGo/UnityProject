using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAI.AI
{
    class BaseActionController
    {
        public ActionCmdManager m_CmdManager;
        public GameEvent m_directionCmd = new GameEvent();
        public GameEvent m_actionCmd = new GameEvent();

        public virtual void Initialize()
        {
            m_CmdManager = new ActionCmdManager();
        }

        public virtual bool Updtae(float _dt)
        {
            Clear();
            return true;
        }

        public virtual void Clear()
        {
            m_directionCmd.Reset();
            m_actionCmd.Reset();
        }

        public GameEventID GetDirectionCmdType() { return m_directionCmd.m_GEID; }
        public GameEventID GetActionCmdType() { return m_actionCmd.m_GEID; }
    }
}
