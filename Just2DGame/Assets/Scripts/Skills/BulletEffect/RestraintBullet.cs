using UnityEngine;

public class RestraintBullet : MonoBehaviour
{
    [Tooltip("“G‚ª‚±‚Ì’e‚ÆG‚ê‚½‚Æ‚«‚ÉS‘©‚³‚ê‚é•b”")]
    public float restraintDuration = 2f;

    [Tooltip("ƒqƒbƒg‚É‚±‚Ì’e‚ğ”jŠü‚·‚é‚©")]
    public bool destroyOnTrigger = false;

    // 2D •¨——p
    void OnTriggerEnter2D(Collider2D other)
    {
        TryApplyRestraint(other.gameObject);
    }

    void TryApplyRestraint(GameObject target)
    {
        if (target == null) return;

        // ’¼ÚƒAƒ^ƒbƒ`‚³‚ê‚Ä‚¢‚é‚©Ae‚É‚ ‚é‰Â”\«‚ğl—¶‚µ‚ÄŒŸõ
        var enemy = target.GetComponent<EnemyMovement>();
        if (enemy == null)
            enemy = target.GetComponentInParent<EnemyMovement>();

        if (enemy != null)
        {
            enemy.ApplyRestraint(restraintDuration);
            if (destroyOnTrigger)
                Destroy(gameObject);
        }
    }
}
