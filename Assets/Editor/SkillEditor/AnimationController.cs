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
            string animation = modelAnimationClips[SkillEditorWindow.Instance.RolePreAction].name;
            if (RoleLoader.Instance.roleObj != null)
                RoleLoader.Instance.roleObj.animation.Play(animation);
            else
                Debug.LogError("roleObj is null");
		}

		void InitSkillBean(SkillBean bean){

		}
	}
}

