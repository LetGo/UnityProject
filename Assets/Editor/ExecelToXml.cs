using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Linq;
using Excel;
using System.Data;

public class ExecelToXml : EditorWindow
{
    static string m_strRersourcePath;
    static string m_strTargetPath;
    static List<FileInfo> m_lstFiles = new List<FileInfo>();
    static bool m_bDataListing = true;
    static bool[] m_bLstEceleFileCheck;
    static Vector2 m_v2ScrollPos = Vector2.zero;

    [MenuItem("GameTool/EceleToXML")]
    static void Init()
    {
        ExecelToXml window = (ExecelToXml)EditorWindow.GetWindow(typeof(ExecelToXml));
        window.Show();
        m_strRersourcePath = PlayerPrefs.GetString("ecelePath");
        m_strTargetPath = PlayerPrefs.GetString("savePath");
        LoadData(m_strRersourcePath);
    }  
    
    void OnGUI()
    {
        GUILayout.Width(80.0f);
        GUILayout.Label("源文件路径：" + m_strRersourcePath,GUILayout.Height(15));
        if (GUILayout.Button("选择源文件路径",GUILayout.Height(30)))
        {
            string newPath = string.Empty;
            newPath = EditorUtility.OpenFolderPanel("选择源文件路径", m_strRersourcePath, "");
            if (!newPath.Equals(m_strRersourcePath) && !string.IsNullOrEmpty(newPath))
            {
                m_strRersourcePath = newPath;
                PlayerPrefs.SetString("ecelePath",m_strRersourcePath);
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
            
            Debug.LogError("aaaaa");
        }
        SelectEceleData();
    }

    static void LoadData(string path)
    {
        m_lstFiles.Clear();
        var directoryInfo = new DirectoryInfo(path);
        GetFiles(directoryInfo, ref m_lstFiles);
        m_lstFiles.Sort();
        m_bLstEceleFileCheck = new bool[m_lstFiles.Count];
    }

    static void GetFiles(DirectoryInfo di, ref List<FileInfo> files)
    {
        foreach ( FileInfo file in di.GetFiles())
        {
             //获取扩展名
            if (Path.GetExtension(file.FullName).Equals(".xlsx") || Path.GetExtension(file.FullName).Equals(".xls"))
            {
                files.Add(file);
            }
        }

        //遍历子文件夹
        DirectoryInfo[] dis = di.GetDirectories();
        foreach(DirectoryInfo sdi in dis){
            GetFiles(sdi, ref files);
        }

    }


    void SelectEceleData()
    {
        m_bDataListing = EditorGUILayout.Foldout(m_bDataListing, "Exele配置文件");
        m_v2ScrollPos = GUILayout.BeginScrollView(m_v2ScrollPos);
        if (m_bDataListing)
        {
            for (int i = 0; i < m_lstFiles.Count; ++i )
            {
                EditorGUILayout.BeginHorizontal(GUILayout.Width(250));
                m_bLstEceleFileCheck[i] = GUILayout.Toggle(m_bLstEceleFileCheck[i],"",GUILayout.Width(30)); 
                GUILayout.TextArea(m_lstFiles[i].Name, GUILayout.Width(200));
                if (GUILayout.Button("导出",GUILayout.Width(80)))
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
            EditorUtility.DisplayDialog("提示","保存路径不能为空啊亲","Get it");
            return "";
        }

        string fullPath = Path.Combine(m_strRersourcePath, fileName);
        FileStream stream = File.OpenRead(fullPath);
        IExcelDataReader excelReader = null;
        //1. Reading from a binary Excel file ('97-2003 format; *.xls)
        if (fileName.EndsWith(".xls"))
        {
            excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
        } 
        else if(fileName.EndsWith(".xlsx"))
        {//2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
            excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        }
        
        DataSet result = excelReader.AsDataSet();
        int colCount = result.Tables[0].Columns.Count;
        int rowCount = result.Tables[0].Rows.Count;

        Debug.Log("col:" + colCount + "row :" + rowCount);
        return "";
    }
}

