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

    private void SoundButton()
    { }

    private void VibrationButton()
    { }

    private void HomeButton()
    {
        Close(0);
        GameManager.Instance.currentGameState = GameState.Start;
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }

    private void ContinueButton()
    {
        Close(0);
        GameManager.Instance.currentGameState = GameState.Playing;
    }
}
