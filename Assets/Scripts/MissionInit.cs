using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MissionInit : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private List<GameObject> mapBlocksPrefabs;
    
    void Start()
    {
        BuildMap();
    }

    private void BuildMap()
    {
        for (int i = 0; i < GameManager.Instance.OmenAmount(); i++)
        {
            GameObject map = Instantiate(mapBlocksPrefabs[Random.Range(0, mapBlocksPrefabs.Count)]);
            map.transform.position = new Vector3(0, 0 - (i * 170.00f), 0);
        }
    }
}
