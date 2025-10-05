using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardsVisualManager : MonoBehaviourSingleton<CardsVisualManager>
{
    [System.Serializable]
    public struct AgeIcons
    {
        public AgeCategory age;
        public Sprite happyIcon;
        public Sprite sadIcon;
    }

    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Transform _cardParent;

    [SerializeField] private int _minSwipeShowDistance;
    [SerializeField] private int _minSwipeDecisionDistance;
    [SerializeField] private float _rotationSpeedMult;
    [SerializeField] private float _transitionDuration;
    [SerializeField] private float _scaleChange;

    [SerializeField] private List<AgeIcons> _ageIcons;

    private Card _currentCard;
    private bool _direction;
    private bool _diplayStatChange;
    private bool _makeDecisionOnTouchEnd;

    private InputAction _pointAction;
    private Vector2 _touchStartPosition;



    private bool _isPressed;
    protected override void Awake()
    {
        base.Awake();

        _pointAction = InputSystem.actions.FindAction("Point2");
        _pointAction.performed += OnTouch;

        var clickAction = InputSystem.actions.FindAction("Click2");
        clickAction.performed += OnTouchStarted;
        clickAction.canceled += OnTouchEnded;
    }

    public void ShowCard(CardSO card)
    {
        _currentCard = Instantiate(_cardPrefab, _cardParent).GetComponent<Card>();
        Sprite icon = GetCurrentIcon();
        _currentCard.ShowCard(card, icon);
    }

    private Sprite GetCurrentIcon()
    {
        bool isHappy = true;
        for (int i = 0; i < GameplayManager.Instance.CurrentStats.Length; i++) {
            if (i == (int)StatsCategory.Stress && GameplayManager.Instance.CurrentStats[i] > 70)
                isHappy = false;
            if (i != (int)StatsCategory.Stress && GameplayManager.Instance.CurrentStats[i] < 30)
                isHappy = false;
        }

        foreach (var ageData in _ageIcons) {
            if(ageData.age == GameplayManager.Instance.CurrentAgeCategory) {                
                return isHappy ? ageData.happyIcon : ageData.sadIcon;
            }
        }
        return null;
    }

    private void OnTouchStarted(InputAction.CallbackContext _)
    {
        if (_currentCard == null)
            return;

        if (_pointAction.ReadValue<Vector2>() == Vector2.zero) {
            DOVirtual.DelayedCall(0, () =>
            {
                _isPressed = true;
                _touchStartPosition = _pointAction.ReadValue<Vector2>();
                _currentCard.transform.DOScale(Vector3.one * _scaleChange, _transitionDuration);
            });
            return;
        }
        _isPressed = true;
        _touchStartPosition = _pointAction.ReadValue<Vector2>();
        _currentCard.transform.DOScale(Vector3.one * _scaleChange, _transitionDuration);
    }

    private void OnTouch(InputAction.CallbackContext _)
    {
        if (!_isPressed)
            return;

        Vector2 currentPosition = _pointAction.ReadValue<Vector2>();
        float distance = _touchStartPosition.x - currentPosition.x;

        _direction = distance > 0;
        _diplayStatChange = Mathf.Abs(distance) > _minSwipeShowDistance;
        _makeDecisionOnTouchEnd = Mathf.Abs(distance) > _minSwipeDecisionDistance;

        StatsUI.Instance.UpdatePreview(_direction, _diplayStatChange);

        _cardParent.eulerAngles = new Vector3(0, 0, distance * _rotationSpeedMult / 1000);
    }

    private void OnTouchEnded(InputAction.CallbackContext _)
    {
        if(_cardParent == null) return;

        _isPressed = false;

        if (_makeDecisionOnTouchEnd) {
            if(_currentCard != null)
                _currentCard.HideCard();
            _cardParent.rotation = Quaternion.identity;
            GameplayManager.Instance.PlayCurrentCard(_direction);
            return;
        }

        _cardParent.DORotate(Vector3.zero, _transitionDuration);
        _currentCard.transform.DOScale(Vector3.one, _transitionDuration);
    }

}
