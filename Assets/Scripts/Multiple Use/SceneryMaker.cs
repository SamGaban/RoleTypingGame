using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class SceneryMaker : MonoBehaviour
{
    
    [TabGroup("references", "References")] [SerializeField]
    private List<GameObject> isleList;

    private void Start()
     {
         SpawnIsland();
     }
             
     private void SpawnIsland()
     {
         Instantiate(isleList[Random.Range(0, isleList.Count)]);
     
         Invoke(nameof(SpawnIsland), Random.Range(3f, 12f));
     }
}
