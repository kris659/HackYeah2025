using UnityEngine;

public class MainMenu : MonoBehaviourSingleton<MainMenu>
{
    private GameObject _mainMenuGO;

    protected override void Awake()
    {
        base.Awake();

        _mainMenuGO = transform.GetChild(0).gameObject;
        OpenMainMenu();
    }

    public void OpenMainMenu()
    {
        _mainMenuGO.SetActive(true);
    }

    public void CloseMainMenu()
    {
        _mainMenuGO.SetActive(false);
    }
}
