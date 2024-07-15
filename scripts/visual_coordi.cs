using UnityEngine;

public class visual_coordi : MonoBehaviour
{
    // public  Quaternion targetEulerAngles;
    // public Camera mainCamera;

    void Start()
    {
        // // mainCamera = Camera.main;
        
        // // Convert target Euler angles to Quaternion
        // // Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);
        // Quaternion targetRotation1 = Quaternion.Euler(new Vector3(0f, 0f, 0f));

        // // Get the camera's rotation
        // Quaternion cameraRotation = mainCamera.transform.rotation;

        // // Calculate the relative rotation
        // Quaternion relativeRotation = Quaternion.Inverse(cameraRotation) * transform.rotation;

        // // Log the relative quaternion
        // Debug.Log("Relative Quaternion: " + relativeRotation);
        // Debug.Log("Relative Quaternion: " + targetRotation1);

        // Apply the relative rotation to this GameObject
        // transform.rotation = mainCamera.transform.rotation * relativeRotation;
    }

    void Update()
    {
        // Quaternion cameraRotation = mainCamera.transform.rotation;
        // Quaternion relativeRotation = Quaternion.Inverse(cameraRotation) * transform.rotation;
        // targetEulerAngles = relativeRotation;

        // Debug.Log("Relative Quaternion: " + relativeRotation);
        // transform.rotation = mainCamera.transform.rotation * relativeRotation;
    }

    void OnDrawGizmos()
    {
        // Draw the local axis of the GameObject for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position +transform.forward * 0.2f );
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (-transform.right * 0.2f));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (transform.up * 0.2f));
    }
}
