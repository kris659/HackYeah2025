using DG.Tweening;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _leftText;
    [SerializeField] private TMP_Text _rightText;

    [SerializeField] private float _fadeDuration;

    private CanvasGroup _canvasGroup;

    public void ShowCard(CardSO card)
    {
         _canvasGroup = GetComponent<CanvasGroup>();

        _descriptionText.text = card.description;
        _leftText.text = card.rightText;
        _rightText.text = card.leftText;
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, _fadeDuration);
    }

    public void HideCard()
    {
        transform.SetParent(null);
        _canvasGroup.DOFade(0, _fadeDuration).SetEase(Ease.InQuad).onComplete = () => Destroy(gameObject);
    }
}
