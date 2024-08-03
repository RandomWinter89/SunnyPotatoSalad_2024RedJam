using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData : MonoBehaviour
{
    [SerializeField] private MeshRenderer groundMesh;

    public static WorldData Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 GetRandomWorldPosition()
    {
        Bounds bounds = groundMesh.bounds;

        Vector3 groundPos = groundMesh.transform.position;

        // convert to global position and get random
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;

        Vector3 randPos = new Vector3(Random.Range(min.x, max.x),
                                        Random.Range(min.y, max.y),
                                         Random.Range(min.z, max.z));
        Vector3 position = new Vector3(groundPos.x + randPos.x, 0f, groundPos.z + randPos.z);

        return position;
    }
}
