using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private GameObject weaponOnHand;

    public void AttackEvent()
    {
        character.AttackEvent();
        SoundManager.PlaySound(SoundType.Attack);
    }

    public void HideWeapon()
    {
        weaponOnHand.SetActive(false);
    }

    public void ShowWeapon()
    {
        weaponOnHand.SetActive(true);
    }
}
