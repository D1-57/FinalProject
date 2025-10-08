//using UnityEngine;

//public class Checkpoint : MonoBehaviour
//{
//    public Transform checkSpawn;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.tag == "Player")
//        {
//            other.gameObject.GetComponentInParent<RespawnPlayers>().instance.ChangeSpawn(gameObject.transform);
//        }
//    }
//}
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform checkSpawn;
    private bool isActivated = false;
    public AudioSource audio;
    public AudioClip clip;

    void Start()
    {
        // If no specific spawn point is assigned, use this object's transform
        if (checkSpawn == null)
        {
            checkSpawn = transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            RespawnPlayers.instance.ChangeSpawn(checkSpawn);

            // Visual feedback (optional)
            Debug.Log("Checkpoint activated at: " + checkSpawn.position);

            // You can add visual effects here like changing color, playing sound, etc.
            audio.PlayOneShot(clip);
        }
    }
}