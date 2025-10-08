//using UnityEngine;

//public class TrapsSpawn : MonoBehaviour
//{
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
//            other.gameObject.GetComponentInParent<RespawnPlayers>().instance.RespawnAllPlayers();
//        }
//    }
//}
using UnityEngine;

public class TrapsSpawn : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip clip;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit trap, respawning...");
            audio.PlayOneShot(clip);
            RespawnPlayers.instance.RespawnAllPlayers();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            RespawnPlayers.instance.RespawnAllPlayers();
        }
    }
}