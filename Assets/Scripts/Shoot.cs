using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gunBarrel;
    [SerializeField] float range;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shooting();
        }
    }

    void Shooting()
    {
        RaycastHit hit;
        if (Physics.Raycast(gunBarrel.position, gunBarrel.right, out hit, range))
        {

        }
    }
    
}
