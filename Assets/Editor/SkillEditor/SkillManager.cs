using System;
using System.Collections;
using System.Collections.Generic;

namespace SkillEditor
{
	public class SkillManager : Singleton<SkillManager>
	{
		public List<System.Object> ActionList = new List<System.Object>();

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
			if (SkillEditorWindow.Instance.RoleAttackAction != AnimationController.Instance.modelAnimationClips.Length) {
				if(SkillEditorWindow.Instance.RolePreAction != AnimationController.Instance.modelAnimationClips.Length){
					type = SKillType.DoubleSKill;
				}		
				else if(SkillEditorWindow.Instance.RolePreAction == AnimationController.Instance.modelAnimationClips.Length){
					type = SKillType.SingleSkill;
				}
//				else if(SkillEditorWindow.Instance.RolePreAction != AnimationController.Instance.modelAnimationClips.Length){
//					type = SKillType.SingleSkillMovement;
//				}
//				else if(SkillEditorWindow.Instance.RolePreAction != AnimationController.Instance.modelAnimationClips.Length){
//					type = SKillType.SingleSkillMovement;
//				}
			}
			return type;
		}

		public void AddActionEvent(ActionEvent type ){
			switch (type) {
				case ActionEvent.MovementActionBean:
					ActionList.Add(SkillEditorWindow.Instance.movementActionBean);
					break;
				case ActionEvent.NormalEffectActionBean:
					break;
			}
		}

        public void DeleteActionEvent(List<bool> selects)
        {
            for (int i = selects.Count - 1; i >= 0; --i )
            {
                if (selects[i])
                {
                    ActionList.RemoveAt(i);
                }
            }
        }
	}
}

