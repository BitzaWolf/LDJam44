using UnityEngine;

namespace Bitzawolf
{
    public class SignSpin : MonoBehaviour
    {
        public Vector3 rotationAmount = new Vector3();

        void Update()
        {
            transform.Rotate(rotationAmount * Time.deltaTime);
        }
    }
}
