using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneLoaderGeneral : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("Drag Scene Here")]
    public SceneAsset sceneAsset;   // Drag & drop scene
#endif

    [SerializeField, HideInInspector]
    private string sceneName;        // Stored safely for runtime

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneAsset != null)
        {
            sceneName = sceneAsset.name;
        }
    }
#endif

    // Call this from Button / Trigger / Script
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene not assigned!");
        }
    }
}
