using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [Header("Gameplay Gameobject")]
    public Gameplay gameplay;
    [Header("Bullet Gameobject")]
    public GameObject bullet;
    [Header("Ship`s flame Gameobject")]
    public GameObject flame;
    [Header("Ship`s traction force")]
    public float thrust = 6f;
    [Header("Ship`s maximum speed")]
    public float MaxSpeed = 4.5f;
    private float rotationSpeed = 180f;
    private Camera mainCam;
    private Rigidbody2D rb;

    private void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        bullet.SetActive(false);
    }

    private void FixedUpdate()
    {
        ControlRocket();
        CheckPosition();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            flame.SetActive(true);
        } 
        else
        {
            flame.SetActive(false);
        }
    }

    private void ControlRocket()
    {
        transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime);
        rb.AddForce(transform.up * thrust * Input.GetAxis("Vertical"));
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -MaxSpeed, MaxSpeed), Mathf.Clamp(rb.velocity.y, -MaxSpeed, MaxSpeed));
    }

    private void CheckPosition()
    {

        float sceneWidth = mainCam.orthographicSize * 2 * mainCam.aspect;
        float sceneHeight = mainCam.orthographicSize * 2;

        float sceneRightEdge = sceneWidth / 2;
        float sceneLeftEdge = sceneRightEdge * -1;
        float sceneTopEdge = sceneHeight / 2;
        float sceneBottomEdge = sceneTopEdge * -1;

        if (transform.position.x > sceneRightEdge)
        {
            transform.position = new Vector2(sceneLeftEdge, transform.position.y);
        }
        if (transform.position.x < sceneLeftEdge) { transform.position = new Vector2(sceneRightEdge, transform.position.y); }
        if (transform.position.y > sceneTopEdge)
        {
            transform.position = new Vector2(transform.position.x, sceneBottomEdge);
        }
        if (transform.position.y < sceneBottomEdge)
        {
            transform.position = new Vector2(transform.position.x, sceneTopEdge);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "UFOBullet(Clone)")
        {
            gameplay.RocketFail();
        }
    }

    public void ResetRocket()
    {
        transform.position = new Vector2(0f, 0f);
        transform.eulerAngles = new Vector3(0, 0, 0);
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.angularVelocity = 0f;
    }

    void Shoot()
    {
        GetComponent<AudioSource>().Play();
        GameObject BulletClone = Instantiate(bullet, new Vector2(bullet.transform.position.x, bullet.transform.position.y), transform.rotation);
        BulletClone.SetActive(true);
        BulletClone.GetComponent<Bullet>().KillOldBullet();
        BulletClone.GetComponent<Rigidbody2D>().AddForce(transform.up * 350);
    }
}
