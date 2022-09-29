using System.Collections.Generic;
using UnityEngine;

// controls level changing, restarting, pausing player

public class LevelController : MonoBehaviour
{
    [SerializeField] private List<GridController> _levels;

    private GridController _currentLevel;

    private void Start()
    {
        _currentLevel = _levels[0];
        _currentLevel.gameObject.SetActive(true);

        _currentLevel.Theseus.OnExitReached += OnPlayerWon;

        for (int i = 0; i < _levels.Count; i++)
        {
            _levels[i].gameObject.SetActive(i == 0);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _currentLevel.gameObject.SetActive(false);
            ChangeLevel(_levels.IndexOf(_currentLevel));
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            _currentLevel.Theseus.CanMove = false;
            _currentLevel.Minotaur.MinotaurStepsCount = 10;
            _currentLevel.Minotaur.Move();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            NextLevel();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            var currentIndex = _levels.IndexOf(_currentLevel);
            _levels[currentIndex].gameObject.SetActive(false);

            if (currentIndex > 0)
            {
                currentIndex--;
            }
            else
            {
                currentIndex = 0;
            }

            ChangeLevel(currentIndex);
        }
    }

    private void OnPlayerWon()
    {
        NextLevel();
        _currentLevel.Minotaur.MinotaurStepsCount = 2;
    }

    private void NextLevel()
    {
        var currentIndex = _levels.IndexOf(_currentLevel);
        _levels[currentIndex].gameObject.SetActive(false);

        if (currentIndex < _levels.Count - 1)
        {
            currentIndex++;
        }
        else
        {
            currentIndex = 0;
        }

        ChangeLevel(currentIndex);
    }

    private void ChangeLevel(int newLevelIndex)
    {
        _currentLevel.Theseus.OnExitReached -= OnPlayerWon;

        _currentLevel = _levels[newLevelIndex];
        _currentLevel.gameObject.SetActive(true);
        _currentLevel.Theseus.gameObject.SetActive(true);
        _currentLevel.Minotaur.gameObject.SetActive(true);

        _currentLevel.Theseus.OnExitReached += OnPlayerWon;
    }
}
