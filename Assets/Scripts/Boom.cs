using UnityEngine;

public class Boom : MonoBehaviour
{
    [Header("Move")]
    public float speed = 5f;
    public float flyRadius = 2f;
    public float curveStrength = 1.5f;

    [Header("Damage")]
    public int damageBoom = 6;
    public float explodeRadius = 1.5f;


    public LayerMask monsterLayer;

    public Transform player;
    Vector2 targetPos;
    float curveOffset;
    void Start()
    {
        Vector2 randomDir;
        do
        {
            randomDir = Random.insideUnitCircle;
        }
        while (randomDir.sqrMagnitude < 0.01f);

        randomDir.Normalize();

        targetPos = (Vector2)transform.position + randomDir * flyRadius;

        curveOffset = Random.Range(-0.8f, 0.8f);
    }

    void Update()
    {
        if (player == null) return;

        Vector2 toTarget = targetPos - (Vector2)transform.position;

        Vector2 forward = toTarget.normalized;

        Vector2 side = new Vector2(-forward.y, forward.x);

        Vector2 moveDir = forward + side * curveOffset * curveStrength;

        transform.position += (Vector3)(moveDir.normalized * speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            Explode();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & monsterLayer) != 0)
        {
            Explode();
        }
    }

    public void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explodeRadius, monsterLayer);

        foreach (var hit in hits)
        {
            hit.GetComponent<Monster>()?.TakeHit(damageBoom);
        }
        Destroy(gameObject);
    }
}
