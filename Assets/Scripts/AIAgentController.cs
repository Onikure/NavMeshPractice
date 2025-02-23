using UnityEngine;
using UnityEngine.AI;
using System.Collections; // Added for IEnumerator

public class AIAgentController : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private bool onPlatform = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = target.position;
    }

    void Update()
    {
        if (onPlatform)
        {
            agent.autoTraverseOffMeshLink = true;
            agent.destination = target.position;
        }
        else
        {
            if (!isCoroutineRunning)
                StartCoroutine(CheckPlatformAndMove());
        }
    }

    private bool isCoroutineRunning = false;

    IEnumerator CheckPlatformAndMove()
    {
        isCoroutineRunning = true;
        if (agent.isOnOffMeshLink)
        {
            onPlatform = true;
            yield return StartCoroutine(CompleteOffMeshLink(agent));
            onPlatform = false;
        }
        isCoroutineRunning = false;
    }

    IEnumerator CompleteOffMeshLink(NavMeshAgent navAgent)
    {
        OffMeshLinkData data = navAgent.currentOffMeshLinkData;
        Vector3 startPos = navAgent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * navAgent.baseOffset;
        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            navAgent.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        navAgent.transform.position = endPos;
        navAgent.CompleteOffMeshLink();
    }
}