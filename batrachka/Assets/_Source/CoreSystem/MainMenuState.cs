using UnityEngine;

namespace CoreSystem
{
    public class MainMenuState : IState
    {
        private GameObject mainMenu;
        private GameObject addMenu;
        private GameObject removeMenu;

        public MainMenuState(GameObject mainMenu, GameObject addMenu, GameObject removeMenu)
        {
            this.mainMenu = mainMenu;
            this.addMenu = addMenu;
            this.removeMenu = removeMenu;
        }

        public void Enter()
        {
            mainMenu.SetActive(true);
            addMenu.SetActive(false);
            removeMenu.SetActive(false);
        }

        public void Exit() { }
    }
}