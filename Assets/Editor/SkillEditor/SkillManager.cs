using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SkillEditor
{
    public class SkillManager : Singleton<SkillManager>
    {
        public List<System.Object> ActionList = new List<System.Object>();

        //AnimationEvent
        //    AnimationClip
        public override void Initialize()
        {
            base.Initialize();
            RoleLoader.Instance.Initialize();
            AnimationController.Instance.Initialize();
        }

        public override void UnInitialize()
        {
            base.UnInitialize();
            AnimationController.Instance.UnInitialize();
            RoleLoader.Instance.UnInitialize();
        }

        public SKillType GetSkillType()
        {

            SKillType type = SKillType.None;
            if (AnimationController.Instance.modelAnimationClips == null)
                return type;
            if (SkillEditorWindow.Instance.RoleAttackAction != AnimationController.Instance.modelAnimationClips.Length)
            {
                if (SkillEditorWindow.Instance.RolePreAction != AnimationController.Instance.modelAnimationClips.Length &&
                    null == SkillManager.Instance.ActionList.Find(C => (C as MovementActionBean) != null))
                {
                    type = SKillType.DoubleSKill;
                }
                else if (SkillEditorWindow.Instance.RolePreAction == AnimationController.Instance.modelAnimationClips.Length &&
                    null == SkillManager.Instance.ActionList.Find(C => (C as MovementActionBean) != null))
                {
                    type = SKillType.SingleSkill;
                }
                else if (SkillEditorWindow.Instance.RolePreAction == AnimationController.Instance.modelAnimationClips.Length &&
                   null != SkillManager.Instance.ActionList.Find(C => (C as MovementActionBean) != null))
                {
                    type = SKillType.SingleSkillMovement;
                }
                else if (SkillEditorWindow.Instance.RolePreAction != AnimationController.Instance.modelAnimationClips.Length &&
                     null != SkillManager.Instance.ActionList.Find(C => (C as MovementActionBean) != null))
                {
                    type = SKillType.DoubleSkillMovement;
                }
            }
            return type;
        }
        
        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="type"></param>
        public void AddActionEvent(ActionEvent type)
        {
            switch (type)
            {
                case ActionEvent.MovementActionBean:
                    ActionList.Add(SkillEditorWindow.Instance.movementActionBean.Clone());
                    break;
                case ActionEvent.NormalEffectActionBean:
                    break;
                case ActionEvent.AttackActionBean:
                    AttackEventBean bean = new AttackEventBean();
                    bean.startTime = SkillEditorWindow.Instance.eventInvokeTime;
                    bean.delayTime = SkillEditorWindow.Instance.eventInvokeDelayTime;
                    bean.bInvoke = false;
                    ActionList.Add(bean);
                    break;
                case ActionEvent.CustomAnimationEvent:
                    CustomAnimationEvent cae = new CustomAnimationEvent();
                    cae.functionName = "OnAnimationMsg";
                    cae.clipsIndex = GetClipIndex();
                    cae.time = GetStartInvokeTime();
                    Debug.Log("time :" + cae.time);
                    ActionList.Add(cae);
                    break;
            }
            PraseActionEvent();
        }

        public void DeleteActionEvent()
        {
            if (ActionList.Count > 0 && SkillEditorWindow.Instance.roleActionEventIndex != -1)
            {
                ActionList.RemoveAt(SkillEditorWindow.Instance.roleActionEventIndex);
            }
            SkillEditorWindow.Instance.RestMovementActionBean();
            SkillEditorWindow.Instance.roleActionEventIndex = -1;
            PraseActionEvent();
        }

        /// <summary>
        /// Prases the action event.  show in window
        /// </summary>
        void PraseActionEvent()
        {
            SkillEditorWindow.Instance.roleActionEvents = null;
            SkillEditorWindow.Instance.roleActionEvents = new string[ActionList.Count];
            for (int i = 0; i < ActionList.Count; ++i)
            {
                SkillEditorWindow.Instance.roleActionEvents[i] = " 特效，事件类型：" + ActionList[i].GetType().ToString();
            }
        }
        /// <summary>
        /// window 事件选中item
        /// </summary>
        public void PraseWindowSelectActionEvent()
        {
            if (ActionList.Count > SkillEditorWindow.Instance.roleActionEventIndex && SkillEditorWindow.Instance.roleActionEventIndex != -1)
            {
                System.Object actionObj = ActionList[SkillEditorWindow.Instance.roleActionEventIndex];
                if (actionObj is MovementActionBean)
                {
                    SkillEditorWindow.Instance.ActionEventType = ActionEvent.MovementActionBean;

                    MovementActionBean action = actionObj as MovementActionBean;
                    MovementActionBean movementActionBean = SkillEditorWindow.Instance.movementActionBean;
                    movementActionBean.moveTime = action.moveTime;
                    movementActionBean.moveAnimationClip = action.moveAnimationClip;
                    movementActionBean.endTime = action.endTime;
                    movementActionBean.startTime = action.startTime;
                    movementActionBean.isUseAnimationTime = action.isUseAnimationTime;

                }
                if (actionObj is NormalEffectActionBean)
                {
                }
                if (actionObj is AttackEventBean)
                {
                    SkillEditorWindow.Instance.ActionEventType = ActionEvent.AttackActionBean;
                    AttackEventBean action = actionObj as AttackEventBean;
                    SkillEditorWindow.Instance.eventInvokeDelayTime = action.delayTime;
                    SkillEditorWindow.Instance.eventInvokeTime = action.startTime;
                }
            }
        }

        /// <summary>
        /// 计算触发时间
        /// </summary>
        /// <returns>The start invoke time.</returns>
        public float GetStartInvokeTime()
        {
            float value = 0;
            SKillType type = GetSkillType();
            AnimationClip[] modelAnimationClips = AnimationController.Instance.modelAnimationClips;
            SkillEditorWindow window = SkillEditorWindow.Instance;
            switch (type)
            {
                case SKillType.SingleSkill:
                    value = modelAnimationClips[window.RoleAttackAction].length * window.ModelActionSlider;
                    AnimationEvent[] all = AnimationUtility.GetAnimationEvents(modelAnimationClips[window.RoleAttackAction]);
                    foreach (AnimationEvent e in all)
                    {
                        Debug.LogError(e.time);
                    }
                    break;
                case SKillType.SingleSkillMovement: //0-0.2跑动
                    if (SkillEditorWindow.Instance.ModelActionSlider < 0.2)
                    {
                        SkillEditorWindow.Instance.ModelActionSlider = 0.2f;
                        EditorUtility.DisplayDialog("", "跑动过程中无法添加事件", "ok");
                    }
                    else
                    {
                        value = modelAnimationClips[window.RoleAttackAction].length * (window.ModelActionSlider - 0.2f) + window.movementActionBean.moveTime;
                    }
                    break;
                case SKillType.DoubleSKill:
                    if (window.ModelActionSlider < 0.5f)
                    {
                        value = modelAnimationClips[window.RolePreAction].length * window.ModelActionSlider / 0.5f;
                    }
                    else
                    {
                        value = modelAnimationClips[window.RoleAttackAction].length * (window.ModelActionSlider - 0.5f) / 0.5f;
                    }
                    break;
                case SKillType.DoubleSkillMovement:
                    if (window.ModelActionSlider >= 0.4f && window.ModelActionSlider < 0.6f)
                    {
                        EditorUtility.DisplayDialog("", "跑动过程中无法添加事件", "ok");
                    }
                    else if (window.ModelActionSlider < 0.4f)
                    {
                        value = modelAnimationClips[window.RolePreAction].length * window.ModelActionSlider / 0.4f;
                    }
                    else
                    {
                        value = modelAnimationClips[window.RoleAttackAction].length * (window.ModelActionSlider - 0.6f) / 0.4f;
                    }
                    break;
            }

            return value;
        }

        int GetClipIndex()
        {
            int index = 0;
            SKillType type = GetSkillType();
            SkillEditorWindow window = SkillEditorWindow.Instance;
            switch (type)
            {
                case SKillType.SingleSkill:
                    index = 3;
                    break;
                case SKillType.SingleSkillMovement: //0-0.2跑动
                    if (SkillEditorWindow.Instance.ModelActionSlider < 0.2)
                        index = 2;
                    else
                        index = 3;
                    break;
                case SKillType.DoubleSKill:
                    if (window.ModelActionSlider < 0.5f)
                        index = 1;
                    else
                        index = 3;
                    break;
                case SKillType.DoubleSkillMovement:
                    if (window.ModelActionSlider >= 0.4f && window.ModelActionSlider < 0.6f)
                        index = 2;
                    else if (window.ModelActionSlider < 0.4f)
                        index = 1;
                    else
                        index = 3;
                    break;
            }
            return index;
        }

       public static void AttachEvens(SkillBean bean)
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

        static void SetEvent(List<CustomAnimationEvent> events,AnimationClip clip)
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
}

