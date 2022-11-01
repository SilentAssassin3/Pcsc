using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    private Rigidbody2D myRB;
    public GameObject bullet;
    public int health = 3;
    public int maxHealth = 5;
    public float bulletSpeed = 50;
    public float fireRate = 1.0f;
    public float bulletLifespan = 1;
    public float fireRateAmplifier = .5f;
    public float fireRateTimer = 0;
    public float frBoostCooldown = 5;
    public float speedAmplifier = 25;
    public float speedBoostCooldown = 5;
    public float speedBoostTimer = 0;
    //public float groundDetectDistance = .1f;
    private float fireCountdown = 0;
    public bool canShoot = false;
    public bool speedBoostEnabled = false;
    public bool fireRateBoostEnabled = false;
    public float movementSpeed = 7.5f;
    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        Vector2 raycastPos = new Vector2(transform.position.x, transform.position.y- .51f);
        Vector2 tempVelocity = myRB.velocity;
        if (canShoot)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                GameObject b = Instantiate(bullet, transform.position,Quaternion.identity);
                Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(),b.GetComponent<CapsuleCollider2D>());
                b.GetComponent<Rigidbody2D>().rotation = 90;
                b.GetComponent<Rigidbody2D>().velocity = Vector2.right *bulletSpeed;
                Destroy(b, bulletLifespan);
                canShoot = false;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                GameObject b = Instantiate(bullet, transform.position,Quaternion.identity);
                Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(),b.GetComponent<CapsuleCollider2D>());
                b.GetComponent<Rigidbody2D>().rotation = 90;
                b.GetComponent<Rigidbody2D>().velocity = Vector2.left *bulletSpeed;
                Destroy(b, bulletLifespan);
                canShoot = false;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                GameObject b = Instantiate(bullet, transform.position,Quaternion.identity);
                Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(),b.GetComponent<CapsuleCollider2D>());
                b.GetComponent<Rigidbody2D>().velocity = Vector2.up * bulletSpeed;
                Destroy(b, bulletLifespan);
                canShoot = false;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                GameObject b = Instantiate(bullet, transform.position,Quaternion.identity);
                Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(),b.GetComponent<CapsuleCollider2D>());
                b.GetComponent<Rigidbody2D>().velocity = Vector2.down * bulletSpeed;
                Destroy(b, bulletLifespan);
                canShoot = false;
            }
        }
        else if (!canShoot)
        {
            fireCountdown += Time.deltaTime;
            if (fireCountdown >= fireRate)
            {
                fireCountdown = 0;
                canShoot = true;
            }
        }
        if (speedBoostEnabled)
        {
            speedBoostTimer += Time.deltaTime;
            if (speedBoostTimer >= speedBoostCooldown)
            {
                speedBoostTimer = 0;
                movementSpeed /= speedAmplifier;
                speedBoostEnabled = false;
            }
        }
        if (fireRateBoostEnabled)
        {
            fireRateTimer += Time.deltaTime;
            if (fireRateTimer >= frBoostCooldown)
            {
                fireRateTimer = 0;
                fireRate /= fireRateAmplifier;
                fireRateBoostEnabled = false;
            }
        }
        tempVelocity.x = Input.GetAxisRaw("Horizontal") * movementSpeed;
        /*
        if (Input.GetKeyDown(KeyCode.Space) && Physics2D.Raycast(raycastPos, Vector2.down, groundDetectDistance))
            tempVelocity.y = jumpHeight;
        */
        tempVelocity.y = Input.GetAxisRaw("Vertical") * movementSpeed;
        myRB.velocity = tempVelocity;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "health") && (health < maxHealth))
        {
            Destroy(collision.gameObject);
            health++;
        }
        if ((collision.gameObject.tag == "speed") && !speedBoostEnabled)
        {
            Destroy(collision.gameObject);
            movementSpeed *= speedAmplifier;
            speedBoostEnabled = true;
        }
        if ((collision.gameObject.tag == "fire") && !fireRateBoostEnabled)
        {
            Destroy(collision.gameObject);
            fireRate *= fireRateAmplifier; // Number must be small to shoot faster
            fireRateBoostEnabled = true;
        }
    }
}