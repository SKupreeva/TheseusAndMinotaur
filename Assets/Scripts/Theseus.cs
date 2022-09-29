using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

// Class for player input. Controls player's moves, death, next level detection

public class Theseus : MonoBehaviour
{
    [SerializeField] private GridController _controller;
    [SerializeField] private float _moveDuration;
    [SerializeField] private Ease _moveEase = Ease.InBack;
    [SerializeField] private Minotaur _minotaur;

    public bool CanMove;
    public UnityAction OnExitReached;

    private Moving _moving;

    private void OnEnable()
    {
        _moving = new Moving();
        CanMove = true;
    }

    private void Update()
    {
        if (CanMove)
        {
            MoveInput();
        }
    }

    private void FixedUpdate()
    {
        _moving.Horizontal = new Vector2(RaycastCheck(Vector2.left) ? -1 : 0, RaycastCheck(Vector2.right) ? 1 : 0);
        _moving.Vertical = new Vector2(RaycastCheck(Vector2.up) ? 1 : 0, RaycastCheck(Vector2.down) ? -1 : 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Minotaur")
        {
            //death
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            //next level
            CanMove = false;
            OnExitReached?.Invoke();
        }
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

    // controls key downs
    private void MoveInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && _moving.Horizontal.x < 0)
        {
            transform.DOLocalMoveX(transform.localPosition.x - _controller.GridItemSize, _moveDuration)
                .SetEase(_moveEase).OnComplete(OnTheseusMoveComplete);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && _moving.Vertical.x > 0)
        {
            transform.DOLocalMoveY(transform.localPosition.y + _controller.GridItemSize, _moveDuration)
                .SetEase(_moveEase).OnComplete(OnTheseusMoveComplete);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && _moving.Horizontal.y > 0)
        {
            transform.DOLocalMoveX(transform.localPosition.x + _controller.GridItemSize, _moveDuration)
                .SetEase(_moveEase).OnComplete(OnTheseusMoveComplete);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && _moving.Vertical.y < 0)
        {
            transform.DOLocalMoveY(transform.localPosition.y - _controller.GridItemSize, _moveDuration)
                .SetEase(_moveEase).OnComplete(OnTheseusMoveComplete);
        }
    }

    // starts minotaur's moves and block player moves
    private void OnTheseusMoveComplete()
    {
        CanMove = false;
        _minotaur.Move();

        StopAllCoroutines();

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(WaitWhileMinotaurWalks());
        }
    }

    // waits until minotaur finishes movement to unblock player moves
    private IEnumerator WaitWhileMinotaurWalks()
    {
        while (_minotaur.IsMoving)
        {
            yield return null;
        }

        CanMove = true;
    }
}
