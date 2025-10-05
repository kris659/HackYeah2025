using UnityEngine;

[System.Serializable]
public struct LongTermStatChange
{
    public StatsCategory category;
    public int valueChange;
    public int duration;
}
