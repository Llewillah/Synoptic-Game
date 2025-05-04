using Unity.VisualScripting;
using UnityEngine;

public class InputManager: MonoBehaviour
{
    enum inputState 
    { 
        None,
        Build
    }
    Vector3 startPos;
    inputState curState = inputState.None;
    private void Update()
    {
        CheckState();
    }
    public static Vector3 GetMousePos() 
    {
        return Input.mousePosition;
    }

    private void Build() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }


    private void CheckState() 
    {
        switch (curState) 
        {
            case inputState.None:
                break;
            case inputState.Build:
                Build();
                break;
        }
    }

    public void ChangeBuildState() 
    { 
        curState = inputState.Build;
    }

    public void ChangeNoneState()
    {
        curState = inputState.None;
    }

    public GameObject BuildButton(int buildingNum) 
    {
        return BuildingGrid.instance.BuildBuilding(buildingNum);
    }
}
