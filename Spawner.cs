using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private int _distanceBeetwenSpawn;
    [SerializeField] private int _startSpawnCount;
    [SerializeField] private float _tileLength;
    
    [SerializeField] private Transform _player;

    private List<GameObject> _activeBaseLevel = new List<GameObject>();
    private List<GameObject> _activeEnviromentElements = new List<GameObject>();

    private int _valueSpawnEnviroments = 0;
    private int _valueDestroy1 = 0;
    private int _valueDestroy2 = 0;
    private int _valueDestroy3 = 0;

    [Header("BaseLevel")]
    [SerializeField] private GameObject _levelTemplate;
    [SerializeField] private BaseSpawnPoint _baseSpawnPoint;

    [Header("Enviroment")]
    [SerializeField] private GameObject[] _enviromentTemplates;
    [SerializeField] private EnviromentSpawnPoint[] _enviromentSpawnPoints;
    [SerializeField] private int _enviromentSpawnChance;

    private void Start()
    {
        for(int i = 0; i < _startSpawnCount; i++)
        {
            GenerateBaseLevel(_baseSpawnPoint, _levelTemplate);
            GenerateEnviromentElements(_enviromentSpawnPoints, _enviromentTemplates, _enviromentSpawnChance);
            MoveSpawner();
        }
    }

    private void Update()
    {
        if ((_player.position.z - _tileLength > _activeBaseLevel[0].transform.position.z))
        {
            DeleteTile();
            GenerateBaseLevel(_baseSpawnPoint, _levelTemplate);
            GenerateEnviromentElements(_enviromentSpawnPoints, _enviromentTemplates, _enviromentSpawnChance);
            MoveSpawner();
        }
    }

    private void GenerateBaseLevel(SpawnPoint spawnPoint, GameObject generatedElement)
    {
        GameObject nextTile = Instantiate(generatedElement, spawnPoint.transform.position, Quaternion.identity);
        _activeBaseLevel.Add(nextTile);
    }

    private void GenerateEnviromentElements(SpawnPoint[] spawnPoints, GameObject[] generatedElements, int spawnChance)
    {
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject generatedElement = generatedElements[Random.Range(0, generatedElements.Length)];
            if(Random.Range(0,100) < spawnChance)
            {
                GameObject nextTile = Instantiate(generatedElement, spawnPoints[i].transform.position, Quaternion.identity);
                _activeEnviromentElements.Add(nextTile);
                _valueSpawnEnviroments += 1;
            }
        }
        _valueDestroy1 = _valueDestroy2;
        _valueDestroy2 = _valueDestroy3;
        _valueDestroy3 = _valueSpawnEnviroments;
        _valueSpawnEnviroments = 0;
    }

    private void MoveSpawner()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + _distanceBeetwenSpawn);
    }

    private void DeleteTile()
    {
        Destroy(_activeBaseLevel[0]);
        _activeBaseLevel.RemoveAt(0);
        for (int i = 0; i < _valueDestroy1; i++)
        {
            Destroy(_activeEnviromentElements[0]);
            _activeEnviromentElements.RemoveAt(0);
        }
    }
}
