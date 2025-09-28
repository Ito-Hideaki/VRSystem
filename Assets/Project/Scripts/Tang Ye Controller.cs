using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TangYeController : MonoBehaviour
{
    public enum Status
    {
        Stopped,
        Released
    }

    Vector3 defaultPos = new Vector3(0, 3, -3); //default relative position from Dr.Ohno
    [SerializeField] DrOhnoScript drOhno;
    [SerializeField] ParticleSystem typs;
    [SerializeField] TextMeshProUGUI debugUGUI;

    Vector3 velocity;
    Status status;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        velocity = Vector3.zero;

        SetStatus(Status.Stopped);
    }

    // Update is called once per frame
    void Update()
    {
        debugUGUI.text = Time.deltaTime.ToString();

        Vector3 center = drOhno.transform.position + this.defaultPos;
        Vector3 diff = center - this.transform.position;

        if (status == Status.Stopped) {
            //get close to center
            velocity = diff;
        }

        if (status == Status.Released) {

            //try to get close to center(Dr.Ohno)
            velocity += Time.deltaTime * 1f * diff;

            //similar to rotation
            {
                velocity.x += 1f * velocity.z * Time.deltaTime;
                velocity.y += -1f * velocity.x * Time.deltaTime;
                velocity.z += 1f * velocity.y * Time.deltaTime;
            }

            //accelerate if too slow, deccelerate if too fast
            float GENERAL_SPEED = 1f;
            velocity *= 1f + (GENERAL_SPEED - velocity.magnitude) * 1f * Time.deltaTime;
        }

        this.transform.Translate(velocity * Time.deltaTime);
        this.transform.Translate(new Vector3(0, 0, drOhno.coordinateVelocityZ) * Time.deltaTime);
    }

    public void SetStatus(Status status)
    {
        this.status = status;
        if (status == Status.Stopped)
        {
            typs.Pause();
        }
        if (status == Status.Released)
        {
            typs.Play();
        }
    }

    public Status GetStatus()
    {
        return this.status;
    }
}
