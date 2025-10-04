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
        _leftText.text = card.leftText;
        _rightText.text = card.rightText;
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, _fadeDuration);
    }

    public void HideCard()
    {
        _canvasGroup.DOFade(0, _fadeDuration);
    }
}
