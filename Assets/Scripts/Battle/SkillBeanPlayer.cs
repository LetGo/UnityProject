using UnityEngine;
using System.Collections;

/// <summary>
/// 技能播放器
/// </summary>
public class SkillBeanPlayer
{
    public delegate void AttackDelegate();

	AnimationPlyer animationPlay;
	GameObject roleObj;
	SkillBean bean;
	ActionStatus actionStatus = ActionStatus.Idle;

	bool bInit = false;
	bool bPlayedPre = false;        //是否播放完准备动作
	bool bMovetEnd = false;         //是否移动完
	public float startMoveTime = 0;
	float startPlayTime = 0;
    AttackDelegate attackEndDelegate;
    AttackDelegate attackEventDelegate;

    public SkillBeanPlayer(AttackDelegate attackEndDelegate, AttackDelegate attackEventDelegate)
	{
	    //TODO 添加事件
	    animationPlay = new AnimationPlyer();
	    animationPlay.parent = this;
        this.attackEndDelegate = attackEndDelegate;
        this.attackEventDelegate = attackEventDelegate;
	}

	public void Init(GameObject role, SkillBean bean, ActionStatus status)
	{
	    this.roleObj = role;
	    this.bean = bean;
	    animationPlay.Init(bean, roleObj);
	    bInit = true;
	    bPlayedPre = false;
	    ChangeStatus(status);
	}

	public void Init(GameObject role, SkillBean bean, Vector3 attackTargetPos, ActionStatus status)
	{
	    this.roleObj = role;
	    this.bean = bean;
	    animationPlay.Init(bean, roleObj, attackTargetPos);
	    bInit = true;
	    bPlayedPre = false;
	    bMovetEnd = false;
	    ChangeStatus(status);
	}

	public bool IsPlaying(){
		return actionStatus == ActionStatus.Play;
	}

	public void ChangeStatus(ActionStatus status)
	{
	    switch (status)
	    {
	        case ActionStatus.Idle:

	            break;
	        case ActionStatus.Play:
                animationPlay.Start();
				//startPlayTime = Time.realtimeSinceStartup;
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
	    actionStatus = status;
	}

    bool bSet = false;
	public void Update(float realtimeSinceStartup)
	{
        if (!bInit || actionStatus != ActionStatus.Play)
        {
            return;
        }

        if(!bSet)
        {
            startPlayTime = realtimeSinceStartup;
            bSet = true;
        }
		//触发事件
		for (int i = 0; i <bean.attackEventBeanList.Count; ++i) {
			if( !bean.attackEventBeanList[i].bInvoke ){
				if(realtimeSinceStartup - startPlayTime >= bean.attackEventBeanList[i].startTime + bean.attackEventBeanList[i].delayTime){
					Debug.Log("attackEventBeanList invoke");
					bean.attackEventBeanList[i].bInvoke = true;
                    if (attackEventDelegate != null)
                    {
                        attackEventDelegate();
                    }
				}
			}			
		}

	    //1 准备动作
			if ( !IsPreActionOver (realtimeSinceStartup)) {
				return;		
			}
	    //2 是否冲锋 移动
			if (!IsMoveOver (realtimeSinceStartup)) {
				return;			
			}
	    //3攻击动作
	    CheckAttackOver ();


	}

	bool IsPreActionOver (float realtimeSinceStartup)
	{
		if (animationPlay.havePreAction && !bPlayedPre) {
			if (!roleObj.animation.IsPlaying (bean.preAnimation.name)) {
				Debug.Log ("actionPreTime end");
				bPlayedPre = true;
				//1-1判断是否冲锋 是播放冲锋 否播放攻击
				if (bean.Movement) {
					startMoveTime = realtimeSinceStartup;
					Debug.Log ("move now :" + realtimeSinceStartup);
					animationPlay.MoveToTarget ();
				}
				else {
					animationPlay.PlayAttack ();
				}
				return true;
			}
			else {
				return false;
			}
		}
		return true;
	}

	bool IsMoveOver (float realtimeSinceStartup)
	{
		if (bean.Movement && !bMovetEnd) {
			if (realtimeSinceStartup - startMoveTime >= bean.movementActionBeanList [0].moveTime) {
				Debug.Log ("Movement end :" + realtimeSinceStartup);
				bMovetEnd = true;
				animationPlay.PlayAttack ();
				return true;
			}
			else {
				return false;
			}
		}
		return true;
	}

	void CheckAttackOver ()
	{
        if (bean.attackAnimation != null && !roleObj.animation.IsPlaying(bean.attackAnimation.name))
        {
            Debug.Log("attackAnimation end " + bean.attackAnimation.name);
            actionStatus = ActionStatus.Idle;
            animationPlay.MoveBack();
            bSet = false;
            if (attackEndDelegate != null)
            {
                attackEndDelegate();
            }
        }
	}
}

public enum ActionStatus
{
	Idle,
	Play,
	Stop,
	Pause,
	Resume,
}

