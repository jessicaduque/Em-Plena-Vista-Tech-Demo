using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private float _animationTime = 0.5f;
    private Button[] _buttons = new Button[4];

    [Header("Normal Buttons")]
    [SerializeField] Button b_continue;
    [SerializeField] Button b_menu;

    [Header("Music Settings")]
    [SerializeField] Button b_music;
    SpriteState _musicState;
    [SerializeField] Sprite sprite_musicOffUnpressed;
    [SerializeField] Sprite sprite_musicOffPressed;
    private Sprite sprite_musicOnUnpressed;
    private Sprite sprite_musicOnPressed;
    private Image im_music;
    bool _musicOn;
    bool _musicCooldown = false;

    [Header("Sound Effects Settings")]
    [SerializeField] Button b_effects;
    SpriteState _effectsState;
    [SerializeField] Sprite sprite_effectsOffUnpressed;
    [SerializeField] Sprite sprite_effectsOffPressed;
    private Sprite sprite_effectsOnUnpressed;
    private Sprite sprite_effectsOnPressed;
    private Image im_effects;
    bool _effectsOn;
    bool _effectsCooldown = false;
    
    private const string keyMixerEffects = "Sfx";
    private const string keyMixerMusic = "Music";
    private AudioManager _audioManager => AudioManager.I;
    private void Awake()
    {
        im_music = b_music.GetComponent<Image>();
        im_effects = b_effects.GetComponent<Image>();

        _musicState = b_music.spriteState;
        sprite_musicOnUnpressed = im_music.sprite;
        sprite_musicOnPressed = _musicState.pressedSprite;

        _effectsState = b_effects.spriteState;
        sprite_effectsOnUnpressed = im_effects.sprite;
        sprite_effectsOnPressed = _effectsState.pressedSprite;

        _buttons[0] = b_continue;
        _buttons[1] = b_menu;
        _buttons[2] = b_music;
        _buttons[3] = b_effects;
    }

    private void Start()
    {
        SetupButtons();
    }

    private void OnEnable()
    {
        CheckInicialAudio();
        StartCoroutine(WaitForPanelStart());
    }

    private IEnumerator WaitForPanelStart()
    {
        ResetButtons();

        yield return new WaitForSecondsRealtime(Helpers.panelFadeTime / 2);

        ButtonsAnimation();

        yield return new WaitForSecondsRealtime(_animationTime);

        ButtonsActivationControl(true);
    }


    #region Buttons
    private void SetupButtons()
    {
        b_music.onClick.AddListener(ChangeMusicState);
        b_effects.onClick.AddListener(ChangeEffectsState);
        b_menu.onClick.AddListener(() => { _audioManager.PlayCrossFade("menumusic"); BlackScreenController.I.FadeOutScene("Menu"); });
        b_continue.onClick.AddListener(() => { ButtonsActivationControl(false); UIManager.I.ControlPausePanel(false); });
    }

    private void ResetButtons()
    {
        ButtonsActivationControl(false);

        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].gameObject.transform.localScale = Vector3.zero;
        }
    }

    private void ButtonsActivationControl(bool state)
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].enabled = state;
        }
    }

    private void ButtonsAnimation()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].GetComponent<RectTransform>().DOScale(1, _animationTime).SetEase(Ease.OutBounce).SetDelay(i * 0.1f).SetUpdate(true);
            if(i == 2)
            {
                _buttons[i + 1].GetComponent<RectTransform>().DOScale(1, _animationTime).SetEase(Ease.OutBounce).SetDelay(i * 0.1f).SetUpdate(true);
                i = 3;
            }
        }
    }

    #endregion

    #region Audio 

    private void CheckInicialAudio()
    {
        _musicOn = (PlayerPrefs.HasKey(keyMixerMusic) ? (PlayerPrefs.GetInt(keyMixerMusic) == 0 ? false : true) : true);
        _effectsOn = (PlayerPrefs.HasKey(keyMixerEffects) ? (PlayerPrefs.GetInt(keyMixerEffects) == 0 ? false : true) : true);

        ChangeSpritesMusic();
        ChangeSpritesEffects();

    }

    private void ChangeMusicState()
    {
        if (!_musicCooldown)
        {
            _audioManager.PlaySfx("buttonclick");
            _musicOn = !_musicOn;
            ChangeSpritesMusic();
            _audioManager.ChangeStateMixerMusic(_musicOn);
            StartCoroutine(MusicCooldown());
            b_music.enabled = false;
            _musicCooldown = true;
        }
    }
    private void ChangeEffectsState()
    {
        if (!_effectsCooldown)
        {
            _effectsOn = !_effectsOn;
            ChangeSpritesEffects();
            _audioManager.ChangeStateMixerSFX(_effectsOn);
            if (_effectsOn) { _audioManager.PlaySfx("buttonclick"); }
            StartCoroutine(EffectsCooldown());
            b_effects.enabled = false;
            _effectsCooldown = true;
        }
    }

    private void ChangeSpritesMusic()
    {
        if (_musicOn)
        {
            im_music.sprite = sprite_musicOnUnpressed;
            _musicState.pressedSprite = sprite_musicOnPressed;
        }
        else
        {
            im_music.sprite = sprite_musicOffUnpressed;
            _musicState.pressedSprite = sprite_musicOffPressed;
        }
    }
    private void ChangeSpritesEffects()
    {
        if (_effectsOn)
        {
            im_effects.sprite = sprite_effectsOnUnpressed;
            _effectsState.pressedSprite = sprite_effectsOnPressed;
        }
        else
        {
            im_effects.sprite = sprite_effectsOffUnpressed;
            _effectsState.pressedSprite = sprite_effectsOffPressed;
        }
    }

    #endregion

    #region Cooldowns

    private IEnumerator MusicCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        b_music.enabled = true;
        _musicCooldown = false;
    }

    private IEnumerator EffectsCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        b_effects.enabled = true;
        _effectsCooldown = false;
    }

    #endregion
}
