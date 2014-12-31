using UnityEngine;
using System.Collections;
//4种动作基类
public class PraseAnimation  {

	public SkillBean skillBean{ get; set;}
	public GameObject roleObj{ get; set;}
	Animation roleAnimation{ get; set;}
	
	protected string currentAnimationClip = string.Empty;
	
	public virtual void Play(){
		
	}
	
	public virtual void Stop(){
		
	}
	
	public virtual void Pause(){
		
	}
	
	public virtual void Resume(){
		
	}

}
