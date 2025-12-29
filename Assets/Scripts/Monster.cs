using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    

    Player player;
    public float speed = 3f;
    public int hp = 2;
    public int damage = 1;

    public Animator animator;
    public SpriteRenderer renderer;

    public Transform center;

    void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    void Update()
    {
       Vector3 dir = player.transform.position - transform.position;
        transform.position += dir.normalized * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, player.transform.position) < 0.01f)
        {
            transform.position = player.transform.position;
        }

        if (dir.x > 0)
        {
            renderer.flipX = false;
        }
        else if (dir.x < 0)
        {
            renderer.flipX = true;
        }
    }

    public void TakeHit(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //animator.SetBool("IsDie", true);
        Destroy(gameObject);
    }
    /*public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }*/

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }

}


