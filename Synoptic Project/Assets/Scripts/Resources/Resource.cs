using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Resource")]
public class Resource: ScriptableObject
{
    public Sprite sprite;
    public int value;
    public int weight;
}
