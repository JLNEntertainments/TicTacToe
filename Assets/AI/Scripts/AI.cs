using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI
{
    private TTTGameManager gameManager;
    private int maxDepth;

    public AI(TTTGameManager manager, int maxDepth = 9) // Tic-Tac-Toe has a max depth of 9
    {
        gameManager = manager;
        this.maxDepth = maxDepth;
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
                    gameManager.boardState[x, y] = TTTGameManager.CellState.Player2; // AI's move

                    // Compute the score using Minimax with alpha-beta pruning
                    int alpha = int.MinValue;
                    int beta = int.MaxValue;
                    int score = Minimax(0, false, alpha, beta); // Start at depth 0, AI is the minimizing player initially

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



    private int Minimax(int depth, bool isMaximizing, int alpha, int beta)
    {
        int score = EvaluateBoard();

        // Check if the maximum depth has been reached or the game is over
        if (depth >= maxDepth || score != 0 || gameManager.IsBoardFull())
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
                        gameManager.boardState[i, j] = TTTGameManager.CellState.Player2;
                        int scoreTemp = Minimax(depth + 1, false, alpha, beta);
                        gameManager.boardState[i, j] = TTTGameManager.CellState.Empty;
                        bestScore = Mathf.Max(scoreTemp, bestScore);
                        alpha = Mathf.Max(alpha, bestScore);
                        if (beta <= alpha)
                            break;
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
                        gameManager.boardState[i, j] = TTTGameManager.CellState.Player1;
                        int scoreTemp = Minimax(depth + 1, true, alpha, beta);
                        gameManager.boardState[i, j] = TTTGameManager.CellState.Empty;
                        bestScore = Mathf.Min(scoreTemp, bestScore);
                        beta = Mathf.Min(beta, bestScore);
                        if (beta <= alpha)
                            break;
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
