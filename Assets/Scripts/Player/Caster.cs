using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TypingComparator;
using System.Text;
using Sirenix.OdinInspector;
using JetBrains.Annotations;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif
using Random = UnityEngine.Random;

/// <summary>
/// Class responsible for all of the player's spell casting
/// </summary>
public class Caster : MonoBehaviour
{
    #region References
    [TabGroup("References", "Base")]
    [SerializeField] Player _player;
    [TabGroup("References", "Base")]
    [SerializeField] private Move _moveScript;
    [TabGroup("References", "Base")]
    [SerializeField] Canvas _canvas;
    [TabGroup("References", "Base")]
    [SerializeField] TMP_Text _text;

    private EndOfMission _mission;

    private bool isDead = false;
    
    [TabGroup("References", "Spells")] [SerializeField] [FoldoutGroup("Logo List")]
    private List<Sprite> logoList;
    
    [TabGroup("References", "Spells")] [SerializeField] [FoldoutGroup("Fireball (id1)")]
    GameObject FireBall;

    [TabGroup("References", "Spells")] [ShowInInspector] [FoldoutGroup("Fireball (id1)")]
    private int WordCount1;
    
    [TabGroup("References", "Spells")] [SerializeField] [FoldoutGroup("Forcefield (id2)")]
    private GameObject ForceField;
    [TabGroup("References", "Spells")] [ShowInInspector] [FoldoutGroup("Forcefield (id2)")]
    private int WordCount2;
    
    [TabGroup("References", "Spells")] [ShowInInspector] [FoldoutGroup("Purify (id3)")]
    private int WordCount3;
    
    [TabGroup("References", "Spells")] [SerializeField] [FoldoutGroup("Goospell (id4)")]
    private GameObject GoospelPrefab;
    [TabGroup("References", "Spells")] [ShowInInspector] [FoldoutGroup("Goospell (id4)")]
    private int WordCount4;

    [TabGroup("References", "Spells")]
    [ShowInInspector]
    [FoldoutGroup("TimeBend(id5)")]
    private int WordCount5 = 5;


    private SpellEffectsCanvas _spellEffectsCanvas;



    [TabGroup("References", "Data")] [ShowInInspector]
    public Dictionary<int, string> spellNames = new Dictionary<int, string>()
    {
        { 1, "Fireball" },
        { 2, "Force Field" },
        { 3, "Purifying Spell" },
        { 4, "Goo Spell" },
        { 5, "Time Bend" },
        {6, "Clock Teleport"},
        {7, "Pacifier"},
        {8, "Earth Anger"},
        {9, "Thunder Feet"}
    };
    

    

    
    // Base word counts (set in editor)

    [Range(1, 14)] [SerializeField] [TabGroup("References", "Spell Word Count")]
    private int baseWordCount1;
    [Range(1, 14)] [SerializeField] [TabGroup("References", "Spell Word Count")]
    private int baseWordCount2;
    [Range(1, 14)] [SerializeField] [TabGroup("References", "Spell Word Count")]
    private int baseWordCount3;
    [Range(1, 14)] [SerializeField] [TabGroup("References", "Spell Word Count")]
    private int baseWordCount4;
    [Range(1, 14)] [SerializeField] [TabGroup("References", "Spell Word Count")]
    private int baseWordCount5;
    [Range(1, 14)] [SerializeField] [TabGroup("References", "Spell Word Count")]
    private int baseWordCount6 = 1;
    [Range(1, 14)] [SerializeField] [TabGroup("References", "Spell Word Count")]
    private int baseWordCount7 = 4;
    [Range(1, 14)] [SerializeField] [TabGroup("References", "Spell Word Count")]
    private int baseWordCount8 = 3;
    [Range(1, 14)] [SerializeField] [TabGroup("References", "Spell Word Count")]
    private int baseWordCount9 = 4;

    // ################################



    #region FeedbackCanvas




    [TabGroup("References", "Feedback Canvas")]
    [SerializeField] private Canvas feedBackCanvas;
    [TabGroup("References", "Feedback Canvas")]
    [SerializeField] private TMP_Text precisionText;
    [TabGroup("References", "Feedback Canvas")]
    [SerializeField] private TMP_Text wpmText;
    [TabGroup("References", "Feedback Canvas")]
    [SerializeField] private Animator fbCanvasAnimator;
    
    
    Sentence _sentence; // Sentence being currently typed / last sentence that has been typed
    int _skillToLaunch = -1; // Skill that is currently being cast / last skill cast
    private int characterCount = 0; // Char count of the current sentence
    
    #endregion
    
    
    // ######################### ID 1 : Fireball ################################################
    //Sends a fireball towards the direction you're facing, zone damage, affected by precision/wpm
    
    // ######################### ID 2 : Forcefield ##############################################
    //Creates a shield around you keeping enemies and projectiles away, shield grows per char
    
    
    
    private GameObject actuallyCasting; // Actually stored / casting spell

    private bool forceFieldActuallyCasting = false;
    
    
    
    // ######################### ID 3 : Purify ##################################################
    // Purifies unholy grounds by destroying the omens
    
    
    [TabGroup("References", "Actual Omen")] [ShowInInspector]
    private GameObject actualOmen;
    
    
    // ######################### ID 4 : GooSpell ################################################

    private GameObject actualGoospell;

    public void ForgetGoospell()
    {
        actualGoospell = null;
    }
    // ######################### ID 5 : TimeSlow ################################################

    private bool isTimeSlowed = false;

    private float timeSlowEnd = 0f;



    // ######################### ID 6 : Teleport ################################################

    public bool isTeleporting = false;

    [TabGroup("References", "Spells")] [SerializeField] [FoldoutGroup("Teleport (id6)")]
    private GameObject _clockTeleportPrefab;
    
    [TabGroup("References", "Spells")] [SerializeField] [FoldoutGroup("Teleport (id6)")]
    private int WordCount6 = 1;

    
    // ######################## ID 7 : Pacifier #################################################

    public bool isPacified = false;
    
    [TabGroup("References", "Spells")] [SerializeField] [FoldoutGroup("Pacify (id7)")]
    private int WordCount7 = 4;

    private void StopEasyTime()
    {
        isPacified = false;
    }
    
    // ######################### ID 8 : Spike Zone ##############################################

    [TabGroup("References", "Spells")] [SerializeField] [FoldoutGroup("Spike Zone (id8)")]
    private GameObject spikeZonePrefab;

    [TabGroup("References", "Spells")] [SerializeField] [FoldoutGroup("Spike Zone (id8)")]
    private int WordCount8 = 3;
    
    
    private GameObject _actualSpikeZone;

    public void ForgetSpikeZone()
    {
        _actualSpikeZone = null;
    }
    
    // ######################### ID 9 : Speed Up!  ##############################################
    
    [TabGroup("References", "Spells")] [SerializeField] [FoldoutGroup("Speed Up (id9)")]
    private int WordCount9 = 4;

    private bool spedUp = false;

    /// <summary>
    /// Calls the speed up method from player move
    /// </summary>
    /// <param name="modifier">speed modifier</param>
    /// <param name="duration">duration of the speed boost</param>
    private void speedUp(float modifier, float duration)
    {
        _moveScript.SpeedUp(modifier, duration);
    }

    private void turnSpeedUpOffHelper()
    {
        spedUp = false;
    }
    
    
    // ##########################################################################################
    // ##########################################################################################
    // ##########################################################################################
    // ##########################################################################################

    
    
    // ##########################################################################################
    // ##########################################################################################



    #endregion

    #region Difficulty Related

    public enum DifficultyLevel { VeryEasy, Easy, Normal, Hard, VeryHard }
    private Dictionary<DifficultyLevel, float> difficultyMultipliers = new Dictionary<DifficultyLevel, float>()
    {
        { DifficultyLevel.VeryEasy, 0.35f },
        { DifficultyLevel.Easy, 0.50f },
        { DifficultyLevel.Normal, 0.75f },
        { DifficultyLevel.Hard, 1f },
        { DifficultyLevel.VeryHard, 1.75f }
    };
    
    public void AdjustDifficulty(DifficultyLevel newDifficulty)
    {
        difficultyLevel = GameManager.Instance.difficultyLevel;

        
        WordCount1 = baseWordCount1;
        WordCount2 = baseWordCount2;
        WordCount3 = baseWordCount3;
        WordCount4 = baseWordCount4;
        WordCount5 = baseWordCount5;
        WordCount6 = baseWordCount6;
        //Skipping Spell 7 Since it's the difficulty modifier spell
        WordCount8 = baseWordCount8;
        WordCount9 = baseWordCount9;
        
        float multiplier = difficultyMultipliers[newDifficulty];
    
        WordCount1 = Mathf.Max(1, Mathf.RoundToInt(baseWordCount1 * multiplier));
        WordCount2 = Mathf.Max(1, Mathf.RoundToInt(baseWordCount2 * multiplier));
        WordCount3 = Mathf.Max(1, Mathf.RoundToInt(baseWordCount3 * multiplier));
        WordCount4 = Mathf.Max(1, Mathf.RoundToInt(baseWordCount4 * multiplier));
        WordCount5 = Mathf.Max(1, Mathf.RoundToInt(baseWordCount5 * multiplier));
        WordCount6 = Mathf.Max(1, Mathf.RoundToInt(baseWordCount6 * multiplier));
        //Skipping Spell 7 Since it's the difficulty modifier spell

        // Update any other related game settings or UI here
        WordCount8 = Mathf.Max(1, Mathf.RoundToInt(baseWordCount8 * multiplier));
        WordCount9 = Mathf.Max(1, Mathf.RoundToInt(baseWordCount9 * multiplier));
    }

    public DifficultyLevel difficultyLevel = DifficultyLevel.Normal;
    
    

    #endregion
    

    private Dictionary<int, int> SlotToSpellIdDictionary;

    public List<Sprite> ReturnLogoList()
    {
        return logoList;
    }

    public Dictionary<int, int> ReturnSlotDictionary()
    {
        return SlotToSpellIdDictionary;
    }
    
    
    private void Start()
    {
        _mission = FindObjectOfType<EndOfMission>();

        _spellEffectsCanvas = FindObjectOfType<SpellEffectsCanvas>();

        SlotToSpellIdDictionary = GameManager.Instance.slotToSpellDic;

        WordCount1 = baseWordCount1;
        WordCount2 = baseWordCount2;
        WordCount3 = baseWordCount3;
        WordCount4 = baseWordCount4;
        WordCount5 = baseWordCount5;
        WordCount6 = baseWordCount6;
        WordCount7 = baseWordCount7;
        WordCount8 = baseWordCount8;
        WordCount9 = baseWordCount9;
        
        AdjustDifficulty(GameManager.Instance.difficultyLevel);
    }

    /// <summary>
    /// Feeds omen to the current omen parameter
    /// </summary>
    public void FeedOmen(GameObject omen)
    {
        actualOmen = omen;
    }
    
    /// <summary>
    /// Nulls the current omen reference
    /// </summary>
    public void ForgetOmen()
    {
        actualOmen = null;
    }

    /// <summary>
    /// Accesser for the Session build mode
    /// </summary>
    private void OnBuild()
    {
        GameSession session = FindObjectOfType<GameSession>();
        session.OnBuild();
    }

    /// <summary>
    /// Accesser for the interaction
    /// </summary>
    private void OnInteraction()
    {
        GameSession session = FindObjectOfType<GameSession>();
        session.OnInteraction();
    }
    
//#####################################################################################
/// <summary>
/// To Add A Spell
/// 
/// 1. Create the full execution of the spell, with the parameters it will take, into the SPELL HELPER REGION
///                                     <see cref="SkillOne"/>
/// 
/// 2. Then, in the SPELL WRAPPER REGION, call the sentence creation method, then call the first method just created.
///    You can then pass the needed parameters from the sentence just created into the helper method.
///                                     <see cref="SpellId1"/>
/// 
/// 
/// 3. Add the entry in "ExecuteSpellFromSlot" for the ID you just created;
///                                     <see cref="ExecuteSpellFromSlot"/>
///
/// 
/// 4. Add entry in SlotToSpellDictionary (SLOT_NUMBER , SKILL_ID) => Older Versions, not needed due to serialization
/// To change/Add the slots using the skills + LOGOLIST (GameManager) => LogoList still needed => EDITOR REFERENCE
/// 
/// 
/// 5. Logic related to start / during / after / cancelation  spell must be inserted
/// inside the cases vvv EachGoodKeyPress() - EndOfSpellCast() - OnCast()
///        <see cref="EachGoodKeyPress"/>       <see cref="EndOfSpellCast"/>      <see cref="OnCast"/>
///
/// 
///
/// 6. Do not forget to add the wordCount(SPELL_ID) and basewordcount(in start method) props.
///                                 <see cref="Start"/>
/// 
/// 
/// 7. Add the spell references in AdjustDifficulty for it to scale relative to difficulty.
///                                 <see cref="AdjustDifficulty"/>
/// 
/// 
/// 8. Finally, add the reference to the spell and its skull price in the GameManager Singleton
/// (SpellValues in save/load) (And eventually in the cheatcode to unlock all easily just under it).
///                             <see cref="GameManager.SpellValues"/>
/// 
/// 9. PLUS SPELLNAMES IN THIS SCRIPT
/// <see cref="spellNames"/>
/// 
/// </summary>
//#####################################################################################


    private void Update() // Typing Cast Logic in Here
    {
        if (isTimeSlowed)
        {
            if (timeSlowEnd - Time.time <= 0)
            {
                isTimeSlowed = false;
                Time.timeScale = 1;
                SoundMaster.Instance.TimeWarpOut();
            }
        }

        if (isDead) return;
        
        if (_player.IsDead())
        {
            isDead = true;
            if (_player.ActualState() == Player.state.Casting)
            {
                _player.ToggleCasting();
            }
            _canvas.gameObject.SetActive(false);
            return;
        }
        
        FlipCanvas();
        

        if (_player.ActualState() == Player.state.Casting)
        { // 1. GOING INTO SPELL CAST MODE
            
            if (_sentence.IsDone) // 3. SENTENCE TERMINATED = END OF SPELL CAST
            {
                EndOfSpellCast();
            }

            _canvas.gameObject.SetActive(true);
            _text.text = $"<color=green>{_sentence.ShowAchieved()}</color>";
            _text.text += _sentence.ShowRemaining();

            foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                {
                    if (IsRelevantKey(kcode))
                    {
                        SoundMaster.Instance.KeyboardClick();
                        char keyChar;

                        if (kcode == KeyCode.Alpha6)
                        {
                            keyChar = '-';
                        }
                        else
                        {
                            keyChar = (char)kcode;
                        }

                        if (_sentence.TypeIn(keyChar)) // 2. FOR EACH GOOD KEYPRESS WHILE SPELL CASTING
                        {
                            EachGoodKeyPress();
                        }
                    }
                }
            }

        }
        else
        {

            
            characterCount = 0; // resetting character count after a cast, or annulation
            
            _canvas.gameObject.SetActive(false);
        }
    }


    
    /// <summary>
    /// Method called on each successfull keypress
    /// </summary>
    private void EachGoodKeyPress()
    {
        switch (_skillToLaunch) // CASES ARE SKILL IDs
        {
            case 2: // FORCEFIELD
                                    
                if (actuallyCasting != null)
                {
                    Forcefield script = actuallyCasting.GetComponent<Forcefield>();
                    if (script != null)
                    {
                        script.Grow();
                    }
                }

                break;
            default:
                break;
        }
                            
        characterCount++; // +1 Per character correctly typed in
        _sentence.ClearChars(1);
    }
    
    /// <summary>
    /// Method called at the successfull end of a spellcast
    /// </summary>
    private void EndOfSpellCast() // What happens when each skill has been typed correctly
    {
        decimal charNumberMod = (Convert.ToDecimal(characterCount) / Convert.ToDecimal(_sentence.WordCount()));
        decimal charPerWord = (charNumberMod / 5) * 100;
        precisionText.text = $"{_sentence.TypePrecision()}%";
        wpmText.text = $"{_sentence.WordsPerMinute()}WPM";
        TurnFeedBackCanvasOn();
        Invoke("TurnFeedBackCanvasOff", 2f);
        _player.ToggleCasting();

        if (_sentence.TypePrecision() == 100) // PERFECT SOUND
        {
            SoundMaster.Instance.OneHundredPercent();
        }

        switch (_skillToLaunch) // CASES ARE SKILL IDs
        {
            case 1: // FIREBALL
                SkillOne(_sentence.TypePrecision(), _sentence.WordsPerMinute(), charPerWord);
                SoundMaster.Instance.ExplosionSound();
                break;
            case 2: // FORCEFIELD
                actuallyCasting = null;
                forceFieldActuallyCasting = false;
                break;
            case 3: // PURIFY
                SoundMaster.Instance.PurifySpell();
                SkillThree();
                break;
            case 4: // GOOSPELL
                SkillFour(_sentence.TypePrecision(), _sentence.WordsPerMinute());
                SoundMaster.Instance.GooSpell();
                break;
            case 5: //TIME SLOW
                SkillFive(_sentence.TypePrecision(), _sentence.WordsPerMinute());
                SoundMaster.Instance.TimeWarpIn();
                break;
            case 6: // TELEPORT
                SkillSix();
                break;
            case 7: // PACIFIER
                SkillSeven(_sentence.TypePrecision(), _sentence.WordsPerMinute());
                break;
            case 8:
                SkillEight(_sentence.TypePrecision(), _sentence.WordsPerMinute());
                break;
            case 9:
                SkillNine(_sentence.TypePrecision(), _sentence.WordsPerMinute());
                break;
            default:
                break;

        }

        // #################################### SENDING TO DATABASE ############################

        if (_mission != null)
        {
            _mission.AddToPrecision(_sentence.TypePrecision());
            _mission.AddToWpm(_sentence.WordsPerMinute());
        }

        // #####################################################################################

    }
    
    /// <summary>
    /// Cancel a began spellcast
    /// </summary>
    private void OnCast() // ON PUSHING THE CAST BUTTON BEFORE SPELL IS COMPLETE
    {
        if (_player.IsDead()) return;
        
        if (_player.ActualState() != Player.state.Casting) { return; }
        
        if (_skillToLaunch == 2) // If actually casting FORCEFIELD (CANCEL AND KILL SHIELD)
        {
            if (actuallyCasting != null)
            {
                Forcefield script = actuallyCasting.GetComponent<Forcefield>();
                if (script != null) {}
                script.KillShield();
                actuallyCasting = null;
            }
            forceFieldActuallyCasting = false;
        }
        
        _player.ToggleCasting(); // TOGGLE CASTING STATE
    }

    
    
    private void TurnFeedBackCanvasOn()
    {
        fbCanvasAnimator.SetTrigger("isFadingIn");
    }
    private void TurnFeedBackCanvasOff()
    {
        fbCanvasAnimator.SetTrigger("isFadingOut");
    }
    
    
    
    
    /// <summary>
    /// <para>Defines a number of word necessary to cast a spell, and the ID of said spell, and creates a random sentence for this process while toggling Cast</para>
    /// <para>stops all moveInput stored so that the character doesn't move after a cast without input</para>
    /// </summary>
    /// <param name="numberOfWordsForCast">Numbers of words necessary for the casting of the skill</param>
    /// <param name="skillId">ID of the skill that is aimed to be cast</param>
    private void LaunchSkill(int numberOfWordsForCast, int skillId)
    {
        if (_player.ActualState() == Player.state.Casting) { return; }
        if (_player.ActualEquipped() == Player.equipped.Spear) { return; }
        
        _moveScript.StopMoveInput();

        _sentence = new Sentence(GenerateLowerSentence(numberOfWordsForCast), true);
        _skillToLaunch = skillId;
        _player.ToggleCasting();
    }

    /// <summary>
    /// Method to call inside the keypresses, to call the spell associated to that key within the dictionary
    /// </summary>
    private void ExecuteSpellFromSlot(int slotId)
    {
        switch (SlotToSpellIdDictionary[slotId])
        {
            case 1:
                SpellId1();
                break;
            case 2:
                SpellId2();
                break;
            case 3:
                SpellId3();
                break;
            case 4:
                SpellId4();
                break;
            case 5:
                SpellId5();
                break;
            case 6:
                SpellId6();
                break;
            case 7:
                SpellId7();
                break;
            case 8:
                SpellId8();
                break;
            case 9:
                SpellId9();
                break;
        }
    }

    /// <summary>
    /// Bool to see if the state the player's in right now makes him be able to cast
    /// </summary>
    private bool CanCast()
    {
        if (_player.ActualState() == Player.state.Casting || _player.ActualEquipped() == Player.equipped.Spear)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    /// <summary>
    /// Checks if the key pressed needs to be registered by the program
    /// </summary>
    /// <param name="keyCode"></param>
    /// <returns></returns>
    bool IsRelevantKey(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.A:
                return true;
            case KeyCode.B:
                return true;
            case KeyCode.C:
                return true;
            case KeyCode.D:
                return true;
            case KeyCode.E:
                return true;
            case KeyCode.F:
                return true;
            case KeyCode.G:
                return true;
            case KeyCode.H:
                return true;
            case KeyCode.I:
                return true;
            case KeyCode.J:
                return true;
            case KeyCode.K:
                return true;
            case KeyCode.L:
                return true;
            case KeyCode.M:
                return true;
            case KeyCode.N:
                return true;
            case KeyCode.O:
                return true;
            case KeyCode.P:
                return true;
            case KeyCode.Q:
                return true;
            case KeyCode.R:
                return true;
            case KeyCode.S:
                return true;
            case KeyCode.T:
                return true;
            case KeyCode.U:
                return true;
            case KeyCode.V:
                return true;
            case KeyCode.W:
                return true;
            case KeyCode.X:
                return true;
            case KeyCode.Y:
                return true;
            case KeyCode.Z:
                return true;
            case KeyCode.Comma:
                return true;
            case KeyCode.Alpha6:
                return true;
            case KeyCode.Space:
                return true;
            default:
                return false;
        }
    }
    
    /// <summary>
    /// Flips canvas in relation to player's direction to keep it straight
    /// </summary>
    private void FlipCanvas()
    {
        if (_player.transform.localScale.x == 1)
        {
            _canvas.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
            feedBackCanvas.transform.localScale = new Vector3(0.0045f, 0.0045f, 0.0045f);
        }
        else
        {
            _canvas.transform.localScale = new Vector3(-0.025f, 0.025f, 0.025f);
            feedBackCanvas.transform.localScale = new Vector3(-0.0045f, 0.0045f, 0.0045f);
        }
    }
    
    // KEYPRESS REGION
    #region Keypresses
    /// <summary>
    /// Key J
    /// </summary>
    private void OnSkill1()
    {
        if (!CanCast()) return;

        if (!GameManager.Instance.slotToSpellDic.ContainsKey(1)) return; // If the slot isn't assigned to ability
        
        ExecuteSpellFromSlot(1);
    }
    /// <summary>
    /// Key K
    /// </summary>
    private void OnSkill2()
    {
        if (!CanCast()) return;
        
        if (!GameManager.Instance.slotToSpellDic.ContainsKey(2)) return;
        
        ExecuteSpellFromSlot(2);
    }
    /// <summary>
    /// Key L
    /// </summary>
    private void OnSkill3()
    {
        if (!CanCast()) return;
        
        if (!GameManager.Instance.slotToSpellDic.ContainsKey(3)) return;

        ExecuteSpellFromSlot(3);
    }
    /// <summary>
    /// Key M
    /// </summary>
    private void OnSkill4()
    {
        if (!CanCast()) return;
        
        if (!GameManager.Instance.slotToSpellDic.ContainsKey(4)) return;

        ExecuteSpellFromSlot(4);

    }
    /// <summary>
    /// Key U
    /// </summary>
    private void OnSkill5()
    {
        if (!CanCast()) return;

        if (!GameManager.Instance.slotToSpellDic.ContainsKey(5)) return;

        ExecuteSpellFromSlot(5);
    }
    /// <summary>
    /// Key I
    /// </summary>
    private void OnSkill6()
    {
        if (!CanCast()) return;

        if (!GameManager.Instance.slotToSpellDic.ContainsKey(6)) return;

        ExecuteSpellFromSlot(6);
    }
    /// <summary>
    /// Key O
    /// </summary>
    private void OnSkill7()
    {
        if (!CanCast()) return;

        if (!GameManager.Instance.slotToSpellDic.ContainsKey(7)) return;

        ExecuteSpellFromSlot(7);
    }
    /// <summary>
    /// Key P
    /// </summary>
    private void OnSkill8()
    {
        if (!CanCast()) return;

        if (!GameManager.Instance.slotToSpellDic.ContainsKey(8)) return;

        ExecuteSpellFromSlot(8);
    }
    #endregion
    // KEYPRESS REGION

    // SPELL HELPER REGION





    #region Spell Helper Region

    /// <summary>
    /// Fireball skill
    /// </summary>
    /// <param name="precisionMod">Typing precision</param>
    /// <param name="wordsPerMinute">WPM</param>
    public void SkillOne(int precisionMod, int wordsPerMinute, decimal charPerWord)
    {
        float dir = Mathf.Sign(_player.transform.localScale.x);


        GameObject fireball = Instantiate(FireBall, new Vector2(_player.transform.position.x, _player.transform.position.y + 0.22f), Quaternion.Euler(0f, 0f, dir == 1 ? 90f : 270f)); // Rotate 45 degrees around the z-axis
        Fireball script = fireball.GetComponent<Fireball>();
        script.Inititalize(_player.direction, precisionMod, wordsPerMinute, charPerWord);
    }

    /// <summary>
    /// Forcefield Skill
    /// </summary>
    public GameObject SkillTwo(int charCount)
    {
        GameObject forceField = Instantiate(ForceField, _player.transform.position, Quaternion.identity);
        Forcefield script = forceField.GetComponent<Forcefield>();
        script.ActivateShield(charCount);
        return forceField;
    }

    /// <summary>
    /// Purify skill
    /// </summary>
    private void SkillThree()
    {
        if (actualOmen == null) return;

        Omen script = actualOmen.GetComponent<Omen>();

        if (script == null) return;
        
        script.LivesDown();
    }

    /// <summary>
    /// Goospell skill / Slows enemies
    /// </summary>
    private void SkillFour(int precision, int wpm)
    {
        if (actualGoospell != null) // Destroy the possibly existing goospell
        {
            Destroy(actualGoospell.gameObject);
        }

        actualGoospell = Instantiate(GoospelPrefab); // Instantiates and sets the position in front of the player
        actualGoospell.transform.position = new Vector3(_player.transform.position.x + (10f * _player.direction),
            _player.transform.position.y - 0.4f, _player.transform.position.z);
        Goospell script = actualGoospell.GetComponent<Goospell>();
        script.Init(precision, wpm); // Initializes
    }

    /// <summary>
    /// Slows time
    /// </summary>
    /// <param name="precision">Precision of the typing in percentage</param>
    /// <param name="wpm">words per minute of typing</param>
    private void SkillFive(int precision, int wpm)
    {
        if (isTimeSlowed) return;

        float maxSlowFactor = 0.6f;

        maxSlowFactor = (maxSlowFactor / 100f) * precision;

        maxSlowFactor += (wpm > 80) ? 0.1f : -0.1f;

        Time.timeScale = (1f - maxSlowFactor);

        isTimeSlowed = true;

        timeSlowEnd = Time.time + ((6f / 100f) * precision);

        float totalDuration = timeSlowEnd - Time.time;

        _spellEffectsCanvas.CreateNewItem(totalDuration, logoList[4]);
    }


    private void SkillSix() // TELEPORT
    {
        if (isTeleporting) return;

        isTeleporting = true;

        GameObject tpClock = Instantiate(_clockTeleportPrefab);

        ClockTeleportScript script = tpClock.GetComponent<ClockTeleportScript>();
        
        script.Init(_player.transform, Vector2.zero);
    }

    private void SkillSeven(int precision, int wpm)
    {
        if (isPacified) return;
        
        isPacified = true;

        float easyMaxDuration = 15f;
        
        easyMaxDuration = (easyMaxDuration / 100f) * precision;

        easyMaxDuration += (wpm > 80) ? 2f : -2f;
        
        
        _spellEffectsCanvas.CreateNewItem(easyMaxDuration, logoList[6]);
        Invoke("StopEasyTime", easyMaxDuration);
    }
    
    private void SkillEight(int precision, int wpm)
    {
        if (_actualSpikeZone != null) // Destroy the possibly existing spike zone
        {
            Destroy(_actualSpikeZone.gameObject);
        }

        _actualSpikeZone = Instantiate(spikeZonePrefab); // Instantiates and sets the position in front of the player
        _actualSpikeZone.transform.position = new Vector3(_player.transform.position.x + (10f * _player.direction),
            _player.transform.position.y - 0.4f, _player.transform.position.z);
        SpikeZoneScript script = _actualSpikeZone.GetComponent<SpikeZoneScript>();
        script.Init(precision, wpm); // Initializes
    }

    private void SkillNine(int precision, int wpm)
    {
        if (spedUp) return;

        spedUp = true;

        float speedMod = 1f + (precision / 100f) + (wpm <= 90 ? 0.5f : 1f);
        
        float easyMaxDuration = 15f;
        
        easyMaxDuration = (easyMaxDuration / 100f) * precision;

        easyMaxDuration += (wpm > 80) ? 2f : -2f;
        
        _spellEffectsCanvas.CreateNewItem(easyMaxDuration, logoList[8]);
        
        _moveScript.SpeedUp(speedMod, easyMaxDuration);
        
        Invoke("turnSpeedUpOffHelper", easyMaxDuration);
    }


    #endregion
    // SPELL HELPER REGION


    //SPELL WRAPPER REGION
    #region Spells Region Wrap Spell Here When Done

    private void SpellId1() // Fireball
    {
        LaunchSkill(WordCount1, 1);
    }

    private void SpellId2() // ForceField
    {
        LaunchSkill(WordCount2, 2);


        if (forceFieldActuallyCasting) return;

        forceFieldActuallyCasting = true;

        SoundMaster.Instance.ShieldSpell();

        Forcefield actuallyExistingForcefield = FindObjectOfType<Forcefield>();
        if (actuallyExistingForcefield != null)
        {
            actuallyExistingForcefield.KillShield();
        }
                    
        if (actuallyCasting == null)
        {
            actuallyCasting = SkillTwo(_sentence.CharCount());
        }
    }

    private void SpellId3() // Purify
    {
        if (actualOmen == null) return; // If no omen in range, do not begin a cast
        
        LaunchSkill(WordCount3, 3);
    }

    private void SpellId4() // Goospell
    {
        LaunchSkill(WordCount4, 4);
    }
    private void SpellId5()
    {
        LaunchSkill(WordCount5, 5);
    }

    private void SpellId6()
    {
        LaunchSkill(WordCount6, 6);
    }

    private void SpellId7()
    {
        LaunchSkill(WordCount7, 7);
    }

    private void SpellId8()
    {
        LaunchSkill(WordCount8, 8);
    }

    private void SpellId9()
    {
        LaunchSkill(WordCount9, 9);
    }
    

    #endregion
    //SPELL WRAPPER REGION

    
    #region RandomWordsGenerator related code
    private readonly string[] _wordArray = new string[]
        { "you", "the", "to", "it", "not", "that", "and", "of", "do", "have", "what", "we", "in", "get", "this", "my", "me", "go", "oh", "can", "no", "on", "for", "know", "just", "your", "all", "so", "with", "he", "but", "yeah", "well", "think", "here", "want", "out", "about", "good", "come", "up", "say", "now", "at", "one", "hey", "they", "see", "if", "how", "like", "she", "look", "make", "right", "guy", "take", "let", "really", "okay", "her", "uh", "tell", "him", "why", "there", "who", "time", "thing", "from", "will", "like", "when", "as", "because", "some", "our", "yes", "there", "back", "mean", "man", "little", "give", "his", "us", "them", "need", "then", "shall", "or", "talk", "okay", "something", "where", "great", "way", "never", "call", "too", "by", "sorry", "over", "love", "wait", "more", "down", "day", "two", "people", "God", "very", "off", "work", "thank", "big", "try", "dad", "maybe", "feel", "friend", "even", "sure", "find", "kid", "these", "boy", "put", "please", "happen", "much", "stop", "night", "bad", "into", "those", "any", "right", "first", "leave", "year", "hear", "right", "ever", "Mr", "again", "use", "mom", "may", "hi", "life", "nice", "new", "still", "kind", "anything", "only", "baby", "than", "fine", "hello", "keep", "girl", "help", "believe", "woman", "lot", "play", "ask", "start", "home", "nothing", "hmm", "their", "meet", "huh", "show", "around", "guess", "old", "hell", "before", "always", "three", "wow", "listen", "thanks", "minute", "actually", "eat", "place", "live", "away", "after", "bring", "every", "everything", "money", "person", "watch", "other", "remember", "house", "wrong", "kill", "school", "everyone", "run", "late", "care", "car", "move", "ah", "idea", "another", "someone", "today", "turn", "real", "happy", "whole", "week", "job", "fun", "problem", "break", "world", "which", "must", "party", "buy", "through", "together", "room", "family", "stay", "lose", "stuff", "son", "stupid", "name", "everybody", "last", "long", "tonight", "child", "sit", "course", "pretty", "hold", "game", "forget", "else", "own", "five", "second", "doctor", "dollar", "enough", "dog", "funny", "wear", "die", "sir", "hard", "honey", "sound", "sex", "hate", "suppose", "God", "head", "understand", "whoa", "movie", "worry", "cool", "marry", "miss", "pay", "hour", "crazy", "change", "hot", "most", "excuse", "mother", "check", "pick", "word", "same", "yourself", "ready", "seem", "win", "walk", "father", "story", "already", "hope", "part", "open", "lady", "read", "drink", "sleep", "number", "write", "morning", "tomorrow", "next", "phone", "four", "last", "once", "somebody", "probably", "without", "many", "such", "eye", "drive", "wife", "book", "hang", "since", "throw", "name", "dead", "stand", "myself", "aw", "dinner", "anyone", "hand", "each", "anyway", "television", "learn", "shut", "town", "beautiful", "both", "date", "spend", "office", "hit", "yet", "save", "true", "sweet", "until", "food", "while", "send", "high", "anymore", "also", "news", "Christmas", "ten", "ass", "business", "only", "couple", "totally", "door", "gay", "exactly", "parent", "few", "month", "easy", "deal", "ow", "hurt", "nobody", "OK", "perfect", "lie", "free", "young", "weird", "whatever", "brother", "work", "kid", "ago", "end", "other", "ball", "finally", "line", "its", "ha", "important", "fall", "heart", "long", "fat", "class", "shoot", "picture", "sell", "side", "wish", "love", "mind", "hair", "cut", "wedding", "reason", "become", "least", "look", "bite", "fuck", "under", "bed", "paper", "different", "catch", "mine", "six", "set", "face", "speak", "suck", "sometimes", "city", "special", "stick", "question", "dude", "realize", "birthday", "point", "enjoy", "fact", "dance", "soon", "bar", "wonder", "joke", "relationship", "chance", "black", "almost", "fight", "card", "song", "little", "bye", "coffee", "awesome", "sick", "apartment", "sorry", "back", "figure", "pull", "box", "dream", "water", "decide", "store", "bet", "lunch", "face", "anybody", "afraid", "buddy", "cute", "close", "bathroom", "show", "mind", "steal", "full", "company", "front", "ahead", "moment", "case", "date", "though", "body", "Mrs", "pants", "bitch", "promise", "glad", "kiss", "either", "fire", "grow", "build", "table", "ticket", "matter", "teach", "cat", "sister", "girlfriend", "hat", "touch", "terrible", "beer", "damn", "club", "amaze", "smell", "gift", "serious", "plan", "street", "team", "order", "cry", "zero", "drop", "act", "alone", "seat", "eh", "between", "eight", "twenty", "foot", "seven", "finish", "gentleman", "hand", "point", "blow", "small", "trouble", "sweetie", "sing", "pass", "beat", "piece", "shoe", "welcome", "god", "white", "kick", "bag", "fire", "early", "excite", "wonderful", "seriously", "country", "mouth", "question", "dear", "quite", "smart", "husband", "shh", "invite", "rest", "yours", "behind", "key", "end", "dress", "red", "laugh", "help", "agree", "machine", "yep", "return", "space", "mad", "truth", "ice", "follow", "outside", "idiot", "rule", "absolutely", "next", "against", "American", "need", "scare", "notice", "chicken", "ride", "music", "join", "next", "ruin", "war", "screw", "light", "along", "poor", "sense", "able", "matter", "secret", "fly", "top", "sure", "fair", "relax", "boyfriend", "plan", "mistake", "luck", "group", "daughter", "college", "president", "far", "sign", "close", "message", "freak", "fault", "quick", "till", "except", "single", "shirt", "choice", "attention", "power", "quit", "answer", "trust", "fifty", "leg", "wake", "human", "star", "student", "air", "chair", "gun", "begin", "death", "tree", "boss", "while", "present", "instead", "lucky", "safe", "cold", "ahh", "explain", "video", "brain", "completely", "voice", "cake", "drink", "trip", "monkey", "hundred", "fix", "entire", "expect", "million", "allow", "forever", "ugh", "hide", "huge", "interesting", "fight", "roll", "clothes", "fast", "grab", "teacher", "crap", "animal", "burn", "tough", "restaurant", "sort", "tooth", "marriage", "proud", "uncle", "push", "butt", "offer", "lord", "unless", "feeling", "suit", "floor", "cream", "favorite", "naked", "list", "ring", "clean", "apologize", "clear", "Earth", "share", "fill", "cool", "pizza", "ridiculous", "alive", "pretend", "hospital", "sad", "bunch", "half", "police", "fish", "window", "busy", "sign", "call", "pie", "answer", "raise", "somewhere", "sandwich", "thirty", "sale", "choose", "definitely", "swear", "pretty", "boat", "tired", "ho", "upset", "less", "nine", "thousand", "smoke", "embarrass", "band", "bear", "none", "Santa", "strong", "law", "toilet", "jump", "count", "egg", "Saturday", "blue", "horrible", "shit", "favor", "handle", "wall", "art", "cover", "arm", "perhaps", "simple", "bus", "appreciate", "himself", "surprise", "candy", "finger", "worth", "state", "possible", "rich", "short", "king", "knock", "cookie", "penis", "history", "imagine", "third", "blood", "drug", "future", "prove", "surprise", "alright", "captain", "evening", "congratulations", "system", "record", "age", "deserve", "normal", "yesterday", "jerk", "yay", "bird", "nose", "bother", "fan", "letter", "rather", "head", "interested", "owe", "pregnant", "destroy", "bear", "evil", "milk", "sport", "admit", "apparently", "conversation", "obviously", "jacket", "during", "accept", "dumb", "bit", "consider", "mention", "step", "deal", "hungry", "situation", "lead", "twelve", "fantastic", "hole", "plus", "note", "test", "honest", "character", "holy", "manager", "lesson", "soul", "nope", "dark", "difference", "wine", "road", "calm", "personal", "clown", "computer", "horse", "gosh", "mayor", "inside", "sexy", "dirty", "carry", "professor", "usually", "bottle", "cheese", "summer", "remind", "afternoon", "pee", "across", "angry", "fella", "film", "reach", "shower", "cup", "lawyer", "camera", "pain", "hurry", "desk", "monster", "certainly", "cop", "credit", "prepare", "anywhere", "breakfast", "bastard", "damn", "church", "hire", "decision", "Miss", "pig", "loser", "robot", "deep", "park", "chocolate", "service", "Jew", "tape", "kitchen", "half", "rid", "grandpa", "hero", "shot", "spot", "folks", "yell", "awful", "scene", "trick", "asshole", "issue", "lovely", "visit", "honor", "clean", "second", "chick", "costume", "Friday", "hall", "Ms", "fake", "forgive", "grade", "fifteen", "ought", "cheat", "Chinese", "crap", "create", "comfortable", "hotel", "magazine", "settle", "accident", "boob", "excellent", "neighbor", "train", "board", "pop", "spirit", "cow", "building", "ear", "giant", "ugly", "toy", "cancel", "Internet", "strange", "aunt", "island", "extra", "fit", "rock", "step", "action", "bill", "field", "kiss", "fresh", "level", "cost", "size", "cell", "serve", "shake", "neck", "bowl", "control", "loud", "bedroom", "check", "heaven", "right", "commercial", "draw", "guest", "insane", "fail", "cook", "pleasure", "truck", "science", "careful", "nervous", "although", "gas", "partner", "ew", "scream", "pool", "appear", "clearly", "silly", "feed", "charge", "neither", "wash", "stink", "magic", "plane", "tiny", "prison", "cause", "photo", "public", "button", "flower", "memory", "own", "fast", "base", "involve", "madam", "blame", "tea", "whoo", "bike", "freeze", "sexual", "code", "celebrate", "couch", "inside", "price", "assume", "delicious", "forty", "player", "soup", "waste", "coat", "doll", "security", "warm", "football", "model", "whose", "besides", "middle", "shop", "garbage", "client", "ground", "lame", "project", "dare", "shop", "episode", "glass", "green", "lock", "award", "straight", "unbelievable", "court", "experience", "final", "large", "salad", "belong", "fuck", "station", "area", "vote", "crime", "meat", "romantic", "treat", "forward", "glasses", "lie", "taste", "weight", "mail", "cab", "two hundred", "boring", "information", "total", "channel", "page", "suddenly", "sake", "Thanksgiving", "private", "French", "winner", "past", "pen", "twice", "cousin", "jealous", "mess", "planet", "scary", "universe", "upstairs", "genius", "dangerous", "nuts", "ourselves", "race", "suggest", "turn", "sea", "officer", "meal", "popular", "report", "welcome", "flight", "change", "driver", "wheel", "dump", "five hundred", "member", "yo", "add", "confuse", "form", "government", "Sunday", "order", "continue", "event", "quiet", "low", "color", "Jewish", "nah", "breast", "bank", "parking", "ride", "wild", "turkey", "blah", "thought", "famous", "gold", "pound", "skin", "one hundred", "rat", "switch", "tie", "career", "juice", "like", "protect", "shame", "bottom", "respect", "underwear", "Indian", "closet", "meeting", "sun", "afford", "bald", "engage", "plant", "towel", "mall", "regular", "bone", "dig", "fear", "cancer", "discuss", "fancy", "control", "advice", "center", "apple", "everywhere", "holiday", "cash", "common", "crush", "mess", "peace", "warn", "welcome", "adult", "noise", "rock", "super", "bread", "fucking", "porn", "three hundred", "type", "borrow", "department", "plate", "breathe", "classic", "farm", "disgusting", "tall", "taste", "replace", "somehow", "theater", "discover", "incredible", "plenty", "baseball", "comedy", "enter", "introduce", "whenever", "butter", "possibly", "sweetheart", "jail", "season", "study", "miracle", "complete", "golf", "hook", "perfectly", "stare", "flag", "jeez", "roommate", "ship", "wood", "actor", "break", "prize", "thanks", "corner", "fourth", "sneak", "piss", "pocket", "tip", "alone", "lately", "queen", "tear", "especially", "lay", "pressure", "rip", "assistant", "camp", "judge", "North", "often", "Halloween", "vacation", "impossible", "square", "left", "grandma", "gross", "pal", "smile", "speech", "bust", "gee", "handsome", "safety", "test", "community", "beach", "gym", "toast", "disease", "paint", "themselves", "Monday", "pack", "punch", "customer", "healthy", "invent", "princess", "merry", "someday", "blind", "certain", "solve", "study", "sweater", "terrific", "wet", "attack", "beg", "cigarette", "sugar", "uhh", "weekend", "damn", "sock", "emergency", "mood", "peanut", "stage", "tight", "gum", "moon", "nature", "program", "straight", "angel", "corporate", "damn", "exist", "ghost", "sauce", "stomach", "block", "herself", "murder", "speed", "Jesus", "mix", "track", "opportunity", "society", "upon", "difficult", "killer", "lip", "market", "goodness", "pillow", "tie", "arrive", "mountain", "slip", "belt", "museum", "oil", "press", "respect", "airplane", "airport", "honestly", "contest", "fabulous", "spell", "whoever", "bell", "friendship", "national", "alien", "dream", "fool", "past", "bless", "cartoon", "near", "swim", "burger", "fruit", "roof", "theory", "according", "guilty", "potato", "dish", "sound", "uncomfortable", "wrap", "cheap", "employee", "interview", "perform", "spring", "text", "tour", "treat", "awkward", "expensive", "unfortunately", "purse", "charge", "divorce", "ring", "bra", "brown", "duck", "English", "celebrity", "double", "period", "rent", "today", "barely", "chip", "ignore", "language", "laundry", "social", "dress", "soft", "apology", "concert", "disappoint", "knife", "hilarious", "judge", "blanket", "comic", "leader", "local", "neighborhood", "trap", "West", "bury", "whore", "cross", "sheet", "suffer", "tax", "bath", "receive", "sometime", "split", "soda", "talent", "account", "convince", "dessert", "purpose", "report", "weak", "cheer", "move", "support", "research", "tongue", "Valentine", "pill", "snake", "battle", "license", "nut", "health", "natural", "gorgeous", "steak", "vagina", "audience", "knee", "term", "dance", "score", "sue", "whether", "artist", "attack", "bang", "bean", "onto", "attractive", "breath", "cover", "empty", "lonely", "painting", "truly", "army", "avoid", "gang", "land", "nerd", "others", "slap", "when", "appointment", "dick", "lesbian", "outfit", "adventure", "devil", "liar", "nurse", "pot", "responsible", "salesman", "slow", "smile", "wallet", "commit", "example", "fake", "obvious", "pirate", "radio", "chase", "due", "familiar", "homework", "birth", "Canadian", "favorite", "prefer", "rub", "sky", "basically", "coach", "deliver", "laboratory", "address", "lift", "concern", "eleven", "round", "wish", "guard", "contact", "over", "package", "travel", "sixty", "anniversary", "cent", "force", "rest", "spread", "adorable", "ocean", "percent", "shit", "wing", "above", "alcohol", "crash", "insurance", "nuclear", "pathetic", "row", "sight", "trash", "available", "brave", "climb", "earn", "East", "impress", "league", "online", "waste", "within", "writer", "crowd", "flip", "hug", "drag", "funeral", "literally", "lousy", "opinion", "pack", "spit", "van", "behavior", "complain", "future", "interest", "itself", "mirror", "recently", "stripper", "subject", "bright", "design", "general", "kidney", "result", "strike", "corn", "correct", "grandmother", "hug", "nightmare", "ours", "yellow", "rise", "Christian", "doughnut", "original", "position", "quarter", "fool", "annoy", "can", "match", "play", "traffic", "actual", "banana", "conference", "lake", "medical", "medicine", "pray", "shave", "tub", "bake", "option", "South", "creepy", "douchebag", "eventually", "interrupt", "library", "rude", "advertisement", "danger", "fourteen", "master", "math", "propose", "Thursday", "apart", "darling", "gather", "mostly", "support", "bubble", "energy", "heavy", "laser", "manage", "meanwhile", "network", "weapon", "condition", "copy", "female", "park", "quickly", "religion", "snow", "Tuesday", "version", "bomb", "clear", "faith", "innocent", "remove", "survive", "bee", "bride", "cause", "fifth", "several", "basketball", "downtown", "elephant", "freak", "wipe", "arrest", "bored", "bully", "clock", "indeed", "massage", "shape", "skip", "strike", "dry", "remain", "style", "surgery", "toe", "yard", "brilliant", "circle", "duty", "enemy", "focus", "lover", "midnight", "simply", "Spanish", "boom", "describe", "legal", "Mexican", "powerful", "series", "wire", "candle", "diaper", "direction", "divorce", "eighteen", "express", "plastic", "responsibility", "starve", "united", "worker", "AIDS", "hope", "immediately", "nowhere", "separate", "watch", "emotional", "hardly", "pilot", "vampire", "attitude", "balloon", "exact", "frankly", "hip", "pet", "prank", "announcement", "effect", "escape", "golden", "nipple", "rough", "stick", "trade", "twin", "waiter", "architect", "beauty", "mate", "official", "practice", "bug", "crack", "four hundred", "half", "smoke", "contract", "nail", "recognize", "scientist", "set", "shoulder", "successful", "turd", "view", "basement", "degree", "fortune", "hit", "invitation", "nail", "oops", "professional", "search", "swing", "train", "weather", "alarm", "fun", "kitty", "nap", "practice", "precious", "product", "rabbit", "role", "snack", "sucker", "tag", "chef", "chew", "evidence", "fantasy", "operation", "puppy", "rain", "spin", "throat", "present", "reality", "saint", "top", "victim", "waitress", "booze", "condom", "director", "hunt", "menu", "mystery", "quiet", "regret", "technically", "ton", "attract", "aware", "chest", "dentist", "far", "focus", "illegal", "junior", "mouse", "pencil", "sentence", "sixteen", "squeeze", "audition", "lobster", "success", "terrorist", "asleep", "fashion", "glove", "item", "recommend", "tuna", "warehouse", "Italian", "lazy", "tank", "whale", "zone", "honor", "panic", "bachelor", "chain", "creature", "diamond", "however", "image", "parade", "rocket", "solution", "cable", "culture", "garage", "male", "revenge", "shrimp", "taco", "thin", "vote", "aside", "bum", "distract", "DVD", "hehe", "humiliate", "locker", "native", "performance", "policy", "pony", "release", "stone", "woohoo", "advantage", "basket", "breakup", "device", "garden", "patient", "pink", "represent", "thirteen", "treasure", "amount", "fart", "newspaper", "wind", "act", "ashamed", "champion", "light", "per", "scout", "guitar", "mental", "sensitive", "heat", "otherwise", "seventeen", "string", "wind", "downstairs", "impressive", "poop", "property", "skill", "walk", "dammit", "lead", "pancake", "slow", "stranger", "charity", "crap", "freedom", "pour", "stuff", "tradition", "beef", "bite", "bullet", "curious", "disaster", "factory", "forest", "middle", "odd", "provide", "repeat", "section", "subway", "choke", "cowboy", "dirt", "frog", "pumpkin", "swallow", "bacon", "clever", "competition", "heh", "lick", "mission", "pair", "soap", "tail", "tattoo", "activity", "bridge", "detail", "diet", "insult", "theme", "university", "champagne", "charming", "compare", "gut", "map", "napkin", "punch", "apply", "challenge", "collect", "cupcake", "fridge", "imagination", "joke", "pad", "script", "whip", "left", "affair", "benefit", "beyond", "book", "citizen", "fart", "grant", "junk", "magical", "prom", "schedule", "studio", "value", "wise", "Bible", "clue", "suicide", "Wednesday", "friendly", "claim", "complicate", "doubt", "generation", "grave", "require", "stair", "album", "depend", "maid", "moron", "necessary", "oven", "paint", "refer", "scratch", "spill", "stain", "adopt", "brush", "delivery", "disappear", "elementary", "humor", "tear", "trap", "amazing", "Asian", "boot", "connection", "eve", "happiness", "lifetime", "officially", "quality", "agreement", "argument", "carpet", "crisis", "design", "drawer", "further", "incredibly", "loose", "pair", "refuse", "risk", "selfish", "village", "anger", "assure", "bet", "bingo", "bunny", "crawl", "demand", "desperate", "elevator", "goat", "honeymoon", "insist", "million", "murder", "penny", "usual", "beloved", "copy", "festival", "tennis", "threaten", "beginning", "cigar", "dancer", "heck", "low", "material", "Yankee", "chapter", "chat", "homeless", "lap", "major", "mark", "river", "shark", "strength", "accidentally", "ancient", "collection", "exercise", "include", "muffin", "offense", "screen", "tomato", "aha", "cafeteria", "crack", "crappy", "Japanese", "statue", "supply", "versus", "winter", "tool", "asleep", "bend", "commitment", "disturb", "fish", "joy", "presentation", "shove", "tube", "being", "damn", "experiment", "fellow", "German", "jar", "mint", "physical", "punish", "remote", "slut", "twist", "cap", "criminal", "eight hundred", "inspire", "main", "panda", "sink", "smooth", "snap", "thumb", "witness", "abandon", "affect", "beard", "bucket", "county", "gain", "poem", "reporter", "review", "training", "among", "battery", "deny", "electric", "flavor", "forbid", "fudge", "hooray", "obsess", "positive", "rob", "signal", "solid", "website", "aye", "century", "click", "cracker", "dump", "effort", "gate", "hooker", "impression", "inappropriate", "inspector", "statement", "thrill", "wiener", "bond", "challenge", "filthy", "grandfather", "host", "producer", "witch", "appropriate", "coincidence", "furniture", "laugh", "lemon", "load", "mock", "normally", "physics", "sack", "stock", "storm", "wheelchair", "announce", "bagel", "cocktail", "committee", "crew", "defense", "extremely", "file", "haircut", "loss", "mark", "proof", "salt", "score", "spare", "tape", "ex", "fairy", "fort", "grape", "including", "makeup", "nation", "six hundred", "block", "childhood", "cut", "develop", "race", "reference", "shorts", "talk", "therapist", "tip", "tone", "warm", "accent", "avenue", "below", "cloud", "concern", "distance", "doubt", "exhaust", "goal", "roll", "suggestion", "grocery", "belief", "bump", "inch", "jury", "neat", "odds", "reservation", "rush", "spider", "squirrel", "topic", "toss", "uniform", "violence", "wide", "zoo", "boo", "bullshit", "cruel", "dial", "jeans", "pile", "poison", "pure", "romance", "slave", "slowly", "awake", "belly", "booth", "gentle", "opera", "pipe", "poster", "Russian", "sweep", "sword", "ultimate", "debate", "explode", "float", "justice", "kick", "poker", "pride", "psych", "rain", "rare", "rumor", "title", "vision", "wrestle", "column", "connect", "hockey", "hook", "kidnap", "limo", "painful", "path", "permission", "prince", "process", "senator", "smash", "mean", "anytime", "back", "bat", "ceremony", "declare", "helpful", "iron", "picture", "print", "rope", "sail", "sleep", "attend", "doom", "education", "flush", "fry", "melt", "pardon", "passion", "personally", "poop", "popcorn", "pussy", "trick", "zombie", "ankle", "bail", "bowling", "discount", "entertainment", "excuse", "jazz", "kind", "lion", "match", "prayer", "stamp", "betray", "bracelet", "brownie", "creative", "expression", "lock", "negative", "pickle", "puzzle", "recipe", "stab", "toward", "wave", "wherever", "article", "backwards", "central", "hamburger", "mustache", "pudding", "seek", "slide", "tit", "trial", "trophy", "best", "damage", "decent", "easily", "engine", "journey", "ketchup", "lawn", "place", "pretzel", "routine", "sand", "someplace", "stretch", "whack", "bloody", "brand", "British", "budget", "cabin", "determine", "doggie", "entertain", "helmet", "jackass", "media", "motion", "poke", "practically", "rule", "scotch", "source", "spoil", "teenager", "turtle", "visit", "authority", "bleed", "comment", "compete", "confidence", "convention", "defend", "flash", "heal", "hippie", "inside", "past", "phase", "puppet", "respond", "reveal", "sharp", "transfer", "wig", "aisle", "command", "detective", "dolphin", "exit", "expert", "grand", "heat", "industry", "label", "limit", "mature", "pitch", "pump", "racist", "swing", "whom", "comedian", "compliment", "fork", "guarantee", "magician", "murderer", "occur", "pork", "soccer", "wave", "access", "actress", "backup", "bam", "behave", "encourage", "estate", "fellow", "flirt", "handicap", "hose", "incident", "lost", "occasion", "route", "secretary", "classy", "confess", "drown", "duh", "environment", "false", "highly", "legend", "production", "tale", "talented", "threat", "vegetable", "amen", "capital", "cruise", "experience", "forth", "judgment", "nonsense", "offend", "pepper", "phrase", "picnic", "privacy", "self", "shitty", "smell", "attach", "billion", "cheers", "coma", "contain", "earring", "fascinating", "fear", "gal", "ham", "pronounce", "sample", "victory", "behold", "cereal", "concentrate", "depressed", "drama", "explanation", "fate", "hallway", "ninety", "particular", "pleased", "response", "search", "cold", "ground", "liquor", "lower", "miserable", "pin", "purchase", "satisfy", "sexually", "shout", "site", "thee", "tonight", "unit", "complaint", "condo", "cool", "donate", "fur", "gravy", "imaginary", "lemonade", "muscle", "nearly", "opposite", "rent", "sweat", "tiger", "valuable", "alcoholic", "arrest", "counselor", "failure", "farmer", "horn", "human", "lamp", "orange", "peach", "roller", "staff", "text", "thief", "force", "secret", "approve", "campus", "coast", "cure", "dinosaur", "directly", "dry", "haunt", "location", "open", "owner", "patch", "treatment", "trunk", "abortion", "bark", "bartender", "brunch", "casual", "chaos", "chief", "chop", "dust", "intend", "lottery", "musical", "opening", "personality", "publish", "sausage", "spot", "underneath", "unfair", "violent", "wizard", "aboard", "argue", "attempt", "badly", "bush", "fold", "footage", "height", "labor", "nephew", "pass", "punk", "reaction", "shy", "cage", "civil", "colonel", "courage", "cure", "delightful", "emotion", "expose", "fence", "glue", "noodle", "organ", "pole", "refrigerator", "skull", "taxi", "touch", "airline", "background", "bravo", "cheek", "count", "expense", "Irish", "ladder", "mug", "nickname", "pea", "principal", "rack", "session", "soldier", "urine", "virgin", "wander", "bump", "carrot", "chili", "circus", "former", "generous", "gesture", "halfway", "inside", "ninja", "promise", "prostitute", "round", "shelter", "sperm", "technology", "thanks", "agent", "ability", "awfully", "cherry", "discussion", "documentary", "invest", "jet", "knowledge", "load", "minister", "motherfucker", "offensive", "promote", "retire", "singer", "specific", "sudden", "tuck", "sin", "chart", "grader", "ant", "bull", "carefully", "cleaning", "dork", "dummy", "exam", "helicopter", "moustache", "rate", "recall", "reject", "ski", "spy", "stress", "therapy", "travel", "upper", "very", "sponge", "profit", "assignment", "bounce", "branch", "CD", "daily", "dice", "dignity", "entirely", "flatter", "form", "fraud", "offer", "pit", "slow", "status", "stroke", "terrify", "yogurt", "random", "blond", "cube", "alley", "arrange", "coupon", "ditch", "drunk", "edge", "gamble", "gray", "hostage", "inform", "likely", "mask", "nasty", "Olympics", "panties", "priest", "rock", "shine", "shower", "left", "capable", "corporation", "delete", "dental", "engagement", "entitle", "howdy", "ironic", "movement", "piano", "scooter", "silver", "slightly", "sort", "tragic", "underpants", "cabinet", "debt", "global", "hereby", "kindergarten", "mustard", "procedure", "progress", "scale", "select", "rib", "armed", "astronaut", "backyard", "camp", "capture", "cart", "casino", "Catholic", "chop", "cleaner", "desert", "executive", "foreign", "fully", "interview", "lean", "louse", "march", "novel", "one thousand", "orange", "sense", "slice", "species", "weigh", "weirdo", "shadow", "accomplish", "barbecue", "cast", "curse", "define", "double", "equipment", "fever", "front", "gig", "harassment", "intelligent", "karate", "manner", "marijuana", "operate", "post", "quest", "raise", "raisin", "record", "religious", "skinny", "slide", "stinky", "sushi", "thy", "whew", "adult", "allergic", "appearance", "cave", "drunken", "fireman", "frame", "graduate", "hammer", "naughty", "nine hundred", "salary", "signature", "slap", "special", "bowl", "border", "afterwards", "curtain", "defeat", "desire", "firework", "habit", "lane", "late", "management", "mighty", "Muslim", "nanny", "nicely", "polite", "political", "rage", "rainbow", "smelly", "tissue", "triangle", "useless", "visitor", "vodka", "angle", "anonymous", "calendar", "comic", "dibs", "dip", "financial", "flat", "grand", "identity", "light", "living", "register", "shocking", "shopping", "sixth", "spoon", "stop", "strawberry", "target", "teenage", "thick", "Web", "whoops", "counter", "agency", "carnival", "confirm", "corpse", "disagree", "feature", "firm", "fit", "hill", "hopefully", "jam", "lamb", "local", "log", "party", "printer", "sketch", "ski", "steam", "superhero", "terribly", "toothbrush", "top", "council", "deck", "dragon", "election", "hah", "lack", "lawsuit", "lecture", "mysterious", "pleasant", "pregnancy", "professional", "reward", "risk", "rose", "scientific", "specifically", "tune", "acid", "argh", "candidate", "differently", "economy", "eighty", "fee", "flame", "foundation", "joint", "oy", "punishment", "rally", "reception", "request", "resist", "schedule", "seven hundred", "shock", "sore", "suspect", "union", "telephone", "extra", "accuse", "ape", "badge", "barn", "buck", "catch", "divide", "eternity", "hopeless", "intervention", "jam", "knock", "mattress", "millionaire", "observe", "react", "speaker", "spicy", "stadium", "standard", "syndrome", "unhappy", "vow", "will", "youth", "admire", "ambulance", "bonus", "commission", "crotch", "deer", "fountain", "grease", "immigrant", "injury", "intense", "journal", "launch", "medal", "mile", "plant", "rescue", "semester", "tire", "worship", "seed", "dot", "alert", "confident", "currently", "disco", "escape", "film", "found", "frustrate", "improve", "intercourse", "investment", "jelly", "loan", "musical", "protest", "seal", "shock", "silent", "temperature", "testicle", "typical", "unusual", "approach", "April", "attempt", "bow", "castle", "dine", "fiance", "goo", "governor", "lotion", "measure", "modern", "necklace", "nor", "orgasm", "poetry", "reasonable", "run", "scam", "shift", "sneaker", "straighten", "tap", "track", "unlike", "bench", "blowjob", "championship", "Coke", "darkness", "diary", "figure", "guilt", "horny", "intimate", "laughter", "lend", "organization", "outside", "purple", "rice", "seminar", "senior", "shape", "sheep", "sweat", "upside", "vehicle", "seventy", "achieve", "addiction", "architecture", "certain", "demon", "dumbass", "freezer", "guide", "harm", "honk", "install", "mascot", "massive", "metal", "nest", "nineteen", "possibility", "praise", "prescription", "produce", "salmon", "surround", "tragedy", "trip", "troll", "tune", "wagon", "whisper", "yummy", "ban", "bind", "butterfly", "cast", "ceiling", "constantly", "elbow", "electricity", "exchange", "grass", "misunderstanding", "reunion", "rush", "sacred", "sergeant", "solo", "syrup", "therefore", "timing", "where", "whistle", "knight", "awful", "ballet", "blade", "blast", "celebration", "cheesecake", "communicate", "cough", "cripple", "dozen", "fall", "following", "goddamn", "heel", "hold", "lad", "noon", "onion", "pound", "relieve", "saving", "scheme", "silence", "skate", "volunteer", "application", "cinnamon", "coconut", "construction", "curse", "Easter", "explore", "frame", "highway", "imply", "pottery", "soak", "sticker", "survivor", "task", "yoga", "beast", "biology", "carve", "destiny", "detention", "direct", "drill", "firm", "grill", "potential", "rehearsal", "related", "ribbon", "spaghetti", "stress", "stuffing", "surely", "drum", "accountant", "chill", "coaster", "conditioner", "cone", "deaf", "dismiss", "dive", "error", "file", "fly", "hers", "jewelry", "mercy", "naturally", "nickel", "object", "scissors", "secretly", "smack", "supportive", "suspect", "tick", "waffle", "way", "wicked", "wisdom", "woo", "essay", "land", "audition", "butler", "cash", "deadly", "delicate", "document", "emotionally", "establish", "flute", "gorilla", "groom", "horror", "humanity", "maker", "martini", "mud", "needy", "psychic", "reduce", "retarded", "return", "rod", "rubber", "rug", "steer", "symbol", "testify", "thirsty", "traditional", "try", "twin", "succeed", "mob", "basic", "behalf", "booty", "bridesmaid", "chore", "coin", "correct", "crowded", "envelope", "exciting", "greetings", "living", "medication", "necessarily", "organize", "overreact", "pageant", "press", "principle", "rabbi", "reward", "sneeze", "tasty", "tobacco", "volunteer", "whee", "facility", "skirt", "dedicate", "despite", "download", "fetus", "frighten", "grateful", "gravity", "hail", "handle", "hay", "hideous", "hobby", "legally", "lighting", "lobby", "lung", "musician", "petty", "plot", "poison", "presence", "promotion", "radioactive", "relief", "resource", "sailor", "sober", "sofa", "switch", "tavern", "toaster", "annual", "bathe", "bully", "burrito", "catalog", "cheerleader", "combination", "combine", "complete", "conclusion", "cotton", "crab", "damage", "erase", "fairly", "fund", "hog", "hurricane", "July", "mansion", "motorcycle", "off", "pigeon", "politics", "proper", "qualify", "receptionist", "recover", "release", "sarcasm", "sheriff", "shoo", "soy", "suspicious", "sweaty", "technique", "thigh", "toxic", "vest", "crib", "amusement", "average", "backpack", "balance", "beverage", "boxer", "confession", "deeply", "fabric", "herpes", "hobo", "host", "hybrid", "increase", "meth", "prevent", "prisoner", "protection", "queer", "refresh", "request", "shush", "standard", "strap", "tray", "type", "yuck", "vomit", "academy", "anus", "babysitter", "banner", "beneath", "bid", "bladder", "cult", "dealer", "flesh", "fulfill", "heck", "hint", "hump", "identify", "intimidate", "leather", "list", "madness", "merely", "mole", "Nazi", "passionate", "post", "program", "rap", "recent", "satellite", "shampoo", "share", "sink", "sleepy", "smug", "spray", "straw", "supermarket", "tend", "umbrella", "wrestler", "bud", "boner", "riot", "blast", "chess", "chin", "clip", "concept", "core", "deed", "destruction", "display", "DNA", "eighth", "eliminate", "goose", "homosexual", "invention", "marathon", "meaning", "minus", "ouch", "paperwork", "physical", "pilgrim", "pitch", "plug", "psst", "replacement", "review", "surrender", "towards", "vacuum", "ahoy", "board", "crystal", "data", "done", "dryer", "editor", "evolve", "flow", "germ", "grace", "harsh", "intelligence", "jaw", "juicy", "lipstick", "luckily", "male", "medium", "minority", "overnight", "particularly", "payment", "scenario", "scrub", "shiny", "thoughtful", "tops", "tribe", "worthless", "retirement", "righty", "absolute", "breeze", "adjustment", "adoption", "anyhow", "athlete", "burst", "cocoa", "courtesy", "current", "dough", "embrace", "fax", "foolish", "locate", "memo", "mentally", "minor", "monitor", "moo", "quote", "raccoon", "seduce", "sleeve", "strangle", "sunshine", "tickle", "tower", "troop", "use", "volume", "wingman", "wonder", "wuss", "constant", "cue", "acknowledge", "addition", "alarm", "bass", "blessing", "businessman", "classroom", "clinic", "coward", "driveway", "elect", "exchange", "fist", "giant", "indicate", "ingredient", "instance", "insult", "June", "lighten", "limit", "mailman", "massage", "meteor", "mop", "noble", "opposite", "pajamas", "physically", "print", "pro", "regarding", "reverend", "root", "ship", "slutty", "sniff", "spin", "swell", "value", "mushroom", "advanced", "amuse", "ancestor", "attic", "broccoli", "circumstance", "clothing", "decade", "disorder", "dungeon", "even", "evolution", "exception", "function", "geek", "glorious", "gossip", "happily", "ID", "March", "military", "nude", "pan", "paradise", "pervert", "pet", "pinch", "pity", "rotten", "seventh", "similar", "stand", "tractor", "tunnel", "vanilla", "violate", "wreck", "zip", "bedtime", "cape", "cost", "crossword", "designer", "enormous", "extreme", "fragile", "genetic", "length", "lightning", "magic", "magnificent", "mankind", "marshmallow", "May", "meatball", "messy", "mount", "navy", "oppose", "passenger", "playground", "possession", "proposal", "psycho", "rainforest", "rap", "stereotype", "sudden", "sunglasses", "temp", "thou", "urinal", "wah", "Republican", "abuse", "adjust", "approval", "certificate", "coach", "complex", "contestant", "drop", "era", "exhibit", "fame", "gene", "hop", "invisible", "kitten", "laptop", "maniac", "myth", "needle", "patch", "prime", "puke", "rating", "reserve", "root", "round", "sarcastic", "shelf", "skeleton", "submit", "supervisor", "symptom", "virus", "weakness", "barrel", "applause", "average", "boil", "brag", "buckle", "burglar", "caller", "carton", "color", "donkey", "environmental", "equal", "expand", "flu", "forehead", "greet", "illness", "injure", "lifestyle", "objection", "outrage", "pasta", "practical", "recess", "recycle", "rehearse", "roast", "salsa", "shit", "shred", "sloppy", "somewhat", "spray", "suitcase", "take", "tense", "valley", "web", "entrance", "appetite", "association", "attorney", "bikini", "bun", "clubhouse", "confront", "contribute", "critic", "depressing", "dime", "discovery", "effective", "excitement", "expectation", "eyebrow", "fluffy", "headache", "independent", "integrity", "karma", "lizard", "logic", "Lord", "mug", "plain", "poet", "quietly", "relative", "safely", "shrink", "slam", "suit", "surprisingly", "testing", "throughout", "tournament", "twist", "wardrobe", "wrist", "nerve", "deposit", "trail", "collar", "bicycle", "ahem", "alike", "allergy", "atheist", "chamber", "cheeseburger", "conscience", "cuddle", "existence", "gallery", "instinct", "instruction", "loyal", "maintain", "maintenance", "meaningless", "memorize", "missile", "moist", "nag", "paintball", "pattern", "physicist", "pointless", "polish", "reading", "silence", "sitcom", "slight", "softball", "star", "strip", "temple", "torture", "torture", "wax", "wooden", "inner", "elf", "claw", "layer", "refund", "cycle", "mighty", "awareness", "baggage", "closing", "cricket", "dear", "decorate", "definition", "development", "discipline", "double", "ending", "entry", "explosion", "frozen", "guard", "hack", "homemade", "hunter", "interest", "kite", "master", "meantime", "mortgage", "nacho", "oho", "pact", "palace", "password", "pledge", "pond", "power", "resolution", "rig", "sacrifice", "sector", "steam", "sunset", "superior", "surgeon", "tease", "thus", "vegetarian", "videotape", "view", "wide", "workout", "wound", "pyramid", "buffet", "antique", "appeal", "arrangement", "base", "blues", "closure", "dramatic", "generally", "guinea", "hysterical", "ill", "lid", "method", "outrageous", "peel", "rash", "sacrifice", "sophisticated", "stool", "struggle", "subtle", "suspend", "tension", "water", "wealthy", "spare", "cushion", "rebel", "Mafia", "aim", "auto", "bowel", "bug", "bummer", "cater", "CEO", "chubby", "comfort", "conflict", "convert", "dairy", "division", "donation", "edit", "expire", "flash", "flyer", "freaky", "goods", "hamster", "honesty", "ignorant", "inspiration", "masturbate", "melon", "metaphor", "midget", "moral", "motel", "net", "one hundred fifty", "participate", "quarterback", "radiation", "raw", "relate", "resident", "snuggle", "southern", "sticky", "tin", "workplace", "zoom", "acting", "spiritual", "slot", "activate", "air", "briefcase", "cologne", "competitive", "cross", "depression", "early", "extend", "fighter", "gently", "gone", "graduation", "grief", "growth", "infect", "latte", "leak", "logical", "lure", "macaroni", "millennium", "olive", "ordinary", "outer", "oxygen", "patient", "peaceful", "phony", "protocol", "realistic", "recital", "remarkable", "sabotage", "sadly", "senior", "sparkle", "tow", "vulnerable", "waters", "welcome", "wolf", "wreck", "yum", "loaf", "perfume", "bare", "resume", "scar", "bang", "address", "agenda", "assign", "autograph", "await", "billionaire", "bookstore", "buyer", "charade", "collapse", "conditioning", "consequence", "crooked", "cultural", "dating", "December", "deli", "examine", "fry", "grown", "hormone", "hush", "insecure", "intention", "international", "karaoke", "line", "maple", "one thousand one hundred", "pose", "psychiatrist", "repair", "shotgun", "steady", "strategy", "tempt", "vet", "witness", "yikes", "hood", "dawn", "motor", "tent", "tumor", "bathtub", "bitch", "broad", "brutal", "buzz", "campaign", "conduct", "copier", "cranky", "eternal", "foam", "FYI", "google", "greedy", "hiya", "kisser", "lava", "like", "loosen", "Lord", "mailbox", "naive", "November", "particle", "perspective", "plug", "privilege", "resolve", "scarf", "start", "trainer", "unable", "unacceptable", "unfortunate", "update", "volcano", "weed", "worthy", "vault", "permanent", "gag", "addicted", "bah", "balance", "bitter", "boo", "braces", "cashmere", "chimp", "comfort", "communist", "consultant", "criticize", "cutie", "decaf", "demonstrate", "denial", "distraction", "dunk", "edition", "embarrassment", "extension", "Frisbee", "grind", "hairy", "handy", "humble", "juggle", "kit", "lift", "loyalty", "lyric", "maestro", "margarita", "molest", "occasionally", "overwhelm", "pee", "pharmacy", "preserve", "properly", "restroom", "secure", "settlement", "sundae", "Thai", "toad", "trailer", "transform", "trust", "uterus", "violation", "weekly", "blog", "flying", "advise", "altar", "assembly", "bath", "beaver", "blazer", "compromise", "conspiracy", "critical", "devastated", "disappointment", "drawing", "dumpster", "feather", "feature", "furious", "goddess", "housekeeper", "immature", "jackpot", "janitor", "jungle", "Korean", "lasagna", "liberty", "opponent", "penguin", "photographer", "process", "psychic", "puffy", "quote", "racket", "region", "robe", "sea", "sew", "shatter", "sickness", "sidekick", "skank", "spice", "starter", "stir", "structure", "sub", "tampon", "touchdown", "pimp", "brief", "anxiety", "approach", "bay", "beat", "buzz", "chemistry", "desperately", "diabetes", "diarrhea", "dictionary", "doorman", "exploit", "fatty", "fluid", "funky", "gender", "glitter", "harass", "hen", "improv", "interfere", "irony", "married", "novelty", "phew", "phony", "previous", "rehab", "reindeer", "reminder", "rescue", "resent", "rodeo", "round", "severe", "slippery", "smoker", "spooky", "tap", "terror", "transplant", "villain", "well", "whatsoever", "stunt", "dock", "vomit", "accurate", "alter", "ash", "authentic", "basis", "booger", "broken", "chemical", "clap", "closely", "commissioner", "communication", "drive", "Dutch", "engineer", "fax", "federal", "fetch", "fried", "glory", "godfather", "harmless", "hurry", "importantly", "lounge", "major", "milkshake", "mutant", "near", "negotiate", "notebook", "outlet", "paycheck", "prop", "quantum", "range", "sane", "saxophone", "smiley", "solar", "spaceship", "stem", "sting", "storage", "suite", "trade", "unlock", "urban", "wacky", "warrior", "Congress", "adore", "aggressive", "balcony", "beam", "cappuccino", "charm", "cock", "consume", "crank", "duck", "European", "fatso", "fiction", "headquarters", "holder", "income", "ink", "instrument", "jinx", "kindly", "liquid", "lump", "magnet", "mail", "marketing", "originally", "pfft", "piggy", "pineapple", "poo", "porch", "predict", "priceless", "priority", "public", "ramp", "receipt", "rusty", "sandal", "seize", "servant", "shrink", "sidewalk", "significant", "slaughter", "spine", "stubborn", "stumble", "trouble", "upset", "urge", "urgent", "veal", "wink", "notch", "scum", "sewer", "bargain", "driving", "remark", "abort", "active", "aid", "aspirin", "bakery", "biggie", "bold", "bomb", "brat", "cemetery", "cleanse", "colleague", "colon", "confusion", "congratulate", "contact", "context", "creeps", "demand", "dull", "ego", "fog", "fuzzy", "individual", "insensitive", "investigation", "investor", "irresponsible", "knit", "meter", "nominate", "oatmeal", "omelet", "overcome", "plaque", "pursue", "reflect", "relation", "rep", "retard", "seat", "shack", "sincere", "slice", "snap", "spell", "theft", "tricky", "tummy", "undo", "unnecessary", "unpleasant", "vase", "waist", "IQ", "swan", "premiere", "thunder", "bizarre", "cliff", "comb", "curl", "disable", "district", "emerge", "emperor", "endless", "fantasize", "folk", "fortunately", "forward", "gap", "goggles", "handshake", "illusion", "infection", "influence", "liberal", "monitor", "mutual", "nun", "object", "patience", "pickup", "portrait", "possess", "rabies", "redneck", "regard", "relevant", "restrain", "satisfaction", "secondly", "single", "sip", "sponsor", "sprinkles", "steel", "subtitle", "tackle", "tacky", "territory", "tourist", "tremendous", "trim", "tux", "valentine", "vegan", "womb", "institute", "paw", "absence", "acceptable", "apart", "assemble", "assistance", "back", "bluff", "bribe", "bum", "conceive", "diner", "donor", "drill", "expel", "finals", "flow", "garlic", "golf", "groin", "handful", "hike", "importance", "intimacy", "inventor", "mentor", "mummy", "ninth", "occupy", "pamphlet", "pause", "pretentious", "ranch", "reverse", "rooster", "scramble", "scrape", "sensation", "shuttle", "stall", "storm", "summon", "tad", "tan", "tango", "teeny", "tenth", "thrill", "trash", "unexpected", "unicorn", "unknown", "unpack", "useful", "variety", "vicious", "voter", "warning", "writing", "photograph", "jerky", "manure", "razor", "animate", "ATM", "atmosphere", "boundary", "cardboard", "cherish", "civilization", "commander", "conquer", "convenience", "cook", "cram", "deadline", "detector", "dresser", "elegant", "empire", "extraordinary", "grandson", "helpless", "hike", "historical", "homeless", "hottie", "identical", "initiate", "investigate", "itch", "juvenile", "lack", "Latin", "luggage", "mannequin", "motivate", "niece", "passport", "pharmacist", "pitcher", "planetarium", "poisoning", "policeman", "precisely", "profile", "reconsider", "ritual", "robbery", "semen", "sleigh", "spontaneous", "stat", "stunning", "supper", "tab", "toast", "tolerate", "translate", "tuxedo", "wand", "wipe", "dimension", "serial", "toll", "affection", "anchor", "anthropology", "atom", "author", "blend", "blink", "blonde", "brakes", "breed", "brochure", "clay", "coin", "coitus", "correction", "curly", "custody", "direct", "diversity", "dodge", "drain", "dye", "dynamite", "equation", "errand", "exclusive", "halftime", "hearing", "hoop", "hunk", "invade", "invasion", "jealousy", "jingle", "judgmental", "kingdom", "label", "lollipop", "lying", "manipulate", "max", "mechanic", "mode", "narrow", "oath", "odor", "outstanding", "owl", "pope", "pursuit", "rely", "reputation", "restore", "rhyme", "rum", "scoop", "screw", "scrotum", "spoiler", "superstar", "surface", "teens", "tweet", "two thousand", "undercover", "veteran", "viewer", "wax", "whine", "windshield", "bait", "epic", "working", "shovel", "execute", "experiment", "fireplace", "flee", "flood", "heritage", "housewife", "instructor", "leftover", "lieutenant", "lovable", "mayonnaise", "microphone", "nudity", "obligation", "obsession", "patrol", "salty", "schnapps", "setup", "shortly", "sour", "spectacular", "steroid", "stomp", "survival", "sweatshirt", "swell", "theirs", "traitor", "uptight", "wait", "arrival", "autograph", "babysit", "backstage", "bond", "boogie", "busboy", "buttocks", "camel", "cattle", "chalk", "cheater", "clone", "cobra", "contribution", "cough", "devote", "dip", "doodle", "ease", "erection", "harm", "ideal", "lease", "legendary", "absurd", "additional", "advertise", "appetizer", "attraction", "blow", "brick", "canned", "caterer", "center", "chilly", "classmate", "conclude", "consult", "correctly", "cozy", "curiosity", "dorm", "educate", "educational", "enthusiasm", "filth", "fitness", "forgiveness", "foul", "genitals", "goddamned", "guess", "gutter", "heroin", "horribly", "household", "HR", "infinite", "inject", "leaf", "leap", "leap", "link", "loving", "manly", "many", "masterpiece", "obnoxious", "Olympic", "opener", "orientation", "origin", "orphan", "penalty", "residence", "reverse", "shallow", "shipment", "siren", "soothe", "spank", "strict", "strongly", "superpower", "violin", "volunteer", "mass", "scent", "grip", "recorder", "acceptance", "acquaintance", "acquire", "alrighty", "alternative", "anxious", "applaud", "baloney", "capacity", "childish", "circle", "cola", "comment", "detect", "equal", "exotic", "giggle", "gnome", "guidance", "historic", "kiddo", "kosher", "limbo", "marvelous", "minimum", "missy", "mix", "nauseous", "nurse", "observation", "obstacle", "overlook", "philosophy", "racist", "recruit", "sexuality", "shenanigan", "silk", "swipe", "topless", "trauma", "tunnel", "unemployed", "unlikely", "update", "verdict", "virginity", "vitamin", "whistle", "oral", "affairs", "anthem", "ashtray", "assist", "banker", "benefit", "cafe", "caffeine", "chase", "con", "container", "convenient", "cord", "courthouse", "criticism", "curb", "dawg", "delight", "distant", "dizzy", "dressing", "erotic", "eyeball", "facial", "flap", "freshen", "galaxy", "guardian", "highlight", "hoard", "hygiene", "institution", "keeper", "lettuce", "maiden", "membership", "mild", "misery", "mistaken", "mixer", "parlor", "pity", "platter", "pointy", "poorly", "population", "psychology", "risky", "robber", "runner", "runway", "scholarship", "slim", "swimsuit", "temporary", "tequila", "thankful", "triple", "urinate", "valid", "wing", "winning", "convict", "crown", "floss", "stew", "administration", "auction", "billboard", "bouquet", "bourbon", "brainwash", "breakdown", "caramel", "casting", "cease", "cheesy", "chip", "choir", "cocky", "contraction", "creator", "drain", "drip", "espresso", "essence", "exclamation", "experimental", "faculty", "female", "finale", "flaw", "gallon", "gardener", "greasy", "guide", "hammock", "HIV", "hostess", "initiative", "live", "liver", "loan", "mermaid", "military", "mingle", "moose", "mosquito", "mural", "non", "opening", "orchestra", "orphanage", "pimple", "portal", "prick", "psychological", "rebuild", "reckon", "recovery", "scare", "simulate", "simulation", "squad", "technical", "tolerance", "tribute", "vice", "vile", "wilderness", "worm", "trigger", "calf", "scoop", "accomplishment", "admission", "age", "beside", "blame", "cabbage", "cocaine", "colorful", "controversial", "courtroom", "crispy", "delay", "domestic", "eastern", "elsewhere", "exaggerate", "exercise", "feces", "formula", "freeway", "grade", "grownup", "hallelujah", "hostile", "hurtful", "industrial", "intern", "ironically", "irrelevant", "isolate", "knot", "landlord", "leadership", "majesty", "merge", "monologue", "nine thirty", "orgy", "overdue", "postpone", "rag", "registration", "rhythm", "screwdriver", "scuba", "selection", "shell", "sincerely", "sitter", "startle", "stripe", "stroller", "stud", "subscription", "sunrise", "toupee", "unconscious", "underestimate", "unique", "various", "vein", "weep", "whiskey", "witty", "worry", "memorial", "permit"};


    public int WordCount => _wordArray.Length;

    /// <summary>
    /// Generates a string that contains as many words as the length input
    /// </summary>
    /// <param name="length">Number of random words in the sentence</param>
    /// <returns>Random string sentence</returns>
    public string GenerateSentence(int length)
    {
        StringBuilder toReturn = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            int randomIndex = Random.Range(0, _wordArray.Length-1);
            toReturn.Append(_wordArray[randomIndex].Replace("'", " ").Replace(".", ""));
            if (i == length - 1) { break; } // Doesn't insert a space if it's the last word (So that there's not an invisible character at the end of every sentence)
            toReturn.Append(" ");
        }

        return toReturn.ToString();
    }
    /// <summary>
    /// Generate a randomly worded sentence of the provided length
    /// </summary>
    /// <param name="lengthRaw">Length in words of the sentence</param>
    /// <returns>random sentence of length words</returns>
    public string GenerateLowerSentence(int lengthRaw)
    {
        int length = lengthRaw;
        
        if (isPacified)
        {
            length = lengthRaw - 2 >= 1 ? lengthRaw - 2 : 1;
        }
        
        StringBuilder toReturn = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            int randomIndex = Random.Range(0, _wordArray.Length-1);
            toReturn.Append(_wordArray[randomIndex].ToLower());
            if (i == length - 1) { break; } // Doesn't insert a space if it's the last word (So that there's not an invisible character at the end of every sentence)
            toReturn.Append(" ");
        }

        return toReturn.ToString();
    }

    #endregion
    
}
