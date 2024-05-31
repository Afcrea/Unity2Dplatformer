using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;

    public int health;

    public PlayMove player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextStage()
    {
        stageIndex++;
        totalPoint += stagePoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(health > 1)
            {
                collision.attachedRigidbody.velocity = Vector2.zero;
                collision.transform.position = new Vector2(0, 1);
            }

            HealthDown();
        }
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
        }

        else
        {
            health--;
            player.OnDie();
        }
    }
    
}
