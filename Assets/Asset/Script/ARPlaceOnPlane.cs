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
        //touchCount�� ����Ʈ���� ��ġ�� �Ͼ ���� Ȯ��, 1 �̻��̸� ��ġ�� �Ͼ�ٴ� ���� ��
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            List<ARRaycastHit> hits = new List<ARRaycastHit>();

            text.text = "No Detect";

            if (arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
            {
                text.text = "Touch!";
                //hits�迭�� ray�� �浹�� ���� ��ġ�� ���� ������ ���� �ȴ�.
                Pose hitPose = hits[0].pose;

                if (!spawnObject)
                {
                    //��ġ�� ������Ʈ Ȱ��ȭ
                    spawnObject = Instantiate(placeObject, hitPose.position, hitPose.rotation);
                }
                //���� �� ��ġ�� �̷������ instantiate�� �ٽ� �̷������ �ʰ� ��ġ�� ȸ���� �ٲ۴�.
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
        //ray�� ���� ��ü�� ������
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        //������ ���� �浹�Ǵ� ��� ��ü�� ������
        //Raycast(��� �������� �����, ���� ray�� ���� �浹�� ��ü, � Ÿ���� ������Ʈ�� �����ð���(�߰�������))
        arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes);

        //hit object counting
        if (hits.Count > 0)
        {
            text.text = "Detect!";
            //���� ���� �ε��� ��ġ�� ��ǥ
            Pose placementPose = hits[0].pose;

            //��ġ�� ������Ʈ Ȱ��ȭ
            if (!spawnObject) spawnObject = Instantiate(placeObject, placementPose.position, placementPose.rotation);
            else spawnObject.SetActive(true);
            //�� ������Ʈ�� ��ġ ���� (���� placementPose�� �������ָ� �ȴ�)
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
                //hits�迭�� ray�� �浹�� ���� ��ġ�� ���� ������ ���� �ȴ�.
                Pose hitPose = hits[0].pose;

                spawnObject.transform.rotation = hitPose.rotation;
                spawnObject.transform.position = Vector3.MoveTowards(spawnObject.transform.position, hitPose.position, 0.2f);
            }
        }
        

    }
    
}
