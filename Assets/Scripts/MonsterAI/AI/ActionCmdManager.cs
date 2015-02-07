using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAI.AI
{
    public class ActionCmdManager
    {
		bool m_running = false;
		bool m_locked = false;
		GameEvent m_directionCmd = new GameEvent();
		GameEvent m_actionCmd = new GameEvent();

		List<GameEvent> m_dataContainer = new List<GameEvent>();
		LinkedList< ActionCmdInstance > m_cmdStack = new LinkedList<ActionCmdInstance>();
		public bool ClearStack() {
			if ( m_locked ) {
				return false;
			}
			m_cmdStack.Clear();
			m_running = false;
			return true;
		}
		public void ReloadOnStack( int start = 0, int num = 0x7fffffff ) {
			m_cmdStack.Clear();
			if ( m_dataContainer != null ) {
				if ( m_locked ) {
					return;
				}
				int size = m_dataContainer.Count;
				int begin = start + num - 1;
				begin = Mathf.Clamp( begin, 0, size - 1 );
				int end = start;

				for ( int i = begin; i >= start; --i ) {

					var inst = new ActionCmdInstance( i );
					m_cmdStack.AddLast( inst );

				}
			}
		}

		public bool Update( float deltaTime ) {
			// clear output
			m_directionCmd.Reset();
			m_actionCmd.Reset();
			if ( !m_running ) {
				return false;
			}

			return m_running;
		}

		public GameEvent GetDirectionCmd() { return m_directionCmd; }
		public GameEvent GetActionCmd() { return m_actionCmd; }
		public void Execute() { m_running = true; }
		public bool IsRunning() { return m_running; }
		public void Stop() { m_running = false; }
    }
}
