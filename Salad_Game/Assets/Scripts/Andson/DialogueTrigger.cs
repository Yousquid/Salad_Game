using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public float talkRange = 3f;
    public KeyCode talkKey = KeyCode.E;

    private Transform player;
    private DialogueManager dialogueManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        dialogueManager = GameObject.FindFirstObjectByType<DialogueManager>();
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= talkRange && !dialogueManager.IsTalking())
        {
            if (Input.GetKeyDown(talkKey))
            {
                dialogueManager.StartDialogue(dialogue);
            }
        }
        else if (dialogueManager.IsTalking() && Input.GetKeyDown(talkKey))
        {
            dialogueManager.DisplayNextSentence();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, talkRange);
    }
}
