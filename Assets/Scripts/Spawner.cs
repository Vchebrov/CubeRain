using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CollisionMonitor))]
public class Spawner : MonoBehaviour
{
    [SerializeField] private float _delay = 1f;
  
    [SerializeField] private GameObject _prefab;

    private ObjectPool<GameObject> _pool;

    private float _verticalMax = 10f;
    private float _horizontalMax = 9f;
    private float _verticalMin = 3f;
    private float _horizontalMin = 1f;

    private void Start()
    {
        StartCoroutine(CreateCube());
    }

    private void Awake()
    {
        Debug.Log($"OnRecievePrefab: {_prefab.name}");
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_prefab),
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
        while (_pool == null)
            yield return null;

        while (enabled)
        {
            _pool.Get();
            yield return new WaitForSeconds(_delay);
        }
    }

    public IEnumerator RemoveCube(Transform objTransform)
    {
        Debug.Log($"OnRemoveCube Coroutine: {objTransform.name}");
        var obj = objTransform.gameObject;
        yield return new WaitForSeconds(_delay);
        _pool.Release(obj);
    }

    private void OnRemoveCube(Transform objTransform)
    {
        Debug.Log($"OnRemoveCube: {objTransform.name}");
        StartCoroutine(RemoveCube(objTransform));
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = InitiateCubePosition();
        obj.transform.position = InitiateCubePosition();

        if (obj.TryGetComponent(out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;
            rb.Sleep();
        }

        if (obj.TryGetComponent(out CollisionMonitor collisionMonitor))
        {
            collisionMonitor.ResetState();
            collisionMonitor.OnTouched -= OnRemoveCube;
            collisionMonitor.OnTouched += OnRemoveCube;
        }

        obj.SetActive(true);

        if (obj.TryGetComponent(out Renderer rend))
        {
            var objMaterial = rend.material;
            objMaterial.color = Color.white;
        }
    }

    private Vector3 InitiateCubePosition()
    {
        float x = Random.Range(_horizontalMin, _horizontalMax);
        float y = Random.Range(_verticalMin, _verticalMax);
        float z = Random.Range(_horizontalMin, _horizontalMax);

        return new Vector3(x, y, z);
    }
}