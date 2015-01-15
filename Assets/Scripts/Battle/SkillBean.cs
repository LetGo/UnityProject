using UnityEngine;
using System.Collections;
using UniCommon;
using System.Collections.Generic;

public class SkillBean : ScriptableObject
{
    public uint enityId;
	public string model;
    public SKillType skillType;
    public AnimationClip preAnimation;
    public AnimationClip attackAnimation;
    public bool Movement { get { return movementActionBeanList.Count > 0; } }//是否冲锋
    public List<MovementActionBean> movementActionBeanList = new List<MovementActionBean>();
    public List<NormalEffectActionBean> normalEffectActionBeanList = new List<NormalEffectActionBean>();
	public List<AttackEventBean> attackEventBeanList = new List<AttackEventBean>();
    public List<CustomAnimationEvent> customAnimationEventList = new List<CustomAnimationEvent>();
    public List<CameraShanekActionBean> CameraShanekActionBeanList = new List<CameraShanekActionBean>();

	public SkillBean Clone(){
		SkillBean bean = ScriptableObject.CreateInstance<SkillBean> ();
		bean.enityId = enityId;
		bean.skillType = skillType;
		bean.preAnimation = preAnimation;
		bean.attackAnimation = attackAnimation;
		this.movementActionBeanList.ApplyAll (c => bean.movementActionBeanList.Add (c.Clone ()));
		this.normalEffectActionBeanList.ApplyAll (c => bean.normalEffectActionBeanList.Add (c.Clone ()));
		this.attackEventBeanList.ApplyAll (c => bean.attackEventBeanList.Add (c.Clone ()));
        this.customAnimationEventList.ApplyAll(c => bean.customAnimationEventList.Add(c.Clone()));
        this.CameraShanekActionBeanList.ApplyAll(c => bean.CameraShanekActionBeanList.Add(c.Clone()));
        AttachEvens(bean);
		return bean;
	}

    void AttachEvens(SkillBean bean)
    {
        List<CustomAnimationEvent> preAnimEvent = new List<CustomAnimationEvent>();
        List<CustomAnimationEvent> attackAnimEvent = new List<CustomAnimationEvent>();
        List<CustomAnimationEvent> runAnimEvent = new List<CustomAnimationEvent>();
        foreach (CustomAnimationEvent e in bean.customAnimationEventList)
        {
            if (e.clipsIndex == 1)
            {
                preAnimEvent.Add(e);
            }
            else if (e.clipsIndex == 2)
            {
                runAnimEvent.Add(e);
            }
            else if (e.clipsIndex == 3)
            {
                attackAnimEvent.Add(e);
            }
        }

        SetEvent(attackAnimEvent, bean.attackAnimation);
        SetEvent(preAnimEvent, bean.preAnimation);
        if (bean.movementActionBeanList.Count > 0)
            SetEvent(runAnimEvent, bean.movementActionBeanList[0].moveAnimationClip);
    }

    void SetEvent(List<CustomAnimationEvent> events, AnimationClip clip)
    {
        if (clip == null) return;

        if (events.Count > 0)
        {
            UnityEditor.AnimationUtility.SetAnimationEvents(clip, null);
            for (int i = 0; i < events.Count; ++i)
            {
                CustomAnimationEvent cae = events[i];
                AnimationEvent ae = new AnimationEvent();
                ae.time = cae.time;
                ae.functionName = "OnAnimationMsg";
                clip.AddEvent(ae);
            }
        }
    }
}

[System.Serializable]
public enum SKillType
{
    None = -1,
    SingleSkill = 0, //单技能不冲锋
    SingleSkillMovement = 1,
    DoubleSKill = 2,
    DoubleSkillMovement = 3,
    Count,
}

[System.Serializable]
public class AttackEventBean//触发受击事件
{
	public float startTime; //触发开始时间
	public float delayTime; //延迟时间 
	public bool bInvoke;
	public AttackEventBean Clone()
	{
		return MemberwiseClone() as AttackEventBean;
	}
}

[System.Serializable]
public class CustomAnimationEvent//触发受击事件
{
    public float time;
    public int clipsIndex; // 1-准备 2-跑动 3-攻击
    public string functionName;
    public float floatParameter;
    public CustomAnimationEvent Clone()
    {
        return MemberwiseClone() as CustomAnimationEvent;
    }
}

[System.Serializable]
public class MovementActionBean
{
    public bool isUseAnimationTime;
    public float moveTime; 						//总的移动时间
    public AnimationClip moveAnimationClip;
    public float startTime;						//总的移动时间的百分比 0-1 作为开始时间
	public float endTime;						//总的移动时间的百分比 0-1 作为结束时间

    public MovementActionBean Clone()
    {
        return MemberwiseClone() as MovementActionBean;
    }
}

[System.Serializable]
public class NormalEffectActionBean
{
    public float sliderValue; //0-1
    public bool isInvokeEvent;
    public GameObject effectObj;
    public bool follow;
    public BodyPointEnum bodyPoint; //挂点
    public bool isInvekeByUnderFireEvent;
    public float InvekeByUnderFireEventDelay; //出发延迟时间
    public NormalEffectActionBean Clone()
    {
        return MemberwiseClone() as NormalEffectActionBean;
    }
}

[System.Serializable]
public class CameraShanekActionBean
{
    public int Level;
    public CameraShanekActionBean Clone()
    {
        return MemberwiseClone() as CameraShanekActionBean;
    }
}

[System.Serializable]
public enum BodyPointEnum
{
    Body = 0,
    Chest,
    LeftHand,
    RightHand,
    Bottom,
    Head,
    Center,
    WeaponR,
    WeaponL,
}

[System.Serializable]
public enum ActionEvent
{
    None,
    NormalEffectActionBean,
    MovementActionBean,
	AttackActionBean,
    CustomAnimationEvent,
}
