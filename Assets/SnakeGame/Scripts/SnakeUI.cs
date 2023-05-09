using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeUI : MonoBehaviour
    {
        public static SnakeUI Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(SnakeUI)) as SnakeUI;
 
                return _instance;
            }
        }
        private static SnakeUI _instance;
        
        [SerializeField] private TMP_Text points;
        [SerializeField] private Transform startMenu, afterGame;
        [SerializeField] private List<TMP_Text> highScore = new();
        [SerializeField] private List<TMP_Text> score = new();

        private void Awake()
        {
            startMenu.GetChild(0).gameObject.SetActive(false);
            afterGame.GetChild(0).gameObject.SetActive(false);
        }

        private void Start()
        {
            SetPoints();
        }
        private void OnEnable()
        {
            SnakeGameManager.PointModified += SnakeGameManagerOnPointModified;
        }
        private void OnDisable()
        {
            SnakeGameManager.PointModified -= SnakeGameManagerOnPointModified;
        }
        
        private void SnakeGameManagerOnPointModified()
        {
            SetPoints();
        }
        
        void SetPoints() => points.text = SnakeGameManager.Instance.Points.ToString();

        public void ToggleStartMenu(bool b)
        {
            SetupUI(b);
            startMenu.GetChild(0).gameObject.SetActive(b);
        }
        
        public void ToggleAfterGameMenu(bool b)
        {
            SetupUI(b);
            afterGame.GetChild(0).gameObject.SetActive(b);
        }

        void SetupUI(bool b)
        {
            points.gameObject.SetActive(!b);
            foreach (var hs in highScore)
            {
                hs.text = PlayerPrefs.GetInt("HighScore").ToString();
            }

            foreach (var s in score)
            {
                s.text = SnakeGameManager.Instance.Points.ToString();
            }
        }
    }
}