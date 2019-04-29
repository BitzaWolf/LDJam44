/**
 * Handles everything and anything to do with the player's werewolf character including
 * movement, reacting to collision, and detecting where the wolf can move.
 */

using UnityEngine;

namespace Bitzawolf
{
    public class PlayerWolf : BitzawolfGameObject
    {
        public float moveSpeed, turnSpeed;

        private float forwardAmount, turnAmount;
        private Vector3 groundNormal, move;
        //private Rigidbody rigidbody;

        public override void OnStart()
        {
            base.OnStart();
            //rigidbody = GetComponent<Rigidbody>();
        }

        public override void UpdateInLevel()
        {
            base.UpdateInLevel();

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            move = v * Vector3.forward + h * Vector3.right;

            if (move.magnitude > 1.0f)
                move.Normalize();

            move = transform.InverseTransformDirection(move);
            move = Vector3.ProjectOnPlane(move, Vector3.up);

            turnAmount = Mathf.Atan2(move.x, move.z);
            forwardAmount = move.z;

            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);

            if (forwardAmount < 0.2)
                return;

            Vector3 distanceMoved = transform.forward * moveSpeed * Time.deltaTime;
            Vector3 newPos = transform.position + distanceMoved;

            bool collision = Physics.Raycast(transform.position, transform.forward, distanceMoved.magnitude);
            if (!collision)
                transform.position += distanceMoved;
            /*
            Vector3 velocity = move * (moveSpeed / Time.deltaTime);
            velocity.y = rigidbody.velocity.y;
            rigidbody.velocity = velocity;*/
        }
    }
}
