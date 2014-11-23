using UnityEngine;
using System.Collections;

namespace GameState
{
    public class GameSceneManager : Singleton<GameSceneManager>
    {
        GameStageItem m_stage = GameStageItem.None;
        bool m_isTransaction = false;
        GameSceneBase m_currentTickScene = null;
        GameSceneBase m_destScene = null;

        public override void Initialize()
        {
            base.Initialize();
        }

        public void Update(float deltaTime)
        {
            switch (m_stage)
            {
                case GameStageItem.Idle:
                    break;
                case GameStageItem.LaunchState:
                    if ( m_currentTickScene != null)
                    {
                        m_currentTickScene._Exit_Internal();
                    }
                    m_currentTickScene = m_destScene;
                    m_destScene = null;
                    m_currentTickScene.OnEnterScene();
                    m_stage = GameStageItem.TransitionToNew;
                    break;
                case GameStageItem.TransitionToNew:
                    if (m_currentTickScene.OnProcessTransition())
                    {
                        m_stage = GameStageItem.RunningState;
                    }
                    break;
                case GameStageItem.RunningState:
                    m_isTransaction = false;
                    m_currentTickScene._Update_Internal( deltaTime );
                    break;
            }
        }

        public bool LaunchGameScene(GameSceneBase scene)
        {
            System.Diagnostics.Debug.Assert(scene != null);
            if (m_isTransaction)
            {
                return false;
            }
            m_stage = GameStageItem.LaunchState;
            m_destScene = scene;
            m_isTransaction = true;
            return true;
        }

        public virtual void Destroy()
        {
            if (m_currentTickScene != null)
            {
                m_currentTickScene._Exit_Internal();
            }
        }

        public virtual void Render()
        {
            if (m_currentTickScene != null && !m_isTransaction)
            {
                m_currentTickScene._Render_Internal();
            }
        }
    }
}
