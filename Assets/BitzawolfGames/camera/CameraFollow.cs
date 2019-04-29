/**
 * Smoothly follows around a character using a top-down approach. The target game object can
 * be anything and changed at any time.
 */

using UnityEngine;

namespace Bitzawolf
{
    public class CameraFollow : MonoBehaviour
    {
        [Tooltip("The target the camera is following. Free to be changed anytime.")]
        public GameObject target = null;

        [Tooltip("Stops the camera from moving")]
        public bool paused = false;

        public float behind = 10;
        public float up = 10;

        [Range(0, 360)]
        public float angle = 270;

        [Range(0, 100)]
        public float moveSpeed = 50;

        // privates for optimization (no need to initialize new vars each update)
        private Vector3 behindVector, upVector, finalPosition, moveDir, newPosition;
        private float moveProportion;

        private void Start()
        {
            behindVector = new Vector3();
            upVector = new Vector3();
            finalPosition = new Vector3();
            moveDir = new Vector3();
            newPosition = new Vector3();
            moveProportion = 0;
        }

        void Update()
        {
            if (target == null || paused)
                return;

            // find the final position
            behindVector.Set(Mathf.Cos(angle * Mathf.PI / 180) * behind, 0, Mathf.Sin(angle * Mathf.PI / 180) * behind);
            upVector.y = up;
            finalPosition = target.transform.position - behindVector + upVector;
            moveDir = finalPosition - transform.position;

            // if close enough, then we're done here
            if (moveDir.magnitude <= 0.01)
            {
                return;
            }

            // interpolate over there
            //      get some proportion of the total distance to travel based on time elapsed.
            //      if speed is set to 100, then 100% if elapsed 1 second, 10% if elapsed 0.10 seconds
            //      unit of moveDir. proportion = (moveSpeed / 100) * timeElapsed. moveAmount = moveDir * proportion
            moveProportion = (moveSpeed / 10) * Time.deltaTime * moveDir.magnitude;
            newPosition = transform.position + (moveDir.normalized * moveProportion);
            transform.position = newPosition;
        }
    }
}
