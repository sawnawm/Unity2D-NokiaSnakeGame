using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
//public class BoardMaker : MonoBehaviour
//{
//    public GameObject Cell;

//    private void OnEnable()
//    {
//        float y = 4.5f;
//        for (int i = 0; i < 10; i++)
//        {
//            float x = -4.5f;
            
//            for (int j = 0; j < 10; j++)
//            {
//                GameObject newCell = Instantiate(Cell, gameObject.transform);
//                newCell.gameObject.name = "Cell: " + (i+1) + " " + (j+1);
//                newCell.transform.position = new Vector2(x,y);
//                x += 1f;
//                if(i == 0)
//                {
//                    Board.Instance.CellsCollective.Add(new Column(newCell.GetComponent<Cell>()));
//                }
//                else
//                {
//                    Board.Instance.CellsCollective[j].columnCells.Add(newCell.GetComponent<Cell>());
//                }
//            }
//            y -= 1f;
//        }
//    }
//}
