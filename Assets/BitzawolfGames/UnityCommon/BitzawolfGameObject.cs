using UnityEngine;

/**
 * Facilitates adding and removing itself as a GameObject from the Game Manager's
 * internal collection which is used to update Game Objects based on the game's state.
 * 
 * Each possible game state the manager has must have a corresponding virtual update
 * function here. Virtual because we want decendents to override the empty function with
 * actual utility.
 * 
 * Decendents do not need to override each state function, only those it wants to add
 * functionality to.
 */

namespace Bitzawolf
{
    public class BitzawolfGameObject : MonoBehaviour
    {
        private void Start()
        {
            GameManager.GetInstance().AddGameObject(this);
            OnStart();
        }

        private void OnDestroy()
        {
            GameManager.GetInstance().RemoveGameObject(this);
            OnDestroyed();
        }
        
        public virtual void OnStart() { }
        public virtual void OnDestroyed() { }
        public virtual void UpdateInit() { }
        public virtual void UpdateInLevel() { }
        public virtual void UpdateLevelEnding() { }
        public virtual void UpdateLevelLoading() { }
        public virtual void UpdateLevelStarting() { }
        public virtual void UpdateMainMenu() { }
        public virtual void UpdatePaused() { }
        public virtual void FixedUpdateInit() { }
        public virtual void FixedUpdateInLevel() { }
        public virtual void FixedUpdateLevelEnding() { }
        public virtual void FixedUpdateLevelLoading() { }
        public virtual void FixedUpdateLevelStarting() { }
        public virtual void FixedUpdateMainMenu() { }
        public virtual void FixedUpdatePaused() { }
    }
}
