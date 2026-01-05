using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    [Header("Distance")]
    public float stopDistanceToPlayer = 0.6f;
    public float separationRadius = 0.5f;

    [Header("Force")]
    public float separationWeight = 1.2f;
    public float pushForce = 1.5f;

    public LayerMask monsterLayer;

    public GameObject expPrefab;
    public bool dropped = false;

    Player player;
    public float speed = 3f;
    public int hp = 2;
    public int damage = 1;

    public Animator animator;
    public SpriteRenderer render;
    public bool isDead = false;
    public Transform center;
    public Transform floatDamageSpawner;
    public GameObject floatDamagePrefab;

    Vector2 lastDir;

    void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    void Update()
    {
        //Vector3 dir = player.transform.position - transform.position;
        // transform.position += dir.normalized * speed * Time.deltaTime;
        // //if (Vector3.Distance(transform.position, player.transform.position) < 0.01f)
        // //{
        // //    transform.position = player.transform.position;
        // //}

        // if (dir.x > 0)
        // {
        //     renderer.flipX = false;
        // }
        // else if (dir.x < 0)
        // {
        //     renderer.flipX = true;
        // }
        Move();
    }
    void Move()
    {
        Vector2 dir = GetMoveDirection();

        dir = Vector2.Lerp(lastDir, dir, 5f * Time.deltaTime);

        if (dir != Vector2.zero)
            transform.position += (Vector3)(dir * speed * Time.deltaTime);

        lastDir = dir;
    }
    Vector2 GetMoveDirection()
    {
        Vector2 dir = Vector2.zero;

        dir += GetPlayerForce();
        dir += SeparateMonster();

        return dir.normalized;
    }
    Vector2 GetPlayerForce()
    {
        Vector2 toPlayer = player.transform.position - transform.position;


        float d = toPlayer.magnitude;

        if (d > stopDistanceToPlayer)
        return toPlayer.normalized;
        else
            return Vector2.zero;
    }
    Vector2 SeparateMonster()
    {
        Collider2D[] near = Physics2D.OverlapCircleAll(
            transform.position,
            separationRadius,
            monsterLayer
        );

        Vector2 push = Vector2.zero;

        foreach (var col in near)
        {
            if (col.gameObject == gameObject) continue;

            Vector2 d = transform.position - col.transform.position;
            if (d.magnitude > 0)
                push += d.normalized / d.magnitude;
        }

        return push * separationWeight;
    }
    public void TakeHit(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }

        Instantiate(floatDamagePrefab, floatDamageSpawner.position, Quaternion.identity)
        .GetComponent<FloatDamage>().SetText(damage);
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        //animator.SetBool("IsDie", true);
        Destroy(gameObject);
        DropExp();
    }
    //public void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Player player = other.GetComponent<Player>();
    //        if (player != null)
    //        {
    //            player.TakeDamage(damage);
    //        }
    //    }
    //}

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Vector2 dir = (transform.position - other.transform.position).normalized;
                transform.position += (Vector3)(dir * pushForce * Time.deltaTime);
            }
        }
    }
    void DropExp()
    {

        if (dropped) return;
        dropped = true;
        
        /*float roll = Random.value;

        if (roll < 0.15f)
        {
            Instantiate(expPrefab, transform.position, Quaternion.identity).GetComponent<ExpOrb>().SetBig();
        }
        else if (roll < 0.55f)
        {
            Instantiate(expPrefab, transform.position, Quaternion.identity).GetComponent<ExpOrb>().SetSmall();
        }
        else
        {
            return;
        }*/
        float r = Random.value;

        if (r < 0.03f) Drop(3, 0);
        else if (r < 0.08f) Drop(2, 0);
        else if (r < 0.15f) Drop(1, 1);
        else if (r < 0.35f) Drop(1, 0);
        else if (r < 0.55f) Drop(0, 2);
        else if (r < 0.80f) Drop(0, 1);

    }
    void Drop(int big, int small)
    {
        for (int i = 0; i < big; i++) Spawn(true);
        for (int i = 0; i < small; i++) Spawn(false);
    }
    void Spawn(bool isBig)
    {
        Vector3 offset = Random.insideUnitCircle * 0.6f;
        var exp = Instantiate(expPrefab, transform.position + offset, Quaternion.identity);
        var orb = exp.GetComponent<ExpOrb>();
        if (isBig) orb.SetBig();
        else orb.SetSmall();
    }

}


