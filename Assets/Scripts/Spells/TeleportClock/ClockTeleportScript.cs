using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClockTeleportScript : MonoBehaviour
{
    
    private List<KeyCode> keyCodeArray = new List<KeyCode>()
    {
        KeyCode.A,
        KeyCode.D,
        KeyCode.E,
        KeyCode.F,
        KeyCode.Q,
        KeyCode.R,
        KeyCode.S,
        KeyCode.Z
    };

    [TabGroup("references", "Settings")] [SerializeField] [Range(5, 100)]
    private float distance = 5f;


    

    [TabGroup("references", "References")] [SerializeField]
    private GameObject prefab;

    public void Init(Transform newParent, Vector2 newPosition)
    {
        this.transform.SetParent(newParent);

        this.transform.localPosition = newPosition;
        
        for (int i = 0; i < 8; i++)
        {
            Vector2 position;
            
            switch (i)
            {
                case 0:
                    position = new Vector2(0, distance);
                    break;
                case 1:
                    position = new Vector2(distance, distance);
                    break;
                case 2:
                    position = new Vector2(distance * 1.25f, 0);
                    break;
                case 3:
                    position = new Vector2(distance, -distance);
                    break;
                case 4:
                    position = new Vector2(0, -distance);
                    break;
                case 5:
                    position = new Vector2(-distance, -distance);
                    break;
                case 6:
                    position = new Vector2(-distance * 1.25f, 0);
                    break;
                case 7:
                    position = new Vector2(-distance, distance);
                    break;
                default:
                    position = Vector2.zero;
                    break;
            }

            GameObject newObj = Instantiate(prefab);
            TpSpotScript script = newObj.GetComponent<TpSpotScript>();

            int rnd = Random.Range(0, keyCodeArray.Count - 1);
            
            script.Initialize(this.transform, keyCodeArray[rnd], position);
            
            keyCodeArray.RemoveAt(rnd);
        }
    }

    private void Start()
    {
        
    }
}
