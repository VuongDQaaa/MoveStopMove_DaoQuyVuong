using UnityEngine;
using UnityEngine.UI;

public class CanvasMainMenu : UICanvas
{
    [SerializeField] private Button ADSButton, vibrationButton, soundButton, weaponButton, skinButton, playButton;
    [SerializeField] private Image vibrationActive, soundActive, vibractionDeactive, soundDeactive;
    [SerializeField] private GameObject[] leftElements;
    [SerializeField] private GameObject[] rightElements;

    [Header("UI praremeter")]
    [SerializeField] private float UITransactionSpeed;
    private bool isMoved;
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
        isMoved = false;
    }

    private void Update()
    {
        MoveUIElement();
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
        isMoved = true;
        UIManager.Instance.CloseUI<CanvasMainMenu>(0);
    }

    private void MoveUIElement()
    {
        if (isMoved == true)
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
