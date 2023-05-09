using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SnakeGame
{
    public class SnakeFood : MonoBehaviour
    {
        [SerializeField] private Collider2D foodArea;
        public static event Action OnFoodCollected;

        private void Start()
        {
            RandomizeFoodPosition();
        }

        private void OnEnable()
        {
            Snake.SnakeLose += RandomizeFoodPosition;
        }

        private void OnDisable()
        {
            Snake.SnakeLose -= RandomizeFoodPosition;
        }

        void RandomizeFoodPosition()
        {
            Bounds bounds = foodArea.bounds;

            // Pick a random position inside the bounds
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);

            // Round the values to ensure it aligns with the grid
            x = Mathf.Round(x);
            y = Mathf.Round(y);

            transform.position = new Vector2(x, y);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                RandomizeFoodPosition();
                OnFoodCollected?.Invoke();
            }
        }
    }
}
