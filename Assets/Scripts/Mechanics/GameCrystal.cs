using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCrystal : MonoBehaviour
{
    public CrystalType type;
    public List<Sprite> crystalColours;

    public Sprite winColor;

    private SpriteRenderer spriteRender;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameController = GameObject.FindWithTag("GameController");
        GameController gc = gameController.GetComponent<GameController>();
        gc.RegisterCrystal(this);
        spriteRender = GetComponent<SpriteRenderer>();
    }

    public void SetupWithType(DungeonGenerator.RoomType roomType)
    {
        if(spriteRender == null)
        {
            spriteRender = GetComponent<SpriteRenderer>();
        }
        switch (roomType)
        {
            case DungeonGenerator.RoomType.VICTORY:
                type = CrystalType.C_WIN;
                spriteRender.sprite = winColor;
                break;
            case DungeonGenerator.RoomType.CRYSTAL_2:
                type = CrystalType.C_1;
                spriteRender.sprite = crystalColours[1];
                break;
            case DungeonGenerator.RoomType.CRYSTAL_3:
                type = CrystalType.C_2;
                spriteRender.sprite = crystalColours[2];
                break;
            case DungeonGenerator.RoomType.CRYSTAL_1:
            default:
                type = CrystalType.C_0;
                spriteRender.sprite = crystalColours[0];
                break;
        }
    }

    public bool IsWin()
    {
        return type == CrystalType.C_WIN;
    }

    public int GetNumber()
    {
        switch (type)
        {
            case CrystalType.C_0:
                return 0;
            case CrystalType.C_1:
                return 1;
            case CrystalType.C_2:
                return 2;
            default:
                return -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            StartCoroutine(KillNextFrame());
        }
    }

    private IEnumerator KillNextFrame()
    {
        yield return null;
        Destroy(this.gameObject);
    }
}

public enum CrystalType
{
    C_0, C_1, C_2, C_WIN
}
