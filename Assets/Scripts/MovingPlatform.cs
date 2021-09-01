using System;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{   
    public static MovingPlatform currentPlatform { get; private set; }
    public static MovingPlatform lastPlatform { get; private set; }
    public PlatformSpawner.MoveDirection MoveDirection { get; set; }

    [SerializeField] private float _speed = 2f;

    private void OnEnable()
    {
        if (lastPlatform == null)
        {
            lastPlatform = GameObject.Find("StartPlatform").GetComponent<MovingPlatform>();          
        }

        currentPlatform = this;

        GameManager.platforms.Add(gameObject);
        GetComponent<Renderer>().material.color = SetColor();

        transform.localScale = new Vector3(lastPlatform.transform.localScale.x, transform.localScale.y, lastPlatform.transform.localScale.z);
    }

    private Color SetColor()
    {
        float step;
        Color color = lastPlatform.GetComponent<Renderer>().material.color;

        if (color == Color.white)
        {
            step = 1f;
            return new Color(UnityEngine.Random.Range(0, step), UnityEngine.Random.Range(0, step), UnityEngine.Random.Range(0, step));
        }
        else
        {
            step = 0.07f;
            color.r += UnityEngine.Random.Range(-step, step);
            color.g += UnityEngine.Random.Range(-step, step);
            color.b += UnityEngine.Random.Range(-step, step);
        }

        return color;
    }

    internal void Stop()
    {
        _speed = 0;

        float difference = (GetDifference());
        float direction = difference;
        float sideValue = MoveDirection == PlatformSpawner.MoveDirection.Z || MoveDirection == PlatformSpawner.MoveDirection.Znegative ? lastPlatform.transform.localScale.z : lastPlatform.transform.localScale.x;

        if (Mathf.Abs(difference) >= sideValue)
        {
            lastPlatform = null; 
            currentPlatform= null;
        }

        if (direction > 0)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        if (currentPlatform != null)
        {
            if (MoveDirection == PlatformSpawner.MoveDirection.Z || MoveDirection == PlatformSpawner.MoveDirection.Znegative)
            {
                CutThePlatformZ(difference, direction);
            }
            
            else
            {
                CutThePlatformX(difference, direction);
            }
        }
        lastPlatform = this;
    }

    private float GetDifference()
    {
        if (MoveDirection == PlatformSpawner.MoveDirection.Z || MoveDirection == PlatformSpawner.MoveDirection.Znegative)
        {
            return transform.position.z - lastPlatform.transform.position.z;
        }

        else
        {
            return transform.position.x - lastPlatform.transform.position.x;
        }
    }

    private void CutThePlatformZ(float difference, float direction)
    {
        float newZAxis = lastPlatform.transform.localScale.z - Mathf.Abs(difference * direction);
        float fallingPlatformZAxis = transform.localScale.z - newZAxis;
        float newZPosition = lastPlatform.transform.position.z + (difference / 2);

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZAxis);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        float platformPart = transform.position.z + (newZAxis / 2 * direction);
        float fallingPlatformZPosition = platformPart + fallingPlatformZAxis / 2f * direction;

        CreateFallingPlatform(fallingPlatformZPosition, fallingPlatformZAxis);
    }

    private void CutThePlatformX(float difference, float direction)
    {
        float newXAxis = lastPlatform.transform.localScale.x - Mathf.Abs(difference * direction);
        float fallingPlatformZAxis = transform.localScale.x - newXAxis;
        float newXPosition = lastPlatform.transform.position.x + (difference / 2);

        transform.localScale = new Vector3(newXAxis, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);

        float platformPart = transform.position.x + (newXAxis / 2 * direction);
        float fallingPlatformXPosition = platformPart + fallingPlatformZAxis / 2f * direction;

        CreateFallingPlatform(fallingPlatformXPosition, fallingPlatformZAxis);
    }

    private void CreateFallingPlatform(float fallingPlatformAxis, float fallingPlatformSize)
    {
        GameObject fallingPlatform = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (MoveDirection == PlatformSpawner.MoveDirection.Z || MoveDirection == PlatformSpawner.MoveDirection.Znegative)
        {
            fallingPlatform.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingPlatformSize);
            fallingPlatform.transform.position = new Vector3(transform.position.x, transform.position.y, fallingPlatformAxis);
        }
        else
        {
            fallingPlatform.transform.localScale = new Vector3(fallingPlatformSize, transform.localScale.y, transform.localScale.z);
            fallingPlatform.transform.position = new Vector3(fallingPlatformAxis, transform.position.y, transform.position.z);
        }

        fallingPlatform.AddComponent<Rigidbody>();
        fallingPlatform.GetComponent<Renderer>().material.color = gameObject.GetComponent<Renderer>().material.color;
       
        if (fallingPlatform.transform.localScale.z < 0.01f || fallingPlatform.transform.localScale.x < 0.01f)
        {
            Destroy(fallingPlatform.gameObject);

            if (currentPlatform != GameObject.Find("StartPlatform"))
            {
                currentPlatform.GetComponentInChildren<ParticleSystem>().Play(true);
            }
        }
        else
        {
            Destroy(fallingPlatform.gameObject, 1f);
        }
    }

    private void FixedUpdate()
    {
        switch (MoveDirection)
        {
            case PlatformSpawner.MoveDirection.Z:
                transform.position += transform.forward * Time.deltaTime * _speed;
                break;
            case PlatformSpawner.MoveDirection.X:
                transform.position += transform.right * Time.deltaTime * _speed;
                break;
            case PlatformSpawner.MoveDirection.Znegative:
                transform.position += -transform.forward * Time.deltaTime * _speed;
                break;
            case PlatformSpawner.MoveDirection.Xnegative:
                transform.position += -transform.right * Time.deltaTime * _speed;
                break;

        }
        if (transform.position.z >= 2f || transform.position.x >= 2f)
        {
            _speed = _speed * -1;
        }

        if (transform.position.z <= -2f || transform.position.x <= -2f)
        {
            _speed = _speed * -1;
        }
    }
}
