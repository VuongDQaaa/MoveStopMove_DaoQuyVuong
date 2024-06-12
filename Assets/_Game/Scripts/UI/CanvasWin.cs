using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasWin : UICanvas
{
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI goldText;
    private void OnEnable()
    {
        SoundManager.PlaySound(SoundType.Win);
        continueButton.onClick.AddListener(ContinueButton);
        goldText.text = GameManager.Instance.GetRewardInfor().ToString();
        GameManager.Instance.UpdateGold(GameManager.Instance.GetRewardInfor());
        //LevelManager.Instance.UpdateMapLevel();
    }

    private void OnDisable() 
    {
        continueButton.onClick.RemoveAllListeners();
    }

    private void ContinueButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        Close(0);
        GameManager.Instance.currentGameState = GameState.Start;
        GameManager.Instance.ClearMap();
        ObjectPooling.Instance.ClearPool();
        LevelManager.Instance.UpdateMapLevel();
        UIManager.Instance.OpenUI<CanvasMainMenu>();
        GameManager.Instance.SpawnMap();
    }
}
