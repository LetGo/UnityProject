using UnityEngine;
using System.Collections;

public class BattleEntity  
{
	public GameObject entityGo;
	public EntityComponent entityComponent;
	public EntityBattleMgr entityBattleMgr;
	public EntityProperties entityProperties;

	public bool IsDead{ get; set;}

	EntityAnimStatus animStatus = EntityAnimStatus.None;
	float currentTime = 0;

	public BattleEntity(){
		IsDead = false;
		animStatus = EntityAnimStatus.None;
		currentTime = 0;
		entityBattleMgr = new EntityBattleMgr (this);
		entityProperties = new EntityProperties (this);
	}

	public void Destroy(){
		GameObject.Destroy (entityGo);
		entityBattleMgr = null;
		entityProperties = null;
	}

	public void Update(float deltaTime){
		if (!IsDead) {
			UpdateAttackProgress(deltaTime);
		}
	}

	void UpdateAttackProgress(float deltaTime){
		currentTime += deltaTime;
		if(currentTime >= entityProperties.AttackSpeed){
			currentTime = 0;
			entityBattleMgr.AddAttackRound();
		}

		entityBattleMgr.skillBeanPlayer.Update (Time.realtimeSinceStartup);	
	}

	public void ChangeAnimStatus(EntityAnimStatus status){
		animStatus = status;
		switch (animStatus) {
		case EntityAnimStatus.Idel:
			entityGo.animation["idle"].wrapMode = WrapMode.Loop;
			entityGo.animation.Play("idle");
			break;
		case EntityAnimStatus.Move:
			entityGo.animation["run"].wrapMode = WrapMode.Loop;
			entityGo.animation.Play("run");
			break;
		case EntityAnimStatus.Dead:
			break;
		}
	}
}
