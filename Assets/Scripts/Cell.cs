using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // Public variables for setting the cell's grid position in the Unity Inspector
    public int X;
    public int Y;
 
    private Vector3 screenPoint;
    private void Start()
    {
        
        
    }
    private void OnMouseDown()
    {
        if (!TTTGameManager.Instance.isGameOver && !TTTGameManager.Instance.isAIMoving)
        {
            OnClick();
        }
        else
        {
            return;
        }
    }
    public void OnClick()
    {
        
        TTTGameManager.Instance.OnCellClicked(this);
    }
}

