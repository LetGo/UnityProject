using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Linq;
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
    static string[] expectStr ;
    static int[] expectNum;
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

    void XLSX()
    {
        string path = Application.dataPath + "/test.xlsx";
        FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

        DataSet result = excelReader.AsDataSet();
        if (result == null)
        {
            Debug.Log("path =====" + path);
            return;
        }
        int columns = result.Tables[0].Columns.Count;
        int rows = result.Tables[0].Rows.Count;


        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                string nvalue = result.Tables[0].Rows[i][j].ToString();
                Debug.Log(nvalue);
            }
        }
    }

    static void LoadData(string path)
    {
        m_lstFiles.Clear();
        var directoryInfo = new DirectoryInfo(path);
        GetFiles(directoryInfo, ref m_lstFiles);
        //m_lstFiles.Sort();
        Debug.Log("files :" + m_lstFiles.Count);
        m_bLstEceleFileCheck = new bool[m_lstFiles.Count];
        expectNum = new int[m_lstFiles.Count];
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

    bool update = false;
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

                expectNum[i] = EditorGUILayout.IntField("不输出项", expectNum[i]);

                if (GUILayout.Button("确定", GUILayout.Width(50)))
                {
                    Debug.Log("count :" + expectNum[i]);
                }

                if (GUILayout.Button("导出", GUILayout.Width(50)))
                {
                    ReadExeleData(m_lstFiles[i].Name);
                }

                EditorGUILayout.EndHorizontal();
            }

        }
        GUILayout.EndScrollView();

    }


    string ReadExeleData(string fileName)
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
        
        Debug.Log("col:" + colCount + "row :" + rowCount);

        int index = fileName.LastIndexOf(".");
        string tabelName = string.Empty;
        if (index != -1)
        {
            tabelName = fileName.Substring(0, index);
        }
        
        List<string> attributeTitleList = new List<string>();
        for (int i = 0; i < colCount; ++i )
        {
            attributeTitleList.Add(table0.Rows[0][i].ToString());
            //Debug.Log(i +" : "+ table0.Rows[0][i].ToString());
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
        string strStart = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\" ?><Root>";
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
}

