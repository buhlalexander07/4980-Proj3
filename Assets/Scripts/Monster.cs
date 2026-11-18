using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] GameObject player;
    private Transform playerTransform;
    void Start()
    {
        playerTransform = player.transform;
    }
    public void Attack()
    {
        StartCoroutine(TriggerKnockback());
        /* Placeholder
         * clock -= 30 seconds;
         * Vector3 RespawnLocation = Vector3(...)
         * player.transform.position = RespawnLocation; */
    }
    IEnumerator TriggerKnockback()
    {
        player.GetComponent<PlayerController>().isKnockedBack = true;
        float lerpTimer = 0.0f;
        float lerpDuration = 0.25f;
        Vector3 startPosition = playerTransform.position;
        Quaternion startRotation = playerTransform.localRotation;
        Vector3 endPosition = new Vector3(playerTransform.position.x, 0, playerTransform.position.z);
        Quaternion endRotation = Quaternion.Euler(-45.0f, playerTransform.eulerAngles.y, playerTransform.eulerAngles.z);

        while (lerpTimer < lerpDuration)
        {
            float t = lerpTimer / lerpDuration;

            playerTransform.position = Vector3.Lerp(startPosition, endPosition, t);
            playerTransform.localRotation = Quaternion.Lerp(startRotation, endRotation, t);
            lerpTimer += Time.deltaTime;

            yield return null;
        }

        playerTransform.position = endPosition;
        playerTransform.localRotation = endRotation;
        player.GetComponent<PlayerController>().isKnockedBack = false;
        yield return new WaitForSeconds(2.0f);

        // after 2.0 seconds, lerp back to original position

        lerpTimer = 0.0f;
        while (lerpTimer < lerpDuration)
        {
            float t = lerpTimer / lerpDuration;

            playerTransform.position = Vector3.Lerp(endPosition, startPosition, t);
            playerTransform.localRotation = Quaternion.Lerp(endRotation, startRotation, t);
            lerpTimer += Time.deltaTime;

            yield return null;
        }
        yield return null;

    }
}
