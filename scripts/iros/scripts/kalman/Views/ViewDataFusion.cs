using UnityEngine;
using LightweightMatrixCSharp;
using static DebugDraw;

/// <summary>
/// Data fusion of IMU velocity with Stereo camera 3D position, reconstruct noisy position
/// </summary>
public class ViewDataFusion : MonoBehaviour
{
    [Header("Kalman settings")]
    [SerializeField]
    private float _processCovariance = 0.0001f;

    public GameObject ee;
    public Vector3 position;

    // [Header("Linked objects")]
    // [SerializeField]
    // private DepthSensingCamera _depthCamera;

    // [SerializeField]
    // private InertialMeasurementUnit _sensorIMU;

    [Header("Debug")]
    [SerializeField]
    private Color _color = Color.cyan;

    // private StatTracker _trackerNormal = new StatTracker();
    // private StatTracker _trackerDropOut = new StatTracker();

    private PositionDataFusion _dataFusion;

    protected void Start()
    {
        // _dataFusion = new PositionDataFusion(Simulation.DeltaTime, transform.position, _processCovariance, _depthCamera.Noise, _sensorIMU.Noise * Simulation.DeltaTime);
        _dataFusion = new PositionDataFusion(1f/30f, transform.position, _processCovariance, 0.05f, 0.01f * 1f/30f);
        this.transform.position = ee.transform.position;
    }

    /// <summary>
    /// Fixed update of the process
    /// </summary>
    public void Update()
    {
        
        position = ee.transform.position;
        
       
        _dataFusion.Update(position);

        transform.position = _dataFusion.GetPositionFromFilter();

        // if (_depthCamera.MeasureAvailable)
        // {
        //     _trackerNormal.AddValue(_sensorIMU.transform.position, transform.position);
        // }
        // {
        //     _trackerDropOut.AddValue(_sensorIMU.transform.position, transform.position);
        // }

        DrawDebugInformations();
    }

    /// <summary>
    /// Draw estimated point and error from the real position
    /// </summary>
    private void DrawDebugInformations()
    {
        // Color debugColor = (_depthCamera.MeasureAvailable) ? _color : new Color(_color.r, _color.g, _color.b, 0.5f);
         Color debugColor =  new Color(_color.r, _color.g, _color.b, 0.5f);

        DrawPoint(transform.position, debugColor);
        // Debug.DrawLine(_sensorIMU.transform.position, transform.position, debugColor, DebugDraw.DebugTime);
    }

    void OnGUI()
    {
        GUILabel("Data Fusion using Kalman Filter", GUIPosition.BottomRight);
        GUILabel("Average Precision (cm)", GUIPosition.BottomRight, 1);
        // GUILabel("• Normal " + (100f * _trackerNormal.GetMeanPrecision()).ToString("0.00"), GUIPosition.BottomRight, 2);
        // GUILabel("• Drop-out " + (100f * _trackerDropOut.GetMeanPrecision()).ToString("0.00"), GUIPosition.BottomRight, 3);
    }

    void OnDestroy()
    {
        _dataFusion = null;
    }
}