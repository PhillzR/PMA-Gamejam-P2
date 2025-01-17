using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class CraterGenerator : MonoBehaviour
{
    public static CraterGenerator Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (Application.isPlaying)
        {
            // DontDestroyOnLoad(gameObject);
        }
    }
    
    [SerializeField] GameObject crater;
    [SerializeField] public int numberOfCraters = 10;
    [SerializeField] float moonRadius = 20f;
    [SerializeField][Tooltip("Min Distance between craters.")] float minDistance = 5f;
    
    Vector2 RandomOnUnitCircle()
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
    void Start()
    {
        ClearAndSpawn();
    }

    public void ClearAndSpawn()
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
            
        }

        foreach (GameObject child in children)
        {
            DestroyImmediate(child);
        }
        
        Spawn();
    }

    private void Spawn()
    {
        List<Vector3> positions = new List<Vector3>();
        float segmentSize = 2 * Mathf.PI / numberOfCraters;
        for (int i = 0; i < numberOfCraters; i++)
        {
            Vector3 randomPosition;
            bool positionValid;
            do
            {
                float angle = Random.Range(segmentSize * i, segmentSize * (i + 1));
                randomPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * moonRadius;
                randomPosition += transform.position;
                positionValid = true;
                foreach (Vector3 existingPosition in positions)
                {
                    if (Vector3.Distance(randomPosition, existingPosition) < minDistance)
                    {
                        positionValid = false;
                        break;
                    }
                }
            } while (!positionValid);
            positions.Add(randomPosition);
            
            // Random position around the moon
            // Rotate the crater so that their normals point towards the moon
            Vector3 direction = (transform.position - randomPosition).normalized;
            float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rotationAngle += 90;
            Quaternion rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
            Instantiate(crater, randomPosition, rotation, this.transform);
        }
    }
    
    // Get closest crater to the right of a point and return the crater and the distance
    public (MoonCrater, float) GetClosestCraterRight(Vector3 point)
    {
        MoonCrater[] craters = Instance.GetComponentsInChildren<MoonCrater>();
        MoonCrater closestCrater = null;
        float closestDistance = float.MaxValue;
        foreach (MoonCrater crater in craters)
        {
            // Debug.Log("point" + point.y + "crater" + crater.transform.position.y);
            if (crater.transform.position.x > point.x && crater.transform.position.y > -5 && crater.isPatched == false)
            {
                // float distance = Vector3.Distance(point, crater.transform.position);
                float distance = Mathf.Abs(point.x - crater.transform.position.x);
                if (distance < closestDistance)
                {
                    closestCrater = crater;
                    closestDistance = distance;
                }
            }
        }
        return (closestCrater, closestDistance);
    }
}
