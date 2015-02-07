using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAI.AI
{
    public class BaseActionController
    {
		private ActionCmdManager m_actionCtrl;
		public ActionCmdManager GetActionCtrl() { return m_actionCtrl; }

        public GameEvent m_directionCmd = new GameEvent();
        public GameEvent m_actionCmd = new GameEvent();
		List<GameEvent> m_container = new List<GameEvent>();

        public virtual void Initialize()
        {
			m_actionCtrl = new ActionCmdManager();
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

		public bool BeginPushAction() {
			if ( m_container != null ) {
				//SetActionContainer( m_container );
				m_container.Clear();
				return true;
			}
			return false;
		}

		public bool PostAction( GameEvent act, bool execute = true ) {
			if ( m_container != null ) {
				m_container.Clear();
				m_container.Add( act );
				if ( execute ) {
					//GetActionCtrl().ReloadOnStack();
					//GetActionCtrl().Execute();
				}
				return true;
			}
			return false;
		}

		public bool EndPushAction() {
			if ( m_container != null ) {
				//GetActionCtrl().ReloadOnStack();
				GetActionCtrl().Execute();
				return true;
			}
			return false;
		}

        public GameEventID GetDirectionCmdType() { return m_directionCmd.m_GEID; }
        public GameEventID GetActionCmdType() { return m_actionCmd.m_GEID; }
    }
}
