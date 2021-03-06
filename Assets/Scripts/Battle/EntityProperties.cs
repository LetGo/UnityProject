﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityProperties{
	public int EntityID;
	public string Name;
	public List<int> SkillList = new List<int>();

	public int Attack;
	public int Deffend;
	public int RunSpeed;
	public float AttackSpeed;
	public int Hp;	
	public int CurrentHp;
    public uint Pos;
	public bool IsEnemy;
	public uint HitRate = 0;
	public uint MisRate = 0;
	public bool IsDead{get {return CurrentHp <= 0;}}
	public uint Block = 0;
	BattleEntity entity;
	public uint Level = 0;

	public EntityProperties(BattleEntity entity,HeroDada herodata){
		this.entity = entity;
        Hp = herodata.HP;
        AttackSpeed = herodata.AttackSpeed;
        Attack = herodata.Attack;
        Pos = herodata.pos;
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
