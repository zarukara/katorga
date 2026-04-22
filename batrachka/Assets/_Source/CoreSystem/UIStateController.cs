using UnityEngine;

namespace CoreSystem
{
    public class UIStateController : MonoBehaviour
    {
        public GameObject mainMenu;
        public GameObject addMenu;
        public GameObject removeMenu;

        private UIStateMachine stateMachine;

        private void Awake()
        {
            stateMachine = new UIStateMachine();
        }

        private void Start()
        {
            stateMachine.ChangeState(new MainMenuState(mainMenu, addMenu, removeMenu));
        }

        public void ShowMainMenu()
        {
            stateMachine.ChangeState(new MainMenuState(mainMenu, addMenu, removeMenu));
        }

        public void ShowAddMenu()
        {
            stateMachine.ChangeState(new AddMenuState(mainMenu, addMenu, removeMenu));
        }

        public void ShowRemoveMenu()
        {
            stateMachine.ChangeState(new RemoveMenuState(mainMenu, addMenu, removeMenu));
        }
    }
}