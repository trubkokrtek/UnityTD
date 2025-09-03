using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;
    public float explosionRadius = 0f;
    public float damage;

    public void Seek(Transform _target, float _damage)
    {
        target = _target;
        damage = _damage;
    }

    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }
    void HitTarget()
    {
        Debug.Log("Hit");

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target, damage);
        }
        Destroy(gameObject);
    }
    void Damage(Transform enemy, float Damage)
    {
        enemy.GetComponent<Enemy>().TakeDamage(Damage);
    }

    void Explode()
    {
        Collider[] coliders = Physics.OverlapSphere(this.transform.position, explosionRadius);
        foreach(Collider collider in coliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform, damage);
            }
        }
    }
}
