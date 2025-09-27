using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    public float pushForce = 5f;
    
    private void OnCollisionEnter(Collision collision)
    {
        // If players collide with each other, push them apart
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 pushDirection = (transform.position - collision.transform.position).normalized;
            Rigidbody otherRb = collision.gameObject.GetComponent<Rigidbody>();
            Rigidbody myRb = GetComponent<Rigidbody>();
            
            if (otherRb != null && myRb != null)
            {
                otherRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
                myRb.AddForce(-pushDirection * pushForce, ForceMode.Impulse);
            }
        }
    }
}