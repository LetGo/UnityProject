using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {
    private GridNode[,] grids;
    public Vector2 gridSize;
    public Transform player;
    public Transform target;
    public FindPath findPath;
    public Transform Plane;
    private Transform mPlaneTrans;
    public float NodeRadius;
    private float nodeDiameter;

    public LayerMask UnwalkLayer;

    public List<GridNode> pathList = new List<GridNode>();
    public int gridCntX;
    public int gridCntY;

    void Awake()
    {
        if (Plane != null)
        {
            mPlaneTrans = Plane;
        }
        else
        {
            mPlaneTrans = transform;
        }
    }
   
    void Start()
    {
        nodeDiameter = NodeRadius * 2;
        gridCntX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridCntY = Mathf.RoundToInt(gridSize.y / nodeDiameter);

        grids = new GridNode[gridCntX, gridCntY];

        CreateGrid();
    }

    void Update()
    {
        findPath.FindingPath(player.transform.position, target.position);
    }

    private void CreateGrid()
    {
        Vector3 startPos = mPlaneTrans.position - gridSize.x * 0.5f * Vector3.right 
                - Vector3.forward * gridSize.y * 0.5f;
        for (int i = 0; i < gridCntX; ++i )
        {
            for (int j = 0; j < gridCntY; ++j)
            {
                Vector3 wordPos = startPos + (i * nodeDiameter + NodeRadius)* Vector3.right +
                     (j * nodeDiameter + NodeRadius) * Vector3.forward;
                //发射圆形射线
                bool walkable = !Physics.CheckSphere(wordPos, NodeRadius,UnwalkLayer);

                grids[i, j] = new GridNode(walkable,wordPos,i,j);
            }
       }
        Debug.Log("node " + grids.Length);
    }

    bool flag = false;
    public GridNode GetGridNodeByPos(Vector3 pos)
    {
        float percenx = (pos.x + gridSize.x * 0.5f) / gridSize.x;
        float perceny = (pos.z + gridSize.y * 0.5f) / gridSize.y;

        percenx = Mathf.Clamp01(percenx);
        perceny = Mathf.Clamp01(perceny);

        int x = Mathf.RoundToInt((gridCntX - 1) * percenx);
        int y = Mathf.RoundToInt((gridCntY - 1) * perceny);

        return grids[x,y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Plane.position, new Vector3(gridSize.x, 1, gridSize.y));

        if (grids == null)
        {
            return;
        }

        GridNode playerNode = GetGridNodeByPos(player.position);

        foreach (var node in grids)
        {
            if (node != null)
            {
                Gizmos.color = node.CanWalk ? Color.white : Color.red;
                Gizmos.DrawCube(node.WordPosition, Vector3.one * (nodeDiameter - 0.1F));
            }
        }

        if (playerNode != null && playerNode.CanWalk)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(playerNode.WordPosition, Vector3.one * (nodeDiameter - 0.1F));
        }

        if (pathList != null)
        {
            
            foreach (var node in pathList)
            {
                if (node != null)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(node.WordPosition, Vector3.one * (nodeDiameter - 0.1F));
                    if (!flag)
                        Debug.Log(node.ToString());
                }
            }
            flag = true;
        }
    }

    public List<GridNode> GetNeibourHood(GridNode a)
    {
        List<GridNode> nodes = new List<GridNode>();
        for (int i = -1; i <= 1; ++i)
        {
            for (int j = -1; j <= 1; ++j)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                int tempx = a.GridX + i;
                int tempy = a.GridY + j;
                if (tempx >= 0 && tempx < gridCntX && tempy >= 0 && tempy < gridCntY)
                {
                    nodes.Add(grids[tempx, tempy]);
                }
            }
        }
        return nodes;
    }
}
