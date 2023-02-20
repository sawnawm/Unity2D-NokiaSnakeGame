using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public static Snake Instance {
        get {
            if (_instance == null)
            {
                GameObject snake = new GameObject("Snake");
                _instance = snake.AddComponent<Snake>();
            }
            return _instance;
        }
        set { _instance = value; }
    }
    public delegate void Notify();
    public Notify OnAte;
    public Notify OnDead;

    private static Snake _instance;
    [SerializeField]
    private List<Cell> _body = new List<Cell>();
    [SerializeField]
    private Vector2 _direction = Vector2.left;
    [SerializeField]
    private Vector2 _newDirection = Vector2.left;

    private void Awake()
    {
        InputManager.Instance.VerticalPress += VerticalCheck;
    }

    private void OnEnable()
    {
        ClearBody();
        AddBody(null);
        _direction = Vector2.left;
        _newDirection = Vector2.left;
        InputManager.Instance.ResetInputs();
        InputManager.Instance.HorizontalPress += HorizontalCheck;
        InputManager.Instance.VerticalPress += VerticalCheck;
        OnAte += Board.Instance.GenerateFood;
    }

    private void OnDisable()
    {
        InputManager.Instance.HorizontalPress -= HorizontalCheck;
        InputManager.Instance.VerticalPress -= VerticalCheck;
        OnAte -= Board.Instance.GenerateFood;
    }

    private bool HorizontalCheck(float input)
    {
        if (_direction.x == 0)
        {
            _newDirection = new Vector2(input, 0);
        }
        else
        {
            return false;
        }
        return true;
    }

    private bool VerticalCheck(float input)
    {
        if (_direction.y == 0)
        {
            _newDirection = new Vector2(0, input);
        }
        else
        {
            return false;
        }
        return true;
    }

    private void ClearBody()
    {
        Cell[] tempList = new Cell[_body.Count];
        _body.CopyTo(tempList);

        foreach (Cell cell in tempList)
        {
            _body.Remove(cell);
        }
    }

    public void Move()
    {
        _direction = _newDirection;
        Cell cell = Board.Instance.SnakeNextCell(_direction);

        if (cell.CellLife == CellLife.Food)
        {
            EatFood(cell);
        }
        else if (cell.CellLife == CellLife.Snake)
        {
            // make cell it the head part if it is last part of the snake
            // because snake last part isn't removed yet
            if (cell == _body[_body.Count - 1])
            {
                _body.Remove(cell);
                _body.Insert(0, cell);
            }
            else
            {
                DeadMove(cell);
            }
        }
        else
        {
            NewHeadMove(cell);
        }
    }

    private void EatFood(Cell cell)
    {
        AddBody(cell);
        OnAte?.Invoke();
    }

    private void NewHeadMove(Cell cell)
    {
        RemoveBody(_body[_body.Count - 1]);
        AddBody(cell);
    }

    private void DeadMove(Cell cell)
    {
        RemoveBody(_body[_body.Count - 1]);
        cell.AnimateDead();
        OnDead?.Invoke();
    }

    // if no body to add then retrieve head
    private void AddBody(Cell newCell)
    {
        if (newCell == null)
        {
            _body.Add(Board.Instance.GetSnakeHead());
        }
        else
        {
            _body.Insert(0, newCell);
            newCell.ChangeCellLife(CellLife.Snake);
        }
    }

    private void RemoveBody(Cell bodyCell)
    {
        _body.Remove(bodyCell);
        bodyCell.ChangeCellLife(CellLife.Empty);
    }
}
