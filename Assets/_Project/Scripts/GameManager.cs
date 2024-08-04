using UnityEngine;

internal class GameManager : MonoBehaviour
{
    private void Awake()
    {
        Instance = this;
    }

    public static GameManager Instance;

    [SerializeField] private Player m_player;
    public Player GetTarget()
    {
        return m_player;
    }
}