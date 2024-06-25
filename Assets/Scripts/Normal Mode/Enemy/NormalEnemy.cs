using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int scoreValue;
    [SerializeField] private Transform castPoint;
    [SerializeField] private Transform hitPoint;
    [SerializeField] private LayerMask groundLayer;


    private float currentSpeed;

    private NormalSpawnPoint normalMapLane;


    public int ScoreValue { get => scoreValue; set => scoreValue = value; }
    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }

    private void Start()
    {
        normalMapLane = NormalSpawnPoint.Instance;

        int enemyLifeTime = 5;
        Invoke(nameof(ReturnObject), enemyLifeTime);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);

        RaycastHit hit;
        if (Physics.Raycast(castPoint.position, (transform.position - castPoint.position), out hit, 10f, groundLayer))
        {
            transform.position = hit.point;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            HealthSystem playerHealth = other.GetComponent<HealthSystem>();
            playerHealth.TakeDamage(1);


            normalMapLane.GetEnemyPartical(hitPoint.position);
        }
    }

    private void OnEnable()
    {
        currentSpeed = speed;
        gameObject.GetComponent<Collider>().enabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(castPoint.position, (transform.position - castPoint.position ) * 10f);
    }

    private void ReturnObject()
    {
        normalMapLane.ReturnEnemy(this);
    }
}
