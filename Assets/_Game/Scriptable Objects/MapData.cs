using Unity.AI.Navigation;
using UnityEngine;

[CreateAssetMenu(menuName = "MapData")]
public class MapData : ScriptableObject
{
    [SerializeField] GameObject mapPrefab;
    [SerializeField] NavMeshSurface navMeshSurface;

    public GameObject GetMapPrefab()
    {
        return mapPrefab;
    }

    public NavMeshSurface GetNavMeshSurface()
    {
        return navMeshSurface;
    }
}
