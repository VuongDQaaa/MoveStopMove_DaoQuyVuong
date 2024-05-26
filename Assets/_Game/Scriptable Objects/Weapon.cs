using UnityEngine;

[CreateAssetMenu(menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField] private string weaponName;
    [SerializeField] public GameObject weapmonPrefab;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public float attackRange;
    [SerializeField] public float attackSpeed;
}
