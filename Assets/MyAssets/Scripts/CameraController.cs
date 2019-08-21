using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject focusObject;

    Vector3 lastFocusPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        CheckFocusMoved();
    }

    //Sets the focus object
    public void SetFocus(GameObject focus)
    {
        focusObject = focus;
    }

    private void CheckInput()
    {

    }

    public void Move(Vector3 vector)
    {
        ResetCameraPosition();
        gameObject.transform.Translate(vector.x, 0, vector.z);
        gameObject.transform.RotateAround(focusObject.transform.position, Vector3.up, vector.y);
    }

    private void ResetCameraPosition()
    {
        Vector3 newPos = focusObject.transform.position;
        newPos += (-focusObject.transform.forward) * 8;
        newPos += Vector3.up * 3;

        gameObject.transform.SetPositionAndRotation(newPos, Quaternion.identity);
        gameObject.transform.LookAt(focusObject.transform);
    }

    private void CheckFocusMoved()
    {
        if (focusObject != null)
        {
            if ((focusObject.transform.position - lastFocusPos).magnitude < new Vector3(1, 1, 1).magnitude);
            {
                ResetCameraPosition();
            }
        }
    }
}
