using Services;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    private SceneController sceneController;
    private AudioManager audioManager;
    
    private void Start()
    {
        sceneController = ServiceLocator.Get<SceneController>();
        audioManager=ServiceLocator.Get<AudioManager>();
    }

    public void StartGame()
    {
        audioManager.StopSound("MainMenu");
        sceneController.LoadNextScene();
    }

    public void QuitGame()
    {
        sceneController.Quit();
    }

    private void OnDestroy()
    {
        foreach (var button in GetComponentsInChildren<NormalButton>())
        {
            button.ResetButton();
        }
    }
}
