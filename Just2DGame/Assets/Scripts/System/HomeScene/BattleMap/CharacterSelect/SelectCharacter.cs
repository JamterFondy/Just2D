using UnityEngine;


public class SelectCharacter : MonoBehaviour
{
    public int characterID;

    SpriteRenderer spriteRenderer;
    int lastCharacterID = int.MinValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // インスペクタで設定された characterID を尊重するため、ここで 0 に上書きしない
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeIcon();
    }

    // Update is called once per frame
    void Update()
    {
        if(characterID != lastCharacterID)
        {
            ChangeIcon();
        }
    }

    void ChangeIcon()
    {
        lastCharacterID = characterID;

        // SpriteRenderer が見つからない場合は色変更をスキップする
        switch (characterID)
        {
            case 0:
                spriteRenderer.enabled = false;
                break;
            case 1:
                spriteRenderer.enabled = true;
                if (spriteRenderer != null) spriteRenderer.color = Color.red;
                break;
            case 2:
                spriteRenderer.enabled = true;
                if (spriteRenderer != null) spriteRenderer.color = Color.yellow;
                break;
            default:
                spriteRenderer.enabled = true;
                if (spriteRenderer != null) spriteRenderer.color = Color.white;
                break;
        }
    }
}
