using UnityEngine;
using System.Collections;

//excel 2.1.0.0 sysytem.data.dll 2.0.0.0
public abstract class UIPanelBase : MonoBehaviour {

	public abstract void Init();

	public abstract void UnInit();

	public abstract bool OnMsg(UIMsgID id);
}


