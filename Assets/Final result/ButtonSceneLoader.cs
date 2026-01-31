using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonSceneLoader : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("Drag Scene Here")]
    public SceneAsset sceneAsset;
#endif

    [SerializeField, HideInInspector]
    private string sceneName;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneAsset != null)
        {
            sceneName = sceneAsset.name;
        }
    }
#endif

    // This is what the BUTTON calls
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
