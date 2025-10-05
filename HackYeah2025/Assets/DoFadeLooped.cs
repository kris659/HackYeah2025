using DG.Tweening;
using TMPro;
using UnityEngine;

public class DoFadeLooped : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<TMP_Text>().alpha = 0;
        GetComponent<TMP_Text>().DOFade(1, 2).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        GetComponent<TMP_Text>().DOKill();
    }
}
