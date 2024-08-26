using System.Collections.Generic;
using UnityEngine;

public class Melon : MonoBehaviour
{

    public static List<Melon> Melons = new();

    public int mergePoints;
    public bool processedCollision;

    public delegate void MelonEvent(Melon melon);
    public delegate void MelonMerge(int points);
    public static event MelonEvent OnCollided;
    public static event MelonEvent OnDeath;
    public static event MelonEvent OnFinalMelon;
    public static event MelonMerge OnMerge;

    public static Melon GetNearestMelon(Vector3 position, float range = Mathf.Infinity)
    {
        float closestDist = Mathf.Infinity;
        Melon closestPlanet = null;

        foreach (Melon melon in Melons)
        {
            float dist = Vector3.Distance(position, melon.transform.position);
            if (dist >= closestDist || dist > range)
                continue;

            closestDist = dist;
            closestPlanet = melon;
        }

        return closestPlanet;
    }

    private void OnEnable()
    {
        Melons.Add(this);
    }

    private void OnDisable()
    {
        Melons.Remove(this);
    }

    private void Update()
    {
        // Below Camera y
        if (transform.position.y > -MelonManager.GetInstance().CurrentCamera.orthographicSize)
            return;

        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        OnCollided?.Invoke(this);

        if (!collision.gameObject.name.Equals(gameObject.name))
            return;

        Melon otherMelon = collision.gameObject.GetComponent<Melon>();
        if (!otherMelon || otherMelon.processedCollision || processedCollision)
            return;

        processedCollision = true;
        otherMelon.processedCollision = true;
        Melon nextMelon = MelonManager.GetInstance().GetNextMelon(gameObject.name);
        if (!nextMelon)
            return;

        if (MelonManager.GetInstance().IsFinalMelon(nextMelon))
            OnFinalMelon?.Invoke(this);

        OnMerge?.Invoke(mergePoints);
        nextMelon.transform.position = (transform.position + otherMelon.transform.position) * 0.5f;
        MelonManager.GetInstance().Pool.Give(otherMelon, this);
    }

}
