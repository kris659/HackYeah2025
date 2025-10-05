using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviourSingleton<MainMenu>
{
    private GameObject _mainMenuGO;

    
    protected override void Awake()
    {
        base.Awake();

        _mainMenuGO = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        OpenMainMenu(true);
    }

    public void OpenMainMenu(bool shouldOpenInstantly)
    {
        _mainMenuGO.SetActive(true);
        _mainMenuGO.GetComponent<CanvasGroup>().DOFade(1, shouldOpenInstantly ? 0 : 1f);

        UnloadLevel();

        var clickAction = InputSystem.actions.FindAction("Click2");
        clickAction.canceled += CloseMainMenu;
    }

    private void CloseMainMenu(InputAction.CallbackContext _)
    {        
        _mainMenuGO.GetComponent<CanvasGroup>().DOFade(0, 1f).SetEase(Ease.InSine).onComplete += () => _mainMenuGO.SetActive(false);
        LoadLevel();
        var clickAction = InputSystem.actions.FindAction("Click2");
        clickAction.canceled -= CloseMainMenu;
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
    }

    private void UnloadLevel()
    {
        if(SceneManager.GetSceneByName("GameScene").isLoaded)
            SceneManager.UnloadSceneAsync("GameScene");
    }
}
