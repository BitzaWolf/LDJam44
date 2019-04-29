using UnityEngine;

namespace Bitzawolf
{
    public class GameOverUI : MonoBehaviour
    {
        public void OnNewGameClicked()
        {
            GameManager.GetInstance().TransitionState(GameManager.State.IN_LEVEL);
            gameObject.SetActive(false);
        }

        public void OnMainMenuClicked()
        {
            GameManager.GetInstance().TransitionState(GameManager.State.MAIN_MENU);
            gameObject.SetActive(false);
        }

        public void OnQuitClicked()
        {
            Application.Quit();
        }
    }
}
