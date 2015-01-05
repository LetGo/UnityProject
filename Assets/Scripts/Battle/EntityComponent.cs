using UnityEngine;
using System.Collections;

public class EntityComponent : MonoBehaviour {
    public delegate void OnAnimationMsgDelegate();

    private OnAnimationMsgDelegate m_onAnimationMsgDelegate;
    public OnAnimationMsgDelegate OnAnimationMsgCallBack
    {
        get { return m_onAnimationMsgDelegate; }
        set { m_onAnimationMsgDelegate += value; }
    }

	public void SetIdle(){
        //if (battleEntity != null) {
        //    battleEntity.ChangeAnimStatus(EntityAnimStatus.Idel);		
        //}
	}

    public void Hurt(int a)
    {

    }

    public void OnAnimationMsg()
    {

        Debug.Log("OnAnimationMsg :" + animation["run"].time + " ::: " + animation["run"].length);
        if (m_onAnimationMsgDelegate != null)
        {
            m_onAnimationMsgDelegate();
        }
    }
}
