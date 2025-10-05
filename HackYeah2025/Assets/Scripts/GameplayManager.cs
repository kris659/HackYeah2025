using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayManager : MonoBehaviourSingleton<GameplayManager>
{
    public CardSO CurrentCard { get; private set; }
    public int[] CurrentStats { get; private set; }

    private List<CardSO> _allCards;
    private List<CardSO> _playedCards = new();
    private List<bool> _playedCardsDirection = new();
    private List<CardSO> _currentAgeCards;

    [SerializeField] private int[] _startingStats;
    [SerializeField] private List<AgeStatValuePair> _ageCardsAmount;

    private AgeCategory _currentAgeCategory;

    private List<LongTermStatChange> _longTermEffects = new();


    private void Start()
    {
        _allCards = Resources.LoadAll<CardSO>("Cards").ToList();
        Debug.Log("All cards: " + _allCards.Count);
        SetStartingStats();
        StartAge(AgeCategory.Teen);
        SelectAndStartPlayingNextCard();
    }

    private void StartAge(AgeCategory ageCategory)
    {
        _currentAgeCategory = ageCategory;
        _currentAgeCards = _allCards.FindAll((card) => card.ageCategory == ageCategory);
        StatsUI.Instance.UpdateTitle(_currentAgeCategory);
        Debug.Log(ageCategory.ToString() + " cards: " + _currentAgeCards.Count);
    }

    private void SetStartingStats()
    {
        CurrentStats = _startingStats;
        StatsUI.Instance.UpdateVisual();
    }

    private void SelectAndStartPlayingNextCard()
    {
        if (_ageCardsAmount[(int)_currentAgeCategory].value <= 0) {
            _currentAgeCategory++;
            if((int)_currentAgeCategory >= _ageCardsAmount.Count) {
                Debug.Log("YOU WON");
                return;
            }
            StartAge(_currentAgeCategory);
        }

        List<CardSO> possibleCards = _currentAgeCards.FindAll((card) => CanPlayCard(card)).ToList();
        CurrentCard = SelectRandomCard(possibleCards);
        _currentAgeCards.Remove(CurrentCard);
        _ageCardsAmount[(int)_currentAgeCategory].value--;        
        CardsVisualManager.Instance.ShowCard(CurrentCard);
    }

    private bool CanPlayCard(CardSO card)
    {
        foreach (var cardRequiremeny in card.requiredRightCards) {
            Debug.Log("Was played: " + _playedCards.Contains(cardRequiremeny) + " " + " side: " + _playedCardsDirection[_playedCards.IndexOf(cardRequiremeny)]);
            if(!(_playedCards.Contains(cardRequiremeny) && _playedCardsDirection[_playedCards.IndexOf(cardRequiremeny)] == true)) 
                return false;
        }
        foreach (var cardRequiremeny in card.requiredLeftCards) {
            if (!(_playedCards.Contains(cardRequiremeny) && _playedCardsDirection[_playedCards.IndexOf(cardRequiremeny)] == false))
                return false;
        }

        foreach (var statRequirement in card.statsRequirements) {
            if (CurrentStats[(int)statRequirement.category] < statRequirement.minValue)
                return false;
            if (CurrentStats[(int)statRequirement.category] > statRequirement.maxValue)
                return false;
        }
        return true;
    }
    private CardSO SelectRandomCard(List<CardSO> cards)
    {
        int totalWeight = 0;
        foreach (var card in cards) {
            totalWeight += card.probability;
        }
        int randomValue = UnityEngine.Random.Range(0, totalWeight);

        int currentValue = 0;
        for (int i = 0; i < cards.Count; i++) {
            currentValue += cards[i].probability;
            if (randomValue < currentValue)
                return cards[i];
        }
        return cards.Last();
    }

    public void PlayCurrentCard(bool swipeDirection)
    {
        _playedCards.Add(CurrentCard);
        _playedCardsDirection.Add(swipeDirection);
        UpdateStats(swipeDirection);
        SelectAndStartPlayingNextCard();
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

        for(int i = 0; i < CurrentStats.Length; i++) {
            newStats[i] = CurrentStats[i];
        }

        if (direction) {
            foreach (var statChange in CurrentCard.rightSwipeStatChanges) {
                newStats[(int)statChange.category] += statChange.value;
            }
        }
        else {
            foreach (var statChange in CurrentCard.leftSwipeStatChanges) {
                newStats[(int)statChange.category] += statChange.value;
            }
        }
        return newStats;
    }
}
