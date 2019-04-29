using UnityEngine;
using UnityEngine.UI;

namespace Bitzawolf
{
    public class PointsUI : MonoBehaviour
    {
        public Text text;
        private GameManager gm;

        private void Start()
        {
            gm = GameManager.GetInstance();
        }

        private void Update()
        {
            int points = gm.GetPoints();
            text.text = gm.GetPoints().ToString("###,###,000") + " Points";
        }
    }
}
