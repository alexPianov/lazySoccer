using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    private Vector2 defaultBallPosition = new Vector2(-1f, 4.5f);

    [SerializeField]
    private float speedupDistance = 2f;
    [SerializeField]
    private float maxDistanceForStickAnimation = 0.1f;
    [SerializeField]
    private float timeToFixSticking = 0.1f;

    public void ResetBall()
    {
        transform.localPosition = defaultBallPosition;
    }

    public void ChangeBallCoords(Vector2 newPosition)
    {
        transform.position = newPosition;
    }

    public void StickToPlayer(Transform playerTransform, float stepTime)
    {
        StartCoroutine(AnimateSticking(playerTransform, stepTime));
    }

    public void PassBetweenPlayers(Transform otherPlayerTransform, float stepTime) 
    {
        StartCoroutine(SetupThrowingAnimation(otherPlayerTransform, stepTime));
    }

    private IEnumerator AnimateSticking(Transform playerTransform, float stepTime)
    {
        bool isTooFar = CheckDistance(playerTransform.position, transform.position, maxDistanceForStickAnimation);

        float passedTime = 0;
        float localStepTime = isTooFar ? stepTime - timeToFixSticking : stepTime;
        
        if(isTooFar)
            yield return StartCoroutine(AnimateThrowing(transform.position, playerTransform, timeToFixSticking));

        while (passedTime <= localStepTime)
        {
            passedTime += Time.deltaTime;
            ChangeBallCoords(playerTransform.position);
            yield return null;
        }
    }

    private IEnumerator SetupThrowingAnimation(Transform otherPlayerTransform, float stepTime)
    {
        Vector3 startPosition = transform.position;

        bool isSpeedup = CheckDistance(otherPlayerTransform.position, startPosition, speedupDistance);   
        float localStepTime = isSpeedup ? stepTime * 0.5f : stepTime * 0.8f;

        yield return StartCoroutine(AnimateThrowing(startPosition, otherPlayerTransform, localStepTime));

        //if (isSpeedup)
            StartCoroutine(AnimateSticking(otherPlayerTransform, stepTime - localStepTime));
    }

    private IEnumerator AnimateThrowing(Vector2 startPosition, Transform otherPlayerTransform, float stepTime)
    {
        float passedTime = 0;

        while (passedTime <= stepTime)
        {
            passedTime += Time.deltaTime;
            ChangeBallCoords(Vector3.Lerp(startPosition, otherPlayerTransform.position, passedTime / stepTime));
            yield return null;
        }  
    }

    private bool CheckDistance(Vector2 pointOne, Vector2 pointTwo, float maxDifference)
        => System.Math.Abs(Vector2.Distance(pointOne, pointTwo)) < maxDifference;
}
