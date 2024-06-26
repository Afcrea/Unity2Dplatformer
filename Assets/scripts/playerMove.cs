using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayMove : MonoBehaviour
{
    public GameManager gameManager;
    public float maxSpeed;
    public float jumpPower;

    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animetor;
    CapsuleCollider2D col;

    AudioSource audiosource;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animetor = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider2D>();
        audiosource = GetComponent<AudioSource>();
    }

    void PlaySound(string action)
    {
        switch(action)
        {
            case "JUMP":
                audiosource.clip = audioJump;
                break;

            case "ATTACK":
                audiosource.clip = audioAttack;
                break;

            case "DAMAGED":
                audiosource.clip = audioDamaged;
                break;

            case "ITEM":
                audiosource.clip = audioItem;
                break;

            case "DIE":
                audiosource.clip = audioDie;
                break;

            case "FINISH":
                audiosource.clip = audioFinish;
                break;
        }

        audiosource.Play();
    }

    private void Update()
    {
        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            animetor.SetBool("isJumping", true);
            PlaySound("JUMP");
        }

            // Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }
        
        // Animation
        if(Mathf.Abs(rigid.velocity.x)<= 0.3)
        {
            animetor.SetBool("isWorking", false);
        }
        else
        {
            animetor.SetBool("isWorking", true);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Direction Sprite
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxis("Horizontal") < 0;
        }

        rigid.AddForce(Vector2.right * Input.GetAxisRaw("Horizontal"), ForceMode2D.Impulse);

        if(rigid.velocity.x > maxSpeed) // Right Max Speed
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed*(-1)) // Left Max Speed
        {
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);
        }


        // Lending platform
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1f, LayerMask.GetMask("Platform"));
        //Debug.DrawRay(transform.position, Vector3.down, Color.blue, 0.0f, false);
        if (rayHit.collider)
        {
            if(rigid.velocity.y < 0)
            {
                animetor.SetBool("isJumping", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else
                OnDamaged(collision.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Item")
        {
            PlaySound("ITEM");
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");

            
            if(isBronze)
            {
                gameManager.stagePoint += 50;
            }
            if (isSilver)
            {
                gameManager.stagePoint += 100;
            }
            if (isGold)
            {
                gameManager.stagePoint += 150;
            }
            

            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "Flag")
        {
            PlaySound("FINISH");
            gameManager.NextStage();
        }
    }

    void OnDamaged(Vector2 targetPos)
    {
        gameManager.HealthDown();
        // 레이어 변경
        gameObject.layer = 7;

        // 무적 느낌 투명
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // 맞은 리액션
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);
        animetor.SetTrigger("Damaged");


        PlaySound("DAMAGED");
        //피 깎임

        Invoke("OffDamaged", 1);
    }

    void OffDamaged()
    {
        gameObject.layer = 6;

        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    
    void OnAttack(Transform enemy)
    {
        PlaySound("ATTACK");

        gameManager.stagePoint += 100;

        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        enemyMove enemymove = enemy.GetComponent<enemyMove>();

        enemymove.OnDamaged();
    }

    public void OnDie()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        col.enabled = false;

        PlaySound("DIE");

        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector3.zero;
    }
}
