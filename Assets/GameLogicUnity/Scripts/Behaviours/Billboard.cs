using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
    private Transform trans;
    private Transform mainCameraTrans;

    public virtual void Start()
    {
        trans = transform;
        mainCameraTrans = Camera.main.transform;
    }

    // Late update on purpose so it runs after all coroutines have updated object positions
    public virtual void LateUpdate()
    {
        //transform.LookAt(transform.position + mainCameraTrans.rotation * Vector3.forward, mainCameraTrans.rotation * Vector3.up);

        var camRot = mainCameraTrans.rotation;
        trans.rotation = Quaternion.Euler(camRot.eulerAngles.x, camRot.eulerAngles.y, trans.rotation.z);
    }
}
