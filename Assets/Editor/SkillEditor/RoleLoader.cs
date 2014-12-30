using System;
using UnityEngine;

namespace SkillEditor
{
	public class RoleLoader: Singleton<RoleLoader>
	{
        string modelPath = "Character/player_1";
		public GameObject roleObj;

		public override void UnInitialize ()
		{
			base.UnInitialize ();
			if (roleObj != null) {
				GameObject.Destroy(roleObj);			
			}
		}

		public void Load(string role){
			if (roleObj != null && !role.Equals(roleObj.name)) {
				GameObject.Destroy(roleObj);	
				LoadModel(role);
			}else
			{
				LoadModel(role);
			}
			GetModelInfo ();
		}

		private void LoadModel(string model){
			SkillEditorWindow.Instance.Reset ();
			string path = string.Format ("{0}/{1}", modelPath, model);
        
            GameObject prefab = Resources.Load(path) as GameObject;
            if (prefab != null)
            {
                roleObj = GameObject.Instantiate(prefab) as GameObject;
                roleObj.transform.localPosition = new Vector3(0, 0, -6);
            }
            else
            {
                Debug.LogError("load error :" + path);
            }

		}

		public void Delete(string role){
			if (roleObj != null) {
				GameObject.Destroy(roleObj);	
			}
			SkillEditorWindow.Instance.Reset ();
		}

		private void GetModelInfo(){
			if(roleObj == null) return;
			if (roleObj.animation == null) {
				Debug.Log("animation is null");
				return;
			}
			AnimationClip[] modelAnimationClips = UnityEditor.AnimationUtility.GetAnimationClips (roleObj);
            AnimationController.Instance.modelAnimationClips = modelAnimationClips;

			int clips = modelAnimationClips.Length;
            string[] roleModelAnimation = new string[clips + 1];
            
			for (int i = 0; i < clips; ++i) {
				roleModelAnimation[i] = modelAnimationClips[i].name;
			}
            roleModelAnimation[clips] = "None";
            SkillEditorWindow.Instance.roleModelAnimation = roleModelAnimation;
            SkillEditorWindow.Instance.RolePreAction = clips;
            SkillEditorWindow.Instance.RoleAttackActiom = clips;
		}
	}
}

