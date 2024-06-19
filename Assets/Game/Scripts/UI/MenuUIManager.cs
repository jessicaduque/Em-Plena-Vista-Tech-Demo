using UnityEngine;
using UnityEngine.UI;
using Utils.Singleton;
public class MenuUIManager : Singleton<MenuUIManager>
{
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private Button b_start;
    [SerializeField] private Button b_credits;
    [SerializeField] private Button b_exitCredits;
    [SerializeField] private Button b_exit;

    private BlackScreenController _blackScreenController => BlackScreenController.I;
    private AudioManager _audioManager => AudioManager.I;

    private new void Awake() { }

    private void Start()
    {
        _audioManager.FadeInMusic("menumusic");
        ButtonSetup();
        Helpers.LockMouse(false);
    }

    private void ButtonSetup()
    {
        b_start.onClick.AddListener(StartGame);
        b_credits.onClick.AddListener(() => ControlCreditsPanel(true));
        b_exitCredits.onClick.AddListener(() => ControlCreditsPanel(false));
        b_exit.onClick.AddListener(QuitGame);
    }

    #region Button methods
    private void StartGame()
    {
        Helpers.LockMouse(true);
        _blackScreenController.FadeOutScene("Main");
        _audioManager.PlayCrossFade("mainmusic");
    }

    public void ControlCreditsPanel(bool state)
    {
        _blackScreenController.FadePanel(_creditsPanel, state);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}
