using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "ScriptableObjects/Card")]
public class CardSO : ScriptableObject
{
    [Header("Base info")]
    public string title = "Title";
    public string description = "Description";
    public AgeCategory ageCategory;    

    [Header("Effect")]
    public List<StatValuePair> rightSwipeStatChanges;
    public List<StatValuePair> leftSwipeStatChanges;

    [Header("Requirements")]
    public List<CardSO> requiredCards;
    public List<StatValuePair> statsRequirements;

}