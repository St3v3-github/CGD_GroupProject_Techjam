using System.Collections;
using UnityEngine;

public class WallManager : Spell
{
    public float wallHeight = 3.0f; 
    public float wallSpawnTime = 3.0f; 
    public float wallDespawnTime = 3.0f; 

    public static WallManager Instance { get; private set; }

    private void Start()
    {
        
    }

    private void Awake()
    {
        if (Instance == null)
        {
         //   SetStatus();
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SpawnWall(Vector3 position, Quaternion initialRotation)
    {
        Vector3 targetPosition = new Vector3(position.x, position.y, position.z);

       /* GameObject wall = Instantiate(spell.prefab, new Vector3(position.x, position.y, position.z), initialRotation);

        StartCoroutine(MoveWallToPosition(wall, targetPosition, 1.0f));
        StartCoroutine(DisappearWall(wall));*/
    }

    private IEnumerator MoveWallToPosition(GameObject wall, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = wall.transform.position;

        while (elapsedTime < duration)
        {
            wall.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        wall.transform.position = targetPosition;
    }

    private IEnumerator DisappearWall(GameObject wall)
    {
        yield return new WaitForSeconds(wallDespawnTime);

        Destroy(wall);
    }
}