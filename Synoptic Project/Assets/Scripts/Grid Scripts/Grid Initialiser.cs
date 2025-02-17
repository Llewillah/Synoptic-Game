using UnityEngine;

public class GridInitialiser: MonoBehaviour
{
    Grid grid;

    [SerializeField] int gridX, gridY;
    [SerializeField] float cellSize;
    private void Start()
    {
        grid = new Grid(gridX, gridY, cellSize);
    }
}
