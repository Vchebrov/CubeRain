using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    private ObjectPool<GameObject> _pool;

    private float _delay = 2f;

    private float _verticalMax = 3f;
    private float _horizontalMax = 9f;
    private float _verticalMin = 10f;
    private float _horizontalMin = 1f;

    private float _hueMin = 0f;
    private float _hueMax = 1f;
    private float _saturationMin = 0.6f;
    private float _saturationMax = 1f;
    private float _valueMin = 0.6f;
    private float _valueMax = 1f;

    private void Awake()
    {
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

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = InitiateCubePosition();
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.SetActive(true);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.5f, 0.5f);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            ChangeColor(other.gameObject);
            Debug.Log($"{other.gameObject.name} meet the cube");
            StartCoroutine(ResetCube(other.gameObject));
        }
    }

    private IEnumerator ResetCube(GameObject obj)
    {
        while (true)
        {
            yield return new WaitForSeconds(_delay);
            _pool.Release(obj);
        }
    }

    private Vector3 InitiateCubePosition()
    {
        float x = Random.Range(_horizontalMin, _horizontalMax);
        float y = Random.Range(_verticalMin, _verticalMax);
        float z = Random.Range(_horizontalMin, _horizontalMax);

        return new Vector3(x, y, z);
    }

    private void ChangeColor(GameObject cube)
    {
        if (cube.TryGetComponent(out Renderer renderer))
        {
            renderer.material.color = Random.ColorHSV(
                _hueMin, _hueMax,
                _saturationMin, _saturationMax,
                _valueMin, _valueMax
            );
        }
        else
        {
            Debug.LogWarning($"{cube.name}: Renderer not found.");
        }
    }
}