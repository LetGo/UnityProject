using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace SkillEditor
{
    public class SkillBean : ScriptableObject
    {
        public uint enityId;
        public SKillType skillType;
        public AnimationClip preAnimation;
        public AnimationClip attackAnimation;
        public bool Movement { get { return movementActionBeanList.Count > 0; } }//是否冲锋
        public List<MovementActionBean> movementActionBeanList = new List<MovementActionBean>();
        public List<NormalEffectActionBean> normalEffectActionBeanList = new List<NormalEffectActionBean>();
		public List<AttackEventBean> attackEventBeanList = new List<AttackEventBean>();
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
    }
}