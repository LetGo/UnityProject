using UnityEngine;
using System.Collections;

namespace GameState
{
    public class GameSceneManager : Singleton<GameSceneManager>
    {
        GameStageItem m_stage = GameStageItem.None;
        bool m_isTransaction = false;

        public override void Initialize()
        {
            base.Initialize();
        }

        public void Update()
        {

        }
    }
}
