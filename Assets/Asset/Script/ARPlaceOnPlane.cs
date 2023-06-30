using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class ARPlaceOnPlane : MonoBehaviour
{
    // Start is called before she first frame update
    
    //ray caster
    public ARRaycastManager arRaycaster;
    //placed object
    public GameObject placeObject;
    public GameObject spawnObject;

    public GameObject Debugtext;
    private TMP_Text text;
    private int flag;

    void Start()
    {
        flag = 1;
        text = Debugtext.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flag == 1) AutoUpdateCenterObject();
        else if (flag == 2) TouchPlaceObjectByTouch();
        else if (flag == 3) CatMoving();
    }

    public void SetFlag(int newflag)
    {
        flag = newflag;
    }

    public void ModeInitialize()
    {
        spawnObject.SetActive(true);
        Destroy(spawnObject);
        spawnObject = null;
    }


    private void TouchPlaceObjectByTouch()
    {
        //touchCount는 스마트폰의 터치가 일어난 수를 확인, 1 이상이면 터치가 일어났다는 뜻일 듯
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            List<ARRaycastHit> hits = new List<ARRaycastHit>();

            text.text = "No Detect";

            if (arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
            {
                text.text = "Touch!";
                //hits배열에 ray와 충돌한 면의 위치에 대한 정보가 담기게 된다.
                Pose hitPose = hits[0].pose;

                if (!spawnObject)
                {
                    //배치할 오브젝트 활성화
                    spawnObject = Instantiate(placeObject, hitPose.position, hitPose.rotation);
                }
                //따라서 재 터치가 이루어지면 instantiate가 다시 이루어지지 않고 위치와 회전만 바꾼다.
                else
                {
                    spawnObject.transform.position = hitPose.position;
                    spawnObject.transform.rotation = hitPose.rotation;

                }
            }
        }
    }


    private void AutoUpdateCenterObject() {
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
            text.text = "Detect!";
            //가장 먼저 부딪힌 위치의 좌표
            Pose placementPose = hits[0].pose;

            //배치할 오브젝트 활성화
            if (!spawnObject) spawnObject = Instantiate(placeObject, placementPose.position, placementPose.rotation);
            else spawnObject.SetActive(true);
            //그 오브젝트의 위치 설정 (위의 placementPose로 설정해주면 된다)
                spawnObject.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            text.text = "No Detect";
            if (spawnObject) spawnObject.SetActive(false);
        }
    }


    private void CatMoving()
    {
        if (!spawnObject)
        {
            text.text = "No Spawn, Please make object";
            return;
        }

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began){

            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            text.text = "No Detect";

            if (arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
            {
                text.text = "Touch!";
                //hits배열에 ray와 충돌한 면의 위치에 대한 정보가 담기게 된다.
                Pose hitPose = hits[0].pose;

                spawnObject.transform.rotation = hitPose.rotation;
                spawnObject.transform.position = Vector3.MoveTowards(spawnObject.transform.position, hitPose.position, 0.2f);
            }
        }
        

    }
    
}
