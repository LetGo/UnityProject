﻿using UnityEngine;
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
    public class MovementActionBean
    {
        public bool isUseAnimationTime;
        public float moveTime;
        public AnimationClip moveAnimationClip;
        public float startTime;
        public float endTime;

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
    }
}