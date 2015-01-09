using System;
using System.Collections;
using System.Collections.Generic;

public class SkillCalculateData {
	public uint Hurt = 0;
	public bool IsHit = false;
	public bool IsCrit = false;//baoji
	public bool IsBlock = false;//格挡
	public BattleEntity FireEntity = null;
	public BattleEntity BeAttackEntity = null;

	public SkillCalculateData(BattleEntity f,BattleEntity b){
		FireEntity = f;
		BeAttackEntity = b;
	}
}
