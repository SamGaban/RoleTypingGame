using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Class used to load a map randomly created based on the questboard item the player clicked in town
/// </summary>
public class MissionInit : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private List<GameObject> mapBlocksPrefabs;

    [TabGroup("references", "References")] [SerializeField]
    private GameObject mapEnd;
    
    void Start()
    {
        BuildMap();
    }

    /// <summary>
    /// Loads a number of map chunks determined by the amount of omen in the current mission
    /// </summary>
    private void BuildMap()
    {
        int i;
        
        for (i = 0; i < GameManager.Instance.OmenAmount(); i++)
        {
            GameObject map = Instantiate(mapBlocksPrefabs[Random.Range(0, mapBlocksPrefabs.Count)]);
            map.transform.position = new Vector3(0, 0 - (i * 170.00f), 0);
        }

        GameObject mapEnd = Instantiate(this.mapEnd);
        mapEnd.transform.position = new Vector3(0, (0 - (i * 170.00f)) - 0.85f, 0);

    }
}
