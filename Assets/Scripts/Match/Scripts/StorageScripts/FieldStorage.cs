using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldStorage : MonoBehaviour
{
    [SerializeField]
    private RowStorage[] rowStorage;

    public Vector2 GetCellCoordinates(int rowIndex, int cellIndex)
    {
        return rowStorage[rowIndex].GetCellCoordinates(cellIndex);
    }
    
    public Transform GetCellTransform(int rowIndex, int cellIndex)
    {
        return rowStorage[rowIndex].GetCellTransform(cellIndex);
    }

    public float GetCellWidth()
    {
        return rowStorage[1].GetCellWidth();
    }
}
