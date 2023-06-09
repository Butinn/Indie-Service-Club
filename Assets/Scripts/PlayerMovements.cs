using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    private Rigidbody rb;
    private float invertedIndex;

    [Header("==========Player Speed setting==========")]
    [SerializeField] float _playerSpeed;

    [Header("==========Player Jump setting==========")]
    [SerializeField] float _jumpForce;
    [SerializeField] float fallMultiplier;
    [SerializeField] float lowJumpMultiplier;
    [SerializeField] Vector3 boxSize;
    [SerializeField] float maxDistance;
    [SerializeField] LayerMask layerMask;

    [Header("==========Dash setting ==========")]
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] float dashingPower = 24f;
    [SerializeField] float dashingTime = 0.2f;
    [SerializeField] float dashingCooldown = 1f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        TurnAround();
        Movements();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void Movements()
    {
        if (isDashing)
        {
            return;
        }

        //Moving LR
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * _playerSpeed * invertedIndex * Time.deltaTime);

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && GroundCheck())
        {
            rb.velocity = transform.up * _jumpForce;
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !GroundCheck())
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
        }
    }

    bool GroundCheck()
    {
        //check if player are touching the ground
        if (Physics.BoxCast(transform.position, boxSize, -transform.up, transform.rotation, maxDistance, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }

    void TurnAround()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        //rotating body

        //rotating body to match gun pos
        if (mousePosition.x < transform.position.x)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            invertedIndex = -1f;
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            invertedIndex = 1f;
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.useGravity = false;
        rb.velocity = new Vector3(transform.localScale.x * dashingPower * invertedIndex, 0f, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.useGravity = true;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
