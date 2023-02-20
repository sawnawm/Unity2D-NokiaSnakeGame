using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellLife
{
    Null,
    Empty,
    Food,
    Snake
}

[System.Serializable]
public class Cell : MonoBehaviour
{
    public delegate void CellUsedState(Cell cell);
    public CellUsedState OnOccupied;
    public CellUsedState OnEmpty;
    public CellLife CellLife;

    private Color[] _cellColor = new Color[5] 
    {
        Color.gray,     // Null
        Color.gray,     // Empty
        Color.green,    // Food
        Color.red,      // Snake
        Color.magenta   // Dead
    };

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Board.Instance.OnReset += ResetCell;
        OnOccupied += Board.Instance.CellOccupied;
        OnEmpty += Board.Instance.CellEmptied;

        ChangeCellLife(CellLife.Empty);
    }

    private void BoardEmptyCellListCheck(CellLife newLife)
    {
        if (newLife != CellLife)
        {
            if (newLife == CellLife.Empty)
            {
                OnEmpty?.Invoke(this);
            }
            else
            {
                OnOccupied?.Invoke(this);
            }
        }
    }

    private void ResetCell()
    {
        StopAllCoroutines();
        ChangeCellLife(CellLife.Empty);
    }

    public void ChangeCellLife(CellLife newLife)
    {
        BoardEmptyCellListCheck(newLife);

        CellLife = newLife;
        _spriteRenderer.color = _cellColor[(int)CellLife];
    }

    public void AnimateDead()
    {
        StartCoroutine(AnimateHeadDead());
    }

    private IEnumerator AnimateHeadDead()
    {
        bool deadColor = false;
        while (true)
        {
            _spriteRenderer.color = _cellColor[deadColor ? _cellColor.Length - 2 : _cellColor.Length - 1];
            deadColor = !deadColor;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
