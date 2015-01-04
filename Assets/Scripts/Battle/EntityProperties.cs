using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityProperties{
	public int EntityID;
	public string Name;
	public List<int> SkillList = new List<int>();

	public int Attack;
	public int RunSpeed;
	public float AttackSpeed;
	public int Hp;	
	public int CurrentHp;
	public int Pos;
	public bool IsEnemy;
	public bool IsDead{get {return CurrentHp <= 0;}}

	BattleEntity entity;


	public EntityProperties(BattleEntity entity){
		this.entity = entity;
		AttackSpeed = 5f;
	}

	public void BeAttack(int hurt,BattleEntity attackEntity){
		if (!IsDead) {
			CurrentHp -= hurt;
			if(IsDead){
				entity.IsDead = true;
			}
		}
	}
}
