using UnityEngine;
using System.Collections;
//技能动作播放控制器
public class PraseSkillBean {
	PraseAnimation animationPlay ;
	GameObject roleObj;
	SkillBean bean;
	ActionStatus actionStatus = ActionStatus.Idle;

	bool bInit = false;
	bool bPlayedPre = false; //是否播放完准备动作

	public PraseSkillBean(){
		//TODO 添加事件
		animationPlay = new PraseAnimation();

	}

	public void Init(GameObject role,SkillBean bean,ActionStatus status){
		this.roleObj = role;
		this.bean = bean;
		animationPlay.Init(bean,roleObj);
		bInit = true;
		bPlayedPre = false;
		ChangeStatus (status);
	}

	public void Init(GameObject role,SkillBean bean,Vector3 attackTargetPos,ActionStatus status){
		this.roleObj = role;
		this.bean = bean;
		animationPlay.Init(bean,roleObj,attackTargetPos);
		bInit = true;
		bPlayedPre = false;
		ChangeStatus (status);
	}

	public void ChangeStatus( ActionStatus status ){
		actionStatus = status;
		switch(actionStatus){
			case ActionStatus.Idle:
				
				break;
			case ActionStatus.Play:
				animationPlay.Start();
				break;
			case ActionStatus.Stop:
				animationPlay.Stop();
				break;
			case ActionStatus.Pause:
				animationPlay.Pause();
				break;
			case ActionStatus.Resume:
				animationPlay.Resume();
				break;
		}
	}

	public void Update(float realtimeSinceStartup){
		if (!bInit || actionStatus != ActionStatus.Play)
			return;
		//1 准备动作
		if( animationPlay.havePreAction && !bPlayedPre ){
			if( !roleObj.animation.IsPlaying(bean.preAnimation.name) ){
				Debug.Log("actionPreTime end");
				bPlayedPre = true;
				//1-1判断是否冲锋 是播放冲锋 否播放攻击
				animationPlay.PlayAttack();
			}
			else {
				return;
			}
		}
		//2 是否冲锋 移动

		//3攻击动作
		if( bean.attackAnimation != null && !roleObj.animation.IsPlaying(bean.attackAnimation.name) ){
			Debug.Log("attackAnimation end");
			actionStatus = ActionStatus.Idle;
		}
	}
}

public enum ActionStatus{
	Idle,
	Play,
	Stop,
	Pause,
	Resume,
}