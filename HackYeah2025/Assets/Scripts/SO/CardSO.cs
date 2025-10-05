using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Product", menuName = "ScriptableObjects/Card")]
public class CardSO : ScriptableObject
{
    [Header("BASE INFO")]
    //public string title = "Title";
    [TextAreaAttribute]
    public string description = "Description";
    public string leftText = "Description";
    public string rightText = "Description";
    public AgeCategory ageCategory;
    public int probability = 100;

    [Header("INSTANT EFFECT")]
    public List<StatValuePair> leftSwipeStatChanges;
    public List<StatValuePair> rightSwipeStatChanges;

    [Header("LONG TERM EFFECT")]
    public List<LongTermStatChange> leftSwipeStatLongTermChanges;
    public List<LongTermStatChange> rightSwipeStatLongTermChanges;

    [Header("REQUIREMENTS")]
    [FormerlySerializedAs("requiredCards")]
    public List<CardSO> requiredRightCards;
    public List<CardSO> requiredLeftCards;
    public List<StatRequirementData> statsRequirements;

}