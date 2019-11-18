using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public int x, y,z;
    public bool isWalkable;

    public Node parent = null;

    public int hCost, gCost;



    public Node(Vector3Int vector, Node a_parent)
    {
        this.x = vector.x;
        this.y = vector.y;
        this.z = vector.z;

        this.isWalkable = true;
        this.parent = a_parent;
        this.hCost = 0;
        this.gCost = 0;
    }


   



}
