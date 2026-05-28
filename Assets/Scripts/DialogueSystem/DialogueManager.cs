using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI 요소 - 인스펙터 창에서 연결")]
    public GameObject DialoguePanel;
    public Image characterImage;
    public TextMeshProUGUI characternameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("기본 설정")]
    public Sprite defaultCharacterImage;

    [Header("타이핑 효과 설정")]
    public float typingSpeed = 0.05f;
    public bool skipTypingOnClick = true;

    private DialogueDataSQ currentDialogue;
    private int currentLineIndex = 0;

    private bool isDialogueActive = false;
    private bool isTyping = false;

    private Coroutine typingCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 시작 시 대화창 비활성화
        DialoguePanel.SetActive(false);

        // 버튼 클릭 이벤트 연결
        nextButton.onClick.AddListener(HandleNextInput);
    }

    // Update is called once per frame
    void Update()
    {
        // 스페이스바 입력으로 다음 대사 진행
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleNextInput();
        }
    }

    // 글자 타이핑 효과
    IEnumerator TypeText(string textToType)
    {
        isTyping = true;

        dialogueText.text = "";

        for (int i = 0; i < textToType.Length; i++)
        {
            dialogueText.text += textToType[i];

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    // 타이핑 즉시 완료
    private void CompleteTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTyping = false;

        if (currentDialogue != null &&
            currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            dialogueText.text =
                currentDialogue.dialogueLines[currentLineIndex];
        }
    }

    // 현재 대사 출력
    void ShowCurrentLine()
    {
        // 이전 코루틴 종료
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        if (currentDialogue != null &&
            currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            string currentText =
                currentDialogue.dialogueLines[currentLineIndex];

            typingCoroutine =
                StartCoroutine(TypeText(currentText));
        }
    }

    // 다음 대사 출력
    public void ShowNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex >= currentDialogue.dialogueLines.Count)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();
        }
    }

    // 대화 종료
    void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isDialogueActive = false;
        isTyping = false;

        currentLineIndex = 0;

        // 대화창 끄기
        DialoguePanel.SetActive(false);
    }

    // 입력 처리
    public void HandleNextInput()
    {
        // 타이핑 중 클릭 시 즉시 완료
        if (isTyping && skipTypingOnClick)
        {
            CompleteTyping();
        }
        // 타이핑 끝났으면 다음 줄
        else if (!isTyping)
        {
            ShowNextLine();
        }
    }

    // 대화 스킵
    public void SkipDialogue()
    {
        EndDialogue();
    }

    // 현재 대화 여부 반환
    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    // 대화 시작
    public void StartDialogue(DialogueDataSQ dialogue)
    {
        if (dialogue == null ||
            dialogue.dialogueLines.Count == 0)
        {
            return;
        }

        currentDialogue = dialogue;

        currentLineIndex = 0;

        isDialogueActive = true;

        // 대화창 켜기
        DialoguePanel.SetActive(true);

        // 이름 출력
        characternameText.text = dialogue.characterName;

        // 캐릭터 이미지 설정
        if (characterImage != null)
        {
            if (dialogue.characterImage != null)
            {
                characterImage.sprite = dialogue.characterImage;
            }
            else
            {
                characterImage.sprite = defaultCharacterImage;
            }
        }

        // 첫 대사 출력
        ShowCurrentLine();
    }
}