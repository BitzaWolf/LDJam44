/**
 * Creates a mostly empty script to manage the game's state. Facilitates changing
 * between states (level loading, in-level, main menu, paused, etc.) and stores
 * easy access to key components.
 * 
 * Other scripts can easily change behavior based on the current game state by
 * extending BitzawolfGameObject, which itself extends the common Unity GameObject
 * so you still have access to all those tools and properties.
 * 
 * States can be added and deleted without issue. Be sure that the extension method
 * at the end of this file has an entry for your new state. This is how
 * BitzawolfGameObjects are updated based on the current game state.
 * 
 * This script should be attached to an Empty game object in a scene that's loaded
 * right at the start of the game.
 */

using UnityEngine;
using System.Collections.Generic;

namespace Bitzawolf
{
    public class GameManager : MonoBehaviour
    {
        /*************
         * Singleton *
         *************/
        private static GameManager instance = null;

        private void OnEnable()
        {
            if (instance == null)
                instance = this;
        }

        // Get the singleton, accessible from anywhere.
        public static GameManager GetInstance()
        {
            return instance;
        }

        /***********
         * Publics *
         **********/
        // Common, basic game states. Add/delete as needed.
        public enum State
        {
            GAME_INIT,
            MAIN_MENU,
            LEVEL_LOADING,
            LEVEL_STARTING,
            IN_LEVEL,
            LEVEL_ENDING,
            PAUSED
        }

        // Add components that you want to be easily accessible from other scripts here
        public GameObject MainCamera;
        public PlayerWolf playerWolf;
        public PotionBottle bottle;
        public AudioSource aud_mainMenu, aud_inGame, aud_ingredient;

        [Space]
        [Header("UI")]
        public Canvas inLevelUI, mainMenuUI, gameOverUI;

        [Space]
        [Header("Debug")]
        public bool debugMode = false;

        /************
         * Privates *
         ***********/
        private State currentState = State.GAME_INIT;
        private Stack<State> stateStack = new Stack<State>();
        private List<BitzawolfGameObject> gameObjects = new List<BitzawolfGameObject>();
        private List<PotionOrder> potionOrders = new List<PotionOrder>();
        private int points = 0;
        private float timeRemaining = 60;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(MainCamera);
            DontDestroyOnLoad(inLevelUI);
            DontDestroyOnLoad(mainMenuUI);
            DontDestroyOnLoad(gameOverUI);

            currentState = State.MAIN_MENU;
        }

        private void Update()
        {
            if (debugMode)
                UpdateCheats();

            string funcName = currentState.GetUpdateName();

            // We use reflection and dynamic function names here to avoid duplicating the for-each loop
            // inside a switch statement for the State. It's a confusing line of code, but better than duplicated loops.
            foreach (BitzawolfGameObject bgo in gameObjects)
            {
                typeof(BitzawolfGameObject).GetMethod(funcName).Invoke(bgo, new object[] { });
            }

            if (currentState == State.IN_LEVEL && timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                timeRemaining = Mathf.Max(timeRemaining, 0);

                if (timeRemaining <= 0)
                {
                    TransitionState(State.LEVEL_ENDING);
                }
            }
        }

        private void FixedUpdate()
        {
            string fixedFuncName = currentState.GetFixedUpdateName();
            foreach (BitzawolfGameObject bgo in gameObjects)
            {
                typeof(BitzawolfGameObject).GetMethod(fixedFuncName).Invoke(bgo, new object[] { });
            }
        }

        /**
         * Transitions from this current state to the next state with an option to remember
         * which state was just left so it can be returned to.
         * When this function is called, LeaveState and EnterState events are immediately triggered.
         * 
         * @param State nextState The next State the Game Manager should enter.
         * @param bool saveCurrentState If true, then the current state is pushed onto the State stack
         *      so that the state can be returned to without needing to specify the State itself.
         *      Helpful for situations where a pause menu doesn't know what state the game was just in
         *      but it wants the game to return to whatever it was.
         */
        public void TransitionState(State nextState, bool saveCurrentState = false)
        {
            // TODO - trigger events for Leaving State
            switch (currentState)
            {
                case State.MAIN_MENU:
                    aud_mainMenu.Stop();
                    aud_inGame.Play();
                    inLevelUI.gameObject.SetActive(true);
                    break;
            }

            // TODO - trigger events for Entering State
            switch (nextState)
            {
                case State.MAIN_MENU:
                    mainMenuUI.gameObject.SetActive(true);
                    inLevelUI.gameObject.SetActive(false);
                    aud_inGame.Stop();
                    aud_mainMenu.Play();
                    break;
                case State.IN_LEVEL:
                    points = 0;
                    timeRemaining = 60;
                    playerWolf.transform.position = new Vector3(-8, 0, -5);
                    break;
                case State.LEVEL_ENDING:
                    gameOverUI.gameObject.SetActive(true);
                    break;
            }

            if (saveCurrentState)
                stateStack.Push(currentState);
            currentState = nextState;
        }

        private void UpdateCheats()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                timeRemaining = 1;
            }
        }

        /**
         * Adds the BitzawolfGameObject to this Game Manager so that it can be updated and use
         * the manager's deligated state-based update functions.
         */
        public void AddGameObject(BitzawolfGameObject bgo)
        {
            if (bgo == null)
            {
                Debug.LogError("Null BitzawolfGameObject passed to Game Manager AddGameObject");
            }
            gameObjects.Add(bgo);
        }

        /**
         * Removes the BitzawolfGameObject from this Game Manager so that it will no longer be updated
         * and used by the manager's deligated state-based update functions.
         */
        public bool RemoveGameObject(BitzawolfGameObject bgo)
        {
            return gameObjects.Remove(bgo);
        }

        /**
         * Returns the current State the game is in.
         */
        public State GetCurrentState()
        {
            return currentState;
        }

        /**
         * Returns true if the Game Manager has at least one State it can return to.
         */
        public bool hasPreviousState()
        {
            return (stateStack.Count > 0);
        }

        // Simple examples of how to create public functions to change game state.
        /**
         * 
         */
        public void PauseGame()
        {
            TransitionState(State.PAUSED, true);
        }
        
        public void UnpauseGame()
        {
            if (stateStack.Count != 0)
                TransitionState(stateStack.Pop());
            else
                TransitionState(State.IN_LEVEL);
        }
        /*
        public void AddHunter(Hunter e)
        {
            if (e != null)
                hunters.Add(e);
        }

        public void RemoveHunter(Hunter e)
        {
            if (e != null)
                hunters.Remove(e);
        }

        public void AddBullet(Bullet e)
        {
            if (e != null)
                bullets.Add(e);
        }

        public void RemoveBullet(Bullet e)
        {
            if (e != null)
                bullets.Remove(e);
        }
        */

        public bool HasOrders()
        {
            return (potionOrders.Count > 0);
        }

        public void AddOrder(PotionOrder potionOrder)
        {
            potionOrders.Add(potionOrder);
        }

        public PotionOrder GetOrder()
        {
            if (HasOrders())
                return potionOrders[0];
            return null;
        }

        public void CompleteOrder(PotionOrder potionOrder)
        {
            potionOrders.Remove(potionOrder);
        }

        public void AwardPoints(int points)
        {
            if (points <= 0)
                return;

            this.points += points;
        }

        public int GetPoints()
        {
            return points;
        }

        public float GetTimeRemaining()
        {
            return timeRemaining;
        }

        public void GameOver()
        {
            // TODO What happens when a game is over?
        }
    }

    public static class StateMethods
    {
        public static string GetUpdateName(this GameManager.State state)
        {
            switch (state)
            {
                case GameManager.State.GAME_INIT:
                    return "UpdateInit";
                default:
                case GameManager.State.IN_LEVEL:
                    return "UpdateInLevel";
                case GameManager.State.LEVEL_ENDING:
                    return "UpdateLevelEnding";
                case GameManager.State.LEVEL_LOADING:
                    return "UpdateLevelLoading";
                case GameManager.State.LEVEL_STARTING:
                    return "UpdateLevelStarting";
                case GameManager.State.MAIN_MENU:
                    return "UpdateMainMenu";
                case GameManager.State.PAUSED:
                    return "UpdatePaused";
            }
        }

        public static string GetFixedUpdateName(this GameManager.State state)
        {
            switch (state)
            {
                case GameManager.State.GAME_INIT:
                    return "FixedUpdateInit";
                default:
                case GameManager.State.IN_LEVEL:
                    return "FixedUpdateInLevel";
                case GameManager.State.LEVEL_ENDING:
                    return "FixedUpdateLevelEnding";
                case GameManager.State.LEVEL_LOADING:
                    return "FixedUpdateLevelLoading";
                case GameManager.State.LEVEL_STARTING:
                    return "FixedUpdateLevelStarting";
                case GameManager.State.MAIN_MENU:
                    return "FixedUpdateMainMenu";
                case GameManager.State.PAUSED:
                    return "FixedUpdatePaused";
            }
        }
    }
}