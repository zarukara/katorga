using UnityEngine;

namespace CoreSystem
{
    public class RemoveMenuState : IState
    {
        private GameObject mainMenu;
        private GameObject addMenu;
        private GameObject removeMenu;

        public RemoveMenuState(GameObject mainMenu, GameObject addMenu, GameObject removeMenu)
        {
            this.mainMenu = mainMenu;
            this.addMenu = addMenu;
            this.removeMenu = removeMenu;
        }

        public void Enter()
        {
            mainMenu.SetActive(false);
            addMenu.SetActive(false);
            removeMenu.SetActive(true);
        }

        public void Exit() { }
    }
}