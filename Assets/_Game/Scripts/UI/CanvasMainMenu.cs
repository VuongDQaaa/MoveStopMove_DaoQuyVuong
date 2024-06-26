using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class CanvasMainMenu : UICanvas
{
    [SerializeField] private Button ADSButton, vibrationButton, soundButton, weaponButton, skinButton, playButton;
    [SerializeField] private Image vibrationActive, soundActive, vibractionDeactive, soundDeactive;
    [SerializeField] private TextMeshProUGUI goldText, inforText;
    [SerializeField] private List<RectTransform> leftElements;
    [SerializeField] private List<RectTransform> rightElements;

    [Header("UI pararemeters")]
    [SerializeField] private float UITransactionSpeed;
    private bool UIMoved;
    [SerializeField] private List<Vector2> leftOriginPos;
    [SerializeField] private List<Vector2> rightOriginPos;

    private void Awake()
    {
        GameManager.Instance.SpawnMap();
        UIMoved = false;
        StartCoroutine(SaveOriginPos());
    }

    private void OnEnable()
    {
        goldText.text = GameManager.Instance.GetCurrentGoldInfor().ToString();
        ADSButton.onClick.AddListener(ADButton);
        vibrationButton.onClick.AddListener(VibrationButton);
        soundButton.onClick.AddListener(SoundButton);
        weaponButton.onClick.AddListener(WeaponButton);
        skinButton.onClick.AddListener(SkinButton);
        playButton.onClick.AddListener(PlayButton);
    }

    private void Update()
    {
        if (GameManager.Instance.currentGameState == GameState.Start)
        {
            UIMoved = false;
        }
        else
        {
            UIMoved = true;
        }

        MoveUIElement();
        UpdateSoundIcon();
        inforText.text = $"ZONE {LevelManager.Instance.GetMapLevel()} - BEST #100";
    }

    private void OnDisable()
    {
        ADSButton.onClick.RemoveAllListeners();
        vibrationButton.onClick.RemoveAllListeners();
        soundButton.onClick.RemoveAllListeners();
        weaponButton.onClick.RemoveAllListeners();
        skinButton.onClick.RemoveAllListeners();
        playButton.onClick.RemoveAllListeners();
    }

    private void UpdateSoundIcon()
    {
        if (SoundManager.Instance.currentVolume > 0)
        {
            soundActive.gameObject.SetActive(true);
            soundDeactive.gameObject.SetActive(false);
        }
        else
        {
            soundActive.gameObject.SetActive(false);
            soundDeactive.gameObject.SetActive(true);
        }
    }

    private void ADButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        Debug.Log("remove ads fuction");
    }

    private void VibrationButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        if (vibrationActive.IsActive())
        {
            vibrationActive.gameObject.SetActive(false);
            vibractionDeactive.gameObject.SetActive(true);
        }
        else
        {
            vibrationActive.gameObject.SetActive(true);
            vibractionDeactive.gameObject.SetActive(false);
        }
    }

    private void SoundButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        SoundManager.SoundSetting();
    }

    private void WeaponButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        GameManager.Instance.currentGameState = GameState.Shopping;
        Close(0);
        UIManager.Instance.OpenUI<CanvasWeaponStore>();
    }

    private void SkinButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        GameManager.Instance.currentGameState = GameState.Shopping;
        Close(0);
        UIManager.Instance.OpenUI<CanvasSkinStore>();
    }

    private void PlayButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        UIMoved = true;
        GameManager.Instance.currentGameState = GameState.Playing;
        Close(1.5f);
        UIManager.Instance.OpenUI<CanvasGamePlay>();
    }

    private void MoveUIElement()
    {
        if (UIMoved == true)
        {
            foreach (var item in leftElements)
            {
                Vector2 dir = item.anchoredPosition;
                dir.x = Constant.MAINMENU_UI_MOVE_LEFT;
                item.anchoredPosition = Vector2.MoveTowards(item.anchoredPosition, dir, UITransactionSpeed);
            }

            foreach (RectTransform item in rightElements)
            {
                Vector2 dir = item.anchoredPosition;
                dir.x = Constant.MAINMENU_UI_MOVE_RIGHT;
                item.anchoredPosition = Vector2.MoveTowards(item.anchoredPosition, dir, UITransactionSpeed);
            }
        }
        else if (rightOriginPos.Count > 0 && leftOriginPos.Count > 0)
        {
            foreach (var item in leftElements)
            {
                int index = leftElements.IndexOf(item);
                if (item.anchoredPosition != leftOriginPos[index])
                {
                    Vector2 currentPos = item.anchoredPosition;
                    item.anchoredPosition = Vector2.MoveTowards(currentPos, leftOriginPos[index], UITransactionSpeed);
                }
            }

            foreach (var item in rightElements)
            {
                int index = rightElements.IndexOf(item);
                if (item.anchoredPosition != rightOriginPos[index])
                {
                    Vector2 currentPos = item.anchoredPosition;
                    item.anchoredPosition = Vector2.MoveTowards(currentPos, rightOriginPos[index], UITransactionSpeed);
                }
            }
        }
    }

    IEnumerator SaveOriginPos()
    {
        yield return new WaitForSeconds(0.01f);
        //Save origin postion for each UI element before move
        foreach (var item in leftElements)
        {
            leftOriginPos.Add(item.anchoredPosition);
        }
        foreach (var item in rightElements)
        {
            rightOriginPos.Add(item.anchoredPosition);
        }
    }
}
