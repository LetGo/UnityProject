using UnityEngine;
using System.Collections;
public delegate void OnAnimationMsgDelegate();
public delegate void OnSkillPlayEndDelegate();

public class EntityComponent : MonoBehaviour {

    private OnAnimationMsgDelegate m_onAnimationMsgDelegate;
    public OnAnimationMsgDelegate OnAnimationMsgCallBack
    {
        get { return m_onAnimationMsgDelegate; }
        set { m_onAnimationMsgDelegate += value; }
    }

    private OnSkillPlayEndDelegate m_oSkillPlayEndDelegate;
    public OnSkillPlayEndDelegate OnSkillPlayEndCallBack
    {
        get { return m_oSkillPlayEndDelegate; }
        set { m_oSkillPlayEndDelegate += value; }
    }

    /// <summary>
    /// 一次技能播放完成回调
    /// </summary>
	public void OnSkillPlayEnd(){
        if (m_oSkillPlayEndDelegate != null)
        {
            m_oSkillPlayEndDelegate();
        }
	}

    public void OnAnimationMsg()
    {
        if (m_onAnimationMsgDelegate != null)
        {
            m_onAnimationMsgDelegate();
        }
    }
}
