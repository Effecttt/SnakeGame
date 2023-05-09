using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public class Snake : MonoBehaviour
    {
        [field:Range(1, 10)][field:SerializeField]public int SnakeInitialSize { get; private set; }
        [SerializeField]private List<Transform> segments = new();
        public int SnakeSize => segments.Count;
        public static event Action SnakeLose;
        public event Action<float> HeadAngle;
        public event Action SnakeChange;

        public Transform LastSegment => segments[^1];
       /*{
            get
            {
                if (segments.Count == 0) return null;
                return segments[^1];
            }
        }*/

        public Transform PenultimateSegment => segments[^2];
        /*{
            get
            {
                if (segments.Count < 2) return null;
                return segments[^2];
            }
        }*/
        
        public List<Transform> Segments => segments;

        private GameObject _snakeSegment;
        private Vector2 _requestedMovement;
        private Vector2 _lastMovement;
        private SnakeVisualsHandler _visuals;
        private bool _invincible = true, _canMove = true;

        private Dictionary<Vector2, int> _headDirection;


        private void Awake()
        {
            _snakeSegment = Resources.Load<GameObject>("SnakeGame/Prefabs/SnakeSegment");
            _visuals = GetComponent<SnakeVisualsHandler>();
        }

        private void Start()
        {
            StartCoroutine(InvincibleTimer());
            segments.Add(transform);
            _headDirection = new Dictionary<Vector2, int>()
            {
                {Vector2.up, 0},
                {Vector2.right, -90},
                {Vector2.down, 180},
                {Vector2.left, 90}
            };
        }

        private void OnEnable()
        {
            SnakeGameInput.Instance.OnMovementPressed += RequestMovement;
            SnakeTickSystem.Instance.OnTick += SnakeUpdate;
            SnakeFood.OnFoodCollected += SnakeOnFoodCollected;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            if(SnakeGameInput.Instance is not null)
                SnakeGameInput.Instance.OnMovementPressed -= RequestMovement;
            if(SnakeTickSystem.Instance is not null)
                SnakeTickSystem.Instance.OnTick -= SnakeUpdate;
            SnakeFood.OnFoodCollected -= SnakeOnFoodCollected;
        }
        
        private void SnakeUpdate() => HandleDirection();

        void Grow()
        {
            Transform segment = Instantiate(_snakeSegment).transform;
            segment.position = segments[^1].position;
            segments.Add(segment);
            SnakeChange?.Invoke();
        }
        
        void AddSegments(int size)
        {
            for (int i = 0; i < size; i++)
            {
                Grow();
            }
        }
        
        private void SnakeOnFoodCollected() => Grow();
        
        void HandleDirection()
        {
            if (_requestedMovement == Vector2.zero && _lastMovement == Vector2.zero)
            {
                //initial movement
                Move(Vector2.right);
                return;
            }
            
            if (_requestedMovement != Vector2.zero)
            {
                Move(_requestedMovement);

                _requestedMovement = Vector2.zero;
                return;
            }
            
            Move(_lastMovement);
        }
        
        void Move(Vector2 currentMovement)
        {
            if(DontAllowOppositeMovement(_lastMovement, currentMovement) || !_canMove) return;
            
            //Head Rotation
            HeadAngle?.Invoke(_headDirection[currentMovement]);
            
            //segment movement
            for (int i = segments.Count - 1; i > 0; i--)
                segments[i].position = segments[i - 1].position;

            //movement
            Vector3 position = transform.position;
            float x = Mathf.Round(position.x) + currentMovement.x;
            float y = Mathf.Round(position.y) + currentMovement.y;

            position = new Vector2(x, y);
            transform.position = position;
            
            //last movement
            _lastMovement = currentMovement;
        }
        
        private void RequestMovement(Vector2 movement) => _requestedMovement = movement;

        public void ToggleSnake(bool b)
        {
            if (b)
            {
                _visuals.enabled = true;
                transform.GetChild(0).gameObject.SetActive(true);
                _canMove = true;
                StartCoroutine(InvincibleTimer());
                ResetSnake();
                AddSegments(SnakeInitialSize);
            }
            else
            {
                _visuals.enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
                _canMove = false;
                StopAllCoroutines();
            }
        }

        void ResetSnake()
        {
            _requestedMovement = Vector2.zero;
            _lastMovement = Vector2.zero;
            transform.position = Vector3.zero;

            for (int i = segments.Count -1; i > 0; i--)
            {
                Destroy(segments[i].gameObject);
            }
            
            segments.Clear();
            segments.Add(transform);
        }
        
        void Lose()
        {
            ResetSnake();
            SnakeLose?.Invoke();
        }
        
        bool DontAllowOppositeMovement(Vector2 lastMovement, Vector2 currentMovement)
        {
            if (lastMovement == Vector2.right && currentMovement == Vector2.left) return true;
            if (lastMovement == Vector2.left && currentMovement == Vector2.right) return true;
            if (lastMovement == Vector2.up && currentMovement == Vector2.down) return true;
            if (lastMovement == Vector2.down && currentMovement == Vector2.up) return true;
            return false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Wall"))
                Lose();
            if (other.CompareTag("SnakeSegment") && !_invincible)
                Lose();
            
        }
        
        private IEnumerator InvincibleTimer()
        {
            _invincible = true;
            yield return new WaitForSeconds(.1f);
            _invincible = false;
        }
    }
}
