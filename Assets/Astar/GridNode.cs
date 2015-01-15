using UnityEngine;
using System.Collections;
/// <summary>
/// 格子
/// </summary>
public class GridNode
{
    public bool CanWalk;
    public Vector3 WordPosition;
    public int GridX;
    public int GridY;

    public uint Gcost;
    public uint Hcost;

    public uint Fcost
    {
        get { return Gcost + Fcost; }
    }

    public GridNode Parent;

    public GridNode(bool bwalkable,Vector3 wpos,int x,int y)
    {
        CanWalk = bwalkable;
        WordPosition = wpos;
        this.GridX = x;
        this.GridY = y; 
    }



}
