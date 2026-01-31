using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleTable : MonoBehaviour
{
    public string puzzleSceneName = "PuzzleScene";
    public float interactDistance = 2.5f;

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.transform == transform)
            {
                LoadPuzzle();
            }
        }
    }

    void LoadPuzzle()
    {
        SceneManager.LoadScene(puzzleSceneName);
    }
}
