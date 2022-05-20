using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderNode : Node
{
    [SerializeField] private LayerMask floorMask = 0;
    private Vector3 origin;
    private NavMeshAgent agent;
    private EnemyAI ai;

    public WanderNode(Vector3 origin, NavMeshAgent agent, EnemyAI ai)
    {
        this.origin = origin;
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        float randomX;
        float randomY;
        float randomZ;

        float distance = 10.0f;

        //Vector3 randomDestination = UnityEngine.Random.insideUnitSphere * distance;
        agent.SetDestination(RandomNavSphere(origin, distance, floorMask));

        //���⼭ �÷��̾� �Ÿ� üũ�ϰ� ���� ai�� ��ǥ �������� ���µ��߿� 
        //�÷��̾ �Ÿ��� ������ fail����
        //��ǥ �������� ���µ��߿� �÷��̾ �Ÿ��� �ȵ����� success������
        //���ο� ��ǥ���� ã��
        return NodeState.SUCCESS;
    }

    Vector3 RandomNavSphere(Vector3 origin, float distance, LayerMask layerMask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layerMask);

        return navHit.position;
    }
}
