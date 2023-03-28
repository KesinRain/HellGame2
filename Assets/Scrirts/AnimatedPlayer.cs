using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPlayer : MonoBehaviour
{
        public Animator anim;
        public Rigidbody rig;
        public Transform mainCamera;
        public float jumpForce = 3.5f;
        public float walkingSpeed = 2f;
        public float runningSpeed = 6f;
        public float currentSpeed;
        private float animationInterpolation = 1f;
        public Transform groundCheckerTransform;
        public LayerMask groundLayer;
        // Start is called before the first frame update
        void Start()
        {
            // ѕрекрепл€ем курсор к середине экрана
            Cursor.lockState = CursorLockMode.Locked;
            // и делаем его невидимым
            Cursor.visible = false;
        }
        void Run()
        {
            animationInterpolation = Mathf.Lerp(animationInterpolation, 1.5f, Time.deltaTime * 3);
            anim.SetFloat("x", Input.GetAxis("Horizontal") * animationInterpolation);
            anim.SetFloat("y", Input.GetAxis("Vertical") * animationInterpolation);

            currentSpeed = Mathf.Lerp(currentSpeed, runningSpeed, Time.deltaTime * 3);
        }
        void Walk()
        {
            // Mathf.Lerp - отвчает за то, чтобы каждый кадр число animationInterpolation(в данном случае) приближалось к числу 1 со скоростью Time.deltaTime * 3.
            // Time.deltaTime - это врем€ между этим кадром и предыдущим кадром. Ёто позвол€ет плавно переходить с одного числа до второго 
            animationInterpolation = Mathf.Lerp(animationInterpolation, 1f, Time.deltaTime * 3);
            anim.SetFloat("x", Input.GetAxis("Horizontal") * animationInterpolation);
            anim.SetFloat("y", Input.GetAxis("Vertical") * animationInterpolation);

            currentSpeed = Mathf.Lerp(currentSpeed, walkingSpeed, Time.deltaTime * 3);
        }
        private void Update()
        {       

            // «ажаты ли кнопки W и Shift?
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            {
                // «ажаты ли еще кнопки A S D?
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    // ≈сли да, то мы идем пешком
                    Walk();
                }
                // ≈сли нет, то тогда бежим!
                else
                {
                    Run();
                }
            }
            // ≈сли W & Shift не зажаты, то мы просто идем пешком
            else
            {
                Walk();
            }
            //≈сли нажат пробел, то в аниматоре отправл€ем сообщение тригеру, который активирует анимацию прыжка
            if (Input.GetKey(KeyCode.Space))
            {
                anim.SetTrigger("Jump");
                Jump();
            }

            if (Input.GetKey(KeyCode.T))
            {
                anim.SetTrigger("Talk");
            }
    }
        // Update is called once per frame
        void FixedUpdate()
        {
            // «десь мы задаем движение персонажа в зависимости от направлени€ в которое смотрит камера
            // —охран€ем направление вперед и вправо от камеры 
            Vector3 camF = mainCamera.forward;
            Vector3 camR = mainCamera.right;
            // „тобы направлени€ вперед и вправо не зависили от того смотрит ли камера вверх или вниз, иначе когда мы смотрим вперед, персонаж будет идти быстрее чем когда смотрит вверх или вниз

            camF.y = 0;
            camR.y = 0;
            Vector3 movingVector;
            // “ут мы умножаем наше нажатие на кнопки W & S на направление камеры вперед и прибавл€ем к нажати€м на кнопки A & D и умножаем на направление камеры вправо
            movingVector = Vector3.ClampMagnitude(camF.normalized * Input.GetAxis("Vertical") * currentSpeed + camR.normalized * Input.GetAxis("Horizontal") * currentSpeed, currentSpeed);
            // Magnitude - это длинна вектора. € делю длинну на currentSpeed так как мы умножаем этот вектор 
            anim.SetFloat("magnitude", movingVector.magnitude / currentSpeed);
            Debug.Log(movingVector.magnitude / currentSpeed);
            // «десь мы двигаем персонажа! ”станавливаем движение только по x & z потому что мы не хотим чтобы наш персонаж взлетал в воздух
            rig.velocity = new Vector3(movingVector.x, rig.velocity.y, movingVector.z);
            rig.angularVelocity = Vector3.zero;
        }
        public void Jump()
        {
            
            // ¬ыполн€ем прыжок по команде анимации.
            if (Physics.Raycast(groundCheckerTransform.position, Vector3.down, 0.2f, groundLayer))
             {
                rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
             }
        }
    }
