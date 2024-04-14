using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExample : MonoBehaviour
{
    public GameObject projectile;
    public float launchVelocity = 700f;
    public float shootSpeed = 1;
    public bool melee = true;
    public bool projectileTargetPlayer = true;
    float health = 3;
    float shootTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).rotation = Camera.main.transform.rotation;
        movement();

        /*
        if (melee)
            meleeAttack();
        else {
            shootTimer += Time.deltaTime;
            if(shootTimer > shootSpeed) {
                rangedAttack(new Vector3(launchVelocity, 0, 0));
                shootTimer = 0;
            }
        }
        */

    }

    private void meleeAttack(){
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 4, 1 << 7);

        foreach (var hitCollider in hitColliders) {
            PlayerController player = hitCollider.gameObject.GetComponent<PlayerController>();
            if (player){
                //Debug.Log("hit"); // make this do damage
            }
        }
    }

    private void rangedAttack(Vector3 direction){
        GameObject bullet = Instantiate(projectile, transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody>().AddRelativeForce(direction);

        PlayerController player = null;
    }

    private void getHit(float damage){
        health -= damage;
        if (health < 0){Destroy(this.gameObject);}

    }

    private void movement(){
        // navmesh
    }

}
