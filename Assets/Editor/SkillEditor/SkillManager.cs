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
	}
}

