using System;
using System.Collections;
using System.Collections.Generic;

public class HeroDadaConfig
{
    public List<HeroDada> m_heros = new List<HeroDada>();
}

public class HeroDada : Heros
{
    public uint pos;
}

public class HerosManager : Singleton<HerosManager>
{
    private Dictionary<int,HeroDada> m_HeroDadaDic = null;
    string ConfigPath = "/HeroDada.json";

    public override void Initialize()
    {
        base.Initialize();
        m_HeroDadaDic = new Dictionary<int,HeroDada>();
        if (System.IO.File.Exists(UnityEngine.Application.streamingAssetsPath + ConfigPath))
        {
            string str = System.IO.File.ReadAllText(UnityEngine.Application.streamingAssetsPath + ConfigPath);
            HeroDadaConfig data = JsonFx.Json.JsonReader.Deserialize<HeroDadaConfig>(str);
            for (int i = 0; i < data.m_heros.Count;++i )
            {
                HeroDada hero = data.m_heros[i];
                if(!m_HeroDadaDic.ContainsKey(hero.ID))
                    m_HeroDadaDic.Add(hero.ID, hero);
            }
        }
    }

    public override void UnInitialize()
    {
        base.UnInitialize();
        m_HeroDadaDic.Clear();
        m_HeroDadaDic = null;
    }

    /// <summary>
    /// 获得新英雄
    /// </summary>
    /// <param name="heroID"></param>
    public void AddNewHero(int heroID)
    {
        Heros heroCg =null;
        if (DictMgr.Instance.HerosDic.TryGetValue(heroID,out heroCg))
        {
            HeroDada hd = new HeroDada();
            hd.Attack = heroCg.Attack;
            hd.AttackSpeed = heroCg.AttackSpeed;
            hd.HP = heroCg.HP;
            hd.ID = heroCg.ID;
            hd.Model = heroCg.Model;
            hd.Name = heroCg.Name;
            hd.Skills = heroCg.Skills;
            m_HeroDadaDic.Add(heroID, hd);
        }
        List<HeroDada> herolist = new List<HeroDada>();
        foreach (HeroDada data in m_HeroDadaDic.Values)
        {
            herolist.Add(data);
        }
        HeroDadaConfig hereJsonData = new HeroDadaConfig();
        hereJsonData.m_heros = herolist;
        if (System.IO.File.Exists(UnityEngine.Application.streamingAssetsPath + ConfigPath))
        {
            System.IO.File.Delete(UnityEngine.Application.streamingAssetsPath + ConfigPath);
        }
        string str = JsonFx.Json.JsonWriter.Serialize(hereJsonData);
        System.IO.File.WriteAllText(UnityEngine.Application.streamingAssetsPath + ConfigPath, str, System.Text.Encoding.UTF8);
    }

    /// <summary>
    /// 更新英雄属性
    /// </summary>
    /// <param name="level"></param>
    public void UpdateHero(int level)
    {

    }

    public HeroDada Clone(int id,int level = 1)
    {
        HeroDada data = m_HeroDadaDic[id];
       
        if (data != null)
        {
            HeroDada hd = new HeroDada();
            data.HP = data.HP + level * 20;
            data.AttackSpeed = data.AttackSpeed - (int)(0.1 * level);
            data.Attack = data.Attack + (int)(5 + level);
            
            hd.Attack = data.Attack;
            hd.AttackSpeed = data.AttackSpeed;
            hd.HP = data.HP;
            hd.ID = data.ID;
            hd.Model = data.Model;
            hd.Name = data.Name;
            hd.Skills = data.Skills;
            return hd;
        }

        return null;
    }
}
