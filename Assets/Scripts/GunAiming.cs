using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAiming : MonoBehaviour
{
    [SerializeField] Transform gunBarrel;
    [SerializeField] float range;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GunRotation();
    }

    void GunRotation()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        transform.LookAt(mousePosition);
    }

}
