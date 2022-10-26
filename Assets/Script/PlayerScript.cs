using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public UnityEngine.UI.Text score;
    public UnityEngine.UI.Text lives;
    //public UnityEngine.UI.Text winText;

    public GameObject loseTextObject;
    public GameObject winTextObject;
    public GameObject Player;
    
    public AudioSource musicSource;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;
    private int scoreValue = 0;
    private int livesValue = 3;
    private int stage = 0;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    Animator anim;

    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();

        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        if(hozMovement > 0 || hozMovement < 0){
            anim.SetInteger("State", 1);
        }
        else if(hozMovement == 0){
            anim.SetInteger("State", 0);
        }
        if(isOnGround == false){
            anim.SetInteger("State", 2);
        }
        if (facingRight == false && hozMovement > 0){
            Flip();
        }
        else if (facingRight == true && hozMovement < 0){
            Flip();
        }
        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));

    }
    void Flip(){
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.collider.tag == "Coin"){
            scoreValue++;
            score.text = "Score: " + scoreValue.ToString();
            collision.gameObject.SetActive(false);
        }
        if(collision.collider.tag == "Enemy"){
            livesValue--;
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.gameObject);
        }
        if(livesValue == 0){
            loseTextObject.SetActive(true);
            musicSource.clip = musicClipTwo;
            musicSource.loop = false;
            musicSource.Play();
            Destroy(Player);
        }
        if(scoreValue == 4 && stage == 0){
            stage = 1;
            transform.position = new Vector2(58, 2);
            livesValue = 3;
            lives.text = "Lives: " + livesValue.ToString();
        }
        if(scoreValue == 8){
            winTextObject.SetActive(true);
            musicSource.clip = musicClipTwo;
            musicSource.loop = false;
            musicSource.Play();
            Destroy(Player);
        }
    }

    void OnCollisionStay2D(Collision2D collision){
        if(collision.collider.tag == "Ground"){
            if(Input.GetKey(KeyCode.W)){
                rd2d.AddForce(new Vector2(0,3), ForceMode2D.Impulse);
                isOnGround = false;
            }
        }
    }
}
