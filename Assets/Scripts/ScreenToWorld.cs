using UnityEngine;

public class ScreenToWorld : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float maxDistance = 100.0f;
    #endregion
    #region references
    private Camera myCamera;
    private Transform myCameraTransform;
    #endregion
    #region properties
    private RaycastHit myRaycastHit;
    private LayerMask myLayerMask;
    #endregion
    #region methods
    public Vector3 ScreenToWorldPoint(Vector3 screenPoint)
    {
        Ray ray = myCamera.ScreenPointToRay(screenPoint);

        if (Physics.Raycast(ray.origin, ray.direction, out myRaycastHit, maxDistance, myLayerMask))
        {
            return myRaycastHit.point;
        }
        else return myCameraTransform.position;
    }
    #endregion

    void Start()
    {
        myCamera = Camera.main;
        myCameraTransform = myCamera.transform;
        myLayerMask = LayerMask.GetMask("Suelo");
    }
}
