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
        //ray�� ���� ��ü�� ������
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        //������ ���� �浹�Ǵ� ��� ��ü�� ������
        //Raycast(��� �������� �����, ���� ray�� ���� �浹�� ��ü, � Ÿ���� ������Ʈ�� �����ð���(�߰�������))
        arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes);

        //hit object counting
        if (hits.Count > 0)
        {
            //���� ���� �ε��� ��ġ�� ��ǥ
            Pose placementPose = hits[0].pose;

            //��ġ�� ������Ʈ Ȱ��ȭ
            placeObject.SetActive(true);
            //�� ������Ʈ�� ��ġ ���� (���� placementPose�� �������ָ� �ȴ�)
            placeObject.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placeObject.SetActive(false);
        }


    }

}
