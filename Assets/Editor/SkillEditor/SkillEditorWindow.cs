using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace SkillEditor{
	public class SkillEditorWindow : EditorWindow {

		/// <summary>
		/// The SkillEditorWindow instance.
		/// </summary>
		public static SkillEditorWindow Instance = null;
		/// <summary>
		/// The index of the role.
		/// </summary>
		public int RoleIndex = 0;
		/// <summary>
		/// The role pre action.
		/// </summary>
		public int RolePreAction = 0;
		/// <summary>
		/// The role attack actiom.
		/// </summary>
		public int RoleAttackAction = 0;
		/// <summary>
		/// The all role models.
		/// </summary>
        List<string> roleModels = new List<string>();
		/// <summary>
		/// One role model All animations.
		/// </summary>
		public string[] roleModelAnimations = new string[1];

		ActionEvent actionEvent = ActionEvent.None;
		int roleMovementAction = 0;
		public MovementActionBean movementActionBean = new MovementActionBean ();
		float movementTimeValue = 0;

		Vector2 scrollPos = Vector2.zero;
		float modelActionSlider = 0; //用于采样动作
		float animationPlaySpeed = 0; //播放速度

		[MenuItem("GameTool/SkillEditor %E")]
		static void Init(){
			if (EditorApplication.isPlaying && !EditorApplication.isPaused) {
				Instance = EditorWindow.GetWindow<SkillEditorWindow> ("技能编辑器", false, typeof(SkillEditorWindow));
                Instance.GetAllModels();
                SkillManager.Instance.Initialize();
				if (Camera.main == null) {
					GameObject go = new GameObject();
					Camera c = go.AddComponent<Camera>();
					c.tag = "MainCamera";
				}
				Camera.main.transform.rotation = Quaternion.Euler (new Vector3 (35, 320, 0));
				Camera.main.transform.position = new Vector3 (5,5,-10);
			}
			else
				Debug.Log("运行场景");
		}

	     void GetAllModels(){
            DirectoryInfo dic = new DirectoryInfo(Application.dataPath + "/Resources/Character/player_1");
            foreach (FileInfo file in dic.GetFiles())
            {
                if (file.Name.EndsWith(".prefab"))
                {
                    roleModels.Add( file.Name.Substring(0,file.Name.IndexOf('.') ) );
                }
            }
		}

		public void Reset(){
			RolePreAction = 0;	
			RoleAttackAction = 0;
			roleModelAnimations = new string[1];
		}

		void OnGUI(){
			scrollPos = EditorGUILayout.BeginScrollView (scrollPos);

			RoleIndex = EditorGUILayout.Popup ("角色模型", RoleIndex, roleModels.ToArray());

			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("添加模型")) {
				RoleLoader.Instance.Load(roleModels[RoleIndex]);
			}
			if (GUILayout.Button ("删除模型")) {
				RoleLoader.Instance.Delete();
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			RolePreAction = EditorGUILayout.Popup ("准备动作", RolePreAction, roleModelAnimations);
			RoleAttackAction = EditorGUILayout.Popup ("攻击动作", RoleAttackAction, roleModelAnimations);
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.LabelField("调整模型动作");
			modelActionSlider = EditorGUILayout.Slider (modelActionSlider, 0, 1);

			actionEvent = (ActionEvent)EditorGUILayout.EnumPopup ("插入动作事件类型", actionEvent);
			switch (actionEvent) {
			case ActionEvent.MovementActionBean:

				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();
				for(int i = 0;i < roleModelAnimations.Length; ++i){
					if(roleModelAnimations[i] != null && roleModelAnimations[i].Contains("Move")){
						roleMovementAction = i;
						break;
					}
				}
				roleMovementAction = EditorGUILayout.Popup ("移动动作", roleMovementAction, roleModelAnimations);
				movementActionBean.moveAnimationClip = AnimationController.Instance.modelAnimationClips[ roleMovementAction ];

				movementActionBean.isUseAnimationTime = EditorGUILayout.Toggle("使用动画时间",movementActionBean.isUseAnimationTime);
				if(movementActionBean.isUseAnimationTime){
					movementActionBean.moveTime = roleModelAnimations[roleMovementAction].Length;
				}else{
					movementActionBean.moveTime = EditorGUILayout.FloatField("移动时间",roleModelAnimations[roleMovementAction].Length);
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				movementTimeValue = EditorGUILayout.Slider(movementTimeValue,0,1);

				EditorGUILayout.EndHorizontal();

				break;
			case ActionEvent.NormalEffectActionBean:
				break;
			}
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("添加动作事件")) {
				//TEST
                //ScriptableObjectTest sot = ScriptableObject.CreateInstance<ScriptableObjectTest>();
                //sot.content = "test sstring";
                //sot.id = 110;
                //AssetDatabase.CreateAsset(sot,"Assets/Test.asset");
                //AssetDatabase.Refresh();
				SkillManager.Instance.AddActionEvent(actionEvent);
			}
			if (GUILayout.Button ("删除事件")) {

			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.EndScrollView ();

			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("添加受击对象")) {
				RoleLoader.Instance.LoadEnemy(roleModels[RoleIndex]);
			}
			if (GUILayout.Button ("删除受击对象")) {
				RoleLoader.Instance.DeleteEnemy();
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.Space ();
			EditorGUILayout.LabelField("播放速度");
			animationPlaySpeed = EditorGUILayout.Slider (animationPlaySpeed,0,1);
			if (GUILayout.Button ("预览效果")) {
                AnimationController.Instance.DisplaySkill();
			}

			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("保存技能")) {

			}
			if (GUILayout.Button ("读取技能")) {
				
			}
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.Space ();
			EditorGUILayout.EndHorizontal ();
		}

		float currModelActionSlider = 0;
		void Update(){
			if (currModelActionSlider != modelActionSlider) {
				currModelActionSlider = modelActionSlider;
				AnimationController.Instance.SampleClipBySlider(currModelActionSlider);
			}

			AnimationController.Instance.actionPlayer.Update (Time.realtimeSinceStartup);

			if (!EditorApplication.isPlaying) {
				Instance.Close();			
			}
		}

		void Destroy(){
			SkillManager.Instance.UnInitialize ();
		}
	}
}
