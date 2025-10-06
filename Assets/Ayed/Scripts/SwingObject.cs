using UnityEngine;

public class SwingObject : MonoBehaviour
{
    [Header("Swing Settings")]
    public float swingAngle = 45f;
    public float swingSpeed = 2f;
    public float startDelay = 0f;

    [Header("Swing Direction")]
    public bool swingOnXAxis = true;
    public bool swingOnYAxis = false;
    public bool swingOnZAxis = false;

    private Quaternion startRotation;
    private float timer;
    private bool isSwinging = false;

    void Start()
    {
        startRotation = transform.rotation;
        timer = -startDelay;

        if (!swingOnXAxis && !swingOnYAxis && !swingOnZAxis)
        {
            swingOnXAxis = true;
        }
    }

    void Update()
    {
        if (!isSwinging) return;

        timer += Time.deltaTime * swingSpeed;


        float angle = Mathf.Sin(timer) * swingAngle;

        Vector3 newRotation = startRotation.eulerAngles;

        if (swingOnXAxis) newRotation.x = angle;
        if (swingOnYAxis) newRotation.y = angle;
        if (swingOnZAxis) newRotation.z = angle;

        transform.rotation = Quaternion.Euler(newRotation);
    }


    public void StartSwinging()
    {
        isSwinging = true;
    }


    public void StopSwinging()
    {
        isSwinging = false;
        transform.rotation = startRotation;
    }


    public void ToggleSwing()
    {
        isSwinging = !isSwinging;
    }


    void OnEnable()
    {
        StartSwinging();
    }


    void OnDisable()
    {
        StopSwinging();
    }
}
