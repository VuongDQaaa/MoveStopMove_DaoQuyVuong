using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasWeaponStore : UICanvas
{
    [SerializeField] private Button closeButton, nextButton, backButton, selectButton, buyButton;
    [SerializeField] private TextMeshProUGUI weaponName, weaponAblility, weaponPrize, equipedText;
    [SerializeField] private Image weaponImg;
    private List<Weapon> weaponData;
    private Weapon showedWeapon;
    private int index;

    void Awake()
    {
        index = 0;
        weaponData = WeaponManager.Instance.weaponInfor;
    }

    private void OnEnable()
    {
        closeButton.onClick.AddListener(CloseButton);
        nextButton.onClick.AddListener(NextButton);
        backButton.onClick.AddListener(BackButton);
        selectButton.onClick.AddListener(SelectButton);
        buyButton.onClick.AddListener(BuyButton);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveAllListeners();
        nextButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
        selectButton.onClick.RemoveAllListeners();
        buyButton.onClick.RemoveAllListeners();
    }


    // Update is called once per frame
    void Update()
    {
        showedWeapon = weaponData[index];
        //update weapon data
        UpdateWeaponData();
    }

    private void CloseButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }

    private void UpdateWeaponData()
    {
        weaponName.text = showedWeapon.weaponName;
        weaponAblility.text = showedWeapon.weaponAbility;
        weaponPrize.text = showedWeapon.weaponPrize.ToString();
        weaponImg.sprite = showedWeapon.weaponImage;

        //update button based on weapon status
        switch (showedWeapon.weaponStatus)
        {
            case WeaponStatus.Equiped:
                equipedText.gameObject.SetActive(true);
                selectButton.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(false);
                break;
            case WeaponStatus.Locked:
                equipedText.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(true);
                break;
            case WeaponStatus.Unlocked:
                equipedText.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(true);
                buyButton.gameObject.SetActive(false);
                break;
        }
    }

    private void NextButton()
    {
        if (index < weaponData.Count - 1)
        {
            index++;
        }
        else
        {
            index = 0;
        }
    }

    private void BackButton()
    {
        if (index > 0)
        {
            index--;
        }
        else
        {
            index = weaponData.Count - 1;
        }
    }

    private void SelectButton()
    { }

    private void BuyButton()
    { }

    private void UpdateWeaponInfor(Weapon weaponData)
    { }
}
