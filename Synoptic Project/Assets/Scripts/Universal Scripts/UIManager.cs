using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager: MonoBehaviour
{
    [SerializeField] InputManager manager;

    [SerializeField] GameObject BuildingDispayMenu, constructBuldingDisplay, sideMenu, buildMenu, wealthMenu, populationMenu;

    [SerializeField] TMP_Text buildingName, buildingStorage, buildingEmployees, wealthtext, moneyText, populationText, empPopsText;

    public static UIManager Instance;

    private BuildingAttachable curDisplay;

    public UIState curState = UIState.Normal;

    GameObject buildingBuilding = null;


    public enum UIState 
    { 
        Normal,
        Supplychain
    }
    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (curState == UIState.Normal && curDisplay != null)
        {
            DisplayBuilding(curDisplay);
        }

        moneyText.text = "Money: " + PlayerStats.money;
        populationText.text = "Population: " + PlayerStats.civ + "/" + PlayerStats.totCiv;
        empPopsText.text = "Employed Population: " + PlayerStats.empCiv + "/" + PlayerStats.civ;
    }
    public void BuildButton(int buildingNum) 
    {
        if (buildingBuilding != null && BuildingGrid.instance.buildings.Count > 0) 
        {
            Destroy(buildingBuilding);
        }

        buildingBuilding = manager.BuildButton(buildingNum);
    }

    public void CancelButton() 
    {
        manager.ChangeNoneState();
    }

    public void ConfirmButton() 
    {
        manager.ChangeNoneState();
    }

    public void DisplayBuilding(BuildingAttachable building) 
    {
        DisplaySideMenu();


        if (building.buildingType.isHouse) 
        { 
            
        }
        if (building.CanDisplay())
        {
            BuildingDispayMenu.SetActive(true);
            curDisplay = building;

            buildingName.text = building.buildingType.buildingName;

            buildingStorage.text = "Input Storage: ";

            foreach (Resource r in building.buildingType.inputs)
            {

                int val = 0;
                if (building.resourceCount.ContainsKey(r))
                {
                    val = building.resourceCount[r];
                }

                buildingStorage.text += "\n" + r + ": " + val;
            }

            buildingStorage.text += "\n\nOutput Storage: ";

            foreach (Resource r in building.buildingType.outputs)
            {

                int val = 0;
                if (building.resourceCount.ContainsKey(r))
                {
                    val = building.resourceCount[r];
                }

                buildingStorage.text += "\n" + r + ": " + val;
            }

            buildingStorage.text += "\n\n Taking From:";

            foreach (BuildingAttachable b in building.prevBuildings) 
            {
                buildingStorage.text += "\n" + b.buildingType.buildingName;
            }

            buildingEmployees.text = "Employees: " + building.curEmployees + "/" + building.buildingType.totalEmployees;
        }
        else
        {
            constructBuldingDisplay.SetActive(true);
        }
    }

    public void IncreaseEmployee() 
    {
        if (PlayerStats.civ > 0)
        {
            curDisplay.employeeEnter();
        }
    }

    public void DecreaseEmployee() 
    {
        curDisplay.EmployeeLeave();
    }

    public void SupplyChainMenu() 
    {
        DeactivateAllUI();
        curDisplay = null;
        curState = UIState.Supplychain;
    }

    public void DoSupplyChain(BuildingAttachable building) 
    {
        if (curDisplay == null)
        {
            curDisplay = building;
        }
        else 
        {
            Debug.Log("Added Building");
            building.prevBuildings.Add(curDisplay);
            curDisplay = null;
            curState = UIState.Normal;
        }
    }

    public void BuildMenu() {
        curDisplay = null;
        DisplaySideMenu();
        buildMenu.SetActive(true);
    }
    public void PopulationMenu() 
    { 
        DisplaySideMenu();
        curDisplay = null;
        populationMenu.SetActive(false);
    }
    public void WealthMenu() 
    { 
        DisplaySideMenu();
        curDisplay = null;
        wealthMenu.SetActive(true);

        wealthtext.text = "Money: " + PlayerStats.money + "\nColony Value: " + PlayerStats.wealth;
    }
    public void BuildingsMenu() { DisplaySideMenu(); }
    public void TechnologyMenu() { DisplaySideMenu(); }

    private void DisplaySideMenu() 
    {
        DeactivateAllUI();
        sideMenu.SetActive(true);
    }

    public void DeactivateAllUI() 
    {
        if (buildingBuilding != null && BuildingGrid.instance.buildings.Count > 0) 
        {
            Destroy(buildingBuilding);
        }
        BuildingDispayMenu.SetActive(false);
        constructBuldingDisplay.SetActive(false);
        buildMenu.SetActive(false);
        wealthMenu.SetActive(false);
        populationMenu.SetActive(false);
        sideMenu.SetActive(false);
    }

    public void DeleteBuilding() 
    {
        StartCoroutine(curDisplay.DestroyBuilding());
    }

    public void StopDestroy() 
    {
        buildingBuilding = null;
    }
}
