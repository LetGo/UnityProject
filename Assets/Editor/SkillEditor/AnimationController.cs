using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SkillEditor
{
	public class AnimationController: Singleton<AnimationController>
	{
		public AnimationClip[] modelAnimationClips;
		public PraseSkillBean actionPlayer = null;

        public override void Initialize()
        {
            base.Initialize();
            actionPlayer = new PraseSkillBean();
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

			SKillType type = SkillManager.Instance.GetSkillType ();
			Debug.Log ("skill type " + type);
			bean.skillType = type;
			if (type == SKillType.None || type == SKillType.Count)
				return false;
			switch(type){
				case SKillType.SingleSkill:
					bean.attackAnimation = modelAnimationClips[SkillEditorWindow.Instance.RoleAttackAction];
					break;
				case SKillType.SingleSkillMovement:

					break;
				case SKillType.DoubleSKill:
					bean.attackAnimation = modelAnimationClips[SkillEditorWindow.Instance.RoleAttackAction];
					bean.preAnimation = modelAnimationClips[SkillEditorWindow.Instance.RolePreAction];
					break;
				case SKillType.DoubleSkillMovement:
					
					break;
			}

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
			}


			return true;
		}

		public void SampleClipBySlider(float value){
			if (RoleLoader.Instance.roleObj == null) {
				Debug.LogError("RoleLoader.Instance.roleObj is null");
				return;			
			}
			SkillEditorWindow window = SkillEditorWindow.Instance;
			SKillType type = SkillManager.Instance.GetSkillType ();
			switch(type){
				case SKillType.SingleSkill:
				float clipPoint = modelAnimationClips[window.RoleAttackAction].length * value;
				RoleLoader.Instance.roleObj.SampleAnimation(modelAnimationClips[window.RoleAttackAction],clipPoint);
					break;
				case SKillType.SingleSkillMovement:
					
					break;
				case SKillType.DoubleSKill:
					break;
				case SKillType.DoubleSkillMovement:
					
					break;
			}
		}
	}
}

