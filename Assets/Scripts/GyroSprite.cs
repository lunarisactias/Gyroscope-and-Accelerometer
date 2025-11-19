using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GyroSprite : MonoBehaviour
{
    public SpriteRenderer targetSpriteRenderer;
    public Sprite plusX, plusY, plusZ, negX, negY, negZ;
    public Vector3 rotFixEuler = new Vector3(90, 0, 0);

    void Start()
    {
        if (AttitudeSensor.current != null) 
        {
            InputSystem.EnableDevice(AttitudeSensor.current); 
        }
        if (UnityEngine.InputSystem.Gyroscope.current != null) 
        { 
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current); 
        }
        if (!Input.gyro.enabled) 
        {
            Input.gyro.enabled = true; 
        }
    }

    void Update()
    {
        if (!TryGetDeviceRotation(out Quaternion worldRot))
        {
            return;
        }

        Vector3 fwd = worldRot * Vector3.forward;

        float ax = Mathf.Abs(fwd.x), ay = Mathf.Abs(fwd.y), az = Mathf.Abs(fwd.z);
        Sprite s;
        if (ax > ay && ax > az)
        {
            s = fwd.x > 0 ? plusX : negX;
        }
        else if (ay > ax && ay > az)
        {
            s = fwd.y > 0 ? plusY : negY;
        }
        else
        {
            s = fwd.z > 0 ? plusZ : negZ;
        }

        if (targetSpriteRenderer)
        {
            targetSpriteRenderer.sprite = s;
        }
    }

    bool TryGetDeviceRotation(out Quaternion worldRot)
    {
        if (AttitudeSensor.current != null)
        {
            var a = AttitudeSensor.current.attitude.ReadValue();
            var q = new Quaternion(a.x, a.y, -a.z, -a.w);
            worldRot = Quaternion.Euler(rotFixEuler) * q;
            return true;
        }
        if (Input.gyro.enabled)
        {
            var g = Input.gyro.attitude;
            var q = new Quaternion(g.x, g.y, -g.z, -g.w);
            worldRot = Quaternion.Euler(rotFixEuler) * q;
            return true;
        }
        worldRot = Quaternion.identity;
        return false;
    }
}
