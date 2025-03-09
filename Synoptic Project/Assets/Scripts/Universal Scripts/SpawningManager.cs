using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawningManager: MonoBehaviour
{
    public static SpawningManager Instance { get; private set; }

    [SerializeField] GameObject tile;
    private void Start()
    {
        Instance = this;
    }

    public GameObject SpawnTile(Vector3 pos, float scale) 
    {
        GameObject newTile = Instantiate(tile, pos, Quaternion.identity);
        newTile.transform.localScale = new Vector3(scale, scale, scale);
        return newTile;
    }
}
