using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviourSingleton<MainMenu>
{
    private GameObject _mainMenuGO;

    protected override void Awake()
    {
        base.Awake();

        _mainMenuGO = transform.GetChild(0).gameObject;
        //OpenMainMenu();
    }

    public void OpenMainMenu()
    {
        _mainMenuGO.SetActive(true);
        UnloadLevel();
    }

    public void CloseMainMenu()
    {
        _mainMenuGO.SetActive(false);
        LoadLevel();
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
    }

    private void UnloadLevel()
    {
        SceneManager.UnloadSceneAsync("GameScene");
    }
}
