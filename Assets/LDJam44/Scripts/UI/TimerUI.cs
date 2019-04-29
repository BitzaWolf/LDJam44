using UnityEngine;
using UnityEngine.UI;

namespace Bitzawolf
{
    public class TimerUI : BitzawolfGameObject
    {
        public Text text;
        private GameManager gm;

        public override void OnStart()
        {
            base.OnStart();
            gm = GameManager.GetInstance();
        }

        public override void UpdateInLevel()
        {
            base.UpdateInLevel();
            int minutes = (int)(gm.GetTimeRemaining() / 60);
            int seconds = ((int)gm.GetTimeRemaining()) % 60;
            text.text = minutes.ToString("0") + ":" + seconds.ToString("00");
        }
    }
}
