using UnityEngine;

public class Foot : MonoBehaviour
{
    public string touchTag;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        touchTag = collision.gameObject.tag;
    }

    private void OnCollisionExit2D()
    {
        touchTag = null;
    }
}