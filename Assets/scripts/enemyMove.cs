using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMove : MonoBehaviour
{

    Rigidbody2D rigid;
    public int nextMove;
    Animator anime;
    SpriteRenderer spriteRenderer;
    CircleCollider2D enemyCollider;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<CircleCollider2D>();

        Invoke("Think", 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        // Platform check
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1f, LayerMask.GetMask("Platform"));
        Debug.DrawRay(frontVec, Vector3.down, Color.blue, 0.0f, false);
        if (!rayHit.collider)
        {
            Turn();
        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        float nextThink = Random.Range(2f, 5f);
        
        // 걷기 애니
        anime.SetInteger("walking", nextMove);

        // 방향
        if(nextMove != 0) 
        {
            spriteRenderer.flipX = nextMove == 1;
        }

        Invoke("Think", nextThink);
    }
    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("Think", 5);
    }

    public void OnDamaged()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        enemyCollider.enabled = false;

        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        Invoke("DeActive", 5);
    }
    
    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
