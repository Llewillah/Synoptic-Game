using NUnit.Framework.Constraints;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Scriptable Objects/Building")]
public class Building : ScriptableObject
{
    public string buildingName;
    public Sprite sprite;
    public float buildTime;
    public int buildCost;
    public int totalEmployees;
    public Resource[] inputs;
    public Resource[] outputs;
    public int[] inputsCost;
    public int outputCount;
    public float workTime;
    public bool warehouse = false;
    public bool globalOutput = false;
    public bool isTrader = false;
    public bool isHouse = false;
}
