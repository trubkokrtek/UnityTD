using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool doMovement = true;

    public float Speed = 30f;
    public float ScrollSpeed = 5f;
    public float Border = 10f;

    public float Min = 10f;
    public float Max = 80f;

    void Update()
    {
        if(Stats.isGameOver)
        {
            return;
        }
        if(!doMovement)
        {
            return;
        }
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - Border)
        {
            transform.Translate(Vector3.forward * Speed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= Border)
        {
            transform.Translate(Vector3.back * Speed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - Border)
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= Border)
        {
            transform.Translate(Vector3.left * Speed * Time.deltaTime, Space.World);
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;
        pos.y -= scroll * 1000 * Speed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, Min, Max);
        transform.position = pos;
    }
}
