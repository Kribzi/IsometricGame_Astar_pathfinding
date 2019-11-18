# IsometricGame_Astar_pathfinding
A* path finding algorithm for a Isometric Game in Unity Engine. Made for a game that is work in progress.

The GameObjects in the Unity project are starting a new Thread which executes findPath() function. Then the A* algorithm goes to work on a seperate thread and saves the result into a list which the main thread then handles to assign the paths to the GameObject.

# Extracted from the original project
The C# code in this repository has been copied from the original project to a seperate folder for upload to github just for show, this means that you cannot just take this C# code and put it in your project. It will not work :)
