using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using Excel;
using System.Data;
using UniCommon;
using System;
using System.Text;

public class Excel2XML : EditorWindow
{
    static string m_strRersourcePath;
    static string m_strTargetPath;
    static List<FileInfo> m_lstFiles = new List<FileInfo>();
    static bool m_bDataListing = false;
    static bool[] m_bLstEceleFileCheck;
    static Vector2 m_v2ScrollPos = Vector2.zero;

    [MenuItem("GameTool/ExcelToXML New")]
    static void Init()
    {
        Excel2XML window = (Excel2XML)EditorWindow.GetWindow(typeof(Excel2XML));
        window.Show();
        m_strRersourcePath = PlayerPrefs.GetString("exelePath");
        m_strTargetPath = PlayerPrefs.GetString("savePath");
        if (!string.IsNullOrEmpty(m_strRersourcePath))
        {
            LoadData(m_strRersourcePath);
        }
    }

    void OnGUI()
    {
        GUILayout.Width(80.0f);
        GUILayout.Label("源文件路径：" + m_strRersourcePath, GUILayout.Height(15));
        if (GUILayout.Button("选择源文件路径", GUILayout.Height(30)))
        {
            string newPath = string.Empty;
            newPath = EditorUtility.OpenFolderPanel("选择源文件路径", m_strRersourcePath, "");
            if (!string.IsNullOrEmpty(newPath) && !newPath.Equals(m_strRersourcePath))
            {
                m_strRersourcePath = newPath;
                PlayerPrefs.SetString("exelePath", m_strRersourcePath);
            }
        }

        GUILayout.Label("保存文件路径：" + m_strTargetPath, GUILayout.Height(15));
        if (GUILayout.Button("保存文件路径", GUILayout.Height(30)))
        {
            string newPath = string.Empty;
            newPath = EditorUtility.OpenFolderPanel("选择保存路径", m_strRersourcePath, "");
            if (!newPath.Equals(m_strTargetPath) && !string.IsNullOrEmpty(newPath))
            {
                m_strTargetPath = newPath;
                PlayerPrefs.SetString("savePath", m_strTargetPath);
            }
        }

        if (GUILayout.Button("确认选择", GUILayout.Height(30)))
        {
            LoadData(m_strRersourcePath);
        }
        SelectEceleData();
    }

    static int Compare(FileInfo a, FileInfo b)
    {
        return a.Name.CompareTo(b.Name);
    }

    static void LoadData(string path)
    {
        m_lstFiles.Clear();
        var directoryInfo = new DirectoryInfo(path);
        GetFiles(directoryInfo, ref m_lstFiles);
        m_lstFiles.Sort(Compare);
        m_bLstEceleFileCheck = new bool[m_lstFiles.Count];
    }


    /// <summary>
    /// 获取execle文件
    /// </summary>
    /// <param name="di"></param>
    /// <param name="files"></param>
    static void GetFiles(DirectoryInfo di, ref List<FileInfo> files)
    {
        foreach (FileInfo file in di.GetFiles())
        {
            //获取扩展名
            if (Path.GetExtension(file.FullName).Equals(".xlsx") || Path.GetExtension(file.FullName).Equals(".xls"))
            {
                files.Add(file);
            }
        }

        //遍历子文件夹
        DirectoryInfo[] dis = di.GetDirectories();
        foreach (DirectoryInfo sdi in dis)
        {
            GetFiles(sdi, ref files);
        }
    }

    void SelectEceleData()
    {
        m_bDataListing = EditorGUILayout.Foldout(m_bDataListing, "Exele配置文件");
        m_v2ScrollPos = GUILayout.BeginScrollView(m_v2ScrollPos);
        if (m_bDataListing)
        {
            
            for (int i = 0; i < m_lstFiles.Count; ++i)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.Width(400));
                m_bLstEceleFileCheck[i] = GUILayout.Toggle(m_bLstEceleFileCheck[i], "", GUILayout.Width(30));
                GUILayout.TextArea(m_lstFiles[i].Name, GUILayout.Width(100));

                if (GUILayout.Button("导出", GUILayout.Width(50)))
                {
                   string name = ReadExeleData(m_lstFiles[i].Name);
                   EditorUtility.DisplayDialog("提示", "生成文件" + name +"成功", "OK");
                }
				if (GUILayout.Button("导出Json", GUILayout.Width(50)))
				{
					string name = ReadExeleData(m_lstFiles[i].Name,true);
					EditorUtility.DisplayDialog("提示", "生成文件" + name +"成功", "OK");
				}
				if (GUILayout.Button("导出并加密", GUILayout.Width(80)))
				{
                    string path = ReadExeleData(m_lstFiles[i].Name);
                    FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
                    StreamReader sr = new StreamReader(stream);
                    string strEncry = CommonTool.EncryptionContent(sr.ReadToEnd());
                    sr.Close();
                    stream.Close();
                    File.Delete(path);

                    FileStream stream1 = new FileStream(path, FileMode.Create, FileAccess.Write);
                    StreamWriter sw1 = new StreamWriter(stream1);
                    sw1.Write(strEncry);
                    sw1.Close();
                    stream1.Close();
                    EditorUtility.DisplayDialog("提示", "生成文件" + path + "成功", "OK");
                }
                EditorGUILayout.EndHorizontal();
            }

        }
        GUILayout.EndScrollView();

    }

    string ReadExeleData(string fileName,bool bjson = false)
    {
        if (string.IsNullOrEmpty(m_strTargetPath))
        {
            EditorUtility.DisplayDialog("提示", "保存路径不能为空啊!", "Get it");
            return "";
        }

        string fullPath = m_strRersourcePath + "/" + fileName;

        Debug.Log("fullPath :" + fullPath);

        FileStream stream = File.Open(fullPath,FileMode.Open,FileAccess.Read);

        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        if (excelReader == null)
        {
            Debug.LogError("excelReader is null");
            return "";
        }
       
        DataSet result = excelReader.AsDataSet();
        if (result == null)
        {
            Debug.LogError("DataSet is null");
            return "";
        }

        DataTable table0 = result.Tables[0];

        int rowCount = table0.Rows.Count; //行
        int colCount = table0.Columns.Count;//列
        
        int index = fileName.LastIndexOf(".");
        string tabelName = string.Empty;
        if (index != -1)
        {
            tabelName = fileName.Substring(0, index);
        }
        
        List<string> attributeTitleList = new List<string>();
		List<object> aDataRowList = new List<object>();
        for (int i = 0; i < colCount; ++i )
        {
            attributeTitleList.Add(table0.Rows[0][i].ToString());
			object o = table0.Rows[1][i];
			aDataRowList.Add(o);
            //Debug.Log(i +" : "+ table0.Rows[0][i].ToString());
        }

		if (bjson) {
			WriteClass(tabelName,attributeTitleList,aDataRowList)	;	
		}
        List<string> attributeList = new List<string>();
        attributeList.Clear();
        StringBuilder builder = new StringBuilder();

        for (int i = 1; i < rowCount; i++)
        {
            builder.Append("\t<").Append(tabelName).Append(" ");
            for (int j = 0; j < colCount; j++)
            {
                string nvalue = table0.Rows[i][j].ToString();
                if (!string.IsNullOrEmpty(nvalue))
                {
                    builder.Append(string.Format("{0}=\"{1}\"{2}", attributeTitleList[j], nvalue, j == colCount - 1 ? "" : " "));
                }
            }
            builder.Append("/>");
            attributeList.Add(builder.ToString());
            builder.Length = 0;
        }

        return Write(tabelName, attributeList);
    }

    string Write(string name,List<string> attributeList)
    {
        string strStart = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?><Root>";
        string strEnd = "</Root>";
        string strTargetPath = m_strTargetPath + "/" + name +".xml";
        Debug.Log("strTargetPath :" + strTargetPath);
        if (File.Exists(strTargetPath))
        {
            File.Delete(strTargetPath);
        }
        FileStream stream = new FileStream(strTargetPath,FileMode.Create,FileAccess.Write);
        StreamWriter sw = new StreamWriter(stream);
        sw.WriteLine(strStart);
        attributeList.ApplyAll( C => sw.WriteLine(C) );
        sw.WriteLine(strEnd);
        sw.Close();
        stream.Close();
        return strTargetPath;
    }

	void WriteClass(string flilNmae,List<string> attributeTitleList,List<object> aDataRowList){
		string path = Application.dataPath +"/Scripts/DataBase/" + flilNmae +".cs";
		Debug.Log ("json path :" + path);
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		FileStream stream = new FileStream(path,FileMode.Create,FileAccess.Write);
		StreamWriter sw = new StreamWriter(stream);
		sw.WriteLine ("using System;");
		sw.WriteLine ("using System.Collections;");
		sw.WriteLine ("using System.Collections.Generic;");
		sw.WriteLine ("public class " + flilNmae +"{");
		int m_int = 0;
		float m_float = 0;
		for (int i = 0; i < aDataRowList.Count && i < attributeTitleList.Count; ++i) {
			if( Int32.TryParse(aDataRowList[i].ToString(),out m_int) ){
				sw.WriteLine ("\tpublic " + "int" + " " + attributeTitleList[i] + ";");	
			}else if(float.TryParse(aDataRowList[i].ToString(),out m_float)){
				sw.WriteLine ("\tpublic " + "float" + " " + attributeTitleList[i] + ";");	
			}
			else if(aDataRowList[i].ToString().Contains('#')){
				string[] split = aDataRowList[i].ToString().Split('#');
				if( Int32.TryParse(split[0],out m_int) ){
					sw.WriteLine ("\tpublic " + "List<int" + "> " + attributeTitleList[i] + ";");	
				}else {
					sw.WriteLine ("\tpublic " + "List<"  + aDataRowList[i].GetType()+ ">" + attributeTitleList[i] + ";");
				}
			}
			else{
				sw.WriteLine ("\tpublic " + aDataRowList[i].GetType() + " " + attributeTitleList[i] + ";");	
			}
		}

		sw.WriteLine ("}");
		sw.Close();
		stream.Close();
	}
}

