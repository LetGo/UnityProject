using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    class WorldState : GameSceneBase
    {
        public override void OnEnterScene()
        {
            base.OnEnterScene();
            GameManager.Instance.ClientProxy.StartCoroutine(ClosePanel());
        }

        IEnumerator ClosePanel()
        {
            yield return new WaitForSeconds(3);
            UIManager.Instance.OpenUIpanel(UIPanelID.ePanel_FightDemo);
            UIManager.Instance.CloseUIPanel(UIPanelID.ePanel_Loading);
        }

        public override void OnExitScene()
        {
            base.OnExitScene();

        }
    }
}