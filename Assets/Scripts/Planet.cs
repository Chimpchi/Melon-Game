using UnityEngine;

public class Planet : MonoBehaviour
{

    public bool ProcessedCollision { get; private set; }
    private float timer = 3;

    private void Update()
    {
        Vector2 deathHeight = PlanetManager.Instance.DeathHeight;
        if (transform.position.y >= deathHeight.x && transform.position.y <= deathHeight.y)
            timer -= Time.deltaTime;
        else
            timer = 3f;

        if(timer <= 0)
        {
            Debug.Log("Game Over! You Suck...");
        }

        if (transform.position.y < -PlanetManager.Instance.CurrentCamera.orthographicSize)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (!collision.gameObject.name.Equals(gameObject.name))
            return;

        Planet otherPlanet = collision.gameObject.GetComponent<Planet>();
        if (otherPlanet == null || otherPlanet.ProcessedCollision || ProcessedCollision)
            return;

        ProcessedCollision = true;
        otherPlanet.ProcessedCollision = true;
        GameObject nextPlanet = PlanetManager.Instance.GetNextPlanet(gameObject.name);
        if (nextPlanet == null)
            return;

        nextPlanet.transform.position = transform.position;
        Destroy(otherPlanet.gameObject);
        Destroy(gameObject);
    }

}
