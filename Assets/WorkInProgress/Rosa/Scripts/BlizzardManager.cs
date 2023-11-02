using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlizzardManager : MonoBehaviour
{
    public GameObject blizzardPrefab;
    public float blizzardHeight = 3.0f;
    public float blizzardSpawnTime = 3.0f;
    public float blizzardDespawnTime = 3.0f;

    public static BlizzardManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SpawnBlizzard(Vector3 position, Quaternion initialRotation)
    {
        Vector3 targetPosition = new Vector3(position.x, 0f, position.z);
        GameObject blizzard = Instantiate(blizzardPrefab, new Vector3(position.x, 0f, position.z), initialRotation);

        StartCoroutine(MoveBlizzardToPos(blizzard, targetPosition, 1.0f));
        StartCoroutine(DespawnBlizzard(blizzard));
    }

    private IEnumerator MoveBlizzardToPos(GameObject blizzard, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = blizzard.transform.position;

        while(elapsedTime < duration)
        {
            blizzard.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blizzard.transform.position = targetPosition;
    }

    private IEnumerator DespawnBlizzard(GameObject blizzard)
    {
        yield return new WaitForSeconds(blizzardDespawnTime);
        Destroy(blizzard);
    }
}
