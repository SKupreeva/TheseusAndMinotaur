using UnityEngine;

// controls theseus and minotaur's start position

public class GridController : MonoBehaviour
{
    [SerializeField] private int _gridItemScale = 50;
    [SerializeField] private Theseus _theseus;
    [SerializeField] private Minotaur _minotaur;

    public Theseus Theseus => _theseus;
    public Minotaur Minotaur => _minotaur;

    private Vector2 _theseusStartPosition;
    private Vector2 _minotaurStartPosition;
    
    public int GridItemSize => _gridItemScale;

    private void Awake()
    {
        _theseusStartPosition = new Vector2(_theseus.transform.position.x, _theseus.transform.position.y);
        _minotaurStartPosition = new Vector2(_minotaur.transform.position.x, _minotaur.transform.position.y);
    }

    private void OnEnable()
    {
        _theseus.transform.position = _theseusStartPosition;
        _minotaur.transform.position = _minotaurStartPosition;
    }
}
