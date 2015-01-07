using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SkillEditor
{
	public class AnimationController: Singleton<AnimationController>
	{
		public AnimationClip[] modelAnimationClips;
		public SkillBeanPlayer skillbeanPlayer = null;

        public override void Initialize()
        {
            base.Initialize();
            skillbeanPlayer = new SkillBeanPlayer(null,null);
        }

		public override void UnInitialize ()
		{
			base.UnInitialize ();
            if (skillbeanPlayer != null)
                skillbeanPlayer = null;
			if (modelAnimationClips != null) 
				modelAnimationClips = null;		
		}

		public void DisplaySkill(){
            if (RoleLoader.Instance.roleObj == null)
            {
                UnityEditor.EditorUtility.DisplayDialog("添加模型", "添加模型", "O K");
				return;
            }

			SkillBean bean = ScriptableObject.CreateInstance<SkillBean> ();

			if (InitSkillBean (bean)) {
				if(bean.Movement){
					if(RoleLoader.Instance.enemyList.Count <= 0){
						UnityEditor.EditorUtility.DisplayDialog("添加敌人","添加敌人","O K");
						return;
					}
					if(RoleLoader.Instance.enemyList.Count > 1){
						skillbeanPlayer.Init(RoleLoader.Instance.roleObj,bean,RoleLoader.Instance.enemyList[1].transform.position,ActionStatus.Play);
					}else{
						skillbeanPlayer.Init(RoleLoader.Instance.roleObj,bean,RoleLoader.Instance.enemyList[0].transform.position,ActionStatus.Play);
					}
				}else	
					skillbeanPlayer.Init(RoleLoader.Instance.roleObj,bean,ActionStatus.Play);
			}else
			{
				Debug.Log ("无法播放 skill type " + bean.skillType);
			}
		}
		
		bool InitSkillBean(SkillBean bean){
			List<System.Object> actionList = SkillManager.Instance.ActionList;
			int count = actionList.Count;
			for (int i = 0; i < count; ++i) {
				if(actionList[i] is MovementActionBean){
					bean.movementActionBeanList.Add( (actionList[i] as MovementActionBean).Clone() );
				}
				if(actionList[i] is NormalEffectActionBean){
					bean.normalEffectActionBeanList.Add( (actionList[i] as NormalEffectActionBean).Clone() );
				}
				if(actionList[i] is AttackEventBean){
					bean.attackEventBeanList.Add( (actionList[i] as AttackEventBean).Clone() );
				}
                if (actionList[i] is CustomAnimationEvent)
                {
                    CustomAnimationEvent ace = (actionList[i] as CustomAnimationEvent).Clone();
                    bean.customAnimationEventList.Add(ace);
                }
			}

            SetSkillBeanAnimation(bean);
			return true;
		}

		public void SetSkillBeanAnimation (SkillBean bean)
		{
			SKillType type = SkillManager.Instance.GetSkillType ();
			Debug.Log ("skill type " + type);
			bean.skillType = type;
			if (type == SKillType.None || type == SKillType.Count)
				return;
			switch (type) {
				case SKillType.SingleSkill:
                case SKillType.SingleSkillMovement:
					bean.attackAnimation = modelAnimationClips [SkillEditorWindow.Instance.RoleAttackAction];
					break;
				case SKillType.DoubleSKill:
                case SKillType.DoubleSkillMovement:
					bean.attackAnimation = modelAnimationClips [SkillEditorWindow.Instance.RoleAttackAction];
					bean.preAnimation = modelAnimationClips [SkillEditorWindow.Instance.RolePreAction];
					break;
			}

            SkillEditor.SkillManager.AttachEvens(bean);
		}

      

		public void SampleClipBySlider(float value){
			if (RoleLoader.Instance.roleObj == null) {
				Debug.LogError("RoleLoader.Instance.roleObj is null");
				return;			
			}
            RoleLoader.Instance.roleObj.animation.Stop();
			SkillEditorWindow window = SkillEditorWindow.Instance;
			SKillType type = SkillManager.Instance.GetSkillType ();
            Debug.Log(type);
            float clipPoint = 0;
            AnimationClip clip = null;

			switch(type){
				case SKillType.SingleSkill:
                    clip = modelAnimationClips[window.RoleAttackAction];
                    clipPoint = clip.length * value;
					break;
				case SKillType.SingleSkillMovement:
					if (value < 0.4)
					{
                        clip = window.movementActionBean.moveAnimationClip;
                        clipPoint = clip.length * value / 0.4f;
					} 
					else
					{
                        clip = modelAnimationClips[window.RoleAttackAction];
                        clipPoint = clip.length * (value - 0.4f) / 0.6f;
					}
					break;
				case SKillType.DoubleSKill:
                    if (value < 0.5) //准备动作
                    {
                        clip = modelAnimationClips[window.RolePreAction];
                        clipPoint = clip.length * value * 2;
                    } 
                    else
                    {
                        clip = modelAnimationClips[window.RoleAttackAction];
                        clipPoint = clip.length * (value - 0.5f) / 0.5f ;
                    }
					break;
				case SKillType.DoubleSkillMovement:
                    if (value < 0.4) //准备动作
                    {
                        clip = modelAnimationClips[window.RolePreAction];
                        clipPoint = clip.length * value / 0.4f;
                    }
                    else if (value >= 0.4f && value <= 0.6f)
                    {
                        clip = SkillEditorWindow.Instance.movementActionBean.moveAnimationClip;
                        clipPoint = clip.length * (value - 0.4f) / 0.2f;
                    }
                    else
                    {
                        clip = modelAnimationClips[window.RoleAttackAction];
                        clipPoint = clip.length * (value - 0.6f) / 0.6f;
                    }
					break;
			}
            RoleLoader.Instance.roleObj.SampleAnimation(clip, clipPoint);
		}

		public void SamepleMovemtentClip(float value){
			if (RoleLoader.Instance.roleObj == null) {
				Debug.LogError("RoleLoader.Instance.roleObj is null");
				return;			
			}

			float clipPoint = SkillEditorWindow.Instance.movementActionBean.moveAnimationClip.length * value;
			RoleLoader.Instance.roleObj.SampleAnimation(SkillEditorWindow.Instance.movementActionBean.moveAnimationClip, clipPoint);
		}
	}
}

