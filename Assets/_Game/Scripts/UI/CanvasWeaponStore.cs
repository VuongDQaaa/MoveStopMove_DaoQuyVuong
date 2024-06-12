using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasWeaponStore : UICanvas
{
    [SerializeField] private Button closeButton, nextButton, backButton, selectButton, buyButton;
    [SerializeField] private TextMeshProUGUI weaponName, weaponAblility, weaponPrize, equipedText;
    [SerializeField] private Image weaponImg;
    [SerializeField] private Weapon showedWeapon;
    [SerializeField] private int index;

    void Awake()
    {
        index = 0;
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
        showedWeapon = WeaponManager.Instance.weaponInfor[index];
        //update weapon data
        UpdateWeaponUI();
    }

    private void CloseButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        GameManager.Instance.currentGameState = GameState.Start;
        Close(0);
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }

    private void UpdateWeaponUI()
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
        SoundManager.PlaySound(SoundType.Button);
        if (index < WeaponManager.Instance.weaponInfor.Count - 1)
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
        SoundManager.PlaySound(SoundType.Button);
        if (index > 0)
        {
            index--;
        }
        else
        {
            index = WeaponManager.Instance.weaponInfor.Count - 1;
        }
    }

    private void SelectButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        //update showed weapon status
        showedWeapon.weaponStatus = WeaponStatus.Equiped;
        GameManager.Instance.playerController.ChangeWeapon(showedWeapon);

        //find last equiped weapon => change status into Unlock
        List<Weapon> lastEquipedWeapons = WeaponManager.Instance.weaponInfor.FindAll(weapon => weapon.weaponStatus == WeaponStatus.Equiped);
        if (lastEquipedWeapons != null)
        {
            foreach (Weapon item in lastEquipedWeapons)
            {
                item.weaponStatus = WeaponStatus.Unlocked;
            }
        }

        //Update current equiped weapon
        Weapon currentWeapon = WeaponManager.Instance.weaponInfor.FirstOrDefault(weapon => weapon.id == showedWeapon.id);
        if (currentWeapon != null)
        {
            currentWeapon.weaponStatus = WeaponStatus.Equiped;
        }

        WeaponManager.Instance.SaveToJson();
    }

    private void BuyButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        if (GameManager.Instance.GetCurrentGoldInfor() >= showedWeapon.weaponPrize)
        {
            GameManager.Instance.UpdateGold(-showedWeapon.weaponPrize);
            showedWeapon.weaponStatus = WeaponStatus.Unlocked;

            //Update information in list
            Weapon foundedWeapon = WeaponManager.Instance.weaponInfor.FirstOrDefault(weapon => weapon.id == showedWeapon.id);
            if (foundedWeapon != null)
            {
                foundedWeapon.weaponStatus = WeaponStatus.Unlocked;
            }

            //Update player information
            WeaponManager.Instance.SaveToJson();
        }
    }
}
