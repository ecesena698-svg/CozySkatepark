using UnityEngine;

public class WatcherAI : MonoBehaviour
{
    [Header("Hocanýn Ýstediði 3 Deðer")]
    public float gorusMesafesi = 15f; // Varsayýlan 15m
    public float gorusAcisi = 90f; // Varsayýlan 90 derece
    public float gozYuksekligi = 1.5f;

    [Header("Baðlantýlar")]
    public Transform player;
    public Renderer watcherRenderer; // Kýzýn rengini deðiþtirmek için (Mesh Renderer sürükle)
    public Color normalRenk = Color.green;
    public Color algilamaRengi = Color.red;

    void Update()
    {
        if (player == null) return;

        bool goruluyorMu = false;

        // Göz noktasýný ve oyuncunun hedef noktasýný ayarlýyoruz (ayaklarýna deðil gövdeye baksýn)
        Vector3 gozNoktasi = transform.position + Vector3.up * gozYuksekligi;
        Vector3 oyuncuMerkezi = player.position + Vector3.up * 15.0f;

        Vector3 oyuncuyaGidenYon = (oyuncuMerkezi - gozNoktasi).normalized;
        float aradakiMesafe = Vector3.Distance(gozNoktasi, oyuncuMerkezi);

        // 1. Menzil Kontrolü
        if (aradakiMesafe <= gorusMesafesi)
        {
            // 2. Koni (Açý) Kontrolü - NOKTA ÇARPIM (Madde 4)
            float noktaCarpim = Vector3.Dot(transform.forward, oyuncuyaGidenYon);
            float aciSiniri = Mathf.Cos((gorusAcisi / 2f) * Mathf.Deg2Rad);

            if (noktaCarpim >= aciSiniri)
            {
                // 3. Görüþ Hattý Engel Kontrolü - LINECAST (Madde 5)
                // Gözden oyuncuya bir çizgi çekiyoruz. Eðer bir þeye çarparsa "hit" içine kaydedilir.
                if (Physics.Linecast(gozNoktasi, oyuncuMerkezi, out RaycastHit hit))
                {
                    // Çarptýðý þey oyuncuysa aramýzda engel yoktur
                    if (hit.transform == player)
                    {
                        goruluyorMu = true;
                    }
                }
            }
        }

        // 4. Geri Bildirim Uygulama (Madde 6)
        if (goruluyorMu)
        {
            watcherRenderer.material.color = algilamaRengi;

            // ÇAPRAZ ÇARPIM ile Sað/Sol tespiti
            Vector3 caprazCarpim = Vector3.Cross(transform.forward, oyuncuyaGidenYon);

            // Y ekseni pozitifse saðdadýr, negatifse soldadýr
            if (caprazCarpim.y > 0)
                Debug.Log("Oyuncu Gözcünün SAÐINDA!");
            else
                Debug.Log("Oyuncu Gözcünün SOLUNDA!");
        }
        else
        {
            watcherRenderer.material.color = normalRenk;
        }
    }

    // 5. Gizmos Çizimi (Madde 7 - Hocanýn formülüyle)
    void OnDrawGizmos()
    {
        Vector3 gozNoktasi = transform.position + Vector3.up * gozYuksekligi;

        // Tel küre çizimi
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gozNoktasi, gorusMesafesi);

        // Koninin iki kenarýnýn çizimi (Hocanýn verdiði formül)
        Gizmos.color = Color.blue;
        Vector3 sagKenar = Quaternion.AngleAxis(gorusAcisi / 2, Vector3.up) * transform.forward;
        Vector3 solKenar = Quaternion.AngleAxis(-gorusAcisi / 2, Vector3.up) * transform.forward;

        Gizmos.DrawRay(gozNoktasi, sagKenar * gorusMesafesi);
        Gizmos.DrawRay(gozNoktasi, solKenar * gorusMesafesi);
    }
}