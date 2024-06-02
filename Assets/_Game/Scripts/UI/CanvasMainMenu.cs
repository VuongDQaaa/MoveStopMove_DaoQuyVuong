using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasMainMenu : UICanvas
{
    [SerializeField] private Button ADSButton, vibrationButton, soundButton, weaponButton, skinButton, playButton;
    [SerializeField] private Image vibrationActive, soundActive, vibractionDeactive, soundDeactive;
    [SerializeField] private TextMeshProUGUI goldText, inforText;
    [SerializeField] private GameObject[] leftElements;
    [SerializeField] private GameObject[] rightElements;

    [Header("UI pararemeters")]
    [SerializeField] private float UITransactionSpeed;
    private bool UIMoved;
    [SerializeField] private GameObject lefDirection, rightDirection;

    private void OnEnable()
    {
        ADSButton.onClick.AddListener(ADButton);
        vibrationButton.onClick.AddListener(VibrationButton);
        soundButton.onClick.AddListener(SoundButton);
        weaponButton.onClick.AddListener(WeaponButton);
        skinButton.onClick.AddListener(SkinButton);
        playButton.onClick.AddListener(PlayButton);
    }

    private void Awake()
    {
        UIMoved = false;
    }

    private void Update()
    {
        MoveUIElement();
        goldText.text = GameManager.Instance.GetGold().ToString();
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
        Debug.Log("remove ads fuction");
    }

    private void VibrationButton()
    {
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
        Debug.Log("Weapon fuction");
    }

    private void SkinButton()
    {
        Debug.Log("Skin fuction");
    }

    private void PlayButton()
    {
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


}
