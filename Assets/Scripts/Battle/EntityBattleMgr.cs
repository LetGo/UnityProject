using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityBattleMgr {
	public SkillBeanPlayer skillBeanPlayer;

	SkillBean currentSkillBean;
	BattleEntity entity;
    SkillDataInfo skillDataInfo;
	uint attackRound = 0;
	bool isFighting = false;
    List<BattleEntity> targetEntityList;

	public EntityBattleMgr(BattleEntity entity){
		this.entity = entity;
		attackRound = 0;
        isFighting = false;
        skillBeanPlayer = new SkillBeanPlayer(entity,AttackCallBack, AttackEndCallBack);
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
		if ( !isFighting && !skillBeanPlayer.IsPlaying () && attackRound > 0 ) {		
			return true;
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
        targetEntityList.Clear();
        targetEntityList.Add(BattleManager.Instance.TargetTeam.EntityList[0]);
        EntityComponent Component = BattleManager.Instance.TargetTeam.EntityList[0].entityGo.GetComponent<EntityComponent>();
		if (currentSkillBean.Movement) {
			skillBeanPlayer.Init(entity.entityGo,currentSkillBean,
                                 Component.transform.position, ActionStatus.Play);		
		}else
		{
			skillBeanPlayer.Init(entity.entityGo,currentSkillBean,ActionStatus.Play);		
		}
	}

    public void BeAttack(int hurt)
    {
        entity.entityProperties.BeAttack(hurt);
    }

    public void AttackEndCallBack()
    {
        Debug.Log("AttackEndCallBack");
        isFighting = false;
    }

    public void AttackCallBack()
    {
        Debug.Log("AttackCallBack num = " + targetEntityList.Count);
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
