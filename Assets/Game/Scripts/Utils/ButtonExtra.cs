using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonExtra : MonoBehaviour
{
    [field: SerializeField] public string nameTag { get; private set; }
    private Button _thisButton;
    AudioManager _audioManager => AudioManager.I;

    private void Awake()
    {
        _thisButton = GetComponent<Button>();
        _thisButton.onClick.AddListener(MakeSound);
    }

    private void OnEnable()
    {
        _thisButton.interactable = true;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void MakeSound()
    {
        _audioManager.PlaySfx("buttonclick");
        _thisButton.interactable = false;
        StartCoroutine(Reset());
    }        

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(0.1f);
        _thisButton.interactable = true;
    }

}
