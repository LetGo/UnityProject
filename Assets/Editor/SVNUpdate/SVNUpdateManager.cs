using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using System.Threading;

public class SVNUpdateManager : EditorWindow {

	Vector2 m_v2ScrollPos = new Vector2();

	string SVN_CONFIG_PATH = Application.dataPath +"/../svn_config.json" ;

	static string m_strCmd = "";
	/// <summary>
	/// 更新cmd.
	/// </summary>
	string m_strSVNUpdateCmd = "TortoiseProc.exe /command:update /path:\"{0}\" /closeonend:0";
	/// <summary>
	/// 添加cmd.
	/// </summary>
	string m_strSVNAddCmd = "TortoiseProc.exe /command:add /path:\"{0}\" /closeonend:0";

	string m_strSVNCommitCmd = "TortoiseProc.exe /command:commit /path:\"{0}\" /closeonend:0";

	class SVNData
	{
		public string Name;
		public string Path;
	}

	class ListSVNData{
		public List<SVNData> svnDataList = new List<SVNData>();
	}

	/// <summary>
	/// svn 配置数据.
	/// </summary>
	ListSVNData m_ListSVNData = new ListSVNData();

	string strTempIdentityName = "点击输入表名";

	string strTempSVNPath = "";

	[MenuItem("GameTool/SVN更新管理")]
	private static void Init(){
		SVNUpdateManager window = EditorWindow.GetWindow<SVNUpdateManager> (false, "SVN更新管理", true);
		window.Show ();
	}

	void Awake(){
		Reflash ();
	}

	void Reflash(){
		if (File.Exists (SVN_CONFIG_PATH)) {
			string strdata = File.ReadAllText(SVN_CONFIG_PATH);
			m_ListSVNData = JsonFx.Json.JsonReader.Deserialize<ListSVNData>(strdata);
		}
	}

	void OnGUI(){
		m_v2ScrollPos = EditorGUILayout.BeginScrollView ( m_v2ScrollPos);

		GUILayout.Label ("添加U3D可操作目录", EditorStyles.largeLabel, GUILayout.Height (20));
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("自定义目录：", EditorStyles.largeLabel, GUILayout.Height (20));
		strTempIdentityName = GUILayout.TextField (strTempIdentityName, 25);
		GUILayout.EndHorizontal ();

		if (GUILayout.Button ("选择目录路径", GUILayout.Width (120))) {
			strTempSVNPath = EditorUtility.OpenFolderPanel("找到要更新文件目录","","");
		}

		GUILayout.Label ("当前选择的路径为：" + strTempSVNPath, EditorStyles.largeLabel, GUILayout.Height (20));

		if (GUILayout.Button ("添加", GUILayout.Height (40))) {
			AddSVNUpdateSetting(strTempIdentityName,strTempSVNPath);
		}

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		GUILayout.Label ("当前可操作的更新目录列表", EditorStyles.largeLabel, GUILayout.Height (20));
		if (GUILayout.Button ("刷新",GUILayout.Width (120))) {
			Reflash ();
		}

		foreach (SVNData data in m_ListSVNData.svnDataList) {
			GUILayout.Label ("表识名:" + data.Name, EditorStyles.largeLabel, GUILayout.Height (20));
			GUILayout.Label ("路径:" + data.Path, EditorStyles.largeLabel, GUILayout.Height (20));
			EditorGUILayout.BeginHorizontal ();
			if(GUILayout.Button("更新目录",GUILayout.Width(30))){
				UpdateSVNPath(data.Path);
			}
			if(GUILayout.Button("添加文件",GUILayout.Width(30))){
				AddSVNPath(data.Path);
			}
			if(GUILayout.Button("提交目录",GUILayout.Width(30))){
				CommitSVNPath(data.Path);
			}
			if(GUILayout.Button("删除信息",GUILayout.Width(30))){
				RemoveSVNUpdateSetting(data);
				break;
			}
			GUILayout.EndHorizontal ();
		}

		EditorGUILayout.EndScrollView ();
	}

	private void AddSVNUpdateSetting(string name,string path){
		SVNData data = new SVNData ();
		data.Name = name;
		data.Path = path;

		m_ListSVNData.svnDataList.Add (data);

		string strData = JsonFx.Json.JsonWriter.Serialize (m_ListSVNData);
		File.WriteAllText (SVN_CONFIG_PATH, strData);
	}
	private void RemoveSVNUpdateSetting(SVNData data){
		m_ListSVNData.svnDataList.Remove (data);
		
		string strData = JsonFx.Json.JsonWriter.Serialize (m_ListSVNData);
		File.WriteAllText (SVN_CONFIG_PATH, strData);
	}
	void UpdateSVNPath(string path){
		string cmd = string.Format (m_strSVNUpdateCmd, path);
		ExcuteCmd (cmd);
	}

	void AddSVNPath(string path){
		string cmd = string.Format (m_strSVNAddCmd, path);
		ExcuteCmd (cmd);
	}

	void CommitSVNPath(string path){
		string cmd = string.Format (m_strSVNCommitCmd, path);
		ExcuteCmd (cmd);
	}
	/// <summary>
	///开启新线程 
	/// </summary>
	/// <param name="cmd">Cmd.</param>
	void ExcuteCmd(string cmd){
		m_strCmd = cmd;

		Thread t = new Thread (new ThreadStart (RunCmd));
		t.Name = "new Thread";
		t.Start ();
	}

	private static void RunCmd(){
		Process p = new Process ();
		p.StartInfo.FileName = "cmd.exe";
		p.StartInfo.Arguments = "/c " + m_strCmd; 
		p.StartInfo.UseShellExecute = false;
		p.StartInfo.RedirectStandardInput = true;
		p.StartInfo.RedirectStandardOutput = true;
		p.StartInfo.RedirectStandardError = true;
		p.StartInfo.CreateNoWindow = true;
		p.Start ();
	}
}
