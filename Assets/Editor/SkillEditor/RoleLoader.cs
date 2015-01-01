using System;
using UnityEngine;
using System.Collections.Generic;

namespace SkillEditor
{
	public class RoleLoader: Singleton<RoleLoader>
	{

        string modelPath = "Character/player_1";
		public GameObject roleObj;
		public List<GameObject> enemyList = new List<GameObject>();
		Vector3[] postionArray = {new Vector3 (2, 0, 6),new Vector3 (0, 0, 6),new Vector3 (-2, 0, 6),
								new Vector3 (2, 0, 8),new Vector3 (0, 0, 8),new Vector3 (-2, 0, 8),
								new Vector3 (2, 0, 10),new Vector3 (0, 0, 10),new Vector3 (-2, 0, 10)};

		public override void UnInitialize ()
		{
			if (roleObj != null) {
				GameObject.Destroy(roleObj);			
			}
			base.UnInitialize ();
		}

		public void Load(string role){
			SkillEditorWindow.Instance.Reset ();
			if (roleObj != null && !role.Equals(roleObj.name)) {
				GameObject.Destroy(roleObj);	
				roleObj = LoadModel(role);
			}else
			{
				roleObj = LoadModel(role);
			}
			roleObj.transform.localPosition = new Vector3(0, 0, -6);
			roleObj.name = role;
			GetModelInfo ();
		}

		//暂时加载一个
		public void LoadEnemy(string role){

			DeleteEnemy ();

			GameObject go = LoadModel (role);
			go.transform.localPosition = postionArray [1];
			go.transform.localRotation = Quaternion.Euler (new Vector3 (0, 180, 0));
			go.name = "Enemy_" + role;
			enemyList.Add (go);
		}

		private GameObject LoadModel(string model){
			string path = string.Format ("{0}/{1}", modelPath, model);
            GameObject prefab = Resources.Load(path) as GameObject;
			GameObject go = null;
            if (prefab != null)
            {
				go = GameObject.Instantiate(prefab) as GameObject;
            }
            else
            {
                Debug.LogError("load error :" + path);
            }
			return go;
		}

		public void Delete(){
			if (roleObj != null) {
				GameObject.Destroy(roleObj);	
			}
			SkillEditorWindow.Instance.Reset ();
		}

		public void DeleteEnemy(){
			for (int i = 0; i< enemyList.Count; ++i) {
				GameObject.Destroy(enemyList[i]);			
			}	
			enemyList.Clear ();
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
            SkillEditorWindow.Instance.roleModelAnimations = roleModelAnimation;
            SkillEditorWindow.Instance.RolePreAction = clips;
            SkillEditorWindow.Instance.RoleAttackAction = clips;
		}
	}
}

