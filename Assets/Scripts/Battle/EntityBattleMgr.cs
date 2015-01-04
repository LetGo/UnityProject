using UnityEngine;
using System.Collections;

public class EntityBattleMgr {
	public SkillBeanPlayer skillBeanPlayer;

	SkillBean currentSkillBean;
	BattleEntity entity;
	uint attackRound = 0;
	bool fighting = false;

	public EntityBattleMgr(BattleEntity entity){
		this.entity = entity;
		attackRound = 0;
		fighting = false;
		skillBeanPlayer = new SkillBeanPlayer ();
	}

	bool CheckCanfire(){
		//1 检测是否可以攻击
		if (!skillBeanPlayer.IsPlaying () && attackRound > 0) {		
			return true;
		}
		return false;
	}

	void BeginAttack(){
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
			                     BattleManager.Instance.TargetTeam.EntityList[0].entityGo.transform.position,ActionStatus.Play);		
		}else
		{
			skillBeanPlayer.Init(entity.entityGo,currentSkillBean,ActionStatus.Play);		
		}
	}

	public void AddAttackRound(){
		attackRound++;
		if (CheckCanfire ()) {
			fighting = true;
			BeginAttack();
		}
	}
}
