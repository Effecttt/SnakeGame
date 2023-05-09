using System;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeTickSystem : MonoBehaviour
    {
        public static SnakeTickSystem Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(SnakeTickSystem)) as SnakeTickSystem;
 
                return _instance;
            }
        }
        private static SnakeTickSystem _instance;
        
        [field: SerializeField] public float Tick { get; private set; } = 0.05f;
        public event Action OnTick;
        private float _counter;
        

        void Update()
        {
            _counter += Time.deltaTime;
            if (_counter >= Tick)
            {
                _counter = 0;
                OnTick?.Invoke();
            }
        }
    }
}
