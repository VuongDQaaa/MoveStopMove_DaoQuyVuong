using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSkinStore : UICanvas
{
    [SerializeField] private Button closeButton, hatButton, pantButton, shieldButton, skinButton, buyButton, unlockButton, selectButton;
    [SerializeField] private TextMeshProUGUI itemAbility, itemPrize, selectedText;

    private void OnEnable()
    {
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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CloseButton()
    { }

    private void HatButton()
    { }

    private void PantButton()
    { }

    private void ShieldButton()
    { }

    private void SkinButton()
    { }

    private void BuyButton()
    { }

    private void UnlockButton()
    { }

    private void SelectButton()
    { }
}
