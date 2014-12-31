using System;

namespace SkillEditor
{
	public class SkillManager : Singleton<SkillManager>
	{
		public override void Initialize ()
		{
			base.Initialize ();
			RoleLoader.Instance.Initialize ();
			AnimationController.Instance.Initialize ();
		}

		public override void UnInitialize ()
		{
			base.UnInitialize ();
			AnimationController.Instance.UnInitialize();
			RoleLoader.Instance.UnInitialize ();
		}

		public SKillType GetSkillType(){

			SKillType type = SKillType.None;
			if (SkillEditorWindow.Instance.RoleAttackActiom != AnimationController.Instance.modelAnimationClips.Length) {
				if(SkillEditorWindow.Instance.RolePreAction != AnimationController.Instance.modelAnimationClips.Length){
					type = SKillType.SingleSkillMovement;
				}		
				else if(SkillEditorWindow.Instance.RolePreAction == AnimationController.Instance.modelAnimationClips.Length){
					type = SKillType.SingleSkillMovement;
				}
				if(SkillEditorWindow.Instance.RolePreAction != AnimationController.Instance.modelAnimationClips.Length){
					type = SKillType.SingleSkillMovement;
				}
				if(SkillEditorWindow.Instance.RolePreAction != AnimationController.Instance.modelAnimationClips.Length){
					type = SKillType.SingleSkillMovement;
				}
			}
			return type;
		}
	}
}

