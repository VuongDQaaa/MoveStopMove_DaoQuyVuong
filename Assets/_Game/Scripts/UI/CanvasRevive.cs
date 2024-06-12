using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasRevive : UICanvas
{
    [SerializeField] private Button closeButton, reviveGoldButton, reviveADSButton;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private Image circleImage;
    [SerializeField] private float countDownTime, timeLimit;
    [SerializeField] private float rotateSpeed;

    private void OnEnable()
    {
        countDownTime = timeLimit;
        closeButton.onClick.AddListener(CloseButton);
        reviveGoldButton.onClick.AddListener(ReviveGoldButton);
        reviveADSButton.onClick.AddListener(ReviveADSButton);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveAllListeners();
        reviveGoldButton.onClick.RemoveAllListeners();
        reviveADSButton.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        SpinImage();
        if(countDownTime > 0)
        {
            UpdateCountDown();
        }
        else
        {
            Close(0);
            UIManager.Instance.OpenUI<CanvasDie>();
        }
    }

    private void UpdateCountDown()
    {
        countDownTime -= Time.deltaTime;
        int time = (int)countDownTime;
        countDownText.text = time.ToString();
    }

    private void SpinImage()
    {
        circleImage.gameObject.transform.Rotate(Vector3.forward * -rotateSpeed * Time.deltaTime);
    }

    private void CloseButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        countDownTime = 0;
        Close(0);
        UIManager.Instance.OpenUI<CanvasDie>();
    }

    private void ReviveGoldButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        if(GameManager.Instance.GetCurrentGoldInfor() >= 150)
        {
            reviveGoldButton.gameObject.SetActive(true);
            GameManager.Instance.UpdateGold(-150);
            GameManager.Instance.RevivePlayer();
            Close(0);
        }
    }

    private void ReviveADSButton()
    { }
}
