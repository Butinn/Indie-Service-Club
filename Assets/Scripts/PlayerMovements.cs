using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    private Rigidbody rb;

    [Header("==========Player Speed setting==========")]
    [SerializeField] float _playerSpeed;

    [Header("==========Player Jump setting==========")]
    [SerializeField] float _jumpForce;
    [SerializeField] float fallMultiplier;
    [SerializeField] float lowJumpMultiplier;
    [SerializeField] Vector3 boxSize;
    [SerializeField] float maxDistance;
    [SerializeField] LayerMask layerMask;

    private float invertedIndex;

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
    }

    void Movements()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * _playerSpeed * invertedIndex * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && GroundCheck())
        {
            //rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
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
}
