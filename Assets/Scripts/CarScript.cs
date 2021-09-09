using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CarScript : MonoBehaviour
{

    WheelJoint2D[] wheelJoints;
    JointMotor2D frontWheel;
    JointMotor2D backWheel1;
    JointMotor2D backWheel2;
    
    // Физика езды
    public float maxSpeed = -2000f;
    private float maxBackSpeed = 1000f;
    public float acceleration = 650f;
    private float deacceleration = -80f;
    public float brakeForce = 3000f;
    private float gravity = 9.8f;
    private float angleCar = 0;
    
    //Физика автомобиля
    public LayerMask map;
    public Transform transformBackWheel1;
    public Transform transformBackWheel2;
    public Transform transformFrontWheel;
    private float contactRad = 0.63f;
    new private Rigidbody2D rigidbody;
    
    // Дым для колёс в заднюю сторону
    public ParticleSystem wheelSmokeBack1; 
    public ParticleSystem wheelSmokeBack2;
    public ParticleSystem wheelSmokeFront;

    // Дым для колёс в переднюю сторону
    public ParticleSystem wheelSmokeBack1Rear; 
    public ParticleSystem wheelSmokeBack2Rear;
    public ParticleSystem wheelSmokeFrontRear;

    // Дым для выхлопной трубы

    public ParticleSystem tubeSmoke;

    //Вспомогательные явления
    public bool grounded = false;

    private bool groundedBackWheel1 = false;
    private bool groundedBackWheel2 = false;
    private bool groundedFrontWheel = false;

    //Физика прыжка
    private float jumpForce = 3400.0F;
    //Физика частиц
    /*   private int WsmokeParCount = 40;
       private float[] VectorB1;
       private float[] VectorB2;
       private float[] VectorF;
       private float YVector = 268.3f; */

    // Анимация
    public Animation frontLight;
    public Animation backLight;
    private bool isFrontLightON = false;
    private bool isBackLightON = false;
    private bool isBackLightSTOPON = false;

    // Фары
    public Light frontLightValve;
    public Light backLightValve;

    // Звуки
    public AudioSource engineRun;
    public AudioSource engineNeutral;
    public AudioSource engineStart;
    public AudioSource carBreak;

    private bool isEngineRun = false;
    private bool isEngineNeutral = true;
    private bool isEngineStart = false;
    private bool isCarBreak = false;


    private void Jump()
    {
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void SmokePlayForward()
    {
        /*   wSmokeB1.shape.rotation.Set(345f, -268f, 333f);
           wSmokeB2.shape.rotation.Set(345f, -268f, 333f);
           wSmokeF.shape.rotation.Set(345f, -268f, 333f); */
        if (groundedBackWheel1 == true) wheelSmokeBack1.Play(); else wheelSmokeBack1.Stop();
        if (groundedBackWheel2 == true) wheelSmokeBack2.Play(); else wheelSmokeBack2.Stop();
        if (groundedFrontWheel == true) wheelSmokeFront.Play(); else wheelSmokeFront.Stop();
    }
    private void SmokePlayBack()
    {
        /*  wSmokeB1.shape.rotation.Set(345f, 268f, 333f);
         wSmokeB2.shape.rotation.Set(345f, 268f, 333f);
         wSmokeF.shape.rotation.Set(345f, 268f, 333f); */
        if (groundedBackWheel1 == true) wheelSmokeBack1Rear.Play(); else wheelSmokeBack1Rear.Stop();
        if (groundedBackWheel2 == true) wheelSmokeBack2Rear.Play(); else wheelSmokeBack2Rear.Stop();
        if (groundedFrontWheel == true) wheelSmokeFrontRear.Play(); else wheelSmokeFrontRear.Stop();
    }
    private void SmokeStop()
    {
        wheelSmokeBack1.Stop();
        wheelSmokeBack2.Stop();
        wheelSmokeFront.Stop();
        wheelSmokeBack1Rear.Stop();
        wheelSmokeBack2Rear.Stop();
        wheelSmokeFrontRear.Stop();
    }
    private void EngineRunSoundPlay()
    {
        if (isEngineRun != true)
        {
            engineRun.Play();
            isEngineRun = true;
        }
        if (isEngineNeutral != false)
        {
            engineNeutral.Stop();
            isEngineNeutral = false;
        }
        if(isCarBreak != false)
        {
            carBreak.Stop();
            isCarBreak = false;
        }
    }
    private void EngineNeutralSoundPlay()
    {
        if (isEngineRun != false)
        {
            engineRun.Stop();
            isEngineRun = false;
        }
        if (isEngineNeutral == false)
        {
            engineNeutral.Play();
            isEngineNeutral = true;
        }
        if (isCarBreak != false)
        {
            carBreak.Stop();
            isCarBreak = false;
        }
    }
    private void CarStopSoundPlay()
    {
        if (isEngineRun != false)
        {
            engineRun.Stop();
            isEngineRun = false;
        }
        if (grounded == true)
        {
            if (isEngineNeutral != false)
            {
                engineNeutral.Stop();
                isEngineNeutral = false;
            }
            if (isCarBreak != true)
            {
                carBreak.Play();
                isCarBreak = true;
            }
        }
        else
        {
            if (isEngineNeutral != true)
            {
                engineNeutral.Play();
                isEngineNeutral = true;
            }
            if (isCarBreak != false)
            {
                carBreak.Stop();
                isCarBreak = false;
            }
        }
    }

    void Start()
    {

        wheelJoints = gameObject.GetComponents<WheelJoint2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        backWheel1 = wheelJoints[0].motor;
        backWheel2 = wheelJoints[1].motor;
        frontWheel = wheelJoints[2].motor;
        SmokeStop();
        engineStart.Play();
        EngineNeutralSoundPlay();
        /*
        VectorB1[0] = wSmokeB1.shape.rotation.x;
        VectorB1[1] = YVector;
        VectorB1[2] = wSmokeB1.shape.rotation.z;

        VectorB2[0] = wSmokeB2.shape.rotation.x;
        VectorB2[1] = YVector;
        VectorB2[2] = wSmokeB2.shape.rotation.z;

        VectorF[0] = wSmokeF.shape.rotation.x;
        VectorF[1] = YVector;
        VectorF[2] = wSmokeF.shape.rotation.z; */
    }

    private void Update()
    {
        groundedBackWheel1 = Physics2D.OverlapCircle(transformBackWheel1.transform.position, contactRad, map);
        groundedBackWheel2 = Physics2D.OverlapCircle(transformBackWheel2.transform.position, contactRad, map);
        groundedFrontWheel = Physics2D.OverlapCircle(transformFrontWheel.transform.position, contactRad, map);
        grounded = groundedBackWheel1 || groundedBackWheel2 || groundedFrontWheel;
    }

    void FixedUpdate()
    {

        frontWheel.motorSpeed = backWheel1.motorSpeed;
        backWheel2.motorSpeed = backWheel1.motorSpeed;

        angleCar = transform.localEulerAngles.z;

        if (angleCar >= 180)
        {
            angleCar = angleCar - 360;
        }


     //   if (backWheel1.motorSpeed < 0 && backWheel1.motorSpeed > -500) SmokePlayBack(); else SmokeStop();
     //   if (backWheel1.motorSpeed > 0 && backWheel1.motorSpeed < 500) SmokePlayForward(); else SmokeStop();

        if (Input.GetKey(KeyCode.D) == true && Input.GetKey(KeyCode.Space) == false) // Если нажат Газ
        {
            backWheel1.motorSpeed = Mathf.Clamp(backWheel1.motorSpeed - (acceleration - gravity * Mathf.PI * (angleCar / 180) * 100) * Time.deltaTime, maxSpeed, maxBackSpeed);
        //    if (backWheel1.motorSpeed > 0f) SmokePlayBack();  else SmokeStop();
        }
        else if (Input.GetKey(KeyCode.A) == true && Input.GetKey(KeyCode.Space) == false) // Если нажат Газ и включена задняя передача
        {
            backWheel1.motorSpeed = Mathf.Clamp(backWheel1.motorSpeed + (acceleration - gravity * Mathf.PI * (angleCar / 180) * 100) * Time.deltaTime, maxSpeed, maxBackSpeed);
        //    if (backWheel1.motorSpeed < 0f) SmokePlayForward();  else SmokeStop();
        }
        else if (Input.GetKey(KeyCode.Space) == true && backWheel1.motorSpeed > 0) //Тормоз, когда едем вперёд
        {
            backWheel1.motorSpeed = Mathf.Clamp(backWheel1.motorSpeed - brakeForce * Time.deltaTime, 0, maxBackSpeed);
        }
        else if (Input.GetKey(KeyCode.Space) == true && backWheel1.motorSpeed < 0) //Тормоз, когда едем назад
        {
            backWheel1.motorSpeed = Mathf.Clamp(backWheel1.motorSpeed + brakeForce * Time.deltaTime, maxSpeed, 0);
        }


        if (Input.GetKey(KeyCode.D) == true) //Когда пускать дымы из под колёс и воспроизводить звук
        {
            if (backWheel1.motorSpeed > 0f) SmokePlayBack(); else SmokeStop();
            if (backWheel1.motorSpeed < 0f && backWheel1.motorSpeed > -800f) SmokePlayForward(); else SmokeStop();
            EngineRunSoundPlay();
        }
        else if (Input.GetKey(KeyCode.A) == true) 
        {
            if (backWheel1.motorSpeed < 0f) SmokePlayForward(); else SmokeStop();
            if (backWheel1.motorSpeed > 0f && backWheel1.motorSpeed < 800f) SmokePlayBack(); else SmokeStop();
            EngineRunSoundPlay();
        }
        else if (Input.GetKey(KeyCode.Space) == true && rigidbody.velocity.x > 0.05f) // тут заготовка. А так понятно, что ерунда
        {
            SmokePlayBack();
            if (rigidbody.velocity.x > 5f || rigidbody.velocity.x < -5f) CarStopSoundPlay(); else EngineNeutralSoundPlay();
        }
        else if(Input.GetKey(KeyCode.Space) == true && rigidbody.velocity.x < -0.05f)
        {
            SmokePlayForward();
            if (rigidbody.velocity.x > 5f || rigidbody.velocity.x < -5f) CarStopSoundPlay(); else EngineNeutralSoundPlay();
        }
        else
        {
            SmokeStop();
            EngineNeutralSoundPlay();
        }

        if (grounded == true)
        {
            if ((backWheel1.motorSpeed < 0) || (Input.GetKey(KeyCode.D) == false && backWheel1.motorSpeed == 0 && angleCar < 0)) // Если на склоне назад
            {
                backWheel1.motorSpeed = Mathf.Clamp(backWheel1.motorSpeed - (deacceleration - gravity * Mathf.PI * (angleCar / 180) * 100) * Time.deltaTime, maxSpeed, 0);
            }
            else
            if ((Input.GetKey(KeyCode.D) == false && backWheel1.motorSpeed > 0) || (Input.GetKey(KeyCode.D) == false && backWheel1.motorSpeed == 0 && angleCar > 0)) // Если на склоне вперёд
            {
                backWheel1.motorSpeed = Mathf.Clamp(backWheel1.motorSpeed - (-deacceleration - gravity * Mathf.PI * (angleCar / 180) * 100) * Time.deltaTime, 0, maxBackSpeed);
            }

            if (Input.GetKey(KeyCode.D) == false && backWheel1.motorSpeed < 0) // Силы, препятствующие движению вперёд
            {
                backWheel1.motorSpeed = Mathf.Clamp(backWheel1.motorSpeed - deacceleration * Time.deltaTime, maxSpeed, 0);
            }
            else
            if (Input.GetKey(KeyCode.D) == false && backWheel1.motorSpeed > 0) //  Силы, препятствующие движению назад
            {
                backWheel1.motorSpeed = Mathf.Clamp(backWheel1.motorSpeed + deacceleration * Time.deltaTime, 0, maxBackSpeed);
            }
        }

        if (grounded == true && Input.GetKeyDown(KeyCode.F))
        {
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.L) == true && frontLight.isPlaying == false) // Анимация для передней фары
        {
            if(isFrontLightON == false)
            {
                frontLight.Play("FrontLightOn");
                isFrontLightON = true;
            }
            else
            {
                frontLight.Play("FrontLightOff");
                isFrontLightON = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.K) == true && backLight.isPlaying == false) // Анимация для задней фары
        {
            if (isBackLightON == false)
            {
                backLight.Play("BackLightOn");
                isBackLightON = true;
            }
            else
            {
                backLight.Play("BackLightOff");
                isBackLightON = false;
            }
        }

        if (Input.GetKey(KeyCode.Space) == true)
        {
            if(isBackLightON == false && isBackLightSTOPON == false)
            {
                backLight.Play("BackLightOnSTOP");
                backLightValve.intensity = 90;
                isBackLightSTOPON = true;
            }
            else
            {
                backLightValve.intensity = 90;
            }
        }
        else
        {
            backLight.Stop();
            isBackLightSTOPON = false;
            if (isBackLightON == false)
            {
                backLightValve.intensity = 0;
            }
            else
            {
                backLightValve.intensity = 60;
            }
        }

        wheelJoints[0].motor = backWheel1;
        wheelJoints[1].motor = backWheel2;
        wheelJoints[2].motor = frontWheel;
    }
}