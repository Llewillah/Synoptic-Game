using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingAttachable : MonoBehaviour
{
    [SerializeField] SpriteRenderer fade;
    public int ID;
    public List<BuildingAttachable> prevBuildings = new List<BuildingAttachable>();
    List<LineRenderer> connections = new List<LineRenderer>();
    [SerializeField] TMP_Text buildNameText; 

    bool takingResource = false, working = false;
    enum buildingState
    {
        PreCon,
        Construct,
        Sleep,
        Work
    }

    public Dictionary<Resource, int> resourceCount = new Dictionary<Resource, int>();

    public Building buildingType;
    float buildTimer = 0;
    public int curEmployees = 0;

    buildingState curState = buildingState.PreCon;
    void Start()
    {
        foreach (Resource r in buildingType.inputs) 
        {
            resourceCount.Add(r, 0);
        }

        foreach (Resource r in buildingType.outputs)
        {
            resourceCount.Add(r, 0);
        }

        buildNameText.text = buildingType.buildingName;
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
    }

    void CheckState()
    {
        switch (curState)
        {
            case buildingState.PreCon:
                PreCon();
                break;
            case buildingState.Construct:
                Construct();
                break;
            case buildingState.Sleep:
                if (!buildingType.warehouse && !buildingType.isHouse)
                {
                    Sleep();
                }
                else if (!buildingType.isHouse && buildingType.warehouse && curEmployees != 0)
                {
                    WarehouseCode();
                }
                break;
            case buildingState.Work:
                if (!buildingType.warehouse)
                {
                    if (working == false)
                    {
                        StartCoroutine(Work());
                    }
                    CheckAllResources();
                }
                break;
        }
    }

    void PreCon()
    {
        BuildingGrid.instance.GetGrid().GetGridPos(Camera.main.ScreenToWorldPoint(Input.mousePosition), out int x, out int y);
        transform.position = BuildingGrid.instance.GetGrid().GetWorldPos(x, y);

        if (Input.GetMouseButtonDown(0) && PlayerStats.money >= buildingType.buildCost && BuildingGrid.instance.CheckAdjacent(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && BuildingGrid.instance.GetGrid().GetNode(x,y).buildable)
        {
            PlayerStats.money -= buildingType.buildCost;
            BuildingGrid.instance.CompleteBuilding(gameObject);
            curState = buildingState.Construct;
            UIManager.Instance.StopDestroy();
        }

        if (Input.GetMouseButtonDown(1)) 
        {
            voidDestroyBuilding();
        }
    }

    void Construct()
    {
        buildTimer += Time.deltaTime;
        float fadeAmount = (1f - (buildTimer / buildingType.buildTime));
        fade.color = new Color(0.1f, 0.1f, 0.1f, fadeAmount);

        if (buildTimer >= buildingType.buildTime)
        {
            Debug.Log("BUILT");
            fade.color = new Color(29f, 29f, 29f, 0f);
            curState = buildingState.Sleep;
            buildNameText.color = new Color(0, 0, 0);
            PlayerStats.wealth += buildingType.buildCost;
            if (buildingType.isHouse) 
            {
                PlayerStats.totCiv += 4;
            }
        }
    }

    void Sleep()
    {
        bool canBuild = true;
        if (curEmployees != 0)
        {
            for (int i = 0; i < buildingType.inputs.Count(); i++)
            {
                if (resourceCount.ContainsKey(buildingType.inputs[i]))
                {
                    if (buildingType.inputsCost[i] > resourceCount[buildingType.inputs[i]])
                    {
                        canBuild = false;
                        GetResources(buildingType.inputs[i]);
                    }
                }
                else
                {
                    canBuild = false;
                    GetResources(buildingType.inputs[i]);
                }
            }

            if (canBuild)
            {
                curState = buildingState.Work;
                for (int i = 0; i < buildingType.inputs.Count(); i++)
                {
                    resourceCount[buildingType.inputs[i]] -= buildingType.inputsCost[i];
                }
            }
        }
    }

    IEnumerator Work()
    {
        working = true;
        yield return new WaitForSeconds(buildingType.workTime/curEmployees);
        foreach (Resource re in buildingType.outputs)
        {
            OutputResource(re);
        }
        curState = buildingState.Sleep;
        working = false;
    }

    private void CheckAllResources() 
    {
        foreach (Resource r in buildingType.inputs) 
        {
            GetResources(r);
        }
    }
    private void GetResources(Resource check) 
    {
        foreach (BuildingAttachable b in prevBuildings) 
        {
            if (!takingResource)
            {
                StartCoroutine(TakeResource(check, b));
            }
        }
    }

    public void ReduceResource(Resource resource)
    {
            resourceCount[resource]--;
    }

    void OutputResource(Resource resource)
    {
        if (!resourceCount.ContainsKey(resource)) 
        { 
            resourceCount.Add(resource, 0);
        }
        resourceCount[resource] += buildingType.outputCount;
    }

    public void employeeEnter()
    {
        if (curEmployees + 1 <= buildingType.totalEmployees && PlayerStats.empCiv < PlayerStats.civ)
        {
            PlayerStats.empCiv++;
            curEmployees++;
        }
    }

    public void EmployeeLeave()
    {
        if (curEmployees > 0)
        {
            curEmployees--;
            PlayerStats.empCiv--;
        }
    }

    public void SetBuildingType(Building buildType)
    {
        buildingType = buildType;

    }

    IEnumerator TakeResource(Resource resource, BuildingAttachable prevBuilding) 
    {
        if (prevBuilding.resourceCount.ContainsKey(resource) && prevBuilding.resourceCount[resource] > 0) 
        {
            takingResource = true;
            prevBuilding.ReduceResource(resource);
            yield return new WaitForSeconds(3/curEmployees);
            takingResource = false;
            if (!resourceCount.ContainsKey(resource)) 
            {
                resourceCount.Add(resource, 0);
            }

            if (!buildingType.isTrader)
            {
                resourceCount[resource]++;
            }
            else 
            {
                PlayerStats.money += resource.value;
            }
        }
    }

    private void WarehouseCode() 
    {
        foreach (BuildingAttachable b in prevBuildings) 
        {
            foreach (Resource r in b.buildingType.outputs) 
            {
                if (!takingResource) 
                {
                    StartCoroutine(TakeResource(r, b));
                }
            }
        }
    }

    private void OnMouseOver()
    {
        if (UIManager.Instance.curState != UIManager.UIState.Supplychain)
        {
            if (Input.GetMouseButtonDown(0) && curState != buildingState.PreCon)
            {
                UIManager.Instance.DisplayBuilding(this);
            }
        }
        else 
        {
            if (Input.GetMouseButtonDown(0) && curState != buildingState.PreCon && curState != buildingState.Construct && !buildingType.isHouse)
            {
                UIManager.Instance.DoSupplyChain(this);
            }
        }
    }

    public void RemoveConnection(int index) 
    { 
        connections.RemoveAt(index);
        prevBuildings.RemoveAt(index);
    }

    public bool CanDisplay() 
    {
        if (curState == buildingState.Sleep || curState == buildingState.Work) 
        {
            return true;
        }

        return false;
    }

    public IEnumerator DestroyBuilding() 
    {
        BuildingGrid.instance.RemoveBuilding(ID);
        yield return null;
        Destroy(gameObject);
    }

    public void voidDestroyBuilding() 
    {
        Destroy(gameObject);
    }

}
