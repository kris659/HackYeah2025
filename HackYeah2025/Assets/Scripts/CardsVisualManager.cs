using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardsVisualManager : MonoBehaviourSingleton<CardsVisualManager>
{
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Transform _cardParent;

    [SerializeField] private int _minSwipeShowDistance;
    [SerializeField] private int _minSwipeDecisionDistance;
    [SerializeField] private float _rotationSpeedMult;
    [SerializeField] private float _transitionDuration;
    [SerializeField] private float _scaleChange;

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
        _currentCard.ShowCard(card);
    }

    private void OnTouchStarted(InputAction.CallbackContext _)
    {
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

        _direction = distance < 0;
        _diplayStatChange = Mathf.Abs(distance) > _minSwipeShowDistance;
        _makeDecisionOnTouchEnd = Mathf.Abs(distance) > _minSwipeDecisionDistance;

        StatsUI.Instance.UpdatePreview(_direction, _diplayStatChange);

        _cardParent.eulerAngles = new Vector3(0, 0, distance * _rotationSpeedMult / 1000);
    }

    private void OnTouchEnded(InputAction.CallbackContext _)
    {
        _isPressed = false;

        if (_makeDecisionOnTouchEnd) {
            _currentCard.HideCard();
            _cardParent.rotation = Quaternion.identity;
            GameplayManager.Instance.PlayCurrentCard(_direction);
            return;
        }

        _cardParent.DORotate(Vector3.zero, _transitionDuration);
        _currentCard.transform.DOScale(Vector3.one, _transitionDuration);
    }

}
