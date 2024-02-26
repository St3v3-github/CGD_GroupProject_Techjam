using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    public LayerMask playerLayer;
    public float attractionForce;
    public float moveSpeed = 5f;
    public float attractionRadius = 3f;
    public float shakeIntensity = 0.1f; // Adjust the intensity of the shaking
    public float shakeInterval = 1.0f; // Adjust the time between shakes
    private Vector3 initialPosition;
    private bool isAttracting = false;
    private float timeSinceLastShake = 0f;
    GameObject Player;
    GameObject Blackhole;
    private float lifeTime;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {

        Vector3 newScale = new Vector3(2.0f, 2.0f, 2.0f); // Increase by a factor of 2 in all dimensions
        transform.localScale = newScale;
        timeSinceLastShake += Time.deltaTime;

        if (timeSinceLastShake >= shakeInterval)
        {
            // Apply a random shake to the black hole's position in Up, Down, Left, and Right directions
            Vector3 shakeOffset = new Vector3(Random.Range(-shakeIntensity, shakeIntensity), Random.Range(-shakeIntensity, shakeIntensity), 0f);
            transform.position += shakeOffset;

            timeSinceLastShake = 0f;
         }
         if (isAttracting)
         {
            AttractObjects();
         }
        lifeTime += Time.deltaTime;

        // Check if 10 seconds have passed
        if (lifeTime >= 10f)
        {
            Destroy(gameObject); // Destroy the Blackhole object
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("Object In range");
            // Object in the playerLayer collided with the black hole
            isAttracting = true;
            
        }
    }

private void AttractObjects()
{

}
private void OnTriggerStay(Collider col)
{
    if (col.gameObject.CompareTag("Player"))
    {
        Rigidbody rb = col.GetComponentInChildren<Rigidbody>();
        if (rb != null)
        {
            // Attract the object towards the Blackhole's center
            Vector3 attractionDirection = (transform.position - col.transform.position).normalized;
            
            rb.AddForce(attractionDirection * attractionForce);
        }
    }
}

// A helper function to get the root object of a prefab
private GameObject GetRootObject(GameObject obj)
{
    if (obj.transform.parent == null)
    {
        return obj;
    }
    return GetRootObject(obj.transform.parent.gameObject);
}  
}
