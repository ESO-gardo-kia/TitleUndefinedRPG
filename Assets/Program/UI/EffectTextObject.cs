using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectTextObject : MonoBehaviour
{
    [SerializeField] private Text numText;
    [SerializeField] private Color textColor;
    [SerializeField] private float transmissionDeceleration;

    [SerializeField] private float lifeTime;
    public float lifeTimeCount;

    [SerializeField] private Vector3 moveSpeed = Vector3.zero;
    public Vector3 currentMoveSpeed;
    [SerializeField] private float moveSpeedDeceleration;
    void Update()
    {
        MoveText();
        TextFadeout();
        LifeTimeCount();
    }
    public void SetTextInfomation(string setText, Color setTextColor)
    {
        numText.text = setText;
        textColor = setTextColor;
        currentMoveSpeed = moveSpeed;
    }
    private void LifeTimeCount()
    {
        if (lifeTime <= lifeTimeCount)
        { 
            Destroy(gameObject);
        }
        else
        {
            lifeTimeCount += Time.deltaTime;
        }
    }
    private void MoveText()
    {
        transform.position += currentMoveSpeed;
        if (currentMoveSpeed.x > 0) currentMoveSpeed.x -= moveSpeedDeceleration * Time.deltaTime;
        else if (currentMoveSpeed.x < 0) currentMoveSpeed.x = 0;
        if (currentMoveSpeed.y > 0) currentMoveSpeed.y -= moveSpeedDeceleration * Time.deltaTime;
        else if (currentMoveSpeed.y < 0) currentMoveSpeed.y = 0;
        if (currentMoveSpeed.z > 0) currentMoveSpeed.z -= moveSpeedDeceleration * Time.deltaTime;
        else if (currentMoveSpeed.z < 0) currentMoveSpeed.z = 0;
    }
    private void TextFadeout()
    {
        textColor.a -= transmissionDeceleration * Time.deltaTime;
        numText.color = textColor;
    }
}
