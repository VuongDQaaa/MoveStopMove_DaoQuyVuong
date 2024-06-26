using UnityEngine;
using UnityEngine.UI;

public class SkinItem : MonoBehaviour
{
    [SerializeField] private Button itemButton;
    [SerializeField] private Image selectedImage, logIcon, ItemImage;

    [Header("Parameters")]
    public CanvasSkinStore canvasSkinStore;
    public Equipment skin;

    private void Awake()
    {
        itemButton.onClick.AddListener(ItemButton);
    }

    private void Update()
    {
        UpdateItem();
    }

    private void OnDestroy()
    {
        itemButton.onClick.RemoveAllListeners();
    }

    private void ItemButton()
    {
        SoundManager.PlaySound(SoundType.Button);
        canvasSkinStore.currentSelectedSkin = skin;
    }

    private void UpdateItem()
    {
        if (canvasSkinStore != null)
        {
            //selectImage
            if (canvasSkinStore.currentSelectedSkin == skin)
            {
                selectedImage.gameObject.SetActive(true);
            }
            else
            {
                selectedImage.gameObject.SetActive(false);
            }

            //log icon
            if (skin.equipmentStatus == EquipmentStatus.Locked)
            {
                logIcon.gameObject.SetActive(true);
            }
            else
            {
                logIcon.gameObject.SetActive(false);
            }

            //ItemIcon
            ItemImage.sprite = skin.equipmentImage;
        }
    }
}
