using System;
using System.Collections.Generic;
using UniCommon;

class BattleCaculater :Singleton<BattleCaculater>
{
    public bool GetTargetEntity(bool isSelf)
    {
        bool flag = false;
        //List<BattleEntity> entityList = isSelf ? SelfTeamMgr.EntityList : TargetTeam.EntityList;
        //entityList.ApplyAll(C => {
        //    if(!C.IsDead && )})

        return flag;
    }
	/// <summary>
	/// 获取攻击目标
	/// </summary>
	/// <returns>The target list.</returns>
	/// <param name="self">Self.</param>
    public List<BattleEntity> GetTargetList(BattleEntity self,SkillDataInfo skill)
    {
        List<BattleEntity> targetList = new List<BattleEntity>();
		targetList = GetTargetListByTargetType (self.IsSelfTeam, skill.targetType);
		if (targetList.Count > 0) {
			targetList = GetTargetListByTargetPosType (targetList, skill.targetPos);
		}
		if (targetList.Count > 0) {
			targetList = GetTargetListByAIType (targetList, skill.aiType,skill.maxTarget);
		}
        return targetList;
    }

	/// <summary>
	/// 根据释放目标类型获取所有实体
	/// </summary>
	/// <returns>The target list by target type.</returns>
	/// <param name="ttype">Ttype.</param>
	List<BattleEntity> GetTargetListByTargetType(bool isSelf,int ttype){
		List<BattleEntity> targetList = new List<BattleEntity>();

		switch (ttype) {
		case 1://全体
			BattleManager.Instance.SelfTeamMgr.EntityList.ApplyAll(C => {
				if( !C.IsDead)
					targetList.Add(C);
			});			
			BattleManager.Instance.TargetTeam.EntityList.ApplyAll(C => {
				if( !C.IsDead)
					targetList.Add(C);
			});
			break;
		case 2://敌方
			if(isSelf){
				BattleManager.Instance.TargetTeam.EntityList.ApplyAll(C => {
					if( !C.IsDead)
						targetList.Add(C);
				});
			}else{
				BattleManager.Instance.SelfTeamMgr.EntityList.ApplyAll(C => {
					if( !C.IsDead)
						targetList.Add(C);
				});		
			}
			break;
		case 3://自己
			if(isSelf){
				BattleManager.Instance.SelfTeamMgr.EntityList.ApplyAll(C => {
					if( !C.IsDead)
						targetList.Add(C);
				});	

			}else{
				BattleManager.Instance.TargetTeam.EntityList.ApplyAll(C => {
					if( !C.IsDead)
						targetList.Add(C);
				});	
			}
			break;
		}
		
		return targetList;
	}

	/// <summary>
	/// 根据位置获取目标列表
	/// </summary>
	/// <returns>The target list by target position type.</returns>
	/// <param name="tList">T list.</param>
	/// <param name="posType">Position type.</param>
	List<BattleEntity> GetTargetListByTargetPosType(List<BattleEntity> tList,int posType){
		List<BattleEntity> targetList = new List<BattleEntity>();
		switch (posType) {
		case 1://前排
			BattleManager.Instance.SelfTeamMgr.EntityList.ApplyAll(C => {
				if( !C.IsDead)
					targetList.Add(C);
			});			
			BattleManager.Instance.TargetTeam.EntityList.ApplyAll(C => {
				if( !C.IsDead)
					targetList.Add(C);
			});
			break;
		case 2://后排

			break;
		case 3://竖列
		
			break;
		case 4://全体
			
			break;
		}
		return targetList;
	}

	/// <summary>
	/// 根据ai类型获取目标列表
	/// </summary>
	/// <returns>The target list by AI type.</returns>
	/// <param name="tList">T list.</param>
	/// <param name="aiType">Ai type.</param>
	List<BattleEntity> GetTargetListByAIType(List<BattleEntity> tList,int aiType,int maxTarget){
		List<BattleEntity> targetList = new List<BattleEntity>();
		switch (aiType) {
		case 1://无规则
			BattleManager.Instance.SelfTeamMgr.EntityList.ApplyAll(C => {
				if( !C.IsDead)
					targetList.Add(C);
			});			
			BattleManager.Instance.TargetTeam.EntityList.ApplyAll(C => {
				if( !C.IsDead)
					targetList.Add(C);
			});
			break;
		case 2://血量最少
			
			break;
		case 3://对位
			
			break;
		case 4://自身
			
			break;
		}
		return targetList;
	}


	public SkillCalculateData GetEntitySkillAttackData(BattleEntity fire,BattleEntity beAttack){
		SkillCalculateData data = new SkillCalculateData (fire,beAttack);
		uint hurt = 0;
		data.IsHit = this.IsHit (fire, beAttack);
		if (data.IsHit && fire.entityBattleMgr.IsSkillHurt) {
			hurt = GetBaseHit(fire,beAttack);

			data.IsCrit = this.IsCrit(fire,beAttack);
			if(data.IsCrit){
				hurt = (uint)(hurt * 1.5f);
			}

			data.IsBlock = this.IsBlock(fire,beAttack);
			if(data.IsBlock){
				hurt = (uint)(hurt * 0.6f);
			}

			hurt = this.GetSkillAttack(fire,hurt);
			data.Hurt = hurt;
		}
		return data;
	}

	/// <summary>
	/// 是否命中
	/// </summary>
	/// <returns><c>true</c> if this instance is hit the specified fire beAttack; otherwise, <c>false</c>.</returns>
	/// <param name="fire">Fire.</param>
	/// <param name="beAttack">Be attack.</param>
	bool IsHit(BattleEntity fire,BattleEntity beAttack){
		bool hit = false;
		uint hitrate = fire.entityProperties.HitRate - beAttack.entityProperties.MisRate;
		hitrate = hitrate < 25 ? 25 : hitrate;
		hitrate = hitrate > 100 ? 100 : hitrate; 
		if (UnityEngine.Random.Range(0,101) < hitrate) {
			hit = true;
		}
		return hit;
	}

	/// <summary>
	/// 计算基础伤害
	/// </summary>
	/// <returns>The base hit.</returns>
	/// <param name="fire">Fire.</param>
	/// <param name="beAttack">Be attack.</param>
	uint GetBaseHit(BattleEntity fire,BattleEntity beAttack){
		uint attackValue = 0;
		int def = 0;
		int parmB = (int)( ( fire.entityProperties.Level >= beAttack.entityProperties.Level ? 1 : (beAttack.entityProperties.Level - fire.entityProperties.Level) / 100.0f));
		def = (int)(beAttack.entityProperties.Deffend / 1000f * parmB);
		attackValue = (uint)( fire.entityProperties.Attack * (1 - def) );
		return attackValue;
	}

	bool IsCrit(BattleEntity fire,BattleEntity beAttack){
		bool crit = false;

		return crit;
	}

	bool IsBlock(BattleEntity fire,BattleEntity beAttack){
		bool blok = false;
		
		return blok;
	}
	uint GetSkillAttack(BattleEntity fire,uint hurt){
		return (uint)( (1 + fire.entityBattleMgr.SkillHurePercent / 1000f) * hurt + fire.entityBattleMgr.SkillRealHurt);
	}
}

