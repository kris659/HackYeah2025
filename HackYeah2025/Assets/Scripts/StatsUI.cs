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

        for (int i = 0; i < _statsPreviewVisuals.Count; i++) {
            Image previewImage = _statsPreviewVisuals[i];
            previewImage.DOKill();
            previewImage.fillAmount = GameplayManager.Instance.GetNewStats(direction)[i];
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
}
