using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGamePlay : UICanvas
{
    private int aliveNum = 0;
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
        UpdateAliveText(aliveNum);
    }

    public void SetAliveNum(int number)
    {
        aliveNum = number;
    }

    private void SettingButton()
    {
        Debug.Log("Setting button");
    }

    private void UpdateAliveText(int alive)
    {
        //get alive character on current map
        aliveText.text = alive.ToString();
    }
}
