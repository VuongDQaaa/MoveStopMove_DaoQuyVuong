using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDie : UICanvas
{
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI rattingText, killerNameText, goldText;

    private void OnEnable()
    {
        continueButton.onClick.AddListener(ContinueButton);
        rattingText.text = "#" + GameManager.Instance.GetRank().ToString();
        goldText.text = GameManager.Instance.GetReward().ToString();
        killerNameText.text = GameManager.Instance.GetKillerName();
        GameManager.Instance.UpdateGold(GameManager.Instance.GetReward());
    }

    private void OnDisable()
    {
        continueButton.onClick.RemoveAllListeners();
    }

    private void ContinueButton()
    {
        Close(0);
        GameManager.Instance.currentGameState = GameState.Start;
        GameManager.Instance.ClearMap();
        ObjectPooling.Instance.ClearPool();
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }
}
