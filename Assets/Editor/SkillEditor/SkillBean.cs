using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillBean : ScriptableObject {
	public uint enityId;
	public SKillType skillType;
	public AnimationClip preAnimation;
	public AnimationClip attackAnimation;
	public bool isMovement = false; //是否冲锋

}

[System.Serializable]
public enum SKillType{
	None = -1,
	SingleSkill = 0, //单技能不冲锋
	SingleSkillMovement = 1,
	DoubleSKill = 2,
	DoubleSkillMovement = 3,
	Count,
}