using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask groundLayer;
    public static Player Instance { get; private set; }
    public float Speed { get => speed; set => speed = value; }
    public bool IsUpSideDown { get => isUpSideDown; set => isUpSideDown = value; }
    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }

    private bool isUpSideDown;
    private float currentSpeed;

    private PlayerInput inputs;
    private HealthSystem healthSystem;

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
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        IsUpSideDown = false;
        CurrentSpeed = Speed;

        if (healthSystem != null)
        {
            healthSystem.OnDeath += HandleDeath;
        }
    }

    private void Update()
    {
        Move();
        MakePlayerStayOnGround();
        Switch();
    }

    private void Move()
    {
        transform.position += Vector3.right * CurrentSpeed * Time.deltaTime;
    }

    private void MakePlayerStayOnGround()
    {
        Vector3 castDirection = !IsUpSideDown ? Vector3.down : Vector3.up;
        float castHeight = 1f;
        Vector3 castPosition = !IsUpSideDown ? transform.position + Vector3.up * castHeight : transform.position + Vector3.down * castHeight;

        RaycastHit hit;
        if (Physics.Raycast(castPosition, castDirection, out hit, 999f, groundLayer))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
    }

    private void Switch()
    {
        if (inputs.Switch())
        {
            IsUpSideDown = !IsUpSideDown;

            if (IsUpSideDown)
            {
                transform.Rotate(Vector3.left, -180f);
            }
            else
            {
                transform.Rotate(Vector3.left, 180f);
            }
        }
    }

    public void Pause()
    {
        CurrentSpeed = 0;
        inputs.canTap = false;
        inputs.canSwitch = false;
    }

    public void Resume()
    {
        CurrentSpeed = Speed;
        inputs.canTap = true;
        inputs.canSwitch = true;
    }

    private void HandleDeath()
    {
        CurrentSpeed = 0;
        inputs.canTap = false;
        inputs.canSwitch = false;
    }

}
