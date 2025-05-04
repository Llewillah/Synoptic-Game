using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    public static int totCiv = 10, totSpec = 0, totMil = 0, civ = 10, empCiv = 0, spec = 0, mil = 0, money = 50, wealth = 0, techPoints = 0;

    public static Dictionary<Resource, int> globalResources = new Dictionary<Resource,int>();
}
