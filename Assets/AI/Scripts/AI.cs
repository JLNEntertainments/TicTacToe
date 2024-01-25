using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI
{
    private TTTGameManager gameManager;

    public AI(TTTGameManager manager)
    {
        gameManager = manager;
    }

    public (int, int) GetBestMove()
    {
        Debug.Log("AI is calculating the best move");
        int bestScore = int.MinValue;
        (int, int) bestMove = (-1, -1);

        // Iterate through all cells
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                // Check if cell is empty
                if (gameManager.IsCellEmpty(x, y))
                {
                    // Make the move
                    gameManager.boardState[x, y] = TTTGameManager.CellState.Player2; // Assuming AI is Player2

                    // Compute the score using Minimax
                    int score = Minimax(3, true);

                    // Undo the move
                    gameManager.boardState[x, y] = TTTGameManager.CellState.Empty;

                    // Update best score and move
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = (x, y);
                    }
                }
            }
        }

        return bestMove;
    }

    private int Minimax(int depth, bool isMaximizing)
    {
        Debug.Log($"Minimax called at depth {depth} - Maximizing: {isMaximizing}");

        int score = EvaluateBoard();
        Debug.Log($"Evaluated board score: {score}");

        // If the game has ended, return the score from the AI's perspective
        if (score != 0 || gameManager.IsBoardFull())
            return score;

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameManager.boardState[i, j] == TTTGameManager.CellState.Empty)
                    {
                        gameManager.boardState[i, j] = TTTGameManager.CellState.Player2; // AI's move
                        int scoreTemp = Minimax(depth + 1, true);
                        gameManager.boardState[i, j] = TTTGameManager.CellState.Empty; // Undo move
                        bestScore = Mathf.Max(scoreTemp, bestScore);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameManager.boardState[i, j] == TTTGameManager.CellState.Empty)
                    {
                        gameManager.boardState[i, j] = TTTGameManager.CellState.Player1; // Player's move
                        int scoreTemp = Minimax(depth + 1, true);
                        gameManager.boardState[i, j] = TTTGameManager.CellState.Empty; // Undo move
                        bestScore = Mathf.Min(scoreTemp, bestScore);
                    }
                }
            }
            return bestScore;
        }
    }

    private int EvaluateBoard()
    {
        // Check rows for a win
        for (int row = 0; row < 3; row++)
        {
            if (gameManager.boardState[row, 0] == gameManager.boardState[row, 1] && gameManager.boardState[row, 1] == gameManager.boardState[row, 2])
            {
                if (gameManager.boardState[row, 0] == TTTGameManager.CellState.Player2) // AI win
                    return +10;
                else if (gameManager.boardState[row, 0] == TTTGameManager.CellState.Player1) // Player win
                    return -10;
            }
        }

        // Check columns for a win
        for (int col = 0; col < 3; col++)
        {
            if (gameManager.boardState[0, col] == gameManager.boardState[1, col] && gameManager.boardState[1, col] == gameManager.boardState[2, col])
            {
                if (gameManager.boardState[0, col] == TTTGameManager.CellState.Player2) // AI win
                    return +10;
                else if (gameManager.boardState[0, col] == TTTGameManager.CellState.Player1) // Player win
                    return -10;
            }
        }

        // Check diagonals for a win
        if (gameManager.boardState[0, 0] == gameManager.boardState[1, 1] && gameManager.boardState[1, 1] == gameManager.boardState[2, 2] ||
            gameManager.boardState[0, 2] == gameManager.boardState[1, 1] && gameManager.boardState[1, 1] == gameManager.boardState[2, 0])
        {
            if (gameManager.boardState[1, 1] == TTTGameManager.CellState.Player2) // AI win
                return +10;
            else if (gameManager.boardState[1, 1] == TTTGameManager.CellState.Player1) // Player win
                return -10;
        }

        return 0; // No winner, return 0
    }


    // Additional methods (e.g., for evaluating the board) can be added here
}
