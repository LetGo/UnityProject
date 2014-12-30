using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillBean : ScriptableObject {
	public uint enityId;
	public SKillType skillType;
	public AnimationClip preAnimation;
	public AnimationClip animation;

}

[System.Serializable]
public enum SKillType{
	None = -1,
	SingleMovement, //单技能不冲锋
	SingleMovementMove,
	Count,
}