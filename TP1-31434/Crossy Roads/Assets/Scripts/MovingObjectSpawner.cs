using System.Collections;
using UnityEngine;

public class MovingObjectSpawner : MonoBehaviour 
{ 
    [SerializeField] private GameObject spawnObject; 
    [SerializeField] private Transform spawnPos; 
    [SerializeField] private float minSeparationTime = 3f; 
    [SerializeField] private float maxSeparationTime = 6f; 
    [SerializeField] private bool isRightSide = true; 
    [SerializeField] private float spawnCheckRadius = 1f; 
    [SerializeField] private LayerMask vehicleLayer; // opcional, evita colisões com outras layers

    private void Start() 
    { 
        StartCoroutine(SpawnVehicle()); 
    }

    private IEnumerator SpawnVehicle() 
    { 
        while (true) 
        { 
            yield return new WaitForSeconds(Random.Range(minSeparationTime, maxSeparationTime)); 
            
            bool blocked = Physics.CheckSphere(spawnPos.position, spawnCheckRadius, vehicleLayer);

            if (!blocked) 
            { 
                GameObject go = Instantiate(spawnObject, spawnPos.position, Quaternion.identity);

                if (!isRightSide) 
                    go.transform.Rotate(new Vector3(0, 180, 0)); 
            } 
        } 
    } 
}