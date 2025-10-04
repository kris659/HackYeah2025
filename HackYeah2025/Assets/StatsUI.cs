using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviourSingleton<StatsUI>
{
    [SerializeField] private List<Image> _statsVisuals;
    [SerializeField] private List<Image> _statsPreviewVisuals;

    [SerializeField] private TMP_Text _titleText;

    public void UpdateVisual()
    {

    }

    public void UpdatePreview()
    {

    }
}
