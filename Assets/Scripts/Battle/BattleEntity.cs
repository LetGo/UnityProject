using UnityEngine;
using System.Collections;

public class BattleEntity  
{
	public GameObject entityGo;
	EntityAnimStatus animStatus = EntityAnimStatus.None;
	public EntityComponent entityComponent;

	public void Destroy(){
		GameObject.Destroy (entityGo);
	}

	public void Update(){

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
