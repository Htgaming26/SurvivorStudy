using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float speed = 10f;
    public float timeShoot = 2f;
    public int hp = 50;
    public int maxHp { get; private set; }

    public Animator animator;
    public SpriteRenderer renderer;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform center;

    Bound bound;
    List<Monster> monsters;
    HealthBarUI healthBar;

    Vector3 position;

    bool isHurt = false;

    public void Init(Bound bound, List<Monster> monsters, HealthBarUI healthBar)
    {
        this.bound = bound;
        this.monsters = monsters;
        this.healthBar = healthBar;
    }

    void Start()
    {
        position = transform.position;
        ///InvokeRepeating(nameof(Shoot), 1f, timeShoot);
        maxHp = hp;
    }

    void Update()
    {
        int x = (int)Input.GetAxisRaw("Horizontal"); //-> -1 / 1
        int y = (int)Input.GetAxisRaw("Vertical"); // -> -1 / 1

        // fps: frame per second
        // Time.deltaTime: khoang thoi gian tu frame truoc toi frame hien tai = 1/FPS
        
        position += new Vector3(x, y).normalized * speed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, bound.Left, bound.Right);
        position.y = Mathf.Clamp(position.y, bound.Bot, bound.Top);
        transform.position = position;

        if (x != 0 || y != 0)
        {
            animator.SetBool("IsMoving", true);
            if (x > 0)
            {
                renderer.flipX = false;
            }
            else if (x < 0)
            {
                renderer.flipX = true;
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);

            
        }

        #region shoot
        /*if (Input.GetKeyDown(KeyCode.Space))
        {

            // Tim monter ma vien dan se bay toi
            var monster = GetNearestMonster();
            //Debug.Log(monster.name);
            // ban dan
            CreateBullet(monster);
            
        }*/



        #endregion
    }

    public Monster GetNearestMonster()
    {
        if (monsters.Count > 0)
        
            monsters.RemoveAll(ms => ms == null);
        
        Monster nearest = null;
        float minDistance = Mathf.Infinity;
        // Duyet qua 1 danh sach monter 
        foreach ( Monster ms in monsters)
        {
            float distance = Vector3.Distance(position, ms.transform.position);
            
            if ( distance < minDistance )
            {
                minDistance = distance;
                nearest = ms;
            }
        }
        return nearest;
    }

    public void CreateBullet(Monster monster)
    {
        
        // tinh rotation cua bullet

       /* Vector2 direction = (monster.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        GameObject bullet = Instantiate(bulletPrefab, center.position, Quaternion.Euler(0, 0, -angle));
        bullet.GetComponent<Bullet>().SetDirection(direction);
       */
    }
    void Shoot()
    {
        Monster monster = GetNearestMonster();
        if (monster == null) return;

        GameObject bullet = Instantiate(
            bulletPrefab,
            transform.position,
            Quaternion.identity
        );
        bullet.GetComponent<Bullet>().SetTarget(monster);
    }
    public void TakeDamage(int damage)
    {
        if (isHurt)
            return;

        hp -= damage;
        Debug.Log("Player hp: " + hp);

        if (hp <= 0)
        {
            Die();
        }

        // hieu ung hurt
        isHurt = true;
        animator.SetTrigger("TriggerHurt");

        // update health bar
        healthBar.UpdateHealth();
    }
    void Die()
    {
        Debug.Log("Player Dead");
    }

    public void HurtEnd()
    {
        Debug.Log("hurt end");
        isHurt = false;
    }
}
