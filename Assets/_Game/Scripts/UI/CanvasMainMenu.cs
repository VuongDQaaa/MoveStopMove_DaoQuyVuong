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
    [SerializeField] private List<GameObject> leftElements;
    [SerializeField] private List<GameObject> rightElements;

    [Header("UI pararemeters")]
    [SerializeField] private float UITransactionSpeed;
    private bool UIMoved;
    [SerializeField] private GameObject lefDirection, rightDirection;
    [SerializeField] private List<float> leftOriginPos;
    [SerializeField] private List<float> rightOriginPos;

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
        RestoreUIElements();
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
        if (soundActive.IsActive())
        {
            soundActive.gameObject.SetActive(false);
            soundDeactive.gameObject.SetActive(true);
        }
        else
        {
            soundActive.gameObject.SetActive(true);
            soundDeactive.gameObject.SetActive(false);
        }
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
            foreach (GameObject item in leftElements)
            {
                Vector3 dir = item.transform.position;
                dir.x = lefDirection.transform.position.x;
                item.transform.position = Vector3.MoveTowards(item.transform.position, dir, UITransactionSpeed);
            }

            foreach (GameObject item in rightElements)
            {
                Vector3 dir = item.transform.position;
                dir.x = rightDirection.transform.position.x;
                item.transform.position = Vector3.MoveTowards(item.transform.position, dir, UITransactionSpeed);
            }
        }
    }

    private void RestoreUIElements()
    {
        if (UIMoved == false && leftOriginPos.Count > 0 && rightOriginPos.Count > 0)
        {
            foreach (GameObject item in leftElements)
            {
                int index = leftElements.IndexOf(item);
                Vector3 dir = item.transform.position;
                dir.x = leftOriginPos[index];
                item.transform.position = Vector3.MoveTowards(item.transform.position, dir, UITransactionSpeed);
            }

            foreach (GameObject item in rightElements)
            {
                int index = rightElements.IndexOf(item);
                Vector3 dir = item.transform.position;
                dir.x = rightOriginPos[index];
                item.transform.position = Vector3.MoveTowards(item.transform.position, dir, UITransactionSpeed);
            }
        }
    }

    IEnumerator SaveOriginPos()
    {
        yield return new WaitForSeconds(0.1f);
        //Save origin postion for each UI element before move
        foreach (GameObject item in leftElements)
        {
            leftOriginPos.Add(item.transform.position.x);
        }
        foreach (GameObject item in rightElements)
        {
            rightOriginPos.Add(item.transform.position.x);
        }
    }
}
