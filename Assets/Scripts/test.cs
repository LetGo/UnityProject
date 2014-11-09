using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class test : MonoBehaviour {

    public int a = 2;
    public int b = 2;
    public List<Vector3> m_lstVec3 = new List<Vector3>();
    public int index = 0;

    public Transform UISprite;
	// Use this for initialization
	void Awake () {
        Vector3 pos =Vector3.zero;
        float y = 0;
        float lsatY = 0;
        for (float i = -a; i < a; )
        {
            y = UnityEngine.Mathf.Sqrt( (1 - (i * i) / (float)(a * a) ) * (b * b) );
            pos.x = i;
            pos.z = 0;
            pos.y = y;

            if (y - lsatY > 0.1f)
            {
                pos.y = lsatY + 0.1f;
            }
             
            lsatY = pos.y;

            m_lstVec3.Add(pos);

            i = i + 0.1f;

        }

        for (int i = m_lstVec3.Count - 1; i >= 0; i--)
        {
            pos = m_lstVec3[i];
            pos.y = - m_lstVec3[i].y;
            m_lstVec3.Add(pos);
        }
	}
	
    
	void Update () {
        if (index >= m_lstVec3.Count)
	    {
            index = 0;
	    }
        UISprite.localPosition = m_lstVec3[index++];
	}
}
