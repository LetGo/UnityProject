using UnityEngine;
using System.Collections;

public class EntityBattleMgr {
	public SkillBeanPlayer skillBeanPlayer;

	SkillBean currentSkillBean;
	BattleEntity entity;
	public EntityBattleMgr(BattleEntity entity){
		this.entity = entity;
		skillBeanPlayer = new SkillBeanPlayer ();
	}

	public bool CheckCanfire(){
		//1 检测是否可以攻击
		if (!skillBeanPlayer.IsPlaying ()) {
			BeginAttack();		
			return true;
		}
		return false;
	}

	public void BeginAttack(){

		InitSkillBean ("Test");
		PlaySkillBean ();
	}

	void InitSkillBean(string skill){

		SkillBean bean = Resources.Load ("Skills/" + skill) as SkillBean;
		currentSkillBean = bean.Clone ();
	}

	void PlaySkillBean(){

		if (currentSkillBean.Movement) {
			skillBeanPlayer.Init(entity.entityGo,currentSkillBean,BattleManager.Instance.TargetTeam.EntityList[0].entityGo.transform.position,ActionStatus.Play);		
		}else
		{
			skillBeanPlayer.Init(entity.entityGo,currentSkillBean,ActionStatus.Play);		
		}

	}
}
