using UnityEngine;

public class Maincamera : MonoBehaviour
{
    Player player;
    public float speed = 4f;
    Vector3 position;
    void Start()
    {
        player = FindFirstObjectByType<Player>();
        position = transform.position;
    }

    void Update()
    {
        #region di chuyen 1
        /*
        Vector3 dir = player.transform.position - transform.position;
        position += dir.normalized * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, player.transform.position) < 0.01f)
        {
            position = player.transform.position;
        }

        transform.position = new Vector3(position.x, position.y, transform.position.z);
        */
        #endregion


        #region di chuyen 2
        /*
        position = player.transform.position;
        transform.position = new Vector3(position.x, position.y, transform.position.z);*/
        #endregion

        // linear interpolate
        position = Vector3.Lerp(position, player.transform.position, speed * Time.deltaTime);
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }
}
