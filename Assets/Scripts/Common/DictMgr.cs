﻿using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine;

public delegate void delegateFunc(XmlNode node);
public delegate void OnLoadComplete();

public class DictMgr : Singleton<DictMgr> 
{
    public Dictionary<int, UIConfigData> UIConfigDic = null;
    bool bLoadUI = false;



    public override void Initialize()
    {
        base.Initialize();
        UIConfigDic = new Dictionary<int, UIConfigData>();
        LoadHeros();
        LoadSkillsData();
        LoadUIManagerData();
    }

    public override void UnInitialize()
    {
        base.UnInitialize();
    }

#region  UImanager
    void LoadUIManagerData()
    {
        if (!bLoadUI)
        {
            bLoadUI = true;
            Load_XMLData("UIManeger", OnLoadUIManagerData, LoadComplete);
        }
    }

    void OnLoadUIManagerData(XmlNode node)
    {
        UIConfigData data = new UIConfigData();
        XmlAttribute nodeValue = null;
        nodeValue = node.Attributes["ID"];
        data.ID = GetNodeInt(nodeValue);

        nodeValue = node.Attributes["ShareID"];
        data.sharedID = GetNodeInt(nodeValue);

        nodeValue = node.Attributes["ParentID"];
        data.parentID = GetNodeInt(nodeValue);

        nodeValue = node.Attributes["Path"];
        data.path = GetNodetString(nodeValue).Replace("\\", "/");

        if (!UIConfigDic.ContainsKey(data.ID))
        {
            UIConfigDic.Add(data.ID, data);
            Debug.Log(" test : " + data.ID);
        }
        else
        {
            Debug.LogError(" UIconfig Already contain id :" + data.ID);
        }
    }
#endregion
#region skills
    public Dictionary<int, Skills> SkillsDic = new Dictionary<int, Skills>();
    bool bLoadSkills = false;
    void LoadSkillsData()
    {
        if (!bLoadSkills)
        {
            bLoadSkills = true;
            Load_XMLData("Skills", OnLoadSkillsData, null);
        }
    }

    void OnLoadSkillsData(XmlNode node)
    {
        Skills data = new Skills();
        XmlAttribute nodeValue = null;
        nodeValue = node.Attributes["ID"];
        data.ID = GetNodeInt(nodeValue);
        nodeValue = node.Attributes["Name"];
        data.Name = GetNodetString(nodeValue);
        nodeValue = node.Attributes["IsHurt"];
        data.IsHurt = GetNodeInt(nodeValue);
        nodeValue = node.Attributes["Type"];
        data.Type = GetNodeInt(nodeValue);
        nodeValue = node.Attributes["AttackAssets"];
        data.AttackAssets = GetNodeInt(nodeValue);
        nodeValue = node.Attributes["BeAttackAssets"];
        data.BeAttackAssets = GetNodetString(nodeValue);
        nodeValue = node.Attributes["TargetType"];
        data.TargetType = GetNodeInt(nodeValue);
        nodeValue = node.Attributes["Position"];
        data.Position = GetNodeInt(nodeValue);
        nodeValue = node.Attributes["AttackNum"];
        data.AttackNum = GetNodeInt(nodeValue);
        nodeValue = node.Attributes["Hurt_percent"];
        data.Hurt_percent = GetNodeInt(nodeValue);
        nodeValue = node.Attributes["Random"];
        data.Random = GetNodeInt(nodeValue);
        nodeValue = node.Attributes["RealHurt"];
        data.RealHurt = GetNodeInt(nodeValue);
        nodeValue = node.Attributes["AI"];
        data.AI = GetNodeInt(nodeValue);

        if ( !SkillsDic.ContainsKey(data.ID) )
        {
            SkillsDic.Add(data.ID, data);
        }
        else
        {
            Debug.Log("Already contain skill id :" + data.ID);
        }
    }
#endregion
#region Heros
    public Dictionary<int, Heros> HerosDic = new Dictionary<int, Heros>();
    bool bLoadHeros = false;
    void LoadHeros(){
        if(!bLoadHeros)
        {
            bLoadHeros = true;
            Load_XMLData("Heros", OnLoadHerosData, null);
        }
    }

    void OnLoadHerosData(XmlNode node)
    {
        Heros data = new Heros();
        XmlAttribute nodeValue = null;
        nodeValue = node.Attributes["ID"];
        data.ID = GetNodeInt(nodeValue);
        nodeValue = node.Attributes["Name"];
        data.Name = GetNodetString(nodeValue);
        nodeValue = node.Attributes["AttackSpeed"];
        data.AttackSpeed = GetNodeInt(nodeValue);
        nodeValue = node.Attributes["Model"];
        data.Model = GetNodetString(nodeValue);
        nodeValue = node.Attributes["Attack"];
        data.Attack = GetNodeInt(nodeValue);
        nodeValue = node.Attributes["HP"];
        data.HP = GetNodeInt(nodeValue);
        nodeValue = node.Attributes["Skills"];
        string strSkills = GetNodetString(nodeValue);
        string[] skills = strSkills.Split('#');
        for (int i = 0; i < skills.Length; ++i )
        {
            data.Skills.Add(int.Parse(skills[i]));
        }

        if (!HerosDic.ContainsKey(data.ID))
        {
            HerosDic.Add(data.ID, data);
        } 
        else
        {
        }
    }
#endregion
    void LoadComplete()
    {
        GameState.GameSceneManager.Instance.Initialize();
       // UIManager.Instance.OpenUIpanel(UIPanelID.ePanel_Loading);
    }



    void Load_XMLData(string path, delegateFunc callback, OnLoadComplete onLoadComplete)
    {
        path  = path + ".xml";
        path = Application.streamingAssetsPath + "/GameConfig/" + path;
        if (Application.platform != RuntimePlatform.Android)
        {
            path = "file://" + path;
        }

        GameManager.Instance.ClientProxy.StartCoroutine(Load(path, callback, onLoadComplete));
    }

    IEnumerator Load(string path, delegateFunc callback, OnLoadComplete onLoadComplete)
    {
        WWW www = new WWW(path);
        yield return www;
        if (www.isDone)
        {
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("Load " + path + " Error :" + www.error);
                yield break;
            }
            //非加密
            if (www.text.Substring(0, 5).Equals("<?xml"))
            {
                LoadXml(www.text, callback);
            } 
            else
            { //加密
                LoadXml(UniCommon.CommonTool.DecipheringContent(www.text), callback);
            }
            
            if (onLoadComplete != null)
                onLoadComplete();
        }
    }

    void LoadXml(string text, delegateFunc callback)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(text);
        XmlNode node = xmlDoc.SelectSingleNode("Root");

        XmlNode firstNode = node.FirstChild;
        while (firstNode != null)
        {
            callback(firstNode);
            firstNode = firstNode.NextSibling;
        }
    }

    int GetNodeInt(XmlAttribute bute)
    {
        int nvalue = 0;
        if (bute != null)
        {
            nvalue = StringToInt(bute.Value);
        }
        return nvalue;
    }

    string GetNodetString(XmlAttribute bute)
    {
        string str = string.Empty;
        if (bute != null)
        {
            str = bute.Value;
        }
        return str;
    }

    int StringToInt(string s)
    {
        if (string.IsNullOrEmpty(s))
            return 0;
        try
        {
            return System.Convert.ToInt32(s);
        }
        catch (System.Exception ex)
        {
            //Debug.LogException(ex);
            return 0;
        }
    }
}
