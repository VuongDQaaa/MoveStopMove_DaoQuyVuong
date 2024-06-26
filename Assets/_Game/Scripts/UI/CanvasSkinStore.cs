using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSkinStore : UICanvas
{
    [SerializeField] private Button closeButton, hatButton, pantButton, shieldButton, skinButton, buyButton, unlockButton, selectButton;
    [SerializeField] private TextMeshProUGUI itemAbility, itemPrize, selectedText;
    [Header("Parameters")]
    [SerializeField] private SkinItem itemUI;
    public Equipment currentSelectedSkin;
    [SerializeField] private EquipmentType currentSkinType;
    private Color originButtonColor;
    [SerializeField] private List<Equipment> currentSkins;
    [SerializeField] private List<SkinItem> currentItems;
    [SerializeField] private Transform content;

    private void Awake()
    {
        originButtonColor = hatButton.image.color;
    }

    private void OnEnable()
    {
        HatButton();
        currentSkinType = EquipmentType.Hat;
        closeButton.onClick.AddListener(CloseButton);
        hatButton.onClick.AddListener(HatButton);
        pantButton.onClick.AddListener(PantButton);
        shieldButton.onClick.AddListener(ShieldButton);
        skinButton.onClick.AddListener(SkinButton);
        buyButton.onClick.AddListener(BuyButton);
        unlockButton.onClick.AddListener(UnlockButton);
        selectButton.onClick.AddListener(SelectButton);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveAllListeners();
        hatButton.onClick.RemoveAllListeners();
        pantButton.onClick.RemoveAllListeners();
        shieldButton.onClick.RemoveAllListeners();
        skinButton.onClick.RemoveAllListeners();
        buyButton.onClick.RemoveAllListeners();
        unlockButton.onClick.RemoveAllListeners();
        selectButton.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSkinTypeButton();
        UpdateStoreUI();
    }

    private void UpdateStoreUI()
    {
        //Update store UI follow current selected skin item
        itemPrize.text = currentSelectedSkin.equipmentPrize.ToString();
        itemAbility.text = currentSelectedSkin.equipmentAbility;
        //control buybutton, unlogbutton, selectbutton
        if (currentSelectedSkin.equipmentStatus == EquipmentStatus.Locked)
        {
            buyButton.gameObject.SetActive(true);
            unlockButton.gameObject.SetActive(true);
            selectButton.gameObject.SetActive(false);
            selectedText.gameObject.SetActive(false);
        }
        else if (currentSelectedSkin.equipmentStatus == EquipmentStatus.Unlocked)
        {
            buyButton.gameObject.SetActive(false);
            unlockButton.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(true);
            selectedText.gameObject.SetActive(false);
        }
        else if (currentSelectedSkin.equipmentStatus == EquipmentStatus.Equiped)
        {
            buyButton.gameObject.SetActive(false);
            unlockButton.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(false);
            selectedText.gameObject.SetActive(true);
        }
    }

    private void UpdateSkinTypeButton()
    {
        //hat button
        if (currentSkinType == EquipmentType.Hat)
        {
            Color newColor = hatButton.image.color;
            newColor.a = 0;
            hatButton.image.color = newColor;
        }
        else
        {
            hatButton.image.color = originButtonColor;
        }

        //pant button
        if (currentSkinType == EquipmentType.Pant)
        {
            Color newColor = hatButton.image.color;
            newColor.a = 0;
            pantButton.image.color = newColor;
        }
        else
        {
            pantButton.image.color = originButtonColor;
        }

        //shilde button
        if (currentSkinType == EquipmentType.Shield)
        {
            Color newColor = hatButton.image.color;
            newColor.a = 0;
            shieldButton.image.color = newColor;
        }
        else
        {
            shieldButton.image.color = originButtonColor;
        }
    }

    private void UpdateItem()
    {
        currentSkins = EquipmentManager.Instance.equipmentInfor.FindAll(skin => skin.equipmentType == currentSkinType);
        if (currentSkins != null)
        {
            for (int i = 0; i < currentSkins.Count; i++)
            {
                SkinItem skinItem = Instantiate(itemUI, content);
                skinItem.canvasSkinStore = transform.GetComponent<CanvasSkinStore>();
                skinItem.skin = currentSkins[i];
                currentItems.Add(skinItem);
            }
        }
        currentSelectedSkin = currentSkins[0];
    }

    private void ClearCurrentItemsList()
    {
        foreach (SkinItem item in currentItems)
        {
            Destroy(item.gameObject);
        }
        currentItems.Clear();
    }

    private void CloseButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        GameManager.Instance.currentGameState = GameState.Start;
        Close(0);
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }

    private void HatButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        ClearCurrentItemsList();
        currentSkinType = EquipmentType.Hat;
        UpdateItem();
    }

    private void PantButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        ClearCurrentItemsList();
        currentSkinType = EquipmentType.Pant;
        UpdateItem();
    }

    private void ShieldButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        ClearCurrentItemsList();
        currentSkinType = EquipmentType.Shield;
        UpdateItem();
    }

    private void SkinButton()
    { }

    private void BuyButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        if (GameManager.Instance.GetCurrentGoldInfor() >= currentSelectedSkin.equipmentPrize)
        {
            GameManager.Instance.UpdateGold(-currentSelectedSkin.equipmentPrize);
            currentSelectedSkin.equipmentStatus = EquipmentStatus.Unlocked;

            //Update information in list
            Equipment foundedSkin = EquipmentManager.Instance.equipmentInfor.FirstOrDefault(skin => skin.id == currentSelectedSkin.id);
            if (foundedSkin != null)
            {
                foundedSkin.equipmentStatus = EquipmentStatus.Unlocked;
            }

            //Update player information
            EquipmentManager.Instance.SaveToJson();
        }
    }

    private void UnlockButton()
    { }

    private void SelectButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        //update showed weapon status
        currentSelectedSkin.equipmentStatus = EquipmentStatus.Equiped;
        GameManager.Instance.playerController.ChangeSkin(currentSelectedSkin);

        //find last equiped weapon => change status into Unlock
        List<Equipment> lastEquipedSkins = EquipmentManager.Instance.equipmentInfor.FindAll(skin => skin.equipmentStatus == EquipmentStatus.Equiped
                                                                                        && skin.equipmentType == currentSelectedSkin.equipmentType);
        if (lastEquipedSkins != null)
        {
            foreach (Equipment item in lastEquipedSkins)
            {
                item.equipmentStatus = EquipmentStatus.Unlocked;
            }
        }

        //Update current equiped weapon
        Equipment currentSkin = EquipmentManager.Instance.equipmentInfor.FirstOrDefault(skin => skin.id == currentSelectedSkin.id);
        if (currentSkin != null)
        {
            currentSkin.equipmentStatus = EquipmentStatus.Equiped;
        }

        EquipmentManager.Instance.SaveToJson();
    }
}
