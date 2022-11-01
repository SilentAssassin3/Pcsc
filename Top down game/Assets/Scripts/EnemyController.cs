using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 1;
    public float speed = 2.0f;

    public bool advMode = true;
    public bool isFollowing = false;

    public bool strafeOnY = false;
    public bool strafeOnX = false;
    public float maxDistance = 5.0f;

    Vector2 origin;
    Vector2 maxPos;
    Vector2 minPos;

    public Transform playerTarget;
    Rigidbody2D myRB;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();

        origin = transform.position;

        if (strafeOnY)
        {
            myRB.constraints = RigidbodyConstraints2D.FreezePositionX &
                               RigidbodyConstraints2D.FreezeRotation;

            maxPos = new Vector2(origin.x, origin.y + maxDistance);
            minPos = new Vector2(origin.x, origin.y - maxDistance);

            if (strafeOnX)
                myRB.velocity = Vector2.up * speed;
            else
                myRB.velocity = Vector2.up * -speed;
        }
        else
        {
            myRB.constraints = RigidbodyConstraints2D.FreezePositionY &
                               RigidbodyConstraints2D.FreezeRotation;

            maxPos = new Vector2(origin.x + maxDistance, origin.y);
            minPos = new Vector2(origin.x - maxDistance, origin.y);

            if (strafeOnX)
                myRB.velocity = Vector2.right * speed;
            else
                myRB.velocity = Vector2.right * -speed;
        }

        playerTarget = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            Destroy(gameObject);

        if (advMode == false)
        {
            if (strafeOnY)
            {
                if (transform.position.y >= maxPos.y)
                    myRB.velocity = Vector2.up * -speed;
                else if (transform.position.y <= minPos.y)
                    myRB.velocity = Vector2.up * speed;
            }
            else
            {
                if (transform.position.x >= maxPos.x)
                    myRB.velocity = Vector2.right * -speed;
                else if (transform.position.x <= minPos.x)
                    myRB.velocity = Vector2.right * speed;
            }
        }

        else
        {
            if (isFollowing == false)
                myRB.velocity = Vector2.zero;

            else
            {
                Vector3 lookPos = playerTarget.position - transform.position;
                myRB.rotation = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;

                // SideScrollers uncomment the following line
                // lookPos.y = myRB.velocity.y;

                myRB.velocity = lookPos * speed;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "playerBullet")
        {
            Destroy(collision.gameObject);
            health--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Player") && advMode)
            isFollowing = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Player") && advMode)
            isFollowing = false;
    }
}
