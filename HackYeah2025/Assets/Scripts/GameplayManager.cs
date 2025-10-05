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
        Debug.Log(ageCategory.ToString() + " cards: " + _allCards.Count);
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
        _playedCards.Add(CurrentCard);
        _currentAgeCards.Remove(CurrentCard);
        _ageCardsAmount[(int)_currentAgeCategory].value--;        
        CardsVisualManager.Instance.ShowCard(CurrentCard);
    }

    private bool CanPlayCard(CardSO card)
    {
        foreach (var cardRequiremeny in card.requiredRightCards) {
            if(!_playedCards.Contains(cardRequiremeny)) 
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
            Debug.Log("Right preview: " + CurrentCard.rightSwipeStatChanges.Count);
            foreach (var statChange in CurrentCard.rightSwipeStatChanges) {
                Debug.Log("Preview stat change: " + (int)statChange.category + " " + " " + statChange.value);
                newStats[(int)statChange.category] += statChange.value;
            }
            //foreach (var statChange in CurrentCard.rightSwipeStatLongTermChanges) {
            //    _longTermEffects.Add(statChange);
            //}
        }
        else {
            foreach (var statChange in CurrentCard.leftSwipeStatChanges) {
                newStats[(int)statChange.category] += statChange.value;
            }
            //foreach (var statChange in CurrentCard.leftSwipeStatLongTermChanges) {
            //    _longTermEffects.Add(statChange);
            //}
        }
        return newStats;
    }
}
