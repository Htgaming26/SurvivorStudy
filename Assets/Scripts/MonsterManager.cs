using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;

    public List<GameObject> monsters = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    public void Register(GameObject monster)
    {
        monsters.Add(monster);
    }

    public void Unregister(GameObject monster)
    {
        monsters.Remove(monster);
    }
}

