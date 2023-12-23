//C# Example (LookAtPoint.cs)
using UnityEngine;
[ExecuteInEditMode]
public class LookAtPoint : MonoBehaviour
{
    public Vector3 lookAtPoint = Vector3.zero;

    public void Update()
    {
        transform.LookAt(lookAtPoint);
        Vector3 scale = new Vector3(transform.localScale.x,  transform.localScale.y, Vector3.Distance(transform.position, lookAtPoint));
        transform.localScale = scale;
    }
}