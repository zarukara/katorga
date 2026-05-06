using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ViewSystem
{
    public class FadeService : IFadeService
    {
        public void FadeIn(Image image, float duration)
        {
            image.DOKill();

            image.gameObject.SetActive(true);

            Color color = image.color;
            color.a = 0;
            image.color = color;

            image.DOFade(1f, duration);
        }

        public void FadeOut(Image image, float duration)
        {
            image.DOKill();

            image.DOFade(0f, duration)
                .OnComplete(() => image.gameObject.SetActive(false));
        }
    }
}