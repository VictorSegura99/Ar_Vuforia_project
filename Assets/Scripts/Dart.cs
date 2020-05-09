using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    Collider dart_point;
    Rigidbody rigid_body;
    // Start is called before the first frame update
    void Start()
    {
        dart_point = GetComponent<BoxCollider>();
        rigid_body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        rigid_body.constraints = RigidbodyConstraints.FreezeRotation;
        rigid_body.constraints = RigidbodyConstraints.FreezePosition;
    }
}
