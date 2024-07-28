using UnityEngine;
using UnityEngine.UIElements;

public class PlanetManager : MonoBehaviour
{

    public static PlanetManager Instance { get; private set; }
    public Camera CurrentCamera { get; private set; }
    [field: SerializeField] public Vector2 DeathHeight { get; set; } = new Vector2(0f, 5f);

    [SerializeField] private float gravityScale = 1;
    [SerializeField] private PlanetScriptObject[] planets;

    [SerializeField] private Vector2 dropCenter = Vector2.zero;
    [SerializeField] private Vector2 nextSphereLocation = Vector2.zero;
    [SerializeField] private int planetIndexSpawnRange = -1;

    private GameObject currentSphere, nextSphere;

    private void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CurrentCamera = Camera.main;
        Physics.gravity = new Vector3(0, -gravityScale, 0);
        currentSphere = SpawnRandomPlanet();
        nextSphere = SpawnRandomPlanet();
        nextSphere.transform.position = nextSphereLocation;
    }

    private void Update()
    {
        Vector3 mousePoint = CurrentCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 planetPosition = new Vector2(mousePoint.x, dropCenter.y);
        currentSphere.transform.position = planetPosition;
    }

    public void DropPlanet()
    {
        currentSphere.GetComponent<Rigidbody>().isKinematic = false;
        currentSphere.GetComponent<Collider>().enabled = true;
        currentSphere = nextSphere;
        nextSphere = SpawnRandomPlanet();
        nextSphere.transform.position = nextSphereLocation;
    }

    public GameObject GetNextPlanet(string planetName)
    {
        for(int i = 0; i < planets.Length; i++)
        {
            if (!planets[i].name.Equals(planetName))
                continue;

            if (i + 1 >= planets.Length)
                break;

            GameObject planet = SpawnPlanet(i + 1);
            planet.GetComponent<Rigidbody>().isKinematic = false;
            planet.GetComponent<Collider>().enabled = true;
            return planet;
        }

        return null;
    }

    private GameObject SpawnRandomPlanet()
    {
        int length = (planetIndexSpawnRange < 0 || planetIndexSpawnRange >= planets.Length) ? planets.Length : planetIndexSpawnRange;
        return SpawnPlanet(Random.Range(0, length));
    }

    private GameObject SpawnPlanet(int index)
    {
        PlanetScriptObject sObject = planets[index];
        GameObject planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Rigidbody rb = planet.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.mass = sObject.mass;
        planet.name = sObject.name;
        planet.transform.localScale = sObject.scale;
        planet.GetComponent<Collider>().enabled = false;
        planet.GetComponent<MeshRenderer>().material.color = sObject.color;
        planet.AddComponent<Planet>();
        return planet;
    }

    public void OnDrawGizmos()
    {
        Vector2 minHeight = new Vector2(0, DeathHeight.x);
        Vector2 maxHeight = new Vector2(0, DeathHeight.y);
        Vector2 lineBounds = new Vector2(Camera.main.orthographicSize * 2, 0);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(-lineBounds + minHeight, lineBounds + minHeight);
        Gizmos.DrawLine(-lineBounds + maxHeight, lineBounds + maxHeight);
    }

}
