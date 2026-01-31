using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform piecePrefab;

    private List<Transform> pieces = new List<Transform>();
    private Vector3[] solvedPositions;

    private int size = 4;
    private int emptyLocation;

    private bool shuffling = false;
    private bool puzzleSolved = false;
    private bool hasShuffled = false;

    // ---------------- START ----------------

    void Start()
    {
        CreateGamePieces(0.01f);

        // Store solved layout ONCE
        solvedPositions = new Vector3[pieces.Count];
        for (int i = 0; i < pieces.Count; i++)
        {
            solvedPositions[i] = pieces[i].localPosition;
        }

        Shuffle();
        hasShuffled = true;
    }

    // ---------------- CREATE PIECES ----------------

    private void CreateGamePieces(float gapThickness)
    {
        float width = 1f / size;

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameTransform);
                pieces.Add(piece);

                piece.localPosition = new Vector3(
                    -1 + (2 * width * col) + width,
                    +1 - (2 * width * row) - width,
                    0
                );

                piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
                piece.name = $"{(row * size) + col}";

                if (row == size - 1 && col == size - 1)
                {
                    emptyLocation = (size * size) - 1;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                    float gap = gapThickness / 2;
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];

                    uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
                    uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap));
                    uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
                    uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap));

                    mesh.uv = uv;
                }
            }
        }
    }

    // ---------------- UPDATE ----------------

    void Update()
    {
        // One-shot solved detection
        if (hasShuffled && !puzzleSolved && !shuffling && CheckCompletion())
        {
            OnPuzzleSolved();

        }

        if (puzzleSolved) return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero
            );

            if (hit)
            {
                for (int i = 0; i < pieces.Count; i++)
                {
                    if (pieces[i] == hit.transform)
                    {
                        if (SwapIfValid(i, -size, size)) break;
                        if (SwapIfValid(i, +size, size)) break;
                        if (SwapIfValid(i, -1, 0)) break;
                        if (SwapIfValid(i, +1, size - 1)) break;
                    }
                }
            }
        }
    }

    // ---------------- MOVE LOGIC ----------------

    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if ((i % size != colCheck) && (i + offset == emptyLocation))
        {
            (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
            (pieces[i].localPosition, pieces[i + offset].localPosition) =
            (pieces[i + offset].localPosition, pieces[i].localPosition);

            emptyLocation = i;
            return true;
        }
        return false;
    }

    // ---------------- SOLVED CHECK ----------------

    private bool CheckCompletion()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != $"{i}")
                return false;
        }
        return true;
    }

    // ---------------- SHUFFLE ----------------

    private void Shuffle()
    {
        shuffling = true;

        int count = 0;
        int last = -1;

        while (count < size * size * size)
        {
            int rnd = Random.Range(0, size * size);
            if (rnd == last) continue;

            last = emptyLocation;

            if (SwapIfValid(rnd, -size, size) ||
                SwapIfValid(rnd, +size, size) ||
                SwapIfValid(rnd, -1, 0) ||
                SwapIfValid(rnd, +1, size - 1))
            {
                count++;
            }
        }

        shuffling = false;
    }

    // ---------------- FORCE SOLVE ----------------

    public void ForceSolve()
    {
        // Sort pieces by correct index (name)
        pieces.Sort((a, b) =>
            int.Parse(a.name).CompareTo(int.Parse(b.name))
        );

        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].localPosition = solvedPositions[i];
            pieces[i].gameObject.SetActive(true);
        }

        emptyLocation = (size * size) - 1;
        pieces[emptyLocation].gameObject.SetActive(false);

        Debug.Log("🧩 FORCE SOLVED");
        OnPuzzleSolved();
    }

    private void OnPuzzleSolved()
    {
        if (puzzleSolved) return;

        puzzleSolved = true;
        Debug.Log("🎉 PUZZLE SOLVED!");
        SceneManager.LoadScene("Level-1");
    }

}
