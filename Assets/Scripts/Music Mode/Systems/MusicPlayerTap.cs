using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerTap : MonoBehaviour
{
    public static MusicPlayerTap Instance { get; private set; }

    [SerializeField] private MusicTapVisual tapVisualPrefab;
    [SerializeField] private MusicHitVisual hitVisualPrefab;
    [SerializeField] private Transform attackPoint;

    private Queue<MusicTapVisual> TapVisuals = new Queue<MusicTapVisual>();
    private Queue<MusicHitVisual> HitVisuals = new Queue<MusicHitVisual>();

    private MusicTapVisual latestTapVisual;

    private PlayerInput inputs;

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
        inputs = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        if (attackPoint == null)
        {
            Debug.LogError("Attack Point is not assigned in the inspector.");
            return;
        }

        CreateTapQueue();
        CreateHitQueue(); // Ensure hit visuals are created
    }

    private void Update()
    {
        if (inputs.Tap())
        {
            Tap();
        }
    }

    private void CreateTapQueue()
    {
        int numberOfVisuals = 10;
        for (int i = 0; i < numberOfVisuals; i++)
        {
            MusicTapVisual visual = Instantiate(tapVisualPrefab, transform.position, Quaternion.identity);
            visual.gameObject.SetActive(false);
            TapVisuals.Enqueue(visual);
        }
    }

    private void CreateHitQueue()
    {
        int numberOfVisuals = 10;
        for (int i = 0; i < numberOfVisuals; i++)
        {
            MusicHitVisual visual = Instantiate(hitVisualPrefab, transform.position, Quaternion.identity);
            visual.gameObject.SetActive(false);
            HitVisuals.Enqueue(visual);
        }
    }

    private void Tap()
    {
        if (TapVisuals.Count == 0)
        {
            CreateTapQueue();
        }

        MusicTapVisual visual = TapVisuals.Dequeue();
        visual.transform.position = attackPoint.position;
        visual.transform.rotation = attackPoint.rotation;
        visual.gameObject.SetActive(true);
        latestTapVisual = visual;
    }

    public void ReturnTapVisual(MusicTapVisual visual)
    {
        visual.gameObject.SetActive(false);
        TapVisuals.Enqueue(visual);
    }

    public void ReturnHitVisual(MusicHitVisual visual)
    {
        HitVisuals.Enqueue(visual);
        visual.gameObject.SetActive(false);
    }

    public void GetHitVisual(Vector3 position)
    {
        if (HitVisuals.Count == 0)
        {
            CreateHitQueue();
        }

        MusicHitVisual visual = HitVisuals.Dequeue();
        visual.transform.position = position;
        visual.gameObject.SetActive(true);
    }

    public void ReturnLatestTapVisual()
    {
        if (latestTapVisual != null)
        {
            ReturnTapVisual(latestTapVisual);
        }
    }

}
