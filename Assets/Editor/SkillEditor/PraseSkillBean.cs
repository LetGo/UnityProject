using UnityEngine;
using System.Collections;
//技能动作播放器
public class PraseSkillBean {

	PraseAnimation[] praseAnimations;
	GameObject roleObj;
	SkillBean bean;
	public PraseSkillBean(){
		//TODO 添加事件
		praseAnimations = new PraseAnimation[(int)SKillType.Count];
		praseAnimations [(int)SKillType.SingleSkill] = new PraseSingleMovementAnim ();
	}

	public void Play(GameObject role,SkillBean bean){
		Stop ();
		this.roleObj = role;
		this.bean = bean;
		for (int i = 0; i<praseAnimations.Length; ++i) {
			praseAnimations[i].Play();
		}
	}

	public void Stop(){
		for (int i = 0; i<praseAnimations.Length; ++i) {
			praseAnimations[i].Stop();
		}
	}

	public void Pause(){
		for (int i = 0; i<praseAnimations.Length; ++i) {
			praseAnimations[i].Pause();
		}
	}

	public void Resume(){
		for (int i = 0; i<praseAnimations.Length; ++i) {
			praseAnimations[i].Resume();
		}
	}
}
