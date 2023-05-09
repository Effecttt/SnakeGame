using System;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeGameManager : MonoBehaviour
    {
        public static SnakeGameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(SnakeGameManager)) as SnakeGameManager;
 
                return _instance;
            }
        }
        private static SnakeGameManager _instance;
        
        public static event Action PointModified, WinGame;
        public int Points { get; private set; }
        
        private int _pointsToWin;
        
        private Snake _snake;
        private SnakeFood _food;

        private void Awake()
        {
            Application.targetFrameRate = 144;
            _snake = FindObjectOfType<Snake>();
            _food = FindObjectOfType<SnakeFood>();
            _pointsToWin = 46 * 22;
            Points = 0;
        }

        private void Start()
        {
            SnakeUI.Instance.ToggleStartMenu(true);
            ToggleGame(false);
        }

        private void OnEnable()
        {
            SnakeFood.OnFoodCollected += SnakeOnFoodCollected;
            Snake.SnakeLose += SnakeOnLose;
        }
        
        private void OnDisable()
        {
            SnakeFood.OnFoodCollected -= SnakeOnFoodCollected;
            Snake.SnakeLose -= SnakeOnLose;
        }
        
        private void SnakeOnFoodCollected()
        {
            Points++;
            PointModified?.Invoke();
            if(_snake.SnakeSize >= _pointsToWin)
                Win();
        }
        
        private void SnakeOnLose()
        {
            var highScore = PlayerPrefs.GetInt("HighScore");
            if(Points > highScore) PlayerPrefs.SetInt("HighScore", Points);
            PointModified?.Invoke();
            ToggleGame(false);
            SnakeUI.Instance.ToggleAfterGameMenu(true);
        }

        void ToggleGame(bool b)
        {
            if (b)
            {
                _snake.ToggleSnake(true);
                _food.gameObject.SetActive(true);
            }
            else
            {
                _snake.ToggleSnake(false);
                _food.gameObject.SetActive(false);
            }
        }

        void Win()
        {
            WinGame?.Invoke();
            ToggleGame(false);
            SnakeUI.Instance.ToggleAfterGameMenu(true);
        }

        public void Play()
        {
            Points = 0;
            PointModified?.Invoke();
            ToggleGame(true);
        }
    }
}
