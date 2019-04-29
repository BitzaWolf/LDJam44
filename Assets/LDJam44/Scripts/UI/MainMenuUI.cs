using UnityEngine;

namespace Bitzawolf
{
    public class MainMenuUI : MonoBehaviour
    {
        public void OnNewGameClicked()
        {
            GameManager.GetInstance().TransitionState(GameManager.State.IN_LEVEL);
            gameObject.SetActive(false);
        }

        public void OnQuitClicked()
        {
            Application.Quit();
        }
    }
}
