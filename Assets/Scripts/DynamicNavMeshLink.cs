using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class DynamicNavMeshLink : MonoBehaviour
{
    private NavMeshLink navMeshLink;
    private Vector3 initialPos;
    public Transform movingPlatform;
    public NavMeshSurface navMeshSurface; // Reference to NavMeshSurface

    void Start()
    {
        navMeshLink = GetComponent<NavMeshLink>();
        initialPos = transform.position;
        if (navMeshSurface != null)
            navMeshSurface.BuildNavMesh(); // Initial bake
    }

    void Update()
    {
        if (movingPlatform != null)
        {
            Vector3 offset = movingPlatform.position - initialPos;
            navMeshLink.transform.position = initialPos + offset;

            if (navMeshSurface != null)
                navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData); // Dynamic rebake
        }
    }
}
