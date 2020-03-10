using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    [Header("Gameplay Gameobject")]
    public Gameplay gameplay;
    [Header("Ship Gameobject")]
    public GameObject ship;
    [Header("Bullet Gameobject")]
    public GameObject bullet;
    [Header("UFO`s traction force")]
    public float thrust = 6f;
    [Header("UFO`s shoot speed")]
    public float shootSpeed = 4.5f;
    private Camera mainCam;
    private Rigidbody2D rb;
    private float timeToShoot;

    private void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        bullet.SetActive(false);
        timeToShoot = shootSpeed;
    }

    private void FixedUpdate()
    {
        ControlUFO();
        CheckPosition();
        Shoot();
    }

    private void ControlUFO()
    {
        transform.position = Vector2.MoveTowards(transform.position, ship.transform.position, thrust * Time.deltaTime);
        Vector3 dir = ship.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle -90, Vector3.forward);
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
        if (collision.collider.name == "Bullet(Clone)")
        {
            Destroy();
        }
        if (collision.collider.name == "Ship")
        {
            gameplay.RocketFail();
        }
    }

    public void Destroy()
    {
        gameplay.ufoDestroyed();
        Destroy(gameObject, 0.01f);
    }

    void Shoot()
    {
        if (timeToShoot <= 0)
        {
            GetComponent<AudioSource>().Play();
            GameObject BulletClone = Instantiate(bullet, new Vector2(bullet.transform.position.x, bullet.transform.position.y), transform.rotation);
            BulletClone.SetActive(true);
            BulletClone.GetComponent<Bullet>().KillOldBullet();
            BulletClone.GetComponent<Rigidbody2D>().AddForce(transform.up * 350);
            timeToShoot = shootSpeed;
        } 
        else
        {
            timeToShoot -= Time.deltaTime;
        }

    }
}
