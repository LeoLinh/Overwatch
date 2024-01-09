using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    public float damageAmount;

    public float lifeTime, growSpeed = 5f;
    private Vector3 targetSize;

    public bool shouldKnockBack;

    public bool destroyParent;

    // Weapon : Vòng năng lượng đốt quái liên tục. Tạo 1 list để khi quái đi vào thì quái sẽ bị add vào list và bị thiêu đốt
    public bool damageOverTime;
    public float timebetweenDamage;
    private float damageCounter;

    private List<EnemyController> enemiesInrange = new List<EnemyController>();

    public bool destroyOnImpact;

    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject, lifeTime);

        targetSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, growSpeed * Time.deltaTime);

        lifeTime -= Time.deltaTime;

        if(lifeTime <= 0)
        {
            targetSize = Vector3.zero;

            if(transform.localScale.x == 0f)
            {
                Destroy(gameObject);

                if(destroyParent)
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }

        if(damageOverTime == true)
        {
            damageCounter -= Time.deltaTime;

            if(damageCounter <= 0)
            {
                damageCounter = timebetweenDamage;

                for(int i = 0; i < enemiesInrange.Count; i++)
                {
                    if (enemiesInrange[i] != null)
                    {
                        enemiesInrange[i].TakeDamage(damageAmount, shouldKnockBack);
                    }
                    else
                    {
                        enemiesInrange.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageOverTime == false)
        {
            if (collision.tag == "Enemy")
            {
                collision.GetComponent<EnemyController>().TakeDamage(damageAmount, shouldKnockBack);

                if (destroyOnImpact)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (collision.tag == "Enemy")
            {
                enemiesInrange.Add(collision.GetComponent<EnemyController>());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(damageOverTime == true)
        {
            if(collision.tag == "Enemy")
            {
                enemiesInrange.Remove(collision.GetComponent<EnemyController>());
            }
        }
    }
}
