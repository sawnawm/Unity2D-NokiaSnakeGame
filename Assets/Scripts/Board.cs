using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Column
{
    public List<Cell> columnCells = new List<Cell>();

    public Column(Cell cell)
    {
        columnCells.Add(cell);
    }
}

//[ExecuteInEditMode]
[System.Serializable]
public class Board : MonoBehaviour
{
    public List<Column> CellsCollective;
    public static Board Instance;
    public delegate void Notify();
    public Notify OnReset;
    public Notify OnAllSnakeCovered;

    [SerializeField]
    private List<Cell> _emptyCells = new List<Cell>();
    private Vector2 _snakeHead = Vector2.zero; // {x, y}

    private void Awake()
    {
        Instance = this;
        if (CellsCollective == null)
        {
            CellsCollective = new List<Column>();
        }
    }

    public void GenerateFood()
    {
        Cell selectedCell = _emptyCells[Random.Range(0, _emptyCells.Count - 1)];
        if (selectedCell != null)
        {
            selectedCell.ChangeCellLife(CellLife.Food);
        }
        else
        {
            OnAllSnakeCovered?.Invoke();
        }
    }

    public Cell SnakeNextCell(Vector2 direction)
    {
        Cell nextCell;

        if (direction.x != 0)
        {
            _snakeHead.x += direction.x;
            if (_snakeHead.x == CellsCollective.Count)
            {
                _snakeHead.x = 0;
            }
            else if (_snakeHead.x == -1)
            {
                _snakeHead.x = CellsCollective.Count - 1;
            }
        }
        else
        {
            _snakeHead.y -= direction.y;
            if (_snakeHead.y == CellsCollective[(int)_snakeHead.x].columnCells.Count)
            {
                _snakeHead.y = 0;
            }
            else if (_snakeHead.y == -1)
            {
                _snakeHead.y = CellsCollective[(int)_snakeHead.x].columnCells.Count - 1;
            }
        }
        nextCell = GetSnakeHead();

        return nextCell;
    }

    public void CellOccupied(Cell cell)
    {
        _emptyCells.Remove(cell);
    }

    public void CellEmptied(Cell cell)
    {
        _emptyCells.Add(cell);
    }
    
    public void ResetGameData()
    {
        OnReset?.Invoke();
        ResetSnakeHeadPosition();
        GenerateFood();
    }

    private void ResetSnakeHeadPosition()
    {
        _snakeHead = new Vector2((CellsCollective.Count / 2) - 1, (CellsCollective[0].columnCells.Count / 2) - 1);

        GetSnakeHead().ChangeCellLife(CellLife.Snake);
    }

    public Cell GetSnakeHead()
    {
        return CellsCollective[(int)_snakeHead.x].columnCells[(int)_snakeHead.y];
    }
}
