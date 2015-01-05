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
		public string[] roleModelAnimations = new string[0];
		public int roleActionEventIndex = -1; //事件列表索引
		int currRoleActionEventIndex = -1; //当前事件列表索引
		public string[] roleActionEvents = new string[0];

		public ActionEvent ActionEventType = ActionEvent.None;//插入事件类型
		//move
		int roleMovementAction = 0;
		public MovementActionBean movementActionBean = new MovementActionBean ();
		float movementTimeValue = 0;
		float currentMovementTimeValue = 0;
		//AttackEvent
		public float eventInvokeDelayTime = 0f;
		public float eventInvokeTime = 0f;

		Vector2 scrollPos = Vector2.zero;
        Vector2 scrollPos2 = Vector2.zero;
		public float ModelActionSlider = 0; //用于采样动作
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

		void Start(){
			Reset();		
		}

		public void Reset(){
			RolePreAction = 0;	
			RoleAttackAction = 0;
			roleModelAnimations = new string[1];
            roleMovementAction = 0;
            ActionEventType = ActionEvent.None;
			RestMovementActionBean ();
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
			ModelActionSlider = EditorGUILayout.Slider (ModelActionSlider, 0, 1);

			ActionEventType = (ActionEvent)EditorGUILayout.EnumPopup ("插入动作事件类型", ActionEventType);
			switch (ActionEventType) {
    			case ActionEvent.MovementActionBean:
                    OnMovementActionBean();
			    	break;
	    		case ActionEvent.NormalEffectActionBean:
				break;
			case ActionEvent.AttackActionBean:
				EditorGUILayout.BeginHorizontal ();
				eventInvokeTime = EditorGUILayout.FloatField("触发开始时间:",SkillManager.Instance.GetStartInvokeTime());
				eventInvokeDelayTime = EditorGUILayout.FloatField("触发延迟时间",eventInvokeDelayTime);
				EditorGUILayout.EndHorizontal ();
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
				SkillManager.Instance.AddActionEvent(ActionEventType);
			}
			if (GUILayout.Button ("删除事件")) {
               SkillManager.Instance.DeleteActionEvent();
			}
			EditorGUILayout.EndHorizontal ();

			//事件列表
            scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2);
			roleActionEventIndex = GUILayout.SelectionGrid (roleActionEventIndex, roleActionEvents, 1);
			if (currRoleActionEventIndex != roleActionEventIndex) {
				currRoleActionEventIndex = roleActionEventIndex;
				SkillManager.Instance.PraseWindowSelectActionEvent();			
			}
			EditorGUILayout.EndScrollView();

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
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("恢复位置"))
            {
                RoleLoader.Instance.ResetPos();
            }
			if (GUILayout.Button ("预览效果")) {
                AnimationController.Instance.DisplaySkill();
			}
            EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("保存技能")) {
				SkillFile.Save();
			}
			if (GUILayout.Button ("读取技能")) {
				SkillFile.Load();
			}
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.Space ();
			EditorGUILayout.EndHorizontal ();
            EditorGUILayout.EndScrollView();
		}
		
        private void OnMovementActionBean()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < roleModelAnimations.Length; ++i)
            {
                if (roleModelAnimations[i] != null && roleModelAnimations[i].Contains("Move"))
                {
                    roleMovementAction = i;
                    break;
                }
            }
            roleMovementAction = EditorGUILayout.Popup("移动动作", roleMovementAction, roleModelAnimations);
            movementActionBean.moveAnimationClip = AnimationController.Instance.modelAnimationClips[roleMovementAction];

            movementActionBean.isUseAnimationTime = EditorGUILayout.Toggle("使用动画时间(" +
                                            AnimationController.Instance.modelAnimationClips[roleMovementAction].length + ")",
                                            movementActionBean.isUseAnimationTime);
            if (movementActionBean.isUseAnimationTime)
            {
                movementActionBean.moveTime = AnimationController.Instance.modelAnimationClips[roleMovementAction].length;
            }
            else
            {
                movementActionBean.moveTime = EditorGUILayout.FloatField("移动时间",
                    movementActionBean.moveTime);
                if (movementActionBean.moveTime < 0)
                {
                    movementActionBean.moveTime = 0;
                }
				if(roleActionEventIndex != -1){
					if(GUILayout.Button("设置")){
						MovementActionBean moveAction = SkillManager.Instance.ActionList[roleActionEventIndex] as MovementActionBean;
						if(moveAction != null){
							moveAction.moveTime = movementActionBean.moveTime;
						}else{
							Debug.LogError("设置移动时间失败");
						}
					}
				}
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            movementTimeValue = EditorGUILayout.Slider(movementTimeValue, 0, 1);
			if (currentMovementTimeValue != movementTimeValue) {
				currentMovementTimeValue = movementTimeValue;
				AnimationController.Instance.SamepleMovemtentClip(currentMovementTimeValue);
			}

			if (GUILayout.Button ("设置为移动开始时间" + movementActionBean.startTime)) {
				movementActionBean.startTime = movementTimeValue;
				if(roleActionEventIndex != -1){
					MovementActionBean moveAction = SkillManager.Instance.ActionList[roleActionEventIndex] as MovementActionBean;
					if(moveAction != null){
						moveAction.startTime = movementActionBean.startTime;
					}
				}
			}
			if (GUILayout.Button ("设置为移动结束时间" + movementActionBean.endTime)) {
				movementActionBean.endTime = movementTimeValue;
				if(roleActionEventIndex != -1){
					MovementActionBean moveAction = SkillManager.Instance.ActionList[roleActionEventIndex] as MovementActionBean;
					if(moveAction != null){
						moveAction.endTime = movementActionBean.endTime;
					}
				}
			}
            EditorGUILayout.EndHorizontal();

        }
		public void RestMovementActionBean(){
			movementActionBean.moveTime = 0;
			movementActionBean.moveAnimationClip = null;
			movementActionBean.endTime = 1;
			movementActionBean.startTime = 0;
			movementActionBean.isUseAnimationTime = true;
		}

		float currModelActionSlider = 0;
		void Update(){
			if (currModelActionSlider != ModelActionSlider) {
				currModelActionSlider = ModelActionSlider;
				AnimationController.Instance.SampleClipBySlider(currModelActionSlider);
			}
			if(AnimationController.Instance.actionPlayer != null)
				AnimationController.Instance.actionPlayer.Update (Time.realtimeSinceStartup);

			if (!EditorApplication.isPlaying) {
				if(Instance != null)
					Instance.Close();	
			}
		}

		void OnDestroy(){
            movementActionBean = null;
            roleModelAnimations = null;
            roleModels.Clear(); 
            roleModels = null;
			ActionEventType = ActionEvent.None;
			SkillManager.Instance.UnInitialize ();
		}

		public void UpdateLoadSkillBean(SkillBean bean){
			Reset ();
			SkillManager.Instance.UnInitialize ();

		}
	}
}
