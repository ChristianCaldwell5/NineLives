using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public int SelectedCat;
    // -1 indicates no carry over
    public int LivesCarriedOver = -1;
    public int FruitCarriedOver = -1;
    public bool IsMusicMute = false;
    public bool IsSfxMute = false;
    public bool SpeedBoostUnlocked = false;
    public bool RecoveryBoostUnlocked = false;
    public bool FruitBoostUnlocked = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
