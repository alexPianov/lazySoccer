using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private string playerID;

    [Header("AI Pathfinding")]
    [SerializeField]
    private AIDestinationSetter aIDestinationSetter;
    [SerializeField]
    private AIPath aiPath;

    [Header("Body")]
    [SerializeField]
    private GameObject playerBody;

    [Header("Uniform sprites")]
    [SerializeField]
    private SpriteRenderer[] shirtSprites;
    [SerializeField]
    private SpriteRenderer[] patternSprites;

    [Header("Speed Settings")]
    [SerializeField]
    private float speedUpDistance;
    [SerializeField]
    private float slowDownDistance;
    [SerializeField]
    private float maxSpeedCorrectionCoeff = 0.8f;

    [Header("Rotation Settings")]
    [SerializeField]
    private float rotationCorrectionDistance;
    [SerializeField]
    private float rotationCorrectionTimeMin;
    [SerializeField]
    private float rotationCorrectionTimeMax;

    [Header("Running animation settings")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float speedUpAnimationCoeff;
    [SerializeField]
    private float slowDownAnimationCoeff;
    [SerializeField]
    private float defaultAnimationCoeff = 1f;

    private float matchAcceleration;

    public string PlayerID
    {
        get => playerID;
        set => playerID = value;
    }

    public float MatchAcceleration
    {
        set => matchAcceleration = value;
    }

    private void Start()
    {
        StartCoroutine(RotationChecker());
    }

    public void ChangeAnimatorAcceleration()
    {
        animator.speed = defaultAnimationCoeff * matchAcceleration;
    }

    public void ChangeAIActiveStatus(bool isActive)
    {
        aiPath.enabled = isActive;
    }

    public void SetPosition(Vector2 newPosition)
    {
        transform.position = newPosition;
    }

    public void SetLocalPosition(Vector2 newPosition)
    {
        transform.localPosition = newPosition;
    }

    public void DisableRunningAnimation(bool isExtreme)
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isExtreme", isExtreme);
        aiPath.canMove = false;
    }

    public void SetTarget(Transform targetTransform, float turnTime)
    {
        aiPath.canMove = true;
        aIDestinationSetter.target = targetTransform;

        if (targetTransform)
        {
            animator.SetBool("isRunning", true);

            CheckDistance(targetTransform.position, turnTime);
        }
    }

    public void SetColor(Color shirtColor, Color patternColor)
    {
        foreach (SpriteRenderer shirtSprite in shirtSprites)
            shirtSprite.color = shirtColor;

        foreach (SpriteRenderer patternSprite in patternSprites)
            patternSprite.color = patternColor;
    }

    private IEnumerator RotationChecker()
    {
        while (true)
        {
            CheckDirection(gameObject.transform.position.x);
            yield return new WaitForSeconds(Random.Range(rotationCorrectionTimeMin, rotationCorrectionTimeMax));
        }
    }

    private void CheckDistance(Vector2 targetPosition, float turnTime)
    {
        float distance = System.Math.Abs(Vector2.Distance(gameObject.transform.position, targetPosition));
       
        aiPath.maxSpeed = distance * matchAcceleration * maxSpeedCorrectionCoeff / turnTime;

        if (distance > speedUpDistance)
        {
            animator.speed = speedUpAnimationCoeff * matchAcceleration;
        }
        else if (distance > slowDownDistance)
        {
            animator.speed = slowDownAnimationCoeff * matchAcceleration;
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.speed = defaultAnimationCoeff * matchAcceleration;
        }
    }

    private void CheckDirection(float x)
    {
        if (Mathf.Abs(aiPath.destination.x - x) > rotationCorrectionDistance)
            Rotate(aiPath.destination.x < x);
    }

    public void Rotate(bool rotateLeft)
    {
        playerBody.transform.rotation = Quaternion.Euler(0, rotateLeft ? 180 : 0, 0);
    }
}
