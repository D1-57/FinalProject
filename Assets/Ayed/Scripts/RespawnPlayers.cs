//using UnityEngine;

//public class RespawnPlayers : MonoBehaviour
//{
//    public Transform MainPlayer;
//    public Transform Player1;
//    public Transform Player2;
//    public Transform Spawn;
//    public RespawnPlayers instance;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {

//    }
//    private void Awake()
//    {
//        instance = this;
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//    public void RespawnAllPlayers()
//    {
//        MainPlayer.position = Spawn.position;
//        Player1.position += new Vector3(0, 1, 0);
//        Player2.position += new Vector3(0, 1, 0);
//    }
//    public void ChangeSpawn(Transform NewSpawn)
//    {
//        Spawn.position = NewSpawn.position + new Vector3(0, 1, 0);
//    }
//}
using UnityEngine;

public class RespawnPlayers : MonoBehaviour
{
    public Transform MainPlayer;
    public Transform Player1;
    public Transform Player2;
    public Transform Spawn;
    public static RespawnPlayers instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Make sure we don't destroy this object when loading new scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize spawn position if not set
        if (Spawn == null)
        {
            Spawn = new GameObject("DefaultSpawn").transform;
            Spawn.position = Vector3.zero;
        }
    }

    public void RespawnAllPlayers()
    {
        if (MainPlayer != null) MainPlayer.position = Spawn.position;
        if (Player1 != null) Player1.position = Spawn.position;
        if (Player2 != null) Player2.position = Spawn.position;

        Debug.Log("All players respawned at: " + Spawn.position);
    }

    public void ChangeSpawn(Transform NewSpawn)
    {
        if (NewSpawn != null)
        {
            Spawn.position = NewSpawn.position;
            Debug.Log("Spawn point updated to: " + NewSpawn.position);
        }
    }
}