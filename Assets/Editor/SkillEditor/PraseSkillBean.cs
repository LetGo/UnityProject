using UnityEngine;
using System.Collections;

public class PraseSkillBean {

	PraseAnimation[] praseAnimations;

	public PraseSkillBean(){
		praseAnimations = new PraseAnimation[SKillType.Count];
		praseAnimations [(int)SKillType.SingleMovement] = new PraseSingleMovementAnim ();
	}

	public void Play(GameObject role,SkillBean bean){

	}
}
