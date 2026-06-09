using UnityEngine;

public class NenupharControleur : MonoBehaviour
{
    // PARAMÈTRES DE MOUVEMENT DU NÉNUPHAR
    public float vitesseCourant = 2f;
    public float amplitude = 0.05f;
    public float frequence = 2f;
    public float destroyDistance = 40f; 
    
    private Vector3 posInit;
    private float spawnX;

    void Start() 
    {
        posInit = transform.position;
        spawnX = transform.position.x;
    }

    void Update() 
    {
        // LOGIQUE DE DÉPLACEMENT HORIZONTAL ET OSCILLATION
        posInit.x += vitesseCourant * Time.deltaTime;
        float oscillation = Mathf.Sin(Time.time * frequence) * amplitude;
        transform.position = new Vector3(posInit.x, posInit.y + oscillation, transform.position.z);

        // DESTRUCTION SI TROP LOIN
        if (Mathf.Abs(transform.position.x - spawnX) > destroyDistance) 
        {
            Destroy(gameObject);
        }
    }

}