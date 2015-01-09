using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniCommon;

public class EntityBattleMgr {
	public SkillBeanPlayer skillBeanPlayer;

	SkillBean currentSkillBean;
	BattleEntity entity;
    SkillDataInfo skillDataInfo;
	uint attackRound = 0;
	public bool isFighting = false;
    List<BattleEntity> targetEntityList;
    public bool beLock { get; set; } //被攻击方锁定无法发动攻击
	public bool IsSkillHurt{get{return skillDataInfo.IsHurt;}}
	public uint SkillHurePercent{get{return skillDataInfo.hurtPercentage;}}
	public uint SkillRealHurt{get{return skillDataInfo.realHurt;}}
	public uint SkillBlock{get{return skillDataInfo.realHurt;}}

	public EntityBattleMgr(BattleEntity entity){
		this.entity = entity;
		attackRound = 0;
        beLock = false;
        isFighting = false;
		skillBeanPlayer = new SkillBeanPlayer(this.entity,AttackCallBack, AttackEndCallBack);
        targetEntityList = new List<BattleEntity>();
        InitSkillDataInfo();
	}

    void InitSkillDataInfo()
    {
        skillDataInfo = new SkillDataInfo();
        skillDataInfo.hurt = 5;
        skillDataInfo.targetPos = 1;
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
		InitSkillBean ("Test");
		PlaySkillBean ();
	}

	void InitSkillBean(string skill){

		SkillBean bean = Resources.Load ("Skills/" + skill) as SkillBean;
		currentSkillBean = bean.Clone ();
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
            targetEntityList[i].entityBattleMgr.BeAttack(skillDataInfo.hurt);
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
