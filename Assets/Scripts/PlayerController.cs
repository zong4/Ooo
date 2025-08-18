using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Move
    public float moveSpeed = 3f;

    // Bomb
    private Foot _foot;
    public GameObject bombPrefab;
    public int maxBombs = 2;
    private readonly List<Bomb> _bombs = new List<Bomb>();

    private void Start()
    {
        _foot = transform.GetComponentInChildren<Foot>();

        if (!bombPrefab)
        {
            Debug.LogError("Bomb Prefab is not assigned in the inspector.");
        }
    }

    private void Update()
    {
        Move();

        if ((Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0)) && _foot.touchTag is "Ground" or "Bomb" &&
            _bombs.Count < maxBombs)
        {
            if (bombPrefab)
            {
                var oldPosition = transform.position;
                transform.position = new Vector3(transform.position.x,
                    transform.position.y + bombPrefab.transform.localScale.y, transform.position.z);
                _bombs.Add(Instantiate(bombPrefab, oldPosition, Quaternion.identity).GetComponent<Bomb>());
            }
        }

        if ((Input.GetKeyDown(KeyCode.K) || Input.GetMouseButtonDown(1)) && _bombs.Count > 0)
        {
            _bombs[0].Explode();
            _bombs.RemoveAt(0);
        }
    }

    private void Move()
    {
        // // In the air
        // if (_foot.touchTag == null || (_foot.touchTag != "Ground" && _foot.touchTag != "Bomb"))
        //     return;

        var moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        transform.Translate(moveInput * (Time.deltaTime * moveSpeed));
    }
}