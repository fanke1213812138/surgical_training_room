// using UnityEngine;
 
// public class random_point : MonoBehaviour
// {
//     public enum _AreaType { Square, Circle }
//     public _AreaType type;
//     public Color Color = Color.red;
 
//     public Quaternion GetRotation()
//     {
//         return Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
//     }
 
 
//     public Vector3 GetPosition()
//     {
//         if (type == _AreaType.Square)
//         {
//             float x = Random.Range(-transform.localScale.x, transform.localScale.x);
//             float z = Random.Range(-transform.localScale.z, transform.localScale.z);
//             Vector3 v = transform.position + transform.rotation * new Vector3(x, 0, z);
//             return v;
//         }
//         else if (type == _AreaType.Circle)
//         {
//             Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
//             return transform.position + transform.rotation * dir * Random.Range(0, GetMaximumScale());
//         }
 
//         return transform.position;
//     }
 
//     private float GetMaximumScale()
//     {
//         float scale = Mathf.Max(transform.localScale.x, transform.localScale.y);
//         return Mathf.Max(scale, transform.localScale.z);
//     }
 
//     void OnDrawGizmos()
//     {
//         Gizmos.color = Color;
 
//         if (type == _AreaType.Square)
//         {
//             Vector3 p1 = transform.position + transform.rotation * new Vector3(transform.localScale.x, 0, transform.localScale.z);
//             Vector3 p2 = transform.position + transform.rotation * new Vector3(transform.localScale.x, 0, -transform.localScale.z);
//             Vector3 p3 = transform.position + transform.rotation * new Vector3(-transform.localScale.x, 0, transform.localScale.z);
//             Vector3 p4 = transform.position + transform.rotation * new Vector3(-transform.localScale.x, 0, -transform.localScale.z);
 
//             Gizmos.DrawLine(p1, p2);
//             Gizmos.DrawLine(p1, p3);
//             Gizmos.DrawLine(p2, p4);
//             Gizmos.DrawLine(p3, p4);
//         }
//         else if (type == _AreaType.Circle)
//         {
//             UnityEditor.Handles.color = Color;
//             UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, GetMaximumScale());
//         }
//     }
// }