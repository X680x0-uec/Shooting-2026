using UnityEngine;

public class Bullet : MonoBehaviour
{
    // テクスチャを変えるという処理をBullet、弾の発射処理をBulletCommonに分けました
    [SerializeField] private SpriteRenderer mySprite; //自身のSpriteRendererコンポーネント参照用
    [SerializeField] private Sprite normalTexture; //普通時のテクスチャ
    [SerializeField] private Sprite timeStopTexture; //時間停止時のテクスチャ

    void Start() {
        mySprite.sprite = normalTexture; //初期状態は普通時のテクスチャ
    }


    public void Stop() {
        //時間停止時の処理（外部から呼ばれるためpublic）
        mySprite.sprite = timeStopTexture;
    }

    public void Move() {
        //時間が流れる時の処理（外部から呼ばれるためpublic）
        mySprite.sprite = normalTexture;
    }
}
