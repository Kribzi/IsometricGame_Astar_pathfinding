using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;

public class Pathfinding_Astar : MonoBehaviour
{
    //Variables
    Node[,] Nodes;

    Vector3 mapSize;

    bool[,,] walkableArea;


    //Lists
    List<GameObject> objList = new List<GameObject>();
    List<List<Node>> pathsToSet = new List<List<Node>>();
    List<string> gameObjectsPaths = new List<string>();



    private void Start()
    {
        //Initaliazation
        mapSize = GameObject.Find("/GameManager").gameObject.GetComponent<GenerateMap>().mapSize;
        //grid = GameObject.Find("/Map").gameObject.GetComponent<Grid>();
        walkableArea = GameObject.Find("/GameManager").gameObject.GetComponent<GenerateMap>().GetWalkableArea();


        
    }
   

    private void Update()
    {
        while (pathsToSet.Count > 0 )
        {
           


            //After the A_star has found a path, give the path to the requesting GameObject.
            List<Node> path = pathsToSet[0];
            string unitPath = gameObjectsPaths[0];


            if (path != null) {
                GameObject.Find(unitPath).GetComponent<AI>().path = path;

                pathsToSet.RemoveAt(0);
                gameObjectsPaths.RemoveAt(0);
            }
           
            

        }
    }

   
    public void findPath(string unitPath,Vector3Int start, Vector3Int goal)
    {
       
        Node current = new Node(start, null);
        Node a_goal = new Node(goal, null);

        

        current.hCost = calculateCost(current, a_goal);

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        List<Node> suggestions = new List<Node>();
        

        openList.Add(current);

      

        while (openList.Count > 0)
        {

            //Select the best Node and remove it from the open list.
            current = getBestNodeFromList(openList);



            //Goal check
            if (pathFound(current, a_goal))
            {
                
                
                List<Node> a_path = new List<Node>();
                //Reconstruct Path until reaching the starting node.
                while (current.parent != null)
                {
                    a_path.Add(current);
                    current = current.parent;
                }

                a_path.Reverse();
                pathsToSet.Add(a_path);
                gameObjectsPaths.Add(unitPath);
                openList.Clear();

                
                break;
            }

            openList.Remove(current);
            closedList.Add(current);

            
            suggestions = checkNodeNeighbours(current, a_goal, current.gCost);

            
            foreach (var node in suggestions)
            {
                

                if (isInList(node, closedList) == true)
                {
                    int i = indexInList(node, closedList);
                    if (node.gCost + node.hCost <= closedList[i].gCost + closedList[i].hCost)
                    {
                        
                        closedList.RemoveAt(i);
                        openList.Add(node);
                    }

                }



                else if (isInList(node, openList) == true)
                {
                    int i = indexInList(node, openList);
                    if (node.gCost + node.hCost <= openList[i].gCost + openList[i].hCost)
                    {
                        closedList.Add(openList[i]);
                        openList.RemoveAt(i);
                        openList.Add(node);
                    }

                }
                else
                {
                    openList.Add(node);


                }


            }

        }

        
    }



   Node getBestNodeFromList(List<Node> nodeList)
    {
        Node bestNode = nodeList[0];


        for (int i = 0; i < nodeList.Count; i++)
        {
            if (bestNode.gCost + bestNode.hCost >= nodeList[i].gCost + nodeList[i].hCost)
            {
                bestNode = nodeList[i];
            }


        }
        return bestNode;
    }


    bool pathFound(Node current, Node goal)
    {
        if (current.x == goal.x && current.y == goal.y) {
            return true;
        }
        return false;
    }


    int calculateCost(Node current, Node goal)
    {
        return (int)Mathf.Abs(current.x - goal.x) + (int)Mathf.Abs(current.y - goal.y);
        

    }


    bool isInList(Node a_node, List<Node> list)
    {
        foreach (Node node in list)
        {
            if (a_node.x == node.x && a_node.y == node.y)
            {
                return true;
            }
        }

        return false;
    }


    int indexInList(Node a_node, List<Node> list)
    {
        int count = 0;

        foreach (Node node in list)
        {

            if (a_node.x == node.x && a_node.y == node.y)
            {
                return count;
            }
            count++;
        }

        return -1;
    }






    public List<Node> checkNodeNeighbours(Node currentNode,Node goal, int gCost)
    {
       List<Node> nodeList = new List<Node>();

      

        //Top left
        if (currentNode.y > 0)
        {
            Node node = new Node(new Vector3Int(currentNode.x - 1, currentNode.y, currentNode.z), currentNode);
          
            if (walkableArea[node.x, node.y, 1] == true)
            {
                nodeList.Add(node);
            }


        }
        //Top right
        if (currentNode.y > 0)
        {
            Node node = new Node(new Vector3Int(currentNode.x, currentNode.y - 1, currentNode.z), currentNode);
          
            if (walkableArea[node.x, node.y, 1] == true)
            {
                nodeList.Add(node);
            }

        }
        //Down left
        if (currentNode.y < (int)mapSize.y)
        {
            
            Node node = new Node(new Vector3Int(currentNode.x, currentNode.y + 1, currentNode.z), currentNode);
           
            if (walkableArea[node.x, node.y, 1] == true)
            {
                nodeList.Add(node);
            }

        }

        //Down right
        if (currentNode.x < (int)mapSize.x)
        {
            Node node = new Node(new Vector3Int(currentNode.x + 1, currentNode.y, currentNode.z), currentNode);
            
            if (walkableArea[node.x, node.y, 1] == true)
            {
                nodeList.Add(node);
            }
           
           
        }

        
        if (nodeList.Count == 0){
            return new List<Node>(); ;
        }

        foreach (var node in nodeList)
        {
            node.gCost = gCost + 1;
            node.hCost = calculateCost(node, goal);
            node.parent = currentNode;
        }
        




        return nodeList;
    }




    public void printPath(List<Node> list)
    {
        
        foreach (Node node in list)
        {
            Debug.Log("[" + node.x +  "," + node.y + "," + node.z + "] =>");
        }
    }

   
}
