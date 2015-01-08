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
        if (entity.IsSelfTeam)
        {
            AttackSpeed = 5f;
            Hp = 100;
        }
        else
        {
            Hp = Random.Range(30,50);
            AttackSpeed = Random.Range(7.0f,10.0f);
        }
        CurrentHp = Hp;
	}

	public void BeAttack(int hurt){
		if (!IsDead) {
			CurrentHp -= hurt;
            Debug.LogError("BeAttack " + CurrentHp);
			if(IsDead){
				entity.Dead();
			}
		}
	}
}
