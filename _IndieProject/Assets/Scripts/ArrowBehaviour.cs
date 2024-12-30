using System.Collections;
using UnityEngine;

public class ArrowProjection: MonoBehaviour
{
    private Rigidbody2D rb;
    public float arrowSpeed;
    private GameObject player;
    private bool isArrowTouch=false;
    private float arrowDuration = 0.5f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");  
        Debug.DrawLine(transform.position, player.transform.position, Color.red, 2f);
        rb.linearVelocity = Vector2.left * arrowSpeed;
        
    }
    private void Update()
    {
        StartCoroutine(arrowCollision(arrowDuration));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "hitBox"))
        {
            
            Destroy(gameObject);
        }
        else if ((collision.gameObject.tag == "Player"))
        {
            isArrow();
            Destroy(gameObject);
        }
    }

    private IEnumerator arrowCollision(float duration)
    {
        isArrowTouch = true;
        yield return new WaitForSeconds(duration);
        isArrowTouch=false;
    }
    public bool isArrow()
    {
        return isArrowTouch;
    }


}
