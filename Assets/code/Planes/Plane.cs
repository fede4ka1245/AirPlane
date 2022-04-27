using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float force = 250;

    [SerializeField] Game game;
    [SerializeField] Animator anim;
    public float speed = 200;
    public bool isCollisionOff = false;
    public bool IsCrush = false;
    public float MaxSpeed = 400;
    public string Name;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        game = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Game>();
        anim.SetBool("IsCrush", false);
    }
    private void FixedUpdate()
    {
        if (game.button.isClicked)
        {
            rb.AddForce(force * Vector2.up, ForceMode2D.Impulse);
        }
    }

    private void death()
    {
        IsCrush = true;
        StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {
        anim.SetBool("IsCrush", true);
        game.SpeedWithBoost = 0;

        yield return new WaitForSeconds(0.2f);

        game.DeathPanel.SetActive(true);
        game.SaveAll();
        Time.timeScale = 0;
    }

    public void Reborn()
    {
        IsCrush = false;
        anim.SetBool("IsCrush", false);
        StartCoroutine(RebornCoroutine());
    }

    IEnumerator RebornCoroutine()
    {
        isCollisionOff = true;

        yield return new WaitForSeconds(2f);

        isCollisionOff = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "coin")
        {
            game.CollectCoin(collision.gameObject);
        }
        if (isCollisionOff)
            return;
        if (collision.gameObject.tag == "trap")
        {
            death();
        }
    }
}

