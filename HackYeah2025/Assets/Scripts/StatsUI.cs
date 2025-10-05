using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviourSingleton<StatsUI>
{
    [SerializeField] private List<Image> _statsVisuals;
    [SerializeField] private List<Image> _statsPreviewVisuals;

    [SerializeField] private TMP_Text _titleText;

    private bool _currentPreviewDirection;
    private bool _isPreviewActive;

    public void UpdateVisual()
    {
        _isPreviewActive = false;

        for (int i = 0; i < _statsVisuals.Count; i++) {
            Image image = _statsVisuals[i];
            Image previewImage = _statsPreviewVisuals[i];
            image.DOKill();
            previewImage.DOKill();
            image.fillAmount = GameplayManager.Instance.CurrentStats[i] / 100f;
            previewImage.fillAmount = GameplayManager.Instance.CurrentStats[i] / 100f;
        }
    }

    public void UpdatePreview(bool direction, bool diplayStatChange)
    {
        if (diplayStatChange == _isPreviewActive && _currentPreviewDirection == direction)
            return;


        if (diplayStatChange) {
            _isPreviewActive = true;
            _currentPreviewDirection = direction;

            for (int i = 0; i < _statsPreviewVisuals.Count; i++) {
                Image previewImage = _statsPreviewVisuals[i];
                previewImage.fillAmount = GameplayManager.Instance.GetNewStats(direction)[i] / 100f;
                //Debug.Log("Preview: " + i + " -- " + GameplayManager.Instance.GetNewStats(direction)[i] / 100f);
            }
        }
        else {
            _isPreviewActive = false;
            _currentPreviewDirection = direction;

            for (int i = 0; i < _statsPreviewVisuals.Count; i++) {
                Image previewImage = _statsPreviewVisuals[i];
                previewImage.fillAmount = GameplayManager.Instance.CurrentStats[i] / 100f;
                Debug.Log("Preview: " + i + " -- " + GameplayManager.Instance.CurrentStats[i] / 100f);
            }
        }

    }

    private void UpdateStats(int[] stats)
    {
        for (int i = 0; i < _statsVisuals.Count; i++) {
            Image image = _statsVisuals[i];
            image.DOKill();
            image.fillAmount = GameplayManager.Instance.CurrentStats[i] / 100f;
        }
    }

    public void UpdateTitle(AgeCategory currentAge)
    {
        _titleText.text = currentAge.ToString();
    }
}
