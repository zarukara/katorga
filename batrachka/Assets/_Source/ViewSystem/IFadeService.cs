using UnityEngine.UI;

namespace ViewSystem
{
    public interface IFadeService
    {
        void FadeIn(Image image, float duration);
        void FadeOut(Image image, float duration);
    }
}