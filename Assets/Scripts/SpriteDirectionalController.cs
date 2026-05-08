using UnityEngine;

public class SpriteDirecionalController : MonoBehaviour
{
    private void LateUpdate(){
        Vector3 camForwardVector = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
        
    }
}
