﻿using UnityEngine;
using System.Collections;
//动作播放器
public class PraseAnimation  {

	public SkillBean skillBean{ get; set;}
	public GameObject roleObj{ get; set;}
	public Animation roleAnimation{ get; set;}
	public Vector3 attackTargetPos{ get; set;}

	protected string currentAnimationClip = string.Empty;
	public bool havePreAction = false;

	public void Init(SkillBean bean,GameObject role){
		skillBean = bean;
		roleObj = role;
		roleAnimation = role.animation;
		Stop();
	}

	public void Init(SkillBean bean,GameObject role,Vector3 attackTargetPos){
		skillBean = bean;
		roleObj = role;
		roleAnimation = role.animation;
		this.attackTargetPos = attackTargetPos;
		Stop();
	}

	public virtual void Start(){
		if(skillBean.skillType == SKillType.DoubleSKill || skillBean.skillType == SKillType.DoubleSkillMovement){
			havePreAction = true;
			PlayPreAnim();
		}
		else{
			havePreAction = false;
			//判断是否冲锋 是播放冲锋 否播放攻击
			PlayAttack();
		}
	}
	
	public virtual void Stop(){
		roleAnimation.Stop ();
	}
	
	public virtual void Pause(){
		if (!string.IsNullOrEmpty (currentAnimationClip)) {
			roleAnimation[currentAnimationClip].speed = 0;
		}
	}
	
	public virtual void Resume(){
		if (!string.IsNullOrEmpty (currentAnimationClip)) {
			roleAnimation[currentAnimationClip].speed = 1;
			roleAnimation.Play(currentAnimationClip);
		}
	}

	public void PlayPreAnim(){
		currentAnimationClip = skillBean.preAnimation.name;
		roleAnimation.CrossFade (currentAnimationClip);
	}

	public void PlayAttack(){
		currentAnimationClip = skillBean.attackAnimation.name;
		roleAnimation.CrossFade (currentAnimationClip);
	}

	public void MoveToTarget(){
		
	}
}
