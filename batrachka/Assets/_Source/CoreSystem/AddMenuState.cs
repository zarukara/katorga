using UnityEngine;

namespace CoreSystem
{
    public class AddMenuState : IState
    {
        private GameObject mainMenu;
        private GameObject addMenu;
        private GameObject removeMenu;

        public AddMenuState(GameObject mainMenu, GameObject addMenu, GameObject removeMenu)
        {
            this.mainMenu = mainMenu;
            this.addMenu = addMenu;
            this.removeMenu = removeMenu;
        }

        public void Enter()
        {
            mainMenu.SetActive(false);
            addMenu.SetActive(true);
            removeMenu.SetActive(false);
        }

        public void Exit() { }
    }
}