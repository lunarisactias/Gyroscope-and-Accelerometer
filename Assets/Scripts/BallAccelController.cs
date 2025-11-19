using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallAccelController : MonoBehaviour
{
    public float speed = 12f;
    public float deadZone = 0.03f;
    public bool autoCalibrateOnStart = true;

    Rigidbody rb;
    Vector2 calib;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (autoCalibrateOnStart)
        {
            calib = ReadTiltXY();
        }
    }

    void FixedUpdate()
    {
        Vector2 tilt = ReadTiltXY() - calib;

        if (tilt.magnitude < deadZone)
        {
            tilt = Vector2.zero;
        }

        Vector3 force = new Vector3(tilt.x, 0f, tilt.y) * speed;
        rb.AddForce(force, ForceMode.Acceleration);
    }

    Vector2 ReadTiltXY()
    {
        Vector3 a = Input.acceleration;
        return new Vector2(a.x, a.y);
    }

    public void CalibrateNow() => calib = ReadTiltXY();
}
