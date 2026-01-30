using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PuzzleManager : MonoBehaviour
{
    [System.Serializable]
    public class Tile
    {
        public int number;          // 1–8, empty = 0
        public Transform transform; // Tile GameObject
    }

    public Tile[] tiles;   // Size = 9 for 3x3
    public int gridSize = 3;

    private bool puzzleSolved = false;

    // Call this when player clicks a tile
    public void TryMoveTile(int tileIndex)
    {
        if (puzzleSolved)
            return;

        int emptyIndex = GetEmptyTileIndex();

        if (IsAdjacent(tileIndex, emptyIndex))
        {
            SwapTiles(tileIndex, emptyIndex);

            if (IsPuzzleSolved())
            {
                OnPuzzleSolved();
            }
        }
    }

    // ------------------ CORE LOGIC ------------------

    int GetEmptyTileIndex()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].number == 0)
                return i;
        }
        return -1;
    }

    bool IsAdjacent(int a, int b)
    {
        int ax = a % gridSize;
        int ay = a / gridSize;
        int bx = b % gridSize;
        int by = b / gridSize;

        return Mathf.Abs(ax - bx) + Mathf.Abs(ay - by) == 1;
    }

    void SwapTiles(int indexA, int indexB)
    {
        // Swap numbers
        int temp = tiles[indexA].number;
        tiles[indexA].number = tiles[indexB].number;
        tiles[indexB].number = temp;

        // Swap positions
        Vector3 posTemp = tiles[indexA].transform.position;
        tiles[indexA].transform.position = tiles[indexB].transform.position;
        tiles[indexB].transform.position = posTemp;
    }

    // ------------------ SOLVED CHECK ------------------

    bool IsPuzzleSolved()
    {
        // Check all tiles except the last one
        for (int i = 0; i < tiles.Length - 1; i++)
        {
            if (tiles[i].number != i + 1)
                return false;
        }

        // Last tile MUST be empty (0)
        return tiles[tiles.Length - 1].number == 0;
    }


    // ------------------ SOLVED EVENT ------------------

    void OnPuzzleSolved()
    {
        puzzleSolved = true;
        Debug.Log("🎉 PUZZLE SOLVED!");

        // Optional: add effects here
        // Time.timeScale = 0.9f;
        // Show Win UI
        // Play sound
    }

    void Update()
    {
        // PRESS S TO FORCE SOLVE (TEST ONLY)
        if (Input.GetKeyDown(KeyCode.S))
        {
            ForceSolve();
        }
    }

    void ForceSolve()
    {
        for (int i = 0; i < tiles.Length - 1; i++)
        {
            tiles[i].number = i + 1;
        }

        tiles[tiles.Length - 1].number = 0;

        Debug.Log("Force solved puzzle");

        if (IsPuzzleSolved())
        {
            OnPuzzleSolved();
        }
    }

}

