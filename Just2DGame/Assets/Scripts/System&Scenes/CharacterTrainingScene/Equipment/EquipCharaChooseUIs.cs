using UnityEngine;
using UnityEngine.UI;

public class EquipCharaChooseUIs : MonoBehaviour
{
    [SerializeField] Sprite[] equipmentSprites;
    [SerializeField] Sprite[] characterSprites;

    [SerializeField] GameObject equipmentIcon;
    [SerializeField] GameObject characterIcon;

    Vector3 myPos;
     void Awake()
    {
        myPos = transform.position;
    }
     void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            esc();
        }
    }

    public void ReceiveData(string EquipmentName, int charaID)
    {
        transform.position = new Vector3(myPos.x, 0, myPos.z);

        // 画面上の装備アイコンとキャラアイコンを受け取ったデータに基づいて更新
        foreach (var equipSprite in equipmentSprites)
        {
            if(equipSprite.name == EquipmentName)
            {
                equipmentIcon.GetComponent<Image>().sprite = equipSprite;
            }
        }

        characterIcon.GetComponent<Image>().sprite = characterSprites[charaID];
    }

    public void UpArrow()
    {
        // キャラIDを1増やす
        int currentCharaID = System.Array.IndexOf(characterSprites, characterIcon.GetComponent<Image>().sprite);
        int newCharaID = (currentCharaID + 1) % characterSprites.Length; // キャラ数に応じてループ
        characterIcon.GetComponent<Image>().sprite = characterSprites[newCharaID];
    }

    public void DownArrow()
    {
        // キャラIDを1減らす
        int currentCharaID = System.Array.IndexOf(characterSprites, characterIcon.GetComponent<Image>().sprite);
        int newCharaID = (currentCharaID - 1 + characterSprites.Length) % characterSprites.Length; // キャラ数に応じてループ
        characterIcon.GetComponent<Image>().sprite = characterSprites[newCharaID];
    }

    public void ConfirmEquipToChara()
    {
        // 選択された装備とキャラの紐付けを行う。ただし、直接の数値をいじることはしない。
        // キャラのJSONデータに装備中の装備の名前を保存 => 戦闘前のキャラ読み込み段階でキャラの数値と装備の数値を足し合わせる　or 別途装備の数値を組み込んだキャラのJSONファイルを作る

        // パネルを閉じる
        transform.position = new Vector3(myPos.x, myPos.y, myPos.z);
    }

    public void esc()
    {
        transform.position = new Vector3(myPos.x, myPos.y, myPos.z);
    }
}
