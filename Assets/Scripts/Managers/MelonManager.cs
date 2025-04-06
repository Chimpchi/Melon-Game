using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MelonManager : MonoBehaviour
{

    private static MelonManager Instance;
    public Camera CurrentCamera { get; private set; }

    [SerializeField] private float gravityScale = 1;
    [SerializeField] private Vector2 dropArea = new(-1, 1);
    [SerializeField] private float dropHeight;
    [SerializeField] private Transform nextMelonLocation;
    [SerializeField] private int melonIndexSpawnRange = -1;
    [SerializeField] private LayerMask melonLayer;
    [field: SerializeField] public MelonPool Pool { get; private set; }

    public delegate void GameOver();
    public static event GameOver OnGameOver;

    private Melon currentMelon, nextMelon;
    private readonly List<MelonScriptObject> melons = new();
    private LineRenderer lineRenderer;
    private bool droppingMelon;

    private Rigidbody2D currentMelonRb;
    private Collider2D currentMelonCollider;

    public static MelonManager GetInstance()
    {
        if (Instance)
            return Instance;

        return new GameObject("Melon Manager").AddComponent<MelonManager>();
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CurrentCamera = Camera.main;

        melons.AddRange(Resources.LoadAll("Melons", typeof(MelonScriptObject)).Cast<MelonScriptObject>().ToArray());
        melons.Sort((p1, p2) => p1.mergePoints.CompareTo(p2.mergePoints));

        Physics.gravity = new(0, -gravityScale, 0);
        currentMelon = SpawnRandomMelon();
        nextMelon = SpawnRandomMelon();
        nextMelon.transform.position = nextMelonLocation.position;
        lineRenderer = GetComponent<LineRenderer>();
        CachePlanetComponent();
    }

    private void Start()
    {
        GameManager.GetInstance().SetState(GameState.Gameplay);
    }

    private void OnEnable()
    {
        Melon.OnCollided += NewMelon;
        Melon.OnDeath += NewMelon;
    }

    private void OnDisable()
    {
        Melon.OnCollided -= NewMelon;
        Melon.OnDeath -= NewMelon;
    }

    private void Update()
    {
        if (!CurrentCamera || GameManager.GetInstance().currentState != GameState.Gameplay)
            return;

        Vector3 mousePoint = CurrentCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 planetPosition = new(Mathf.Clamp(mousePoint.x, dropArea.x, dropArea.y), dropHeight);

        lineRenderer.enabled = !droppingMelon;

        if (droppingMelon)
            return;

        currentMelon.transform.position = planetPosition;
        RaycastHit2D ray = Physics2D.Raycast(planetPosition, Vector2.down, Mathf.Infinity, melonLayer);
        //Debug.Log($"{planetPosition + 2 * currentMelon.transform.localScale * Vector2.down}, {ray.point}");
        if (ray.point.Equals(Vector2.zero))
            return;

        Vector3[] points = { planetPosition, ray.point };
        lineRenderer.SetPositions(points);
    }

    public Melon GetCurrentMelon() => currentMelon;

    public bool IsFinalMelon(Melon melon) => melons[^1].name.Equals(melon.name);

    public void DropMelon()
    {
        if (!currentMelonRb || !currentMelonCollider)
            return;

        currentMelonRb.isKinematic = false;
        currentMelonCollider.enabled = true;
        droppingMelon = true;
    }

    public void NewMelon(Melon currentMelon)
    {
        if (!currentMelon.Equals(this.currentMelon.GetComponent<Melon>()))
            return;

        if (!droppingMelon)
        {
            OnGameOver?.Invoke();
            return;
        }

        droppingMelon = false;
        this.currentMelon = nextMelon;
        nextMelon = SpawnRandomMelon();
        nextMelon.transform.position = nextMelonLocation.position;
        CachePlanetComponent();
    }

    public Melon GetNextMelon(string currentMelon)
    {
        for (int i = 0; i < melons.Count; i++)
        {
            if (!melons[i].name.Equals(currentMelon))
                continue;

            if (i + 1 >= melons.Count)
                break;

            Melon melon = SpawnMelon(i + 1);
            melon.GetComponent<Rigidbody2D>().isKinematic = false;
            melon.GetComponent<Collider2D>().enabled = true;
            return melon;
        }

        return null;
    }

    private Melon SpawnRandomMelon()
    {
        int length = (melonIndexSpawnRange < 0 || melonIndexSpawnRange >= melons.Count) ? melons.Count : melonIndexSpawnRange;
        return SpawnMelon(UnityEngine.Random.Range(0, length));
    }

    private void CachePlanetComponent()
    {
        currentMelonRb = currentMelon.GetComponent<Rigidbody2D>();
        currentMelonCollider = currentMelon.GetComponent<Collider2D>();
    }

    private Melon SpawnMelon(int index)
    {
        //GameObject Setting
        MelonScriptObject sObject = melons[index];
        Melon melon = Pool.Get();
        melon.mergePoints = sObject.mergePoints;
        melon.processedCollision = false;

        melon.gameObject.name = sObject.melonName;
        melon.gameObject.layer = melonLayer;
        melon.gameObject.AddComponent<CircleCollider2D>();
        melon.transform.localScale = Vector3.one * sObject.scale;

        //Sprite Setting
        SpriteRenderer spriteRender = melon.gameObject.GetComponent<SpriteRenderer>();
        if (!spriteRender)
            spriteRender = melon.gameObject.AddComponent<SpriteRenderer>();

        spriteRender.sprite = sObject.sprite;
        spriteRender.color = sObject.color;

        //Rigidbody Setting
        Rigidbody2D rb = melon.gameObject.GetComponent<Rigidbody2D>();
        if (!rb)
            rb = melon.gameObject.AddComponent<Rigidbody2D>();

        rb.isKinematic = true;
        rb.mass = sObject.mass;

        return melon;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        float screenHeight = Camera.main.orthographicSize * 2f;
        Gizmos.DrawLine(new Vector3(dropArea.x, dropHeight, 0), new Vector3(dropArea.x, -screenHeight));
        Gizmos.DrawLine(new Vector3(dropArea.y, dropHeight, 0), new Vector3(dropArea.y, -screenHeight));
    }

}
