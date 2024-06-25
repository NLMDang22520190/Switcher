using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPlayerTap : MonoBehaviour
{

    public static NormalPlayerTap Instance { get; private set; }

    [SerializeField] private NormalTapVisual tapVisualPrefab;
    [SerializeField] private NormalHitVisual hitVisualPrefab;
    [SerializeField] private Transform attackPoint;

    private Queue<NormalTapVisual> TapVisuals = new Queue<NormalTapVisual>();
    private Queue<NormalHitVisual> HitVisuals = new Queue<NormalHitVisual>();

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
        CreateTapQueue();
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
            NormalTapVisual visual = Instantiate(tapVisualPrefab, transform.position, Quaternion.identity);
            visual.gameObject.SetActive(false);
            TapVisuals.Enqueue(visual);
        }
    }

    private void CreateHitQueue()
    {
        int numberOfVisuals = 10;
        for (int i = 0; i < numberOfVisuals; i++)
        {
            NormalHitVisual visual = Instantiate(hitVisualPrefab, transform.position, Quaternion.identity);
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

        NormalTapVisual visual = TapVisuals.Dequeue();
        visual.transform.position = attackPoint.position;
        visual.transform.rotation = attackPoint.rotation;
        visual.gameObject.SetActive(true);
    }

    public void ReturnTapVisual(NormalTapVisual visual)
    {
        TapVisuals.Enqueue(visual);
        visual.gameObject.SetActive(false);
    }

    public void ReturnHitVisual(NormalHitVisual visual)
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

        NormalHitVisual visual = HitVisuals.Dequeue();
        visual.transform.position = position;
        visual.gameObject.SetActive(true);
    }
}
