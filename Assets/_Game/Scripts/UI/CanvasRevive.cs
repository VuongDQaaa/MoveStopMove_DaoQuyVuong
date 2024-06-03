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
        UpdateCountDown();
        if(countDownTime <= 0.2f)
        {
            Close(0);
            UIManager.Instance.OpenUI<CanvasDie>();
        }
    }

    private void UpdateCountDown()
    {
        if (countDownTime > 0)
        {
            countDownTime -= Time.deltaTime;
            int time = (int)countDownTime;
            countDownText.text = time.ToString();
        }
        else
        {
            countDownTime = timeLimit;
        }
    }

    private void SpinImage()
    {
        circleImage.gameObject.transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }
    
    private void CloseButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasDie>();
    }

    private void ReviveGoldButton()
    { }

    private void ReviveADSButton()
    { }
}
