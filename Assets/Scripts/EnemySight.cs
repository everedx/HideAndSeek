using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{

    private FieldOfView fieldOfView;
    [SerializeField] private GameObject fovPrefab;
    private EnemyChaser eChase;
    private Vector2 vec;

    private void Start()
    {
       fieldOfView = Instantiate(fovPrefab,null).GetComponent<FieldOfView>();
       fieldOfView.setSpawner(gameObject);
       eChase = GetComponent<EnemyChaser>();
    }

    private void LateUpdate()
    {
        fieldOfView.setOrigin(transform.position);
        if (eChase.State == EnemyChaser.States.LookingForPlayer)
        {
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

        if (eChase.State == EnemyChaser.States.Chasing)
        {
            vec = new Vector2(eChase.WorldPosPlayer.x - transform.position.x, eChase.WorldPosPlayer.y - transform.position.y);
            float angle = Vector2.SignedAngle(Vector2.right,vec)+90;
            fieldOfView.setAimDirection(fieldOfView.getVectorFromAngle(angle));
        }

        if (eChase.State == EnemyChaser.States.Patrolling || eChase.State == EnemyChaser.States.ComingBack)
        {
            float angle = Vector2.SignedAngle(Vector2.right, vec) + 90;
            fieldOfView.waveAimDirection(fieldOfView.getVectorFromAngle(angle));
        }
        

    }

    public void setAngle(Vector2 vec,bool doInstant)
    {
        this.vec = vec;
        if (doInstant)
        {
            float angle = Vector2.SignedAngle(Vector2.right, vec) + 90;
            fieldOfView.setAimDirection(fieldOfView.getVectorFromAngle(angle));
        }
        
    }


}
