using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CubeKeeper), typeof(ColorChanger))]
public class Spawner : MonoBehaviour
{
    [SerializeField] private float _delay = 1f;

    private CubeKeeper _cubeKeeper;
    private ObjectPool<GameObject> _pool;

    private float _verticalMax = 10f;
    private float _horizontalMax = 9f;
    private float _verticalMin = 3f;
    private float _horizontalMin = 1f;

    private void Awake()
    {
        _cubeKeeper = GetComponent<CubeKeeper>();
    }

    private void Start()
    {
        StartCoroutine(CreateCube());
    }

    private void OnEnable()
    {
        _cubeKeeper.CubeInfo += OnRecievePrefab;
    }

    private void OnDisable()
    {
        _cubeKeeper.CubeInfo -= OnRecievePrefab;
    }

    private void OnRecievePrefab(GameObject prefab)
    {
        Debug.Log($"OnRecievePrefab: {prefab.name}");
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(prefab),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: 5,
            maxSize: 5
        );
    }

    private IEnumerator CreateCube()
    {
        while (true)
        {
            yield return new WaitForSeconds(_delay);
            _pool.Get();
        }
    }

    public IEnumerator RemoveCube(GameObject obj)
    {
        yield return new WaitForSeconds(_delay);
        _pool.Release(obj);
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = InitiateCubePosition();
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.GetComponent<CollisionMonitor>().ResetState();
        obj.SetActive(true);
        obj.GetComponent<Renderer>().material.color = Color.white;
    }

    private Vector3 InitiateCubePosition()
    {
        float x = Random.Range(_horizontalMin, _horizontalMax);
        float y = Random.Range(_verticalMin, _verticalMax);
        float z = Random.Range(_horizontalMin, _horizontalMax);

        return new Vector3(x, y, z);
    }
}