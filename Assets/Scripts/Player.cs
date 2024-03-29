using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Vector3 prevPos;
    public bool startFollowMouse;
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private Vector2 hitbox = new (0.5f, 0.5f);

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip victorySound;
    // [SerializeField] private AudioSource footstepsAudioSource;
    // [SerializeField] private AudioSource finalFootstepAudioSource;  
    //private bool hasPlayedFinalFootstep;
    private void Awake()
    {
        prevPos = transform.position;
    }

    private void OnMouseDown()
    {
        startFollowMouse = true;
        GetComponent<BoxCollider2D>().size = hitbox;
        //  Cursor.visible = false;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            OnDeath();
        }
    }

    public void OnGoal()
    {
        if (!startFollowMouse) return;
        startFollowMouse = false;
        audioSource.clip = victorySound;
        audioSource.Play();
        
        InvokeRepeating(nameof(FlashPlayer), 0f, 0.2f);
        StartCoroutine(WaitForVictorySFX());
    }

    public void OnDeath()
    {
        if (!startFollowMouse) return;
        startFollowMouse = false;
        audioSource.clip = deathSound;
        audioSource.Play();
        
        InvokeRepeating(nameof(FlashPlayer), 0f, 0.05f);
        StartCoroutine(WaitForDeathSFX());
    }

    private void FlashPlayer()
    {
        var sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        
        sprite.enabled = !sprite.enabled;
    }

    private IEnumerator WaitForDeathSFX()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        GameManager.Instance.RestartLevel();
    }
    
    private IEnumerator WaitForVictorySFX()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        GameManager.Instance.LoadNextLevel();
    }


    // Update is called once per frame
    void Update()
    {
        if (!startFollowMouse) return;
        //if (!audioSource.isPlaying) audioSource.Play();
        float step = speed * Time.deltaTime;
        // var vel = Vector2.Distance(transform.position, prevPos);
        // prevPos = transform.position;
        // var normal = Mathf.InverseLerp(0.01f, 0.2f, vel);
        // var newValue = Mathf.Lerp(0.9f, 1.1f, normal);
        // footstepsAudioSource.pitch = newValue;
        // if (newValue <= 0.91f)
        // {
        //     footstepsAudioSource.Stop();
        // }
        // else
        // {
        //     if (!footstepsAudioSource.isPlaying) footstepsAudioSource.Play();
        // }

        var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var dist = Vector2.Distance(mousePos, transform.position);
        transform.position = Vector2.MoveTowards(transform.position, mousePos, 
            step * dist);

        
        //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
