using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objeto;     
    public Transform spawnPoint1;  
    public Transform spawnPoint2; 

    public float tiempoSpawn = 7f;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= tiempoSpawn)
        {
            SpawnObjeto();
            timer = 0f;
        }
    }

    void SpawnObjeto()
    {
        Transform spawnElegido = Random.value < 0.5f ? spawnPoint1 : spawnPoint2;

        Instantiate(objeto, spawnElegido.position, spawnElegido.rotation);
    }
}