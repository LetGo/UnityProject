using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SkillEditor
{
	public class AnimationController: Singleton<AnimationController>
	{
		public AnimationClip[] modelAnimationClips;
		public SkillBeanPlayer actionPlayer = null;

        public override void Initialize()
        {
            base.Initialize();
            actionPlayer = new SkillBeanPlayer();
        }

		public override void UnInitialize ()
		{
			base.UnInitialize ();
            if (actionPlayer != null)
                actionPlayer = null;
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
						actionPlayer.Init(RoleLoader.Instance.roleObj,bean,RoleLoader.Instance.enemyList[1].transform.position,ActionStatus.Play);
					}else{
						actionPlayer.Init(RoleLoader.Instance.roleObj,bean,RoleLoader.Instance.enemyList[0].transform.position,ActionStatus.Play);
					}
				}else	
					actionPlayer.Init(RoleLoader.Instance.roleObj,bean,ActionStatus.Play);
			}else
			{
				Debug.Log ("无法播放 skill type " + bean.skillType);
			}
		}
		
		bool InitSkillBean(SkillBean bean){

			SetSkillBeanAnimation (bean);

			//TODO 事件添加
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
			}


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
					bean.attackAnimation = modelAnimationClips [SkillEditorWindow.Instance.RoleAttackAction];
					break;
				case SKillType.SingleSkillMovement:
					break;
				case SKillType.DoubleSKill:
					bean.attackAnimation = modelAnimationClips [SkillEditorWindow.Instance.RoleAttackAction];
					bean.preAnimation = modelAnimationClips [SkillEditorWindow.Instance.RolePreAction];
					break;
				case SKillType.DoubleSkillMovement:
					break;
			}
		}

		public void SampleClipBySlider(float value){
			if (RoleLoader.Instance.roleObj == null) {
				Debug.LogError("RoleLoader.Instance.roleObj is null");
				return;			
			}
			SkillEditorWindow window = SkillEditorWindow.Instance;
			SKillType type = SkillManager.Instance.GetSkillType ();
            float clipPoint = 0;
            AnimationClip clip = null;

			switch(type){
				case SKillType.SingleSkill:
                    clip = modelAnimationClips[window.RoleAttackAction];
                    clipPoint = clip.length * value;
					break;
				case SKillType.SingleSkillMovement:
					
					break;
				case SKillType.DoubleSKill:
                    if (value < 0.5) //准备动作
                    {
                        clip = modelAnimationClips[window.RolePreAction];
                        clipPoint = clip.length * value * 2;
                    } 
                    else
                    {
                        //0.5 - 0
                        //0.55--0.1
                        //0.6--0.2
                        //0.65--0.3
                        //0.7--0.4
                        //0.75--0.5
                        //1- 1
                        clip = modelAnimationClips[window.RoleAttackAction];
                        clipPoint = clip.length * (value - 0.5f) / 0.05f * 0.1f;
                    }
					break;
				case SKillType.DoubleSkillMovement:
					
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

