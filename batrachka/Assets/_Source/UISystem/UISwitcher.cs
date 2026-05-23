using ServiceSystem;
using UnityEngine;
using Zenject;

namespace UISystem
{
    public class UISwitcher : MonoBehaviour
    {
        private IUIState currentState;

        private ISaver saver;
        private Score score;
        private MainState.Factory mainStateFactory;

        [Inject]
        public void Construct(
            ISaver saver,
            Score score,
            MainState.Factory mainStateFactory)
        {
            this.saver = saver;
            this.score = score;
            this.mainStateFactory = mainStateFactory;
        }

        private void Start()
        {
            score.Set(
                saver.LoadScore(
                    Application.persistentDataPath + "/save.json"));

            SwitchState(mainStateFactory.Create());
        }

        public void SwitchState(IUIState newState)
        {
            currentState?.Exit();

            currentState = newState;

            currentState.Enter();
        }
    }
}