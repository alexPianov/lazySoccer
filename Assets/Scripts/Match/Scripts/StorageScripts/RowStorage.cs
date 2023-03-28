using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowStorage : MonoBehaviour
{
    [SerializeField]
    private GameObject[] cellStorage;

    public Vector2 GetCellCoordinates(int cellIndex)
    {
        return cellStorage[cellIndex].transform.position;
    }

    public Transform GetCellTransform(int cellIndex)
    {
        return cellStorage[cellIndex].transform;
    }

    public float GetCellWidth()
    {
        return System.Math.Abs(cellStorage[1].transform.position.x - cellStorage[2].transform.position.x);
    }
}
