using UnityEngine;

namespace SnakeGame
{
    public class SnakeVisualsHandler : MonoBehaviour
    {
        [SerializeField] private Transform head;
        [SerializeField] private Sprite segment, tail;
        private Snake _snake;

        private void Awake()
        {
            _snake = GetComponent<Snake>();
        }

        void Start()
        {
        }

        private void OnEnable()
        {
            _snake.HeadAngle += OnHeadRotation;
            _snake.SnakeChange += OnSnakeChange;
        }

        private void OnDisable()
        {
            _snake.HeadAngle -= OnHeadRotation;
            _snake.SnakeChange -= OnSnakeChange;
        }

        void Update()
        {
            HandleTailRotation();
        }

        void HandleTailRotation()
        {
            Transform snakeTail = _snake.LastSegment;
            Transform lastSeg = _snake.PenultimateSegment;

            if (snakeTail.position.x == lastSeg.position.x && snakeTail.position.y > lastSeg.position.y)
            {
                _snake.LastSegment.eulerAngles = new Vector3(0, 0, 180);
            } else if (snakeTail.position.x == lastSeg.position.x && snakeTail.position.y < lastSeg.position.y)
            {
                _snake.LastSegment.eulerAngles = new Vector3(0, 0, 0);
            }else if (snakeTail.position.x > lastSeg.position.x && snakeTail.position.y == lastSeg.position.y)
            {
                _snake.LastSegment.eulerAngles = new Vector3(0, 0, 90);
            }
            else if (snakeTail.position.x < lastSeg.position.x && snakeTail.position.y == lastSeg.position.y)
            {
                _snake.LastSegment.eulerAngles = new Vector3(0, 0, -90);
            }
        }

        void OnHeadRotation(float rotation)
        {
            head.transform.eulerAngles = new Vector3(0,0, rotation);
        }

        void OnSnakeChange()
        {
            foreach (var seg in _snake.Segments)
            {
                if (!seg.TryGetComponent(out Snake s))
                {
                    seg.GetComponent<SpriteRenderer>().sprite = segment;
                }
            }

            if (!_snake.LastSegment.TryGetComponent(out Snake sn))
            {
                _snake.LastSegment.GetComponent<SpriteRenderer>().sprite = tail;
            }
        }
    }
}
