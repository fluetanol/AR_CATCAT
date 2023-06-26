using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaceOnPlane : MonoBehaviour
{
    // Start is called before she first frame update
    
    //ray caster
    public ARRaycastManager arRaycaster;
    //placed object
    public GameObject placeObject;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCenterObject();
    }

    private void UpdateCenterObject() {

        //take camera center point
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        //ray에 맞은 객체를 가져옴
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        //광선을 쏴서 충돌되는 모든 객체를 가져옴
        //Raycast(어느 방향으로 쏠건지, 실제 ray를 쏴서 충돌된 객체, 어떤 타입의 오브젝트만 가져올건지(추가인자임))
        arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes);

        //hit object counting
        if (hits.Count > 0)
        {
            //가장 먼저 부딪힌 위치의 좌표
            Pose placementPose = hits[0].pose;

            //배치할 오브젝트 활성화
            placeObject.SetActive(true);
            //그 오브젝트의 위치 설정 (위의 placementPose로 설정해주면 된다)
            placeObject.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placeObject.SetActive(false);
        }


    }

}
