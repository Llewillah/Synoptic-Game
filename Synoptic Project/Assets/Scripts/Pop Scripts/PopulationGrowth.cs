using UnityEngine;

public class PopulationGrowth : MonoBehaviour
{
    float civGrowth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStats.civ < PlayerStats.totCiv) 
        {
            civGrowth += PlayerStats.wealth / PlayerStats.civ * Time.deltaTime;
        }

        if (civGrowth >= 100)
        {
            PlayerStats.civ++;
            civGrowth = 0;
        }
    }
}
