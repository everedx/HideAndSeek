using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{

    private FieldOfView fieldOfView;
    [SerializeField] private GameObject fovPrefab;
    private EnemyChaser eChase;

    private void Start()
    {
       fieldOfView = Instantiate(fovPrefab,null).GetComponent<FieldOfView>();
       fieldOfView.setSpawner(gameObject);
       eChase = GetComponent<EnemyChaser>();
    }

    private void Update()
    {
        fieldOfView.setOrigin(transform.position);
        switch (eChase.DirToGo)
        {
            case 0:
                fieldOfView.setAimDirection(Vector3.left);
                break;
            case 1:
                fieldOfView.setAimDirection(Vector3.up);
                break;
            case 2:
                fieldOfView.setAimDirection(Vector3.down);
                break;
            case 3:
                fieldOfView.setAimDirection(Vector3.right);
                break;
        }
    }


}
