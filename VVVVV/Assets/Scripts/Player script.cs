using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MCscript : MonoBehaviour
{
    public Rigidbody2D _rb;
    public float _speed = 5f;
    public static MCscript player;
    public bool lookRight = true;
    public bool grounded = false;
    public bool isDead = false;
    public bool upsideDown = false;
    public int checkpoint = 0;
    public Animator anim;
    RaycastHit2D hit;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void Awake()
    {
        if (MCscript.player == null)
        {
            MCscript.player = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (isDead == false)
        {
            hit = Physics2D.Raycast(transform.position, -transform.up, 0.6f);
            Debug.DrawLine(transform.position, transform.position - transform.up * 0.6f, Color.red);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    Debug.Log("Grounded");
                    grounded = true;
                }
            }
            else
            {
                grounded = false;
            }
            if (Input.GetKey(KeyCode.A))
            {
                _rb.velocity = new Vector2(-_speed, _rb.velocity.y);
                anim.SetBool("Iswalking", true);
            }
            if (Input.GetKey(KeyCode.A) && lookRight == true)
            {
                transform.Rotate(0, 180, 0);
                lookRight = false;

            }
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                anim.SetBool("Iswalking", false);
            }
            if (Input.GetKey(KeyCode.D))
            {
                _rb.velocity = new Vector2(_speed, _rb.velocity.y);
                anim.SetBool("Iswalking", true);
            }
            if (Input.GetKey(KeyCode.D) && lookRight == false)
            {
                transform.Rotate(0, 180, 0);
                lookRight = true;

            }
            if (Input.GetKeyDown(KeyCode.Space) && grounded == true)
            {
                _rb.gravityScale *= -1;
                transform.Rotate(180, 0, 0);
                upsideDown = !upsideDown;
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag == "LeftChange")
        {
            transform.position = new Vector2(transform.position.x * -1 + 1, transform.position.y);
        }
        if (collision.gameObject.tag == "RightChange")
        {
            transform.position = new Vector2(transform.position.x * -1 - 1, transform.position.y);
        }
        if(collision.gameObject.tag == "UpChange")
        {
            transform.position = new Vector2(transform.position.x, transform.position.y * -1 + 1);
        }
        if (collision.gameObject.tag == "DownChange")
        {
            transform.position = new Vector2(transform.position.x, transform.position.y * -1 - 1);
        }
        if (collision.gameObject.tag == "Damage")
        {
            isDead = true;
            anim.SetTrigger("Die");
            anim.SetBool("Iswalking", false);
            StartCoroutine(Respawn());
        }
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        isDead = false;
        _rb.velocity = new Vector2(0, 0);
        if (checkpoint == 0)
        {   //cambiar la escena
            SceneManager.LoadScene("FirstScene");
            //x -5 y 2
            transform.position = new Vector2(-5, 2);
        }
        if (upsideDown)
        {
            _rb.gravityScale *= -1;
            transform.Rotate(180, 0, 0);
            upsideDown = false;
        }

    }
}
