using UnityEngine;

public class Effect_OpenMaskDelay : PPEffectBase
{
    [SerializeField] private GameObject m_MaskObj = null;
    [SerializeField] private Transform cameraLocate;
    [SerializeField] private GameObject[] objectToDestory;
    [SerializeField] private GameObject[] objectToActive;

    public override void Init()
    {
        m_Obj = this.gameObject;
        this.gameObject.SetActive(false);
    }

    public override void StartModify()
    {
        Camera cam = Camera.main;
        cam.GetComponent<FollowObjectY>().enabled = false;
        cam.transform.position = new Vector3(cameraLocate.position.x, cameraLocate.position.y, cam.transform.position.z);

        foreach (GameObject item in objectToDestory)
        {
            Destroy(item);
        }

        foreach (GameObject item in objectToActive)
        {
            item.SetActive(true);
        }

        m_MaskObj.SetActive(true);
        base.StartModify();
    }

    public override void Finish()
    {
        base.Finish();
        m_MaskObj.SetActive(false);
    }
}
