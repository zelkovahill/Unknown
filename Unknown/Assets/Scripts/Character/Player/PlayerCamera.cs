using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG{


public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public Camera cameraObject;
    public PlayerManager player;
    [SerializeField] private Transform cameraPivotTransform;


    
    [Header("Camera Settings")]
    private float cameraSmoothSpeed =1; 
    [SerializeField]  private float leftAndRightRotationSpeed = 220;
    [SerializeField] private float upAndDownRotationSpeed = 220;
    [SerializeField] private float minimiumPivot = -30; 
    [SerializeField] private float maximumPivot = 60;
    [SerializeField] private float cameraCollisionRadius = 0.2f;
    [SerializeField] private LayerMask collisionLayer;


    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition;
    [SerializeField] private float leftAndRightLookAngle;
    [SerializeField] private float upAndDownLookAngle;
    private float cameraZPosition;
    private float targetCameraZPosition;
    


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if(player!=null)
        {
            HandleFollowTarget();
            HandleTotations();
            HandleCollisions();
        }

    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position,ref cameraVelocity, cameraSmoothSpeed*Time.deltaTime);
        transform.position = targetCameraPosition;

    }

    private void HandleTotations(){
       leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;

       upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;

       upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimiumPivot, maximumPivot);


       Vector3 cameraRotation = Vector3.zero;
       Quaternion targetRotation;

       cameraRotation.y = leftAndRightLookAngle;
       targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCollisions(){
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        
        if(Physics.SphereCast(cameraPivotTransform.position,cameraCollisionRadius,direction,out hit,Mathf.Abs(targetCameraZPosition),collisionLayer)){
            float distanceFormHitObject = Vector3.Distance(cameraPivotTransform.position,hit.point);
            targetCameraZPosition = -(distanceFormHitObject - cameraCollisionRadius);
        }

        if(Mathf.Abs(targetCameraZPosition)<cameraCollisionRadius){
            targetCameraZPosition = -cameraCollisionRadius;
        }

        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z,targetCameraZPosition,0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }

}
}