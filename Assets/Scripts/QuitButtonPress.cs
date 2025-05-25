using UnityEngine;

public class QuitOnKeyPress : MonoBehaviour
{
    [Header("Press this key to quit the game")]
    public KeyCode quitKey = KeyCode.Escape;  // Default to Escape key

    void Update()
    {
        if (Input.GetKeyDown(quitKey))
        {
            QuitGame();
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        // Stop play mode in the editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application
        Application.Quit();
#endif
    }
}
