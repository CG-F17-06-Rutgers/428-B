using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialForce : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Agent"))
        {
            Vector3 _forceDirection = other.transform.position - this.transform.position;
            other.gameObject.transform.position+=_forceDirection.normalized;
        }
    }
}
