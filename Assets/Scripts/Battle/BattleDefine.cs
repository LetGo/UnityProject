using UnityEngine;
using System.Collections;

public enum BattlePositionType{
	None = -1,
	Pos1,
	Pos2,
	Pos3,
	Pos4,
	Pos5,
	Pos6,
	Center,
	Count,
}

public enum BattleStatus{
	None,
	Preparing,
	Playing,
	Result, //结算
}

public enum EntityAnimStatus{
	None,
	Idel,
	Move,
	Dead,
	Win,
}