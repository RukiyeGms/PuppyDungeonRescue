using UnityEngine;

public class Chest : MonoBehaviour , IInterractable
{
    [SerializeField] int coins = 5;

    [SerializeField] Animator pressAAnim;
    [SerializeField]  SpriteRenderer pressASprite;

    [SerializeField] Sprite chestOpened;
    [SerializeField] Sprite chestClosed;
    SpriteRenderer chestSprite;

    BoxCollider2D chestInterractCollider;
    PlayerController playerController;


    private void Awake()
    {
        chestSprite = GetComponent<SpriteRenderer>();
        chestInterractCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        SetPressAAnim(false);
        chestSprite.sprite = chestClosed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.ChangeInterractable(this);
            SetPressAAnim(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SetPressAAnim(false);
            playerController.InterractableNull();
        }
    }

    public void Interract()
    {
        //chest open, give player money, chest maybe disable collider? Interractable null
        chestSprite.sprite = chestOpened;
        GameController.CoinUp(coins);



        SetPressAAnim(false);
        playerController.InterractableNull();
        chestInterractCollider.enabled = false;

    }

    

    void SetPressAAnim(bool boolean)
    {
        pressAAnim.enabled = boolean;
        pressASprite.enabled = boolean;
    }
}
