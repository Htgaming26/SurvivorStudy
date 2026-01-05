using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    
    bool boomUnlocked;

    public float TimeSpawnBoom = 2f;
    public float speed = 10f;
    public float timeShoot = 2f;
    public int hp = 50;
    public int maxHp { get; private set; }

    public Animator animator;
    public SpriteRenderer render;
    public GameObject bulletPrefab;
    public GameObject boomPrefab;
    public Transform firePoint;
    public Transform center;

    Bound bound;
    List<Monster> monsters;
    HealthBarUI healthBar;
    ExpManager expBar;

    Vector3 position;

    bool isHurt = false;

    public void Init(Bound bound, List<Monster> monsters, HealthBarUI healthBar, ExpManager expBar) // dependency injection
    {
        this.bound = bound;
        this.monsters = monsters;
        this.healthBar = healthBar;
        this.expBar = expBar;
    }

    void Start()
    {
        position = transform.position;
        InvokeRepeating(nameof(Shoot), 0.5f, timeShoot);
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
                render.flipX = false;
            }
            else if (x < 0)
            {
                render.flipX = true;
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);


        }

        if (boomUnlocked && !IsInvoking(nameof(SpawnBoom)))
            InvokeRepeating(nameof(SpawnBoom), 0f, TimeSpawnBoom);

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
    public void StartShoot()
    {
        CancelInvoke(nameof(Shoot));
        InvokeRepeating(nameof(Shoot), 0.5f, timeShoot);
    }
    public void SpawnBoom()
    {
       GameObject boomObj = Instantiate(boomPrefab, transform.position, Quaternion.identity);

        Boom boom = boomObj.GetComponent<Boom>();
        boom.player = transform;
    }

    public void UnlockBoom()
    {
        boomUnlocked = true;
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
    public void SetTimeShoot(float newTime)
    {
        timeShoot = newTime;
        StartShoot();
    }

    public void TakeExp(int exp)
    {
        expBar.AddExperience(exp);
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
        Destroy(gameObject);
    }

    public void HurtEnd()
    {
        Debug.Log("hurt end");
        isHurt = false;
    }


}
