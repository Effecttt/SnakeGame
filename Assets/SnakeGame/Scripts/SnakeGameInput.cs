using System;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeGameInput : MonoBehaviour
    {
        public static SnakeGameInput Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(SnakeGameInput)) as SnakeGameInput;
 
                return _instance;
            }
        }
        private static SnakeGameInput _instance;

        public event Action<Vector2> OnMovementPressed;

        private Vector2 _movement = Vector2.zero;

        private void Update()
        {
            float x = 0;
            float y = 0;

            if (Input.GetKeyDown(KeyCode.W))
            {
                y = 1;
            } else if (Input.GetKeyDown(KeyCode.S))
            {
                y = -1;
            } else if (Input.GetKeyDown(KeyCode.D))
            {
                x = 1;
            } else if (Input.GetKeyDown(KeyCode.A))
            {
                x = -1;
            }

            _movement = new Vector2(x, y);
            
            if(_movement.sqrMagnitude > 0) OnMovementPressed?.Invoke(_movement);
        }
    }
}
