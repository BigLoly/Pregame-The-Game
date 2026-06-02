using UnityEngine;

//This script takes a list of assets (pedestrian walkway themed obstacles) and empty gameobjects (bounds). Using the empty gameobjects' Transforms, this scripts spawns those assets within that area, assuming the area is ***rectangular*** and UNCHANGED FROM OLLIE'S PLACEMENT

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawning Variables")]
    [SerializeField] private float density = .5f;
    [SerializeField] private LayerMask layer;
    [SerializeField] private GameObject[] bounds = new GameObject[2];

    [Header("Obstacles")]
    [SerializeField] private GameObject[] obstacles;

    [Header("Temp and Debugging")]
    [SerializeField] private float boundsCenterX;
    [SerializeField] private float boundsCenterZ;
    [SerializeField] private GameObject testCube;

    void Start()
    {
        //calculating dimensions of bounds (for debugging)
        float boundsAreaLength = Mathf.Abs(bounds[0].transform.position.x - bounds[1].transform.position.x);
        float boundsAreaWidth = Mathf.Abs(bounds[0].transform.position.z - bounds[1].transform.position.z);

        boundsCenterX = bounds[0].transform.position.x - (boundsAreaLength / 2);
        boundsCenterZ = bounds[0].transform.position.z - (boundsAreaWidth / 2);

        //Debug.Log($"boundsAreaLength: {boundsAreaLength}\nboundsAreaWidth: {boundsAreaWidth}\nboundsCenterX: {boundsCenterX}\nboundsCenterZ: {boundsCenterZ}");

        Spawning();

        //Instantiate(testCube, new Vector3 (boundsCenterX, 1, boundsCenterZ), Quaternion.identity);
        //OnDrawGizmos();
    }

    void Spawning()
    {
        // for (int i = 0; i < density * 100; i++)
        // {
        //     GameObject obstacle = ChooseRandomObstacleAsset();
        //     Collider obstacleCollider = obstacle.GetComponent<Collider>();
        //     Vector3 positionInBounds = PositionInBounds();
        //     GameObject spawnedObstacle;

        //     Collider[] hitColliders = Physics.OverlapBox(positionInBounds, obstacle.transform.localScale / 2, Quaternion.identity, layer);

        //     if (hitColliders.Length > 0) continue;

        //     if (obstacle == obstacles[0])
        //     {
        //         spawnedObstacle = Instantiate(obstacle, new Vector3(positionInBounds.x, positionInBounds.y, 0), Quaternion.identity);
        //     } else
        //     {
        //         spawnedObstacle = Instantiate(obstacle, positionInBounds, Quaternion.identity);   
        //     }


        // }

        int spawned = 0;
        int attempts = 0;

        //while (spawned < density && attempts < density * 20)
        while (attempts < density * 20)
        {
            attempts++;

            GameObject obstacle = ChooseRandomObstacleAsset();
            Spawnable spawnable = obstacle.GetComponent<Spawnable>();

            Vector3 position = PositionInBounds();

            if (Physics.CheckSphere(position, spawnable.radius, layer)) 
            {
                Debug.Log("Failed Spawn");
                continue;
            }

            Debug.Log("Successful Spawn");

            Instantiate(obstacle, position, Quaternion.identity);

            spawned++;
        }
    }

    GameObject ChooseRandomObstacleAsset()
    {
        return obstacles[Random.Range(0, obstacles.Length)];
    }

    Vector3 PositionInBounds()
    {
        float x = Random.Range(bounds[0].transform.position.x, bounds[1].transform.position.x);
        float z = Random.Range(bounds[0].transform.position.z, bounds[1].transform.position.z);

        return new Vector3(x, .5305f, z);
    }


    //debugging
    void OnDrawGizmos()
    {
        // for (int i = 0; i < bounds.Length; i++)
        // {
        //     Gizmos.DrawCube(bounds[i].transform.position, Vector3.one);
        // }

        Gizmos.DrawCube(new Vector3(boundsCenterX, 1, boundsCenterZ), Vector3.one);
    }
}
