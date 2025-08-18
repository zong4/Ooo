using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionWidth = 3f;
    public float explosionHeight = 1f;
    public float explosionForce = 5f;

    public void Explode()
    {
        CheckExplosionRange();
        Destroy(gameObject);
    }

    private void CheckExplosionRange()
    {
        var hashSet = new HashSet<Rigidbody2D>();

        {
            var results = new Collider2D[10]; // todo Adjust size as needed
            var size = Physics2D.OverlapBoxNonAlloc(transform.position,
                new Vector2(explosionWidth * transform.localScale.x - 0.1f,
                    explosionHeight * transform.localScale.y - 0.1f),
                0f,
                results,
                LayerMask.GetMask("Player", "Bomb")); // hack

            for (var i = 0; i < size; i++)
            {
                hashSet.Add(results[i].GetComponent<Rigidbody2D>());
            }
        }

        {
            var results = new Collider2D[10];
            var size = Physics2D.OverlapBoxNonAlloc(transform.position,
                new Vector2(explosionHeight * transform.localScale.x, explosionWidth * transform.localScale.y), 0f,
                results,
                LayerMask.GetMask("Player", "Bomb"));

            for (var i = 0; i < size; i++)
            {
                hashSet.Add(results[i].GetComponent<Rigidbody2D>());
            }
        }

        foreach (var rigidbody2D1 in hashSet)
        {
            if (rigidbody2D1.CompareTag("Bomb"))
            {
                rigidbody2D1.bodyType = RigidbodyType2D.Dynamic;
            }

            var direction = Vector3.zero;
            if (rigidbody2D1.position.y > transform.position.y + 0.1f) // hack
            {
                direction = new Vector3(0f, 1f, 0f);
            }
            else if (rigidbody2D1.position.y < transform.position.y - 0.1f)
            {
                direction = new Vector3(0f, -1f, 0f);
            }
            else if (rigidbody2D1.position.x > transform.position.x + 0.1f)
            {
                direction = new Vector3(1f, 0f, 0f);
            }
            else if (rigidbody2D1.position.x < transform.position.x - 0.1f)
            {
                direction = new Vector3(-1f, 0f, 0f);
            }

            rigidbody2D1.AddForce(direction * explosionForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(explosionWidth, explosionHeight));
        Gizmos.DrawWireCube(transform.position, new Vector2(explosionHeight, explosionWidth));
    }
}