using UnityEngine;

[CreateAssetMenu(fileName = "ZakoSpeed", menuName = "Scriptable Objects/ZakoSpeed")]
public class ZakoSpeedSO : ScriptableObject, ISerializationCallbackReceiver
{
    public int hp = 10;
    public int atk = 15;
    public int def = 0;
    public int speed = 10;

    [System.NonSerialized] //参照して使用するのはここ
    public float runtimeHP;
    public float runtimeATK;
    public float runtimeDEF;
    public float runtimeSPEED;

    public void OnAfterDeserialize()
    {
        // デシリアライズ後に、ScriptableObjectの値をランタイム用の変数にコピー
        runtimeHP = hp;
        runtimeATK = atk;
        runtimeDEF = def;
        runtimeSPEED = speed;
    }

    public void OnBeforeSerialize()
    {
        // シリアライズ前に、ランタイム用の変数にScriptableObjectの値をコピー
        //runtimeHP = hp;
        //runtimeATK = atk;
        //runtimeDEF = def;
        //runtimeSPEED = speed;
    }

}
