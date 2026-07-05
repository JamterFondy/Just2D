using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/CollarBoss")]
public class CollarBoss : EnemyGeneralStatus, ISerializationCallbackReceiver
{

    public int phase2HP;

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
