using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public List<GameObject> childEnemy;
    private BoxCollider2D box;

    // Use this for initialization
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        childEnemy.RemoveAll(x => x == null);

        if(ChildEnemyDead())
        {
            box.enabled = true;
        }
        else
        {
            box.enabled = false;
        }
    }

    public bool ChildEnemyDead()
    {
        if(childEnemy.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
