using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace SkillEditor{

	public static class SkillFile{

		public static bool Save(){
			if (RoleLoader.Instance.roleObj != null) {
				SkillBean bean = InitSkillBean();
				AnimationController.Instance.SetSkillBeanAnimation(bean);
				string path = EditorUtility.SaveFilePanelInProject("Save","deafult","asset","保存");
				if(!string.IsNullOrEmpty(path)){
					AssetDatabase.CreateAsset(bean,path);
                    AssetDatabase.Refresh();
				}
			}else
			{
				return false;
			}
			return true;
		}

		static SkillBean InitSkillBean(){
			SkillBean bean = ScriptableObject.CreateInstance<SkillBean> ();
			List<System.Object> actionList = SkillManager.Instance.ActionList;
			int count = actionList.Count;
			for (int i = 0; i< count; ++i) {
				if(actionList[i] is MovementActionBean){
					bean.movementActionBeanList.Add(actionList[i] as MovementActionBean);
				}else if(actionList[i] is NormalEffectActionBean){
					bean.normalEffectActionBeanList.Add(actionList[i] as NormalEffectActionBean);
				}
				else if(actionList[i] is AttackEventBean){
					bean.attackEventBeanList.Add(actionList[i] as AttackEventBean);
				}
                else if (actionList[i] is CustomAnimationEvent)
                {
                    bean.customAnimationEventList.Add(actionList[i] as CustomAnimationEvent);
                }
			}
			return bean;
		}

		public static void Load(){
			string path = EditorUtility.OpenFilePanel("Open","Assets/Resources/Skills","asset");
			if(!string.IsNullOrEmpty(path)){
				path = path.Substring(path.IndexOf("Assets"));
				SkillBean source = AssetDatabase.LoadAssetAtPath(path,typeof(SkillBean)) as SkillBean;
				SkillBean bean = ScriptableObject.CreateInstance<SkillBean>();
				EditorUtility.CopySerialized(source,bean);
				SkillEditorWindow.Instance.UpdateLoadSkillBean(bean);
			}
		}
	}
}

