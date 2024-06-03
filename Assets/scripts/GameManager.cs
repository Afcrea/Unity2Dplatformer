using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;

    public GameObject[] Stages;
    public int health;

    public PlayMove player;

    public Image[] UIhealth;
    public TextMeshProUGUI UIPoint;
    public TextMeshProUGUI UIStage;
    public GameObject UIReStartBtn;
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }

    public void NextStage()
    {
        if(stageIndex < Stages.Length-1)
        {
            Stages[stageIndex].SetActive(false);

            stageIndex++;

            Stages[stageIndex].SetActive(true);

            PlayerReposition();


            UIStage.text = "STAGE" + (stageIndex + 1).ToString();
        }
        else
        {
            Time.timeScale = 0;

            TextMeshProUGUI BtnText = UIReStartBtn.GetComponentInChildren<TextMeshProUGUI>();
            BtnText.text = "Game Clear!";
            UIReStartBtn.SetActive(true);
        }

        

        totalPoint += stagePoint;
        stagePoint = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(health > 1)
            {
                PlayerReposition();
            }

            HealthDown();
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 1, 0);
        player.VelocityZero();

    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIhealth[health].color = new Color(1, 0, 0, 0.4f);
        }

        else
        {
            health--;
            UIhealth[health].color = new Color(1, 0, 0, 0.4f);
            UIReStartBtn.SetActive(true);
            player.OnDie();
        }
    }
 
    public void ReStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
