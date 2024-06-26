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
    [SerializeField] private Equipment showedWeapon;
    [SerializeField] private int index;
    [SerializeField] private List<Equipment> weaponsList;

    void Awake()
    {
        index = 0;
        weaponsList = EquipmentManager.Instance.equipmentInfor.FindAll(weapon => weapon.equipmentType == EquipmentType.Weapon);
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
        showedWeapon = weaponsList[index];
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
        weaponName.text = showedWeapon.equipmentName;
        weaponAblility.text = showedWeapon.equipmentAbility;
        weaponPrize.text = showedWeapon.equipmentPrize.ToString();
        weaponImg.sprite = showedWeapon.equipmentImage;

        //update button based on weapon status
        switch (showedWeapon.equipmentStatus)
        {
            case EquipmentStatus.Equiped:
                equipedText.gameObject.SetActive(true);
                selectButton.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(false);
                break;
            case EquipmentStatus.Locked:
                equipedText.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(true);
                break;
            case EquipmentStatus.Unlocked:
                equipedText.gameObject.SetActive(false);
                selectButton.gameObject.SetActive(true);
                buyButton.gameObject.SetActive(false);
                break;
        }
    }

    private void NextButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        if (index < weaponsList.Count - 1)
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
            index = weaponsList.Count - 1;
        }
    }

    private void SelectButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        //update showed weapon status
        showedWeapon.equipmentStatus = EquipmentStatus.Equiped;
        GameManager.Instance.playerController.ChangeWeapon(showedWeapon);

        //find last equiped weapon => change status into Unlock
        List<Equipment> lastEquipedWeapons = EquipmentManager.Instance.equipmentInfor.FindAll(weapon => weapon.equipmentType == EquipmentType.Weapon
                                                                                                    && weapon.equipmentStatus == EquipmentStatus.Equiped);
        if (lastEquipedWeapons != null)
        {
            foreach (Equipment item in lastEquipedWeapons)
            {
                item.equipmentStatus = EquipmentStatus.Unlocked;
            }
        }

        //Update current equiped weapon
        Equipment currentWeapon = EquipmentManager.Instance.equipmentInfor.FirstOrDefault(weapon => weapon.id == showedWeapon.id);
        if (currentWeapon != null)
        {
            currentWeapon.equipmentStatus = EquipmentStatus.Equiped;
        }

        EquipmentManager.Instance.SaveToJson();
    }

    private void BuyButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        if (GameManager.Instance.GetCurrentGoldInfor() >= showedWeapon.equipmentPrize)
        {
            GameManager.Instance.UpdateGold(-showedWeapon.equipmentPrize);
            showedWeapon.equipmentStatus = EquipmentStatus.Unlocked;

            //Update information in list
            Equipment foundedWeapon = EquipmentManager.Instance.equipmentInfor.FirstOrDefault(weapon => weapon.id == showedWeapon.id);
            if (foundedWeapon != null)
            {
                foundedWeapon.equipmentStatus = EquipmentStatus.Unlocked;
            }

            //Update player information
            EquipmentManager.Instance.SaveToJson();
        }
    }
}
