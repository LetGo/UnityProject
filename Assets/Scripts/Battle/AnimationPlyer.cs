using UnityEngine;
using System.Collections;

/// <summary>
/// 动作播放器
/// </summary>
public class AnimationPlyer
{
	public SkillBean skillBean { get; set; }
	public GameObject roleObj { get; set; }
	public Animation roleAnimation { get; set; }
	public Vector3 attackTargetPos { get; set; }
	public SkillBeanPlayer parent;

	protected string currentAnimationClip = string.Empty;
	public bool havePreAction = false;

	Vector3 roleLoacalPos;

	public void Init(SkillBean bean, GameObject role)
	{
	    skillBean = bean;
	    roleObj = role;
	    roleAnimation = role.animation;
			roleLoacalPos = role.transform.localPosition;
	    Stop();
	}

	public void Init(SkillBean bean, GameObject role, Vector3 attackTargetPos)
	{
	    skillBean = bean;
	    roleObj = role;
	    roleAnimation = role.animation;
	    this.attackTargetPos = attackTargetPos;
		roleLoacalPos = role.transform.localPosition;
	    Stop();
	}

	public virtual void Start()
	{
	    if (skillBean.skillType == SKillType.DoubleSKill || skillBean.skillType == SKillType.DoubleSkillMovement)
	    {
	        havePreAction = true;
	        PlayPreAnim();
	    }
	    else
	    {
	        havePreAction = false;
	        //判断是否冲锋 是播放冲锋 否播放攻击
	        if (skillBean.Movement)
	            MoveToTarget();
	        else
	            PlayAttack();
	    }
	}

	public virtual void Stop()
	{
	    roleAnimation.Stop();
	}

	public virtual void Pause()
	{
	    if (!string.IsNullOrEmpty(currentAnimationClip))
	    {
	        roleAnimation[currentAnimationClip].speed = 0;
	    }
	}

	public virtual void Resume()
	{
	    if (!string.IsNullOrEmpty(currentAnimationClip))
	    {
	        roleAnimation[currentAnimationClip].speed = 1;
	        roleAnimation.Play(currentAnimationClip);
	    }
	}

	public void PlayPreAnim()
	{
	    currentAnimationClip = skillBean.preAnimation.name;
	    roleAnimation.CrossFade(currentAnimationClip);
	}

	public void PlayAttack()
	{
	    currentAnimationClip = skillBean.attackAnimation.name;
        //如果用CrossFade 该动画最后一帧上的事件可能不会触发
	    roleAnimation.Play(currentAnimationClip);
	}

	public void MoveToTarget()
	{
	    roleAnimation.Stop();
	    parent.startMoveTime = Time.realtimeSinceStartup;
	    Debug.Log("move now :" + parent.startMoveTime);
	    MovementActionBean moveBean = skillBean.movementActionBeanList[0];
	    currentAnimationClip = moveBean.moveAnimationClip.name;
	    roleAnimation[currentAnimationClip].wrapMode = WrapMode.Loop;
	    roleAnimation.Play(currentAnimationClip);
	    roleAnimation.cullingType = AnimationCullingType.AlwaysAnimate;

	    roleObj.transform.LookAt(attackTargetPos);
	    Move(moveBean);
	}

	void Move(MovementActionBean moveBean)
	{
			Vector3 desPos = Vector3.zero; //目标位置

			Vector3 dir = Vector3.Normalize (attackTargetPos - roleObj.transform.position); //目标位置与自身的单位向量
			desPos = attackTargetPos - dir;

			roleObj.transform.position = desPos; //设置自身到目标点 用于计算目标点的相对位置
			desPos = roleObj.transform.localPosition; //目标点的相对位置
			roleObj.transform.localPosition = roleLoacalPos; //设置回原来的位置

			float time = moveBean.moveTime * (moveBean.endTime - moveBean.startTime);

			TweenPosition.Begin(roleObj,time, desPos);

	    Debug.Log("Move");
	}

	public void MoveBack(){
		roleObj.transform.LookAt(attackTargetPos);
		TweenPosition.Begin(roleObj,0.01f, roleLoacalPos);

		TweenPosition tp = TweenPosition.Begin (roleObj,0.01f,roleLoacalPos);
		tp.from = roleObj.transform.localPosition;
		tp.to = roleLoacalPos;
		tp.delay = 0.01f;
		tp.duration = 0.01f;
		tp.eventReceiver = roleObj;
		tp.callWhenFinished = "SetIdle";
	}
}
