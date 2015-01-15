using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
    private GridNode[,] grids;
    public Vector2 gridSize;

    public Transform Plane;
    private Transform mPlaneTrans;
    public float NodeRadius;
    private float nodeDiameter;

    public LayerMask UnwalkLayer;


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

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Plane.position, new Vector3(gridSize.x, 1, gridSize.y));

        if (grids == null)
        {
            return;
        }

        foreach (var node in grids)
        {
            if (node != null)
            {
                Gizmos.color = node.CanWalk ? Color.white : Color.red;
                Gizmos.DrawCube(node.WordPosition, Vector3.one * (nodeDiameter - 0.1F));
            }
        }


    }
}
