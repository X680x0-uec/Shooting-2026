using System.Collections;
using UnityEngine;

public class PlayerHp : MonoBehaviour {
    [SerializeField] private float hp = 5;
    [SerializeField] private GameObject player;
    private float invincibleTime = 2f;
    private float blinkInterval = 0.1f;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        spriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (isInvincible) return;

        if (collision.gameObject.CompareTag("EnemyBullet")) {
            if (hp > 0) {
                StartCoroutine(deleteShoot());
                StartCoroutine(InvincibleCoroutine());
                hp -= 1;
            } else {
                Destroy(player.gameObject);
            }
        }
    }


    private IEnumerator deleteShoot() {
        float r = 0;
        GameObject[] eweapons = GameObject.FindGameObjectsWithTag("EnemyBullet");
        while (true) {
            int i = 0;
            foreach (GameObject eweapon in eweapons) {
                if (eweapon == null) {
                    i++;
                    if (i == eweapons.Length) {
                        yield break;
                    }
                    continue;
                }

                float distance = Vector2.Distance(this.transform.position, eweapon.transform.position);
                if (distance <= r) {
                    Destroy(eweapon.gameObject);
                }
            }
            r += 0.3f;
            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator InvincibleCoroutine() {
        isInvincible = true;
        float elapsed = 0f;
        while (elapsed < invincibleTime) {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    public void ChangeHp(float value) {
        hp += value;
    }
}
