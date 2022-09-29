using DG.Tweening;
using UnityEngine;

// class for available moves
public class Moving
{
    public Vector2 Horizontal;
    public Vector2 Vertical;
}

// controls minotaur moves

public class Minotaur : MonoBehaviour
{
    [SerializeField] private Theseus _target;
    [SerializeField] private GridController _controller;
    [SerializeField] private float _moveDuration;
    [SerializeField] private Ease _moveEase = Ease.InBack;
    [SerializeField] public int MinotaurStepsCount = 2;

    public bool IsMoving => StepsCount < MinotaurStepsCount;

    // counting steps, if not max steps count moves minotaur
    private int StepsCount
    {
        get => _stepsCount;
        set
        {
            if (value == _stepsCount)
            {
                return;
            }

            _stepsCount = value;
            if (IsMoving)
            {
                TryToMove();
            }
        }
    }

    private Moving _moving;
    private int _stepsCount;

    private void OnEnable()
    {
        _moving = new Moving();
        StepsCount = MinotaurStepsCount;
    }

    private void FixedUpdate()
    {
        _moving.Horizontal = new Vector2(RaycastCheck(Vector2.left) ? -1 : 0, RaycastCheck(Vector2.right) ? 1 : 0);
        _moving.Vertical = new Vector2(RaycastCheck(Vector2.up) ? 1 : 0, RaycastCheck(Vector2.down) ? -1 : 0);
    }

    //checking walls around with raycast. If direction has wall near player returns true. Draws ray lines in debug
    private bool RaycastCheck(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
        if (hit.collider != null && string.Equals(hit.collider.tag, "Wall") && hit.distance > 1)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(direction) * hit.distance, Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(direction) * 1000, Color.red);
            return false;
        }
    }

    public void Move()
    {
        StepsCount = 0;
    }


    // moves in direction to player
    private void TryToMove()
    {
        var direction = _target.transform.position - transform.position;

        if (direction.x > 0 && _moving.Horizontal.y > 0 && Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
        {
            transform.DOLocalMoveX(transform.localPosition.x + _controller.GridItemSize, _moveDuration).SetEase(_moveEase).OnComplete(() => StepsCount++);
            Debug.Log($"Minotaur moved right");
        }
        else if (direction.x < 0 && _moving.Horizontal.x < 0 && Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
        {
            transform.DOLocalMoveX(transform.localPosition.x - _controller.GridItemSize, _moveDuration).SetEase(_moveEase).OnComplete(() => StepsCount++);
            Debug.Log($"Minotaur moved left");
        }
        else if (direction.y > 0 && _moving.Vertical.x > 0 && Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            transform.DOLocalMoveY(transform.localPosition.y + _controller.GridItemSize, _moveDuration).SetEase(_moveEase).OnComplete(() => StepsCount++);
            Debug.Log($"Minotaur moved up");
        }
        else if (direction.y < 0 && _moving.Vertical.y < 0 && Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            transform.DOLocalMoveY(transform.localPosition.y - _controller.GridItemSize, _moveDuration).SetEase(_moveEase).OnComplete(() => StepsCount++);
            Debug.Log($"Minotaur moved down");
        }
        else
        {
            StepsCount++;
        }
    }
}
