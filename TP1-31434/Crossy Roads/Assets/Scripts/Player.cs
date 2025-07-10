using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private TerrainGenerator terrainGenerator;
    [SerializeField] private Text scoreText;
    [SerializeField] private LayerMask obstacleLayerMask = -1; // Layer dos obstáculos (opcional)
    [SerializeField] private float deathBoundary = -5f; // Limite Y para morte por queda

    private Animator animator;
    private bool isHopping;
    private int score;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        score++;
        
        // Verifica se o jogador caiu do mapa
        if (transform.position.y < deathBoundary)
        {
            Die();
        }
    }
    
    private void Update()
    {
        scoreText.text = "Score " + score;
        
        // Movimento para frente (W)
        if (Input.GetKeyDown(KeyCode.W) && !isHopping)
        {
            float zDifference = 0;
            if (transform.position.z % 1 != 0)
            {
                zDifference = Mathf.Round(transform.position.z) - transform.position.z;
            }
            Vector3 targetPosition = new Vector3(1, 0, zDifference);
            
            if (CanMoveTo(targetPosition))
            {
                MoveCharacter(targetPosition);
            }
        }
        // Movimento para trás (S)
        else if (Input.GetKeyDown(KeyCode.S) && !isHopping)
        {
            float zDifference = 0;
            if (transform.position.z % 1 != 0)
            {
                zDifference = Mathf.Round(transform.position.z) - transform.position.z;
            }
            Vector3 targetPosition = new Vector3(-1, 0, zDifference);
            
            if (CanMoveTo(targetPosition))
            {
                MoveCharacter(targetPosition);
            }
        }
        // Movimento para a esquerda (A)
        else if (Input.GetKeyDown(KeyCode.A) && !isHopping)
        {
            Vector3 targetPosition = new Vector3(0, 0, 1);
            
            if (CanMoveTo(targetPosition))
            {
                MoveCharacter(targetPosition);
            }
        }
        // Movimento para a direita (D)
        else if (Input.GetKeyDown(KeyCode.D) && !isHopping)
        {
            Vector3 targetPosition = new Vector3(0, 0, -1);
            
            if (CanMoveTo(targetPosition))
            {
                MoveCharacter(targetPosition);
            }
        }
    }

    private bool CanMoveTo(Vector3 direction)
    {
        Vector3 targetPosition = transform.position + direction;
        
        // Método 1: Usando OverlapSphere para verificar colisões
        Collider[] colliders = Physics.OverlapSphere(targetPosition, 0.4f, obstacleLayerMask);
        
        foreach (Collider collider in colliders)
        {
            // Verifica se o objeto tem tag de obstáculo
            if (collider.CompareTag("Tree") || collider.CompareTag("Bush") || collider.CompareTag("Obstacle"))
            {
                return false; // Não pode mover
            }
            
            // Alternativa: Verificar por nome do GameObject
            if (collider.name.Contains("Tree") || collider.name.Contains("Bush") || 
                collider.name.Contains("Obstacle"))
            {
                return false; // Não pode mover
            }
        }
        
        return true; // Pode mover
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<MovingObject>() != null)
        {
            if (collision.collider.GetComponent<MovingObject>().isLog)
            {
                transform.parent = collision.collider.transform;
            }
        }
        else
        {
            transform.parent = null;
        }
    }

    private void MoveCharacter(Vector3 difference)
    {
        animator.SetTrigger("hop");
        isHopping = true;
        transform.position = (transform.position + difference);

        terrainGenerator.SpawnTerrain(false, transform.position);
    }

    private void Die()
    {
        // Destrói o jogador quando morre
        Destroy(gameObject);
    }

    public void FinishHop()
    {
        isHopping = false;
    }
}