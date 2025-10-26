using System;
using UnityEngine;

[RequireComponent(typeof(ColorChanger))]
public class CubeKeeper : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private LayerMask _layerMask;

    public event Action<GameObject> CubeInfo;

    private void Start()
    {
        CubeInfo?.Invoke(_prefab);
        Debug.Log($"OnEnable. Cube created and sent {_prefab.name}");
    }
}