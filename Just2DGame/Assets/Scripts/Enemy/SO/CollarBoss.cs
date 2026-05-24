using UnityEngine;

[CreateAssetMenu(fileName = "CollarBoss", menuName = "Scriptable Objects/CollarBoss")]
public class CollarBoss : ScriptableObject, ISerializationCallbackReceiver
{
    public int hp = 100;
    public int atk = 20;
    public int def = 5;
    public int speed = 5;

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
