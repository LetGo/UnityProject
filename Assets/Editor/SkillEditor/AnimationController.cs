using System;
using UnityEngine;

namespace SkillEditor
{
	public class AnimationController: Singleton<AnimationController>
	{
		public AnimationClip[] modelAnimationClips;
		private PraseSkillBean praseSkillBean = null;

        public override void Initialize()
        {
            base.Initialize();
            praseSkillBean = new PraseSkillBean();
        }

		public override void UnInitialize ()
		{
			base.UnInitialize ();
            if (praseSkillBean != null)
                praseSkillBean = null;
			if (modelAnimationClips != null) 
				modelAnimationClips = null;		
		}

		public void DisplaySkill(){
//            string animation = modelAnimationClips[SkillEditorWindow.Instance.RolePreAction].name;
//            if (RoleLoader.Instance.roleObj != null)
//                RoleLoader.Instance.roleObj.animation.Play(animation);
//            else
//                Debug.LogError("roleObj is null");
			SkillBean bean = ScriptableObject.CreateInstance<SkillBean> ();

			if (InitSkillBean (bean)) {
				praseSkillBean.Play(RoleLoader.Instance.roleObj,bean);
			}
		}

		bool InitSkillBean(SkillBean bean){

			SKillType type = SkillManager.Instance.GetSkillType ();
			bean.skillType = type;
			switch(type){
				case SKillType.SingleSkill:
					bean.attackAnimation = modelAnimationClips[SkillEditorWindow.Instance.RoleAttackActiom];
					break;
				case SKillType.SingleSkillMovement:

					break;
				case SKillType.DoubleSKill:
					bean.attackAnimation = modelAnimationClips[SkillEditorWindow.Instance.RoleAttackActiom];
					bean.preAnimation = modelAnimationClips[SkillEditorWindow.Instance.RolePreAction];
					break;
				case SKillType.DoubleSkillMovement:
					
					break;
			}

			//TODO 事件添加
			return true;
		}
	}
}

