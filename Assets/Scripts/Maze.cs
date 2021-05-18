using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public int mazeWidth;
    public int mazeHeight;
    public MazeCell cellPrefab;
    public MazeWall wallPrefab;
    private List<Coordinate> cells;
    private List<Coordinate> walls;

    public void Initialize()
    {
        cells = new List<Coordinate>();
        walls = new List<Coordinate>();

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z <= mazeHeight; z++)
            {
                if (z != mazeHeight)
                {
                    cells.Add(new Coordinate(x, z));
                    CreateCell(x, z);
                }

                CreateWall(x, z, Orientation.horizontal);
                CreateWall(z, x, Orientation.vertical);
            }
        }

        StartCoroutine(CarvePassage());
    }

    private IEnumerator CarvePassage()
    {
        Coordinate currentCell = new Coordinate(0, 0);
        int visitedCells = 1;
        int totalCells = mazeWidth * mazeHeight;
        Stack<Coordinate> visitedStack = new Stack<Coordinate>();

        while (visitedCells <= totalCells)
        {
            List<Coordinate> neighbours = FindUnvisitedNeighbours(currentCell);

            if (neighbours.Count > 0)
            {
                Coordinate chosenNeighbour = neighbours[Random.Range(0, neighbours.Count)];

                RemoveWall(currentCell, chosenNeighbour);

                visitedStack.Push(currentCell);
                currentCell = chosenNeighbour;
                visitedCells++;

                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                currentCell = visitedStack.Pop();
            }
        }
    }

    private List<Coordinate> FindUnvisitedNeighbours(Coordinate currentCell)
    {
        List<Coordinate> neighbours = new List<Coordinate>();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x != 0 && z != 0)
                {
                    if (currentCell.x + x >= 0 && currentCell.z + z >= 0 && currentCell.x + x < mazeWidth && currentCell.z + z < mazeHeight)
                    {
                        if (cells.Find(c => c.x == currentCell.x + x && c.z == currentCell.z).visited == false)
                        {
                            neighbours.Add(cells.Find(c => c.x == currentCell.x + x && c.z == currentCell.z));
                        }

                        if (cells.Find(c => c.x == currentCell.x && c.z == currentCell.z + z).visited == false)
                        {
                            neighbours.Add(cells.Find(c => c.x == currentCell.x && c.z == currentCell.z + z));
                        }
                    }
                }
            }
        }

        return neighbours;
    }

    private void RemoveWall(Coordinate currentCell, Coordinate chosenNeighbour)
    {
        if (chosenNeighbour.x - currentCell.x == 1 && chosenNeighbour.z == currentCell.z)
        {
            Destroy(GameObject.Find("Wall " + chosenNeighbour.x + ", " + chosenNeighbour.z + " (vertical)"));
        }

        if (chosenNeighbour.x - currentCell.x == -1 && chosenNeighbour.z == currentCell.z)
        {
            Destroy(GameObject.Find("Wall " + currentCell.x + ", " + currentCell.z + " (vertical)"));
        }

        if (chosenNeighbour.z - currentCell.z == 1 && chosenNeighbour.x == currentCell.x)
        {
            Destroy(GameObject.Find("Wall " + chosenNeighbour.x + ", " + chosenNeighbour.z + " (horizontal)"));
        }

        if (chosenNeighbour.z - currentCell.z == -1 && chosenNeighbour.x == currentCell.x)
        {
            Destroy(GameObject.Find("Wall " + currentCell.x + ", " + currentCell.z + " (horizontal)"));
        }

        currentCell.visited = true;
        chosenNeighbour.visited = true;
    }

    private void CreateCell(int x, int z)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;

        newCell.name = "Cell " + x + ", " + z;
        newCell.transform.parent = transform.Find("Cell");
        newCell.transform.localPosition = new Vector3(x, 0f, z);
    }

    private void CreateWall(int x, int z, Orientation orientation)
    {
        MazeWall newWall = Instantiate(wallPrefab) as MazeWall;
        newWall.transform.parent = transform.Find("Walls");

        if (orientation == Orientation.horizontal)
        {
            newWall.name = "Wall " + x + ", " + z + " (horizontal)";
            newWall.transform.Rotate(0, 90, 0);
            newWall.transform.localPosition = new Vector3(x, 0.2f, z - 0.45f);
        }
        else
        {
            newWall.name = "Wall " + x + ", " + z + " (vertical)";
            newWall.transform.localPosition = new Vector3(x - 0.45f, 0.2f, z);
        }
    }
}
