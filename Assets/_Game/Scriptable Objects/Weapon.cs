using UnityEngine;

public enum WeaponType
{
    Axe = 0,
    Knife = 1,
    Boomerang = 2
}
[CreateAssetMenu(menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField] private string weaponName;
    private float bulletSpeed;
    [SerializeField] private GameObject weapmonPrefab;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] protected Material weaponSkin;
}
