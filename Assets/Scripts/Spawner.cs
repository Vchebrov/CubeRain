using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    private float _delay = 2f;

    private float _verticalMax = 7f;
    private float _horizontalMax = 9f;
    private float _verticalMin = 3f;
    private float _horizontalMin = 1f;
    
    static public ObjectPool<GameObject> _pool { get; private set; }

    [SerializeField] private GameObject _prefab;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: obj => ActionOnGet(obj),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            defaultCapacity: 5,
            maxSize: 20
        );
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = InitiateCubePosition();
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.GetComponent<Renderer>().material.color = Color.white;
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
    
    private Vector3 InitiateCubePosition()
    {
        float x = Random.Range(_horizontalMin, _horizontalMax);
        float y = Random.Range(_verticalMin, _verticalMax);
        float z = Random.Range(_horizontalMin, _horizontalMax);

        return new Vector3(x, y, z);
    }
}