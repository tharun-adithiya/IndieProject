using System.Collections;
using UnityEngine;

public class ArrowBehaviour: MonoBehaviour
{
    public Transform firePoint;
    public LayerMask playerLayer;
    public float attackRadius=20f;
    public GameObject Arrow;
    float timer;


    private void Start()
    {
        firePoint =GetComponent<Transform>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.tag == "hitBox")
        {
           // StartCoroutine(isDamage());
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (isInRange()&&timer>2f)
        {
            timer = 0;
            shoot();
        }
        
    }

    void shoot()
    {
        GameObject arrow= Instantiate(Arrow,firePoint.position,firePoint.rotation);
        
    }


    bool isInRange()
    {
        return Physics2D.OverlapCircle(firePoint.position, attackRadius, playerLayer); 
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, attackRadius);
        
    }
}
