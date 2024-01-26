using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CloudMaker : MonoBehaviour
{
    [TabGroup("references", "References")] [SerializeField]
    private List<GameObject> cloudList;

    private void Start()
    {
        SpawnCloud();
    }
             
    private void SpawnCloud()
    {
        Instantiate(cloudList[Random.Range(0, cloudList.Count)]);
     
        Invoke(nameof(SpawnCloud), Random.Range(3f, 8f));
    }
}
