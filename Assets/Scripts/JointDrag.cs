using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]

public class JointDrag : MonoBehaviour
{
    public float dragForce;
    private Vector3 screenPoint;
    private Vector3 offset;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        Vector3 direction = curPosition - transform.position;

        GetComponent<Rigidbody>().AddForce(Vector3.Normalize(direction) * dragForce, ForceMode.Impulse);
        //transform.position = curPosition;

    }
    

}