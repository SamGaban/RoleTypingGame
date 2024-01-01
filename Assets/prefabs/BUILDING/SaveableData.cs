using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveableObjectData
{
    // Transform data
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    // Custom data from the "Buildable" script or other components
    public int buildIndex; // or prefabID or any other identifier
    // ... add other fields as necessary

    // Constructor to create save data from a GameObject
    public SaveableObjectData(GameObject obj)
    {
        // Capture transform data
        position = obj.transform.position;
        rotation = obj.transform.rotation;
        scale = obj.transform.localScale;

        // Capture custom data
        Buildable buildable = obj.GetComponent<Buildable>();
        if(buildable != null)
        {
            buildIndex = buildable.buildIndex;
            // ... capture other relevant data from the script
        }
        // ... capture data from other components if needed
    }
}
