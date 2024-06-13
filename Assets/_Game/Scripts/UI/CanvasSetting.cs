using UnityEngine;
using UnityEngine.UI;

public class CanvasSetting : UICanvas
{
    [Header("UIComponents")]
    [SerializeField] private Button soundButton, vibrationButton, homeButton, continueButton;
    [SerializeField] private Image soundIcon, muteIcon, vibrationIcon, standIcond, hideSoud, hideVibration;

    [Header("Parameters")]
    private float offXPostion, onXPostion;

    void OnEnable()
    {
        soundButton.onClick.AddListener(SoundButton);
        vibrationButton.onClick.AddListener(VibrationButton);
        homeButton.onClick.AddListener(HomeButton);
        continueButton.onClick.AddListener(ContinueButton);
    }

    void OnDisable()
    {
        soundButton.onClick.RemoveAllListeners();
        vibrationButton.onClick.RemoveAllListeners();
        homeButton.onClick.RemoveAllListeners();
        continueButton.onClick.RemoveAllListeners();
    }

    private void Update()
    {
        UpdateSoundIcons();
    }

    private void UpdateSoundIcons()
    {
        if (SoundManager.Instance.currentVolume > 0)
        {
            soundIcon.gameObject.SetActive(true);
            muteIcon.gameObject.SetActive(false);
        }
        else
        {
            soundIcon.gameObject.SetActive(false);
            muteIcon.gameObject.SetActive(true);
        }
    }

    private void SoundButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        Debug.Log("remove ads fuction");
    }

    private void VibrationButton()
    { }

    private void HomeButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        Close(0);
        ObjectPooling.Instance.ClearPool();
        GameManager.Instance.ClearMap();
        GameManager.Instance.SpawnMap();
        GameManager.Instance.currentGameState = GameState.Start;
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }

    private void ContinueButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        Close(0);
        GameManager.Instance.currentGameState = GameState.Playing;
    }
}
