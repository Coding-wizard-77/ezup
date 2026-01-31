using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneLoadAtPosition : MonoBehaviour
{
    [Header("Trigger Position")]
    public Vector3 triggerPosition;
    public float tolerance = 0.2f;

    [Header("Scene (Drag Here)")]
#if UNITY_EDITOR
    [SerializeField] private SceneAsset sceneAsset;
#endif

    [SerializeField, HideInInspector]
    private string sceneName;   // stored safely for runtime

    private bool triggered = false;

#if UNITY_EDITOR
    // This runs automatically when you assign the scene in Inspector
    private void OnValidate()
    {
        if (sceneAsset != null)
        {
            sceneName = sceneAsset.name;
        }
    }
#endif

    void Update()
    {
        if (triggered) return;

        if (Vector3.Distance(transform.position, triggerPosition) <= tolerance)
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                triggered = true;
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError("Scene name is empty. Did you assign the scene?");
            }
        }
    }
}
