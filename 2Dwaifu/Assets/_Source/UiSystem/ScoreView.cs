using ScoreSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace UiSystem
{
    public sealed class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;

        private ScoreModel _scoreModel;

        [Inject]
        public void Construct(ScoreModel scoreModel)
        {
            _scoreModel = scoreModel;
        }

        private void Start()
        {
            _scoreModel.ScoreChanged += UpdateView;
            UpdateView(_scoreModel.Score);
        }

        private void OnDestroy()
        {
            if (_scoreModel != null)
            {
                _scoreModel.ScoreChanged -= UpdateView;
            }
        }

        private void UpdateView(int score)
        {
            scoreText.text = score.ToString();
        }
    }
}
