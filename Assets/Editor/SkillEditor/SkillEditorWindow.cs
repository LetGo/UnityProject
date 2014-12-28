using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;

namespace SkillEditor{
	public class SkillEditorWindow : EditorWindow {
		static SkillEditorWindow Instance = null;

		public int RoleIndex = 0;
		public int RolePreAction = 0;
		public int RoleAttackActiom = 0;
		string[] roleModels = new string[1];
		string[] roleModelAnimation = new string[1];

		Vector2 scrollPos = Vector2.zero;
		float modelActionSlider = 0;
		[MenuItem("GameTool/SkillEditor %E")]
		static void Init(){
			if (EditorApplication.isPlaying && !EditorApplication.isPaused) {
				Instance = EditorWindow.GetWindow<SkillEditorWindow> ("技能编辑器", false, typeof(SkillEditorWindow));	
				GetAllModels();
			}
			else
				Debug.Log("运行场景");
		}

		static void GetAllModels(){
					
		}

		void OnGUI(){
			scrollPos = EditorGUILayout.BeginScrollView (scrollPos);

			RoleIndex = EditorGUILayout.Popup ("角色模型", RoleIndex, roleModels);

			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("添加模型")) {
				//TODO roleLoader.load
			}
			if (GUILayout.Button ("删除模型")) {
				//TODO roleLoader.delete
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			RolePreAction = EditorGUILayout.Popup ("准备动作", RolePreAction, roleModelAnimation);
			RoleAttackActiom = EditorGUILayout.Popup ("攻击动作", RoleAttackActiom, roleModelAnimation);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.LabelField("调整模型动作");
			modelActionSlider = EditorGUILayout.Slider (modelActionSlider, 0, 1);

			EditorGUILayout.EndScrollView ();
		}
	}
}
