using UnityEngine;


public class SelectCharacter : MonoBehaviour
{
    public int characterID;

    SpriteRenderer spriteRenderer;
    public int lastCharacterID = int.MinValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // インスペクタで設定された characterID を尊重するため、ここで 0 に上書きしない
        spriteRenderer = GetComponent<SpriteRenderer>();

        characterID = PlayerPrefs.GetInt("GoBattleCharacterID", 0); //保存されたキャラIDを読み込む。保存されていない場合は0（未選択）になる。
        lastCharacterID = characterID; //初期状態を反映させるために lastCharacterID を更新

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
            case 3:
                spriteRenderer.enabled = true;
                if (spriteRenderer != null) spriteRenderer.color = Color.green;
                break;
            default:
                spriteRenderer.enabled = true;
                if (spriteRenderer != null) spriteRenderer.color = Color.white;
                break;
        }
    }
}
