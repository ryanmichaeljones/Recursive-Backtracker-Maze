using UnityEngine;

public class Manager : MonoBehaviour
{
    public Maze mazePrefab;
    private Maze mazeInstance;

    void Start()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        mazeInstance.Initialize();
    }
}
