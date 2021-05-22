using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;  // for stringbuilder
using UnityEngine;
using UnityEngine.Windows.Speech;   // grammar recogniser
using System;
using System.Linq;
using UnityEngine.SceneManagement;


/*
 *  Uses English US in the settings - Keyboard (on the taskbar), Region, Preferred Language and Speech in Settings
 */

[RequireComponent(typeof(AudioSource))]
public class GrammarController : MonoBehaviour
{


    private AudioSource audioSource;
    private GrammarRecognizer gr;
    [SerializeField] private float speed = 5.0f;    // move the player at this speed

    private Rigidbody2D rb;

    // Action is in System, using System; or System.Action
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    // Confidence Level
    private ConfidenceLevel confidenceLevel = ConfidenceLevel.Low;
    [SerializeField] private Bullet bulletPrefab1;  // a variable, type of Bullet (Bullet variable)
    [SerializeField] private Bullet bulletPrefab2;  // a variable, type of Bullet (Bullet variable)
    [SerializeField] private float bulletSpeed = 5.0f;
    public int changeNum=0;
    public GameObject rainingEffects;
    public GameObject sunEffects;
    public static bool GameIsPaused = false;
    public static bool IsRaining = false;
    public static bool IsSunny = false;
    public GameObject pausedMenuUI;
    public GameObject gameStoryUI;

    public static bool showGameStory = false;

    private void Start()
    {
        //Player's movement direction  
        actions.Add("up", Up);  // move the player up
        actions.Add("down", Down);  // move the player down
        actions.Add("left", Left);  // move the player left
        actions.Add("right", Right);  // move the player right
        

        //Game states 
        actions.Add("new", New);  // start the game. Splash screen --> Main Game Scene.
        actions.Add("quit", Quit);  // finith the game and go to the Splash Scene.

        //Fire the bullet (Kill enemies) 
        actions.Add("shot", Shot); // kill the enemies with weapon(bullet)

        //Change weapon 
        actions.Add("change", Change);

        //Change the weather
        actions.Add("rain", Rain); //rain effect
        actions.Add("norain", NoRain);  //sun effect

        //Pause the game 
        actions.Add("pause", PauseGame);  // pause the game
        actions.Add("resume", ResumeGame);  // resume the game 

        //Display the game story 
        actions.Add("display", DisplayGameStory);  // display the game story
        actions.Add("donotDisplay", NoGameStory);  // do not display the game story


        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        gr = new GrammarRecognizer(Path.Combine(Application.streamingAssetsPath, "Grammar.xml"), ConfidenceLevel.Low);
        Debug.Log("Grammar loaded!");
        gr.OnPhraseRecognized += GR_OnPhraseRecognized;
        gr.Start();
        if (gr.IsRunning) Debug.Log("Recogniser running");
        
    }

    private void GR_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder message = new StringBuilder();
        Debug.Log("Recognised a phrase");
        // read the semantic meanings from the args passed in.
        SemanticMeaning[] meanings = args.semanticMeanings;
        // Move pawn from C2 to C4 - Piece, Start, Finish
        
        // semantic meanings are returned as key/value pairs
        // Piece/"pawn", Start/"C2", Finish/"C4"
        // use foreach to get all the meanings.
        foreach(SemanticMeaning meaning in meanings)
        {
            //string keyString = meaning.key.Trim();
            string valueString = meaning.values[0].Trim();
            //message.Append("Key: " + keyString + ", Value: " + valueString + " ");
            message.Append(valueString + " ");
            Debug.Log(message);

            actions[valueString].Invoke();
        }
        // use a string builder to create the string and out put to the user
        //actions[message.ToString()].Invoke();
        Debug.Log(message);
    }

    private void Up()
    {
        transform.Translate(0, 1, 0);
        Debug.Log("Said Move Up");
    }

    private void Down()
    {
        transform.Translate(0, -1, 0);
        Debug.Log("Said Move Down");
    }

    private void Left()
    {
        transform.Translate(-1, 0, 0);
        Debug.Log("Said Move Left");
    }

    private void Right()
    {
        transform.Translate(1, 0, 0);
        Debug.Log("Said Move Right");
    }
     
    private void New()
    {
        Debug.Log("Said Start a new game");
        SceneManager.LoadScene(1);  //Go to the main menu scene
    }

    private void Quit()
    {
        Debug.Log("Said End the game");
        SceneManager.LoadScene(0);  //Go to the splash screen
    }

    private void Shot()
    {
        Debug.Log("Said shot - Kill the ememies");

        FireBullet();
    }

    private void Change()
    {
        Debug.Log("Said change weapon - There are two different weapons to choose from");
        changeNum++;
        
    }

    private void FireBullet()
    {
        // create a bullet variable
        Bullet bullet;
        
        if( changeNum > 0 ) 
        {
            bullet =  Instantiate(bulletPrefab2);
            changeNum--;
        }
        else
            bullet =  Instantiate(bulletPrefab1);
        


        // set the position to the center of the player
        bullet.transform.position = this.transform.position;
                                    // this : because weaponsController is belong to the player, so you dont need to add this.

        
        // make the bullet move via the RigidBody 
        Rigidbody2D rbb = bullet.GetComponent<Rigidbody2D>();
        // make it move to right
        rbb.velocity = Vector2.up * bulletSpeed;
        
    }

    private void Rain()
    {
        Debug.Log("You said Start raining");
        rainingEffects.SetActive(true);
        IsRaining = true;

        // When it's raining, the sun won't be displayed on the scene.
        sunEffects.SetActive(false);
        IsSunny = false;
    }

    private void NoRain()
    {
        Debug.Log("You said Stop raining");
        rainingEffects.SetActive(false);
        IsRaining = false;

        // When it's not raining, the sun comes up!
        sunEffects.SetActive(true);
        IsSunny = true;
    }

    private void PauseGame()
    {
        Debug.Log("You said pause the game");
        pausedMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        
    }

    private void ResumeGame()
    {
        Debug.Log("You said resume the game");
        pausedMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        
    }

    private void DisplayGameStory()
    {
        Debug.Log("You said display the game story");
        gameStoryUI.SetActive(true);
        Time.timeScale = 0f;
        showGameStory = true;
        
    }

    private void NoGameStory()
    {
        Debug.Log("You said do not display the game story");
        gameStoryUI.SetActive(false);
        Time.timeScale = 1f;
        showGameStory = false;
        
    }

    private void OnApplicationQuit()
    {
        if (gr != null && gr.IsRunning)
        {
            gr.OnPhraseRecognized -= GR_OnPhraseRecognized;
            gr.Stop();
        }
    }
}
