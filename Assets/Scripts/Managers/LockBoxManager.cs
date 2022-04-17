using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBoxManager : MonoBehaviour
{
    [SerializeField] private Transform[] _objectsSpawningSpots;
    [SerializeField] private LockBoxObjectData[] _lockBoxObjectsPool;
    [SerializeField] private LockBox _lockBox;

    private List<int> _occupiedSpots = new List<int>();
    private List<LockBoxObjectData> _selectedObjects = new List<LockBoxObjectData>();

    private void Start()
    {
        SelectThreeRandomObjects();
        SpawnObjectsInRandomSpots();
        _lockBox.SetupPassword(_selectedObjects);
    }

    private void SelectThreeRandomObjects()
    {
        int safeFailCounter = 0;
        while (_selectedObjects.Count < 5)
        {
            safeFailCounter++;
            int rand = UnityEngine.Random.Range(0, _lockBoxObjectsPool.Length);

            if (!_selectedObjects.Contains(_lockBoxObjectsPool[rand]))
            {
                _selectedObjects.Add(_lockBoxObjectsPool[rand]);
            }
            if (safeFailCounter >= 100)
                break;
        }
    }

    private void SpawnObjectsInRandomSpots()
    {
        int spotsNumber = UnityEngine.Random.Range(3, _objectsSpawningSpots.Length);

        for (int i = 0; i < _selectedObjects.Count; i++)
        {
            int numberOfObjectsToSpawn = UnityEngine.Random.Range(0, spotsNumber - _occupiedSpots.Count);

            if (i == _selectedObjects.Count - 1)
                numberOfObjectsToSpawn += (spotsNumber - _occupiedSpots.Count) + numberOfObjectsToSpawn;

            _selectedObjects[i].Amount = numberOfObjectsToSpawn;

            int safeFailCounter = 0;

            while (numberOfObjectsToSpawn > 0)
            {
                int rand = UnityEngine.Random.Range(0, _objectsSpawningSpots.Length);

                if (!_occupiedSpots.Contains(rand))
                {
                    GameObject obj = Instantiate(_selectedObjects[i].Prefab, _objectsSpawningSpots[rand].position, Quaternion.identity);
                    obj.transform.parent = _objectsSpawningSpots[rand];
                    _occupiedSpots.Add(rand);
                    numberOfObjectsToSpawn--;
                }
                if (safeFailCounter >= 100)
                    break;
            }
        }
    }

  
}
