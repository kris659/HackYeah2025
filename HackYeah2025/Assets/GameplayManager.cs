using System.Collections.Generic;
using System.Data;
using System.IO.Pipes;
using UnityEngine;

public class GameplayManager : MonoBehaviourSingleton<GameplayManager>
{
    public CardSO CurrentCard { get; private set; }
    public int[] CurrentStats { get; private set; }

    [SerializeField] private List<CardSO> _testingCards;


    private int _currentCardIndex = -1;
    private int[] _startingStats;

    private List<LongTermStatChange> _longTermEffects;

    private void Start()
    {
        SetStartingStats();
        StartPlayingNextCard();
    }

    private void SetStartingStats()
    {
        CurrentStats = _startingStats;
    }

    private void StartPlayingNextCard()
    {
        _currentCardIndex++;
        CurrentCard = _testingCards[_currentCardIndex];
        CardsVisualManager.Instance.ShowCard(CurrentCard);
    }

    public void PlayCurrentCard(bool swipeDirection)
    {
        UpdateStats(swipeDirection);
        StartPlayingNextCard();
    }

    private void UpdateStats(bool swipeDirection)
    {
        if (swipeDirection) {
            foreach (var statChange in CurrentCard.rightSwipeStatChanges) {
                CurrentStats[(int)statChange.category] += statChange.value;
            }
            foreach (var statChange in CurrentCard.rightSwipeStatLongTermChanges) {
                _longTermEffects.Add(statChange);
            }
        }
        else {
            foreach (var statChange in CurrentCard.leftSwipeStatChanges) {
                CurrentStats[(int)statChange.category] += statChange.value;
            }
            foreach (var statChange in CurrentCard.leftSwipeStatLongTermChanges) {
                _longTermEffects.Add(statChange);
            }
        }
        StatsUI.Instance.UpdateVisual();

    }

    public int[] GetNewStats(bool direction)
    {
        int[] newStats = new int[CurrentStats.Length];

        if (direction) {
            foreach (var statChange in CurrentCard.rightSwipeStatChanges) {
                newStats[(int)statChange.category] = CurrentStats[(int)statChange.category] + statChange.value;
            }
            //foreach (var statChange in CurrentCard.rightSwipeStatLongTermChanges) {
            //    _longTermEffects.Add(statChange);
            //}
        }
        else {
            foreach (var statChange in CurrentCard.leftSwipeStatChanges) {
                newStats[(int)statChange.category] = CurrentStats[(int)statChange.category] + statChange.value;
            }
            //foreach (var statChange in CurrentCard.leftSwipeStatLongTermChanges) {
            //    _longTermEffects.Add(statChange);
            //}
        }
        return newStats;
    }
}
