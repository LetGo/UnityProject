using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;

namespace SkillEditor{
	public class SkillEditorWindow : EditorWindow {
		static SkillEditorWindow Instance = null;

		[MenuItem("GameTool/SkillEditor %E")]
		static void Init(){
			if(EditorApplication.isPlaying && !EditorApplication.isPaused)
				Instance = EditorWindow.GetWindow<SkillEditorWindow> ("技能编辑器", false, typeof(SkillEditorWindow));
			else
				Debug.Log("运行场景");
		}

		void OnGUI(){

		}
	}
}
