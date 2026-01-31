using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "Scriptable Objects/EnemyWave")]
public class SOS_EnemyWave : ScriptableObject
{
    [SerializeField] GameObject[] leftSideEnemies;
    [SerializeField] GameObject[] rightSideEnemies;

    public GameObject[] GetLeftSideEnemies()
    {
        return leftSideEnemies;
    }
    public GameObject[] GetRightSideEnemies()
    {
        return rightSideEnemies;
    }

}
