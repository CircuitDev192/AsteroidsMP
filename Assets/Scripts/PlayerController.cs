using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class PlayerController : NetworkAnimator
{

    [SerializeField] private int direction;
    [SerializeField] private int move;
    [SerializeField] private float rotateAngle = 100f;
    [SerializeField] private float moveSpeed = 3f;

    [SerializeField] private Rigidbody rb;

    private bool _dead = false;
    public bool Dead => _dead;

    [SerializeField] private GameObject clone;
    private GameObject[] ghosts = new GameObject[8];
    private float screenWidth;
    private float screenHeight;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;

        Vector3 screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.y));
        Vector3 screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.y));

        screenWidth = screenTopRight.x - screenBottomLeft.x;
        screenHeight = screenTopRight.z - screenBottomLeft.z;

        for (int i = 0; i < 8; i++)
        {
            ghosts[i] = Instantiate(clone, Vector3.zero, Quaternion.identity);
        }

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;
        CheckInput();
        Rotate();
        Move();
        PositionGhostShips();
        CheckIfDead();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Asteroid"))
        {
            _dead = true;
        }
    }

    void CheckInput()
    {
        direction = 0;
        direction = Input.GetKey(KeyCode.A) ? direction - 1 : direction;
        direction = Input.GetKey(KeyCode.D) ? direction + 1 : direction;

        move = Input.GetKey(KeyCode.W) ? 1 : 0;
    }

    void CheckIfDead()
    {
        if (Dead)
        {
            Debug.Log("DEAD");
        }
    }

    void Rotate()
    {
        Quaternion rotation = Quaternion.AngleAxis(direction * Time.deltaTime * rotateAngle, Vector3.up);
        this.transform.forward = rotation * this.transform.forward;
        //this.gameObject.transform.Rotate(Vector3.up * direction * Time.deltaTime * turnSpeed);
    }

    void Move()
    {
        rb.velocity += transform.forward * moveSpeed * move * Time.deltaTime;
        //this.gameObject.transform.Translate(Vector3.forward * move * Time.deltaTime * moveSpeed);
    }

    void PositionGhostShips()
    {
        // TAKEN FROM https://gamedevelopment.tutsplus.com/articles/create-an-asteroids-like-screen-wrapping-effect-with-unity--gamedev-15055
        
        // All ghost positions will be relative to the ships (this) transform,
        // so let's star with that.
        Vector3 ghostPosition = transform.position;

        // We're positioning the ghosts clockwise behind the edges of the screen.
        // Let's start with the far right.
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.z = transform.position.z;
        ghosts[0].transform.position = ghostPosition;

        // Bottom-right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.z = transform.position.z - screenHeight;
        ghosts[1].transform.position = ghostPosition;

        // Bottom
        ghostPosition.x = transform.position.x;
        ghostPosition.z = transform.position.z - screenHeight;
        ghosts[2].transform.position = ghostPosition;

        // Bottom-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.z = transform.position.z - screenHeight;
        ghosts[3].transform.position = ghostPosition;

        // Left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.z = transform.position.z;
        ghosts[4].transform.position = ghostPosition;

        // Top-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.z = transform.position.z + screenHeight;
        ghosts[5].transform.position = ghostPosition;

        // Top
        ghostPosition.x = transform.position.x;
        ghostPosition.z = transform.position.z + screenHeight;
        ghosts[6].transform.position = ghostPosition;

        // Top-right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.z = transform.position.z + screenHeight;
        ghosts[7].transform.position = ghostPosition;

        // All ghost ships should have the same rotation as the main ship
        for (int i = 0; i < 8; i++)
        {
            ghosts[i].transform.rotation = this.transform.rotation;
        }
    }
}
