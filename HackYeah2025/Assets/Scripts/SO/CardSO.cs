using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "ScriptableObjects/Card")]
public class CardSO : ScriptableObject
{
    [Header("BASE INFO")]
    //public string title = "Title";
    public string description = "Description";
    public string leftText = "Description";
    public string rightText = "Description";
    public AgeCategory ageCategory;
    public int probability = 100;

    [Header("INSTANT EFFECT")]
    public List<StatValuePair> rightSwipeStatChanges;
    public List<StatValuePair> leftSwipeStatChanges;

    [Header("LONG TERM EFFECT")]
    public List<LongTermStatChange> rightSwipeStatLongTermChanges;
    public List<LongTermStatChange> leftSwipeStatLongTermChanges;

    [Header("REQUIREMENTS")]
    public List<CardSO> requiredCards;
    public List<StatRequirementData> statsRequirements;

}