using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Takip Ayarlarý")]
    public Transform target; // Nereye bakacaðýz?
    public Vector3 offset = new Vector3(0, 4f, -6f); // Karakterin 4 metre üstü, 6 metre arkasý
    public float smoothSpeed = 5f; // Yumuþaklýk ayarý

    // Hocanýn ödevde özellikle istediði "LateUpdate" fonksiyonu
    void LateUpdate()
    {
        if (target == null) return;

        // Kameranýn gitmek istediði yer
        Vector3 desiredPosition = target.position + offset;

        // "Kare hýzýndan baðýmsýz yumuþatma" þartý (Time.deltaTime ve Lerp kullanýmý)
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Kamera her zaman hedefe dönük olsun
        transform.LookAt(target);
    }
}