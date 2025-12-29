using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Awake");
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable");
    }
    private void OnDisable()
    {
        Debug.Log("OnDisable");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // khai bao các biến cần thiết
        Debug.Log("Start");
    }

    private void OnDestroy()
    {
        Debug.Log("OnDestroy");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update");
    }
}
