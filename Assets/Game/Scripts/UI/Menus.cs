using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject menu;

    private void Awake()
    {
        CursorSetup();
    }

    void CursorSetup()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    #region Scenes Control
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion

    #region Panels Control
    public void ControlCreditsPanel(bool state)
    {
        menu.SetActive(!state);
        credits.SetActive(state);
    }

    #endregion
}
