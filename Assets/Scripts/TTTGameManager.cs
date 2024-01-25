using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TTTGameManager : MonoBehaviour
{
    public GameObject[] cells = new GameObject[9]; // Cells assigned from Unity Inspector

    private enum Player { Player1, Player2 }
    public enum CellState { Empty, Player1, Player2 }

    public static TTTGameManager Instance { get; private set; }
    public GameObject xPrefab; // Assign in Unity Inspector
    public GameObject oPrefab; // Assign in Unity Inspector
    public CellState[,] boardState = new CellState[3, 3]; // Tracks the board's state

    private Player currentPlayer = Player.Player1;
    [SerializeField]
    private bool isVsAI;
    public bool isGameOver = false;
    private AI ai;
    public bool isAIMoving = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (isVsAI)
        {
            ai = new AI(this);
        }

    }

    void Start()
    {
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                boardState[i, j] = CellState.Empty;
            }
        }
    }

    public void OnCellClicked(Cell cell)
    {
        if (!IsCellEmpty(cell.X, cell.Y))
        {
            return;
        }

        PlaceMarker(currentPlayer, cell);

        if (CheckForWin())
        {
            EndGame(currentPlayer + " wins!");
        }
        else if (IsBoardFull())
        {
            EndGame("It's a draw!");
        }
        else
        {
            SwitchTurns();
            if (isVsAI && currentPlayer == Player.Player2)
            {
                isAIMoving = true;
                Invoke("AIPerformMove", 1.0f);
            }
        }
    }

    private void PlaceMarker(Player player, Cell cell)
    {
        GameObject markerPrefab = (player == Player.Player1) ? xPrefab : oPrefab;
        Instantiate(markerPrefab, cell.transform.position, Quaternion.identity, cell.transform);

        Button cellButton = cell.GetComponent<Button>();
        if (cellButton != null)
        {
            cellButton.interactable = false;
        }

        int x = cell.X;
        int y = cell.Y;
        boardState[x, y] = player == Player.Player1 ? CellState.Player1 : CellState.Player2;

        UpdateCellDisplay(cell.X, cell.Y, player == Player.Player1 ? CellState.Player1 : CellState.Player2);
    }

    public bool IsCellEmpty(int x, int y)
    {
        return boardState[x, y] == CellState.Empty;
    }

    public bool IsBoardFull()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (boardState[i, j] == CellState.Empty)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool CheckForWin()
    {
        // Check all rows
        for (int row = 0; row < 3; row++)
        {
            if (boardState[row, 0] != CellState.Empty &&
                boardState[row, 0] == boardState[row, 1] &&
                boardState[row, 1] == boardState[row, 2])
            {
                isGameOver = true;
                return true; // There's a winning row
            }
        }

        // Check all columns
        for (int col = 0; col < 3; col++)
        {
            if (boardState[0, col] != CellState.Empty &&
                boardState[0, col] == boardState[1, col] &&
                boardState[1, col] == boardState[2, col])
            {
                isGameOver = true;
                return true; // There's a winning column
            }
        }

        // Check diagonals
        if (boardState[0, 0] != CellState.Empty &&
            boardState[0, 0] == boardState[1, 1] &&
            boardState[1, 1] == boardState[2, 2])
        {
            isGameOver = true;
            return true; // There's a winning diagonal (top-left to bottom-right)
        }

        if (boardState[0, 2] != CellState.Empty &&
            boardState[0, 2] == boardState[1, 1] &&
            boardState[1, 1] == boardState[2, 0])
        {
            isGameOver = true;
            return true; // There's a winning diagonal (top-right to bottom-left)
        }

        return false; // No winner
    }


    private void SwitchTurns()
    {
        currentPlayer = (currentPlayer == Player.Player1) ? Player.Player2 : Player.Player1;
        UpdateTurnDisplay();
    }

    private void EndGame(string message)
    {
        Debug.Log(message);
        // Add logic to handle the end of the game
    }

    private void AIPerformMove()
    {
        // AI move logic - using a simple heuristic for demonstration
        // A more sophisticated approach would be the Minimax algorithm
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (boardState[i, j] == CellState.Empty)
                {
                    // AI places its marker in the first empty cell it finds
                    boardState[i, j] = CellState.Player2; // Assuming AI is Player2
                    UpdateCellDisplay(i, j, CellState.Player2);

                    if (CheckForWin())
                    {
                        EndGame("AI wins!");
                    }
                    else if (IsBoardFull())
                    {
                        EndGame("It's a draw!");
                    }
                    else
                    {
                        isAIMoving = false;
                        SwitchTurns();
                    }
                    return;
                }
            }
        }
    }


    private void UpdateCellDisplay(int x, int y, CellState state)
    {
        GameObject prefabToInstantiate = state == CellState.Player1 ? xPrefab : oPrefab;

        // Calculate the index in the cells array
        int index = x * 3 + y; // Assuming a 3x3 grid

        if (index >= 0 && index < cells.Length)
        {
            // Get the position of the cell
            Vector3 cellPosition = cells[index].transform.position;

            // Instantiate the prefab at the cell's position
            Instantiate(prefabToInstantiate, cellPosition, Quaternion.identity, cells[index].transform);
        }
        else
        {
            Debug.LogError("Invalid cell index: " + index);
        }
    }

    private void UpdateTurnDisplay()
    {
        // Update UI elements for turn display
    }

    public void SetGameMode(bool vsAI)
    {
        isVsAI = vsAI;
    }

    // Add other necessary methods and logic as required
}
