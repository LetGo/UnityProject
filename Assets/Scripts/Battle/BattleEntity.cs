using UnityEngine;
using System.Collections;

public class BattleEntity  
{
	public GameObject entityGo;
	EntityAnimStatus animStatus = EntityAnimStatus.None;
	public EntityComponent entityComponent;
	public EntityBattleMgr entityBattleMgr;

	public BattleEntity(){
		entityBattleMgr = new EntityBattleMgr (this);
	}

	public void Destroy(){
		GameObject.Destroy (entityGo);
	}

	int frame = 0;
	public void Update(){
		if (++frame == 300) {
			if( animStatus == EntityAnimStatus.Idel && entityBattleMgr.CheckCanfire() ){
				frame = 0;
			}
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
