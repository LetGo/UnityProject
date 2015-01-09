using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GameSceneBase
{
    public virtual bool OnProcessTransition() { return true; }
    public virtual void OnEnterScene() { UIManager.Instance.OpenUIpanel(UIPanelID.ePanel_Loading); }
    public virtual void OnExitScene() { }
    public virtual void OnBeginRender() { }
    public virtual void OnEndRender() { }
    public void _Exit_Internal()
    {

    }

    public void _Update_Internal(float deltaTime)
    {

    }

    public void _Render_Internal()
    {
    }
}
