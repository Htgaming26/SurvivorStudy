using Unity.VisualScripting;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public int expValue;

    public float scatterForce = 1.5f;
    public float scatterDuration = 0.2f;
    public float scatterDistance = 1.2f;

    public float magnetSpeed = 6f;
    public float collectDistance = 0.25f;

    enum State {  Idle, Scatter, Magnet }
    State state = State.Idle;

    Transform player;
    Vector2 scatterStartPos;
    Vector2 scatterTargetPos;
    Vector2 scatterDir;
    float timer;
    //bool magnet = false;

    //void Start()
    //{
    //    player = GameObject.FindWithTag("Player").transform;
    //    scatterDir = Random.insideUnitCircle.normalized;
    //}

    void Update()
    {
        if (player == null && state != State.Idle) return;

        switch (state)
        {
            case State.Scatter:
                ScatterUpdate();
                break;

            case State.Magnet:
                MagnetUpdate();
                break;
        }
    }

    void ScatterUpdate()
    {
        timer += Time.deltaTime;

        float t = timer / scatterDuration;
        t = Mathf.Clamp01(t);

        float ease = Mathf.Sin(t * Mathf.PI * 0.5f);

        transform.position = Vector2.Lerp(scatterStartPos, scatterTargetPos, ease);

        //transform.position += (Vector3)(scatterDir * scatterForce * Time.deltaTime);

        if (t >= 1f)
        {
            state = State.Magnet;
            timer = 0;
        }
    }

    void MagnetUpdate()
    {
        if (timer < 0.08f)
        {
            timer += Time.deltaTime;
            return;
        }

        magnetSpeed += Time.deltaTime * 15f;

        transform.position = Vector2.MoveTowards(transform.position, player.position, magnetSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, player.position) <= collectDistance)
        {
            player.GetComponent<Player>().TakeExp(expValue);
            Destroy(gameObject);
        }
    }

    public void SetBig()
    {
        //Debug.Log("drop exp" + GetInstanceID());
        transform.localScale = Vector3.one * 0.2f;
        expValue = 5;
    }
    public void SetSmall()
    {
        //Debug.Log("drop exp" + GetInstanceID());
        transform.localScale = Vector3.one * 0.15f;
        expValue = 1;
    }


    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (!collision.gameObject.CompareTag("Player"))
    //        return;

    //    Player player = collision.gameObject.GetComponent<Player>();
    //    player.TakeExp(expValue);
    //    Destroy(gameObject);
    //}
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (state != State.Idle) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        player = collision.transform;

        scatterDir = (transform.position - player.position).normalized;
        scatterStartPos = transform.position;
        scatterTargetPos = scatterStartPos + scatterDir * scatterDistance;
        timer = 0;
        state = State.Scatter;
    }
}


