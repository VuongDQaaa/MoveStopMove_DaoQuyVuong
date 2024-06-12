using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGamePlay : UICanvas
{
    [SerializeField] private Button settingButton;
    [SerializeField] private TextMeshProUGUI aliveText;

    private void OnEnable()
    {
        settingButton.onClick.AddListener(SettingButton);
    }

    private void OnDisable()
    {
        settingButton.onClick.RemoveAllListeners();
    }

    private void Update()
    {
        UpdateAliveText();
        if(GameManager.Instance.currentGameState == GameState.Start)
        {
            Close(0);
        }
    }

    private void SettingButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        GameManager.Instance.currentGameState = GameState.Pause;
        UIManager.Instance.OpenUI<CanvasSetting>();
    }

    private void UpdateAliveText()
    {
        //get alive character on current map
        aliveText.text = GameManager.Instance.GetAliveCharacter().ToString();
    }
}
