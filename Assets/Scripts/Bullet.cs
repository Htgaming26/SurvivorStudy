using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float timeShoot = 2f;
    public int damageShoot = 1;
    private Transform target;
    private Vector3 direction;

    public void SetTarget(Monster monster)
    {
        target = monster.transform;
        direction = (monster.center.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, -angle);

    }

    private void Start()
    {
        Destroy(gameObject, 5f);
    }
    void Update()
    {
        //if (direction == default) // khi monster chet giua chung, destroy dan di
        /*if (target == null)
        {
            Destroy(gameObject);
            return;
        }*/
        //transform.position += direction * speed * Time.deltaTime;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeHit(damageShoot);
            }
            Destroy(gameObject);
        }
    }
}
