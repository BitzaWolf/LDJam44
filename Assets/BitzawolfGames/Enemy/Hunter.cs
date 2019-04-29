using UnityEngine;

namespace Bitzawolf
{
    public class Hunter : Enemy
    {
        [Tooltip("Number of turns to spend reloading after firing a bullet.")]
        public int turnsToReload = 2;

        [Tooltip("How far ahead the hunter can see to spot a werewolf.")]
        public int sightRange = 5;

        public override void OnStart()
        {
            base.OnStart();
            //GameManager.GetInstance().AddHunter(this);
        }

        public override void OnDestroyed()
        {
            base.OnDestroyed();
            //GameManager.GetInstance().RemoveHunter(this);
        }
    }
}
