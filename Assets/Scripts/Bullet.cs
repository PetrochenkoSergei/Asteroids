using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * 350);
    }

    public void KillOldBullet()
    {
        Destroy(gameObject, 2.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 0.0f);
    }

}
