using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniCommon;

public class EntityBattleMgr {
	public SkillBeanPlayer skillBeanPlayer;

	SkillBean currentSkillBean;
	BattleEntity entity;
    Skills skillDataInfo; //当前使用的技能
	uint attackRound = 0;
	public bool isFighting = false;
    List<BattleEntity> targetEntityList;
    public uint BeAttackNum { get; set; }
    public bool beLock { get; set; } //被攻击方锁定无法发动攻击
	public bool IsSkillHurt{get{return skillDataInfo.IsHurt == 1;}}
	public int SkillHurePercent{get{return skillDataInfo.Hurt_percent;}}
	public int SkillRealHurt{get{return skillDataInfo.RealHurt;}}
	public int SkillBlock{get{return skillDataInfo.RealHurt;}}

    List<int> skillList = new List<int>();
    int m_currSkillIndex = 0;
    int m_heroID = 0;
	public EntityBattleMgr(BattleEntity entity,List<int> skillList,int heroID){
		this.entity = entity;
		attackRound = 0;
        beLock = false;
        isFighting = false;
        m_heroID = heroID;
        this.skillList = skillList;
		skillBeanPlayer = new SkillBeanPlayer(this.entity,AttackCallBack, AttackEndCallBack);
        targetEntityList = new List<BattleEntity>();
	}

	bool CheckCanfire(){
		//1 检测是否可以攻击
        if (!isFighting && !skillBeanPlayer.IsPlaying() && attackRound > 0 && !beLock) 
        {   
            targetEntityList = BattleManager.Instance.GetTargetEntity(entity.IsSelfTeam);
            if (targetEntityList.Count > 0)
            {
                targetEntityList.ApplyAll(C => C.entityBattleMgr.beLock = true);
                return true;
            }
		}
		return false;
	}

	void BeginAttack(){
        isFighting = true;
		attackRound--;
        skillDataInfo = InitSkillInfo();
        if (skillDataInfo != null)
        {
            InitSkillBean(skillDataInfo.ID.ToString());
            PlaySkillBean();
        }
	}

    Skills InitSkillInfo()
    {
        Skills skill = null;
        int skillid = GetNextSkillID();
        if (skillid > 0)
        {
            skill = DictMgr.Instance.SkillsDic[skillid];
        }
        return skill;
    }

    int GetNextSkillID()
    {
        int indexID = 0;
        if (m_currSkillIndex < skillList.Count)
        {
            indexID = m_currSkillIndex;
        }
        else
        {
            m_currSkillIndex = 0;
            indexID = m_currSkillIndex;
        }
        m_currSkillIndex++;
        return skillList[indexID];
    }

	void InitSkillBean(string skill){

        SkillBean bean = Resources.Load(string.Format("Skills/{0}/{1}" ,m_heroID ,skill) )as SkillBean;
        if (bean != null)
        {
            currentSkillBean = bean.Clone();
        }
        else
        {
            Debug.LogError("InitSkillBean fail id " + skill);
        }
	}

	void PlaySkillBean(){
		if (currentSkillBean.Movement) {
			skillBeanPlayer.Init(entity.entityGo,currentSkillBean,
            targetEntityList[0].entityGo.transform.position, ActionStatus.Play);		
		}else
		{
			skillBeanPlayer.Init(entity.entityGo,currentSkillBean,ActionStatus.Play);		
		}
	}

    public void BeAttack(int hurt)
    {
        entity.entityProperties.BeAttack(hurt);
        beLock = false;
        BeAttackNum++;
    }

    /// <summary>
    /// 技能施放完成回调
    /// </summary>
    public void AttackEndCallBack()
    {
        Debug.Log("AttackEndCallBack");
        isFighting = false;
    }

    /// <summary>
    /// 攻击回调
    /// </summary>
    public void AttackCallBack()
    {
        Debug.Log("AttackCallBack   = " + targetEntityList.Count);
        for (int i = 0; i < targetEntityList.Count; ++i )
        {
            targetEntityList[i].entityBattleMgr.BeAttack(5);
        }
    }

	public void AddAttackRound(){
		attackRound++;
		if (CheckCanfire ()) {
			BeginAttack();
		}
	}

    Vector3 GetTargetPos()
    {
        Vector3 pos = Vector3.zero;


        return pos;
    }

    void GeneraTarget(uint pos)
    {
        targetEntityList.Clear();
        BattleEntity target = null;
        switch (pos)
        {
            case 1: //单个
                targetEntityList = BattleManager.Instance.TargetTeam.EntityList.FindAll(C => C.Position == entity.Position);
                break;
            case 2: //一列
                target = BattleManager.Instance.TargetTeam.EntityList.Find(C => C.Position == entity.Position);
                if ( target != null)
                    targetEntityList.Add(target);
                target = BattleManager.Instance.TargetTeam.EntityList.Find(C => C.Position == entity.Position + 3);
                if ( target != null)
                    targetEntityList.Add(target);
                break;
            case 3://第一行
                break;
            case 4://所有
                break;
            case 5: //后一行

                break;
        }
    }
}
