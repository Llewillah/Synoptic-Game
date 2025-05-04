using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawningManager : MonoBehaviour
{
    public static SpawningManager Instance { get; private set; }
    int totalBuildings = 0;

    [SerializeField] Building[] buildings;
    [SerializeField] GameObject tile, buildingAttach;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
       
    }

    public GameObject SpawnTile(Vector3 pos, float scale) 
    {
        GameObject newTile = Instantiate(tile, new Vector3(pos.x, pos.y, 1), Quaternion.identity);
        newTile.transform.localScale = new Vector3(scale, scale, scale);
        return newTile;
    }

    public GameObject SpawnBuilding(Vector3 pos, float scale, int buildingNum) 
    {
        GameObject newBuilding = Instantiate(buildingAttach, pos, Quaternion.identity);
        newBuilding.GetComponent<BuildingAttachable>().SetBuildingType(buildings[buildingNum]);
        newBuilding.GetComponent<BuildingAttachable>().ID = totalBuildings;
        totalBuildings++;
        newBuilding.transform.localScale = new Vector3(scale, scale, scale);
        return newBuilding;
    }
}