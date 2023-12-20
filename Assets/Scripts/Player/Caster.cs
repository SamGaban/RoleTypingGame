using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TypingComparator;
using System.Text;
using UnityEditor.Experimental.GraphView;
using Random = UnityEngine.Random;

public class Caster : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] Canvas _canvas;
    [SerializeField] TMP_Text _text;
    
    [SerializeField] private Canvas feedBackCanvas;
    [SerializeField] private TMP_Text precisionText;
    [SerializeField] private TMP_Text wpmText;
    [SerializeField] private Animator fbCanvasAnimator;

    [Header("Prefabs")]
    [SerializeField] GameObject FireBall;

    [SerializeField] private GameObject ForceField;

    private GameObject actuallyCasting;

    private bool forceFieldActuallyCasting = false;

    Sentence _sentence;
    int _skillToLaunch = -1;
    private int characterCount = 0;
    

    private void Update() // Typing Cast Logic in Here
    {
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
        switch (_skillToLaunch)
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
    private void EndOfSpellCast()
    {
        decimal charNumberMod = (Convert.ToDecimal(characterCount) / Convert.ToDecimal(_sentence.WordCount()));
        decimal charPerWord = (charNumberMod / 5) * 100;
        precisionText.text = $"{_sentence.TypePrecision()}%";
        wpmText.text = $"{_sentence.WordsPerMinute()}WPM";
        TurnFeedBackCanvasOn();
        Invoke("TurnFeedBackCanvasOff", 2f);
        _player.ToggleCasting();
        switch (_skillToLaunch) // Put all the skills relative to the skills IDs here
        {
            case 1: // FIREBALL
                SkillOne(_sentence.TypePrecision(), _sentence.WordsPerMinute(), charPerWord);
                break;
            case 2: // FORCEFIELD
                actuallyCasting = null;
                forceFieldActuallyCasting = false;
                break;
            case 3:
                Debug.Log("Cast Skill 3");
                break;
            case 4:
                Debug.Log("Cast Skill 4");
                break;
            default:
                break;
        }
    }
    
    /// <summary>
    /// Cancel a began spellcast
    /// </summary>
    private void OnCast() // ON PUSHING THE CAST BUTTON BEFORE SPELL IS COMPLETE
    {
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
    /// </summary>
    /// <param name="numberOfWordsForCast">Numbers of words necessary for the casting of the skill</param>
    /// <param name="skillId">ID of the skill that is aimed to be cast</param>
    private void LaunchSkill(int numberOfWordsForCast, int skillId)
    {
        if (_player.ActualState() == Player.state.Casting) { return; }
        if (_player.ActualEquipped() == Player.equipped.Spear) { return; }

        _sentence = new Sentence(GenerateLowerSentence(numberOfWordsForCast), true);
        _skillToLaunch = skillId;
        _player.ToggleCasting();
    }
    /// <summary>
    /// Key J
    /// </summary>
    private void OnSkill1()
    {
        if (_player.ActualState() == Player.state.Casting) return;
        
        LaunchSkill(4, 1);
    }
    /// <summary>
    /// Key K
    /// </summary>
    private void OnSkill2()
    {
        if (_player.ActualState() == Player.state.Casting) return;

        LaunchSkill(14, 2);


        if (forceFieldActuallyCasting) return;

        forceFieldActuallyCasting = true;

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
    /// <summary>
    /// Key L
    /// </summary>
    private void OnSkill3()
    {
        LaunchSkill(11, 3);
    }
    /// <summary>
    /// Key M
    /// </summary>
    private void OnSkill4()
    {
        LaunchSkill(17, 4);
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
            _canvas.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            feedBackCanvas.transform.localScale = new Vector3(0.0045f, 0.0045f, 0.0045f);
        }
        else
        {
            _canvas.transform.localScale = new Vector3(-0.01f, 0.01f, 0.01f);
            feedBackCanvas.transform.localScale = new Vector3(-0.0045f, 0.0045f, 0.0045f);
        }
    }
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

    public GameObject SkillTwo(int charCount)
    {
        GameObject forceField = Instantiate(ForceField, _player.transform.position, Quaternion.identity);
        Forcefield script = forceField.GetComponent<Forcefield>();
        script.ActivateShield(charCount);
        return forceField;
    }



    #region RandomWordsGenerator related code
    private readonly string[] _wordArray = new string[]
        {
            "I", "you", "the", "a", "to", "it", "not", "that", "and", "of", "do", "have", "what", "we", "in", "get", "this", "my", "me", "go", "oh", "can", "no", "on", "for", "know", "just", "your", "all", "so", "with", "he", "but", "yeah", "well", "think", "here", "want", "out", "about", "good", "come", "up", "say", "now", "at", "one", "hey", "they", "see", "if", "how", "like", "she", "look", "make", "right", "guy", "take", "let", "really", "okay", "her", "uh", "tell", "him", "why", "there", "who", "time", "thing", "from", "will", "like", "when", "as", "because", "some", "our", "yes", "there", "back", "mean", "man", "little", "give", "his", "us", "them", "need", "then", "shall", "or", "talk", "okay", "something", "where", "great", "way", "never", "call", "too", "by", "sorry", "over", "love", "wait", "more", "down", "day", "two", "people", "God", "very", "off", "work", "thank", "big", "try", "dad", "maybe", "feel", "friend", "even", "sure", "find", "kid", "these", "boy", "put", "please", "happen", "much", "stop", "night", "bad", "into", "those", "any", "right", "first", "leave", "year", "hear", "right", "ever", "Mr", "again", "use", "mom", "may", "hi", "life", "nice", "new", "still", "kind", "anything", "only", "baby", "than", "fine", "hello", "keep", "girl", "help", "believe", "woman", "lot", "play", "ask", "start", "home", "nothing", "hmm", "their", "meet", "huh", "show", "around", "guess", "old", "hell", "before", "always", "three", "wow", "listen", "thanks", "minute", "actually", "eat", "place", "live", "away", "after", "bring", "every", "everything", "money", "person", "watch", "other", "remember", "house", "wrong", "kill", "school", "everyone", "run", "late", "care", "car", "move", "ah", "idea", "another", "someone", "today", "turn", "real", "happy", "whole", "week", "job", "fun", "problem", "break", "world", "which", "must", "party", "buy", "through", "together", "room", "family", "stay", "lose", "stuff", "son", "stupid", "name", "everybody", "last", "long", "tonight", "child", "sit", "course", "pretty", "hold", "game", "forget", "else", "own", "five", "second", "doctor", "dollar", "enough", "dog", "funny", "wear", "die", "sir", "hard", "honey", "sound", "sex", "hate", "suppose", "God", "head", "understand", "whoa", "movie", "worry", "cool", "marry", "miss", "pay", "hour", "crazy", "change", "hot", "most", "excuse", "mother", "check", "pick", "word", "same", "yourself", "ready", "seem", "win", "walk", "father", "story", "already", "hope", "part", "open", "lady", "read", "drink", "sleep", "number", "write", "morning", "tomorrow", "next", "phone", "four", "last", "once", "somebody", "probably", "without", "many", "such", "eye", "drive", "wife", "book", "hang", "since", "throw", "name", "dead", "stand", "myself", "aw", "dinner", "anyone", "hand", "each", "anyway", "television", "learn", "shut", "town", "beautiful", "both", "date", "spend", "office", "hit", "yet", "save", "true", "sweet", "until", "food", "while", "send", "high", "anymore", "also", "news", "Christmas", "ten", "ass", "business", "only", "couple", "totally", "door", "gay", "exactly", "parent", "few", "month", "easy", "deal", "ow", "hurt", "nobody", "OK", "perfect", "lie", "free", "young", "weird", "whatever", "brother", "work", "kid", "ago", "end", "other", "ball", "finally", "line", "its", "ha", "important", "fall", "heart", "long", "fat", "class", "shoot", "picture", "sell", "side", "wish", "love", "mind", "hair", "cut", "wedding", "reason", "become", "least", "look", "bite", "fuck", "under", "bed", "paper", "different", "catch", "mine", "six", "set", "face", "speak", "suck", "sometimes", "city", "special", "stick", "question", "dude", "realize", "birthday", "point", "enjoy", "fact", "dance", "soon", "bar", "wonder", "joke", "relationship", "chance", "black", "almost", "fight", "card", "song", "little", "bye", "coffee", "awesome", "sick", "apartment", "sorry", "back", "figure", "pull", "box", "dream", "water", "decide", "store", "bet", "lunch", "face", "anybody", "afraid", "buddy", "cute", "close", "bathroom", "show", "mind", "steal", "full", "company", "front", "ahead", "moment", "case", "date", "though", "body", "Mrs", "pants", "bitch", "promise", "glad", "kiss", "either", "fire", "grow", "build", "table", "ticket", "matter", "teach", "cat", "sister", "girlfriend", "hat", "touch", "terrible", "beer", "damn", "mm-hmm", "club", "amaze", "smell", "gift", "serious", "plan", "street", "team", "order", "cry", "zero", "drop", "act", "alone", "seat", "eh", "between", "eight", "twenty", "foot", "seven", "finish", "gentleman", "hand", "point", "blow", "small", "trouble", "sweetie", "sing", "pass", "beat", "piece", "shoe", "welcome", "god", "white", "kick", "bag", "fire", "early", "excite", "wonderful", "seriously", "country", "mouth", "question", "dear", "quite", "smart", "husband", "shh", "invite", "rest", "yours", "behind", "key", "end", "dress", "red", "laugh", "help", "agree", "machine", "yep", "return", "space", "mad", "truth", "ice", "follow", "outside", "idiot", "rule", "absolutely", "next", "against", "American", "need", "scare", "notice", "chicken", "ride", "music", "join", "next", "good-bye", "ruin", "war", "screw", "light", "along", "poor", "sense", "able", "matter", "secret", "fly", "top", "sure", "fair", "relax", "boyfriend", "plan", "mistake", "luck", "group", "daughter", "college", "president", "far", "sign", "close", "message", "freak", "fault", "quick", "till", "except", "single", "shirt", "choice", "attention", "power", "quit", "answer", "trust", "fifty", "leg", "wake", "human", "star", "student", "air", "chair", "gun", "begin", "death", "tree", "boss", "while", "present", "instead", "lucky", "safe", "cold", "ahh", "explain", "video", "brain", "completely", "voice", "cake", "drink", "trip", "monkey", "hundred", "fix", "entire", "expect", "million", "allow", "forever", "ugh", "hide", "huge", "interesting", "fight", "roll", "clothes", "fast", "grab", "teacher", "crap", "animal", "burn", "tough", "restaurant", "sort", "tooth", "marriage", "proud", "uncle", "push", "butt", "offer", "lord", "unless", "feeling", "suit", "uh-huh", "floor", "cream", "favorite", "naked", "list", "ring", "clean", "apologize", "clear", "Earth", "share", "fill", "cool", "pizza", "ridiculous", "alive", "pretend", "hospital", "sad", "bunch", "half", "police", "fish", "window", "busy", "sign", "call", "pie", "answer", "raise", "somewhere", "sandwich", "thirty", "sale", "choose", "definitely", "swear", "pretty", "boat", "tired", "ho", "upset", "less", "nine", "thousand", "smoke", "embarrass", "band", "bear", "none", "Santa", "strong", "law", "toilet", "jump", "count", "egg", "Saturday", "blue", "horrible", "shit", "favor", "handle", "wall", "art", "cover", "arm", "perhaps", "simple", "bus", "appreciate", "himself", "surprise", "candy", "finger", "worth", "state", "possible", "rich", "short", "king", "knock", "cookie", "penis", "history", "imagine", "third", "blood", "drug", "future", "prove", "surprise", "alright", "captain", "evening", "congratulations", "system", "record", "age", "deserve", "normal", "yesterday", "jerk", "yay", "bird", "nose", "bother", "fan", "letter", "rather", "head", "interested", "owe", "pregnant", "destroy", "bear", "evil", "milk", "sport", "admit", "apparently", "conversation", "obviously", "uh-oh", "jacket", "during", "accept", "dumb", "bit", "consider", "mention", "step", "deal", "hungry", "situation", "lead", "twelve", "fantastic", "hole", "plus", "note", "test", "honest", "character", "holy", "manager", "lesson", "soul", "nope", "dark", "difference", "wine", "road", "calm", "personal", "clown", "computer", "horse", "gosh", "mayor", "inside", "sexy", "dirty", "carry", "professor", "usually", "bottle", "cheese", "summer", "remind", "afternoon", "pee", "across", "angry", "fella", "film", "reach", "shower", "cup", "lawyer", "camera", "pain", "hurry", "desk", "monster", "certainly", "cop", "credit", "prepare", "anywhere", "breakfast", "bastard", "damn", "church", "hire", "decision", "Miss", "pig", "loser", "robot", "deep", "park", "chocolate", "service", "Jew", "tape", "kitchen", "half", "rid", "grandpa", "hero", "shot", "spot", "folks", "yell", "awful", "scene", "trick", "asshole", "issue", "lovely", "visit", "honor", "clean", "second", "chick", "costume", "Friday", "hall", "Ms", "fake", "forgive", "grade", "fifteen", "ought", "cheat", "Chinese", "crap", "create", "comfortable", "hotel", "magazine", "settle", "accident", "boob", "excellent", "neighbor", "train", "board", "pop", "spirit", "cow", "building", "ear", "giant", "ugly", "toy", "cancel", "Internet", "strange", "aunt", "island", "extra", "fit", "rock", "step", "action", "bill", "field", "kiss", "fresh", "level", "cost", "size", "cell", "serve", "shake", "neck", "bowl", "control", "loud", "bedroom", "check", "heaven", "right", "commercial", "draw", "guest", "insane", "fail", "cook", "pleasure", "truck", "science", "careful", "nervous", "although", "gas", "partner", "ew", "scream", "pool", "appear", "clearly", "silly", "feed", "charge", "neither", "wash", "stink", "magic", "plane", "tiny", "prison", "cause", "photo", "public", "button", "flower", "memory", "own", "fast", "base", "involve", "madam", "blame", "tea", "whoo", "bike", "freeze", "sexual", "code", "celebrate", "couch", "inside", "price", "assume", "delicious", "forty", "player", "soup", "waste", "coat", "doll", "security", "warm", "football", "model", "whose", "besides", "middle", "shop", "garbage", "client", "ground", "lame", "project", "dare", "shop", "episode", "glass", "green", "lock", "award", "straight", "unbelievable", "court", "experience", "final", "large", "salad", "belong", "fuck", "station", "area", "vote", "crime", "meat", "romantic", "treat", "forward", "glasses", "lie", "taste", "weight", "mail", "cab", "two hundred", "boring", "information", "total", "channel", "page", "suddenly", "sake", "Thanksgiving", "private", "French", "winner", "past", "pen", "twice", "cousin", "jealous", "mess", "planet", "scary", "universe", "upstairs", "genius", "dangerous", "nuts", "ourselves", "race", "suggest", "turn", "sea", "officer", "meal", "popular", "report", "welcome", "flight", "change", "driver", "wheel", "dump", "five hundred", "member", "yo", "add", "confuse", "form", "government", "Sunday", "order", "continue", "event", "quiet", "low", "color", "Jewish", "nah", "breast", "bank", "parking", "ride", "wild", "turkey", "blah", "thought", "famous", "gold", "pound", "skin", "one hundred", "rat", "switch", "tie", "career", "juice", "like", "protect", "shame", "bottom", "respect", "underwear", "Indian", "closet", "meeting", "sun", "afford", "bald", "engage", "plant", "towel", "mall", "regular", "bone", "dig", "fear", "cancer", "discuss", "fancy", "control", "advice", "center", "apple", "everywhere", "holiday", "cash", "common", "crush", "mess", "peace", "warn", "welcome", "adult", "noise", "rock", "super", "bread", "fucking", "porn", "three hundred", "type", "borrow", "department", "plate", "breathe", "classic", "farm", "disgusting", "tall", "taste", "replace", "somehow", "theater", "discover", "incredible", "plenty", "baseball", "comedy", "enter", "introduce", "whenever", "butter", "possibly", "sweetheart", "jail", "season", "study", "miracle", "complete", "golf", "hook", "perfectly", "stare", "flag", "jeez", "roommate", "ship", "wood", "actor", "break", "prize", "thanks", "corner", "fourth", "sneak", "piss", "pocket", "tip", "alone", "lately", "queen", "tear", "especially", "lay", "pressure", "rip", "assistant", "camp", "judge", "North", "often", "Halloween", "vacation", "impossible", "square", "left", "grandma", "gross", "pal", "smile", "speech", "bust", "gee", "handsome", "safety", "test", "community", "beach", "gym", "toast", "disease", "paint", "themselves", "Monday", "pack", "punch", "customer", "healthy", "invent", "princess", "merry", "someday", "blind", "certain", "solve", "study", "sweater", "terrific", "wet", "attack", "beg", "cigarette", "sugar", "uhh", "weekend", "damn", "sock", "emergency", "mood", "peanut", "stage", "tight", "gum", "moon", "nature", "program", "straight", "angel", "corporate", "damn", "exist", "ghost", "sauce", "stomach", "twenty-five", "block", "herself", "murder", "speed", "Jesus", "mix", "track", "opportunity", "society", "whoo-hoo", "upon", "difficult", "killer", "lip", "market", "goodness", "pillow", "tie", "arrive", "mountain", "slip", "belt", "museum", "oil", "press", "respect", "airplane", "airport", "honestly", "contest", "fabulous", "spell", "whoever", "bell", "friendship", "national", "alien", "dream", "fool", "past", "bless", "cartoon", "near", "swim", "burger", "fruit", "roof", "theory", "according", "guilty", "potato", "dish", "sound", "uncomfortable", "wrap", "cheap", "employee", "interview", "perform", "spring", "text", "tour", "treat", "awkward", "expensive", "unfortunately", "purse", "charge", "ha-ha", "divorce", "ring", "bra", "brown", "duck", "English", "celebrity", "double", "period", "rent", "today", "barely", "bye-bye", "chip", "ignore", "language", "laundry", "social", "dress", "soft", "apology", "concert", "disappoint", "knife", "hilarious", "judge", "blanket", "comic", "leader", "local", "neighborhood", "trap", "West", "bury", "whore", "cross", "sheet", "suffer", "tax", "bath", "receive", "sometime", "split", "soda", "talent", "account", "convince", "dessert", "purpose", "report", "weak", "cheer", "move", "support", "research", "tongue", "Valentine", "pill", "snake", "battle", "license", "nut", "health", "natural", "gorgeous", "steak", "vagina", "audience", "knee", "term", "dance", "score", "sue", "whether", "artist", "attack", "bang", "bean", "onto", "attractive", "breath", "cover", "empty", "lonely", "painting", "truly", "army", "avoid", "gang", "land", "nerd", "others", "slap", "when", "appointment", "dick", "lesbian", "outfit", "adventure", "devil", "liar", "nurse", "pot", "responsible", "salesman", "slow", "smile", "wallet", "commit", "example", "fake", "obvious", "pirate", "radio", "chase", "due", "familiar", "homework", "birth", "Canadian", "favorite", "prefer", "rub", "sky", "basically", "coach", "deliver", "laboratory", "address", "lift", "concern", "eleven", "round", "wish", "guard", "contact", "over", "package", "travel", "sixty", "anniversary", "cent", "force", "rest", "spread", "adorable", "ocean", "percent", "shit", "wing", "above", "alcohol", "crash", "insurance", "nuclear", "pathetic", "row", "sight", "trash", "available", "brave", "climb", "earn", "East", "impress", "league", "online", "waste", "within", "writer", "crowd", "flip", "hug", "drag", "funeral", "literally", "lousy", "opinion", "pack", "spit", "van", "behavior", "complain", "future", "interest", "itself", "mirror", "recently", "stripper", "subject", "bright", "design", "general", "kidney", "result", "strike", "corn", "correct", "grandmother", "hug", "nightmare", "ours", "yellow", "rise", "Christian", "doughnut", "original", "position", "quarter", "fool", "annoy", "can", "match", "play", "traffic", "actual", "banana", "conference", "lake", "medical", "medicine", "pray", "shave", "tub", "bake", "option", "South", "creepy", "douchebag", "eventually", "interrupt", "library", "rude", "advertisement", "danger", "fourteen", "master", "math", "propose", "Thursday", "apart", "darling", "gather", "mostly", "support", "bubble", "energy", "heavy", "laser", "manage", "meanwhile", "network", "weapon", "condition", "copy", "female", "park", "quickly", "religion", "snow", "Tuesday", "version", "bomb", "clear", "faith", "innocent", "remove", "survive", "bee", "bride", "cause", "fifth", "several", "basketball", "downtown", "elephant", "freak", "wipe", "arrest", "bored", "bully", "clock", "indeed", "massage", "shape", "skip", "strike", "dry", "remain", "style", "surgery", "toe", "yard", "brilliant", "circle", "duty", "enemy", "focus", "lover", "midnight", "simply", "Spanish", "boom", "describe", "legal", "Mexican", "powerful", "series", "wire", "candle", "diaper", "direction", "divorce", "eighteen", "express", "plastic", "responsibility", "starve", "united", "worker", "AIDS", "hope", "immediately", "nowhere", "separate", "watch", "emotional", "hardly", "pilot", "vampire", "attitude", "balloon", "exact", "frankly", "hip", "pet", "prank", "announcement", "effect", "escape", "golden", "nipple", "rough", "stick", "trade", "twin", "waiter", "architect", "beauty", "mate", "official", "practice", "t-shirt", "bug", "crack", "four hundred", "half", "smoke", "contract", "nail", "recognize", "scientist", "set", "shoulder", "successful", "turd", "view", "basement", "degree", "fortune", "hit", "invitation", "nail", "oops", "professional", "search", "swing", "train", "weather", "alarm", "fun", "kitty", "nap", "practice", "precious", "product", "rabbit", "role", "snack", "sucker", "tag", "chef", "chew", "evidence", "fantasy", "operation", "puppy", "rain", "spin", "throat", "e-mail", "present", "reality", "saint", "top", "victim", "waitress", "booze", "condom", "director", "hunt", "menu", "mystery", "quiet", "regret", "technically", "ton", "attract", "aware", "chest", "dentist", "far", "focus", "illegal", "junior", "mouse", "pencil", "sentence", "sixteen", "squeeze", "audition", "lobster", "success", "terrorist", "asleep", "fashion", "glove", "item", "recommend", "tuna", "warehouse", "Italian", "lazy", "tank", "whale", "zone", "honor", "panic", "bachelor", "chain", "creature", "diamond", "however", "image", "parade", "rocket", "solution", "cable", "culture", "forty-five", "garage", "male", "revenge", "shrimp", "taco", "thin", "vote", "aside", "bum", "distract", "DVD", "hehe", "humiliate", "locker", "native", "performance", "policy", "pony", "release", "stone", "woohoo", "advantage", "basket", "breakup", "device", "garden", "patient", "pink", "represent", "thirteen", "treasure", "amount", "fart", "newspaper", "wind", "act", "ashamed", "champion", "light", "per", "scout", "guitar", "mental", "sensitive", "twenty-four", "heat", "otherwise", "seventeen", "string", "wind", "downstairs", "impressive", "poop", "property", "skill", "walk", "dammit", "lead", "pancake", "slow", "stranger", "charity", "crap", "freedom", "pour", "stuff", "tradition", "beef", "bite", "bullet", "curious", "disaster", "factory", "forest", "middle", "odd", "provide", "repeat", "section", "subway", "choke", "cowboy", "dirt", "frog", "pumpkin", "swallow", "bacon", "clever", "competition", "e-mail", "heh", "lick", "mission", "pair", "soap", "tail", "tattoo", "activity", "bridge", "detail", "diet", "insult", "theme", "university", "champagne", "charming", "compare", "gut", "map", "napkin", "punch", "apply", "challenge", "collect", "cupcake", "fridge", "imagination", "joke", "pad", "script", "whip", "left", "affair", "benefit", "beyond", "book", "citizen", "fart", "grant", "junk", "magical", "prom", "schedule", "studio", "value", "wise", "Bible", "clue", "suicide", "Wednesday", "friendly", "claim", "complicate", "doubt", "generation", "grave", "require", "stair", "album", "depend", "maid", "moron", "necessary", "oven", "paint", "refer", "scratch", "spill", "stain", "adopt", "brush", "delivery", "disappear", "elementary", "humor", "tear", "trap", "amazing", "Asian", "boot", "connection", "eve", "happiness", "lifetime", "officially", "quality", "agreement", "argument", "carpet", "crisis", "design", "drawer", "further", "incredibly", "loose", "pair", "refuse", "risk", "selfish", "village", "anger", "assure", "bet", "bingo", "bunny", "crawl", "demand", "desperate", "elevator", "goat", "honeymoon", "insist", "million", "murder", "penny", "usual", "beloved", "copy", "festival", "tennis", "threaten", "beginning", "cigar", "dancer", "heck", "low", "material", "Yankee", "chapter", "chat", "homeless", "lap", "major", "mark", "river", "shark", "strength", "accidentally", "ancient", "collection", "exercise", "include", "muffin", "offense", "screen", "tomato", "aha", "cafeteria", "crack", "crappy", "Japanese", "statue", "supply", "versus", "winter", "tool", "asleep", "bend", "commitment", "disturb", "fish", "joy", "presentation", "shove", "tube", "being", "damn", "experiment", "fellow", "German", "jar", "mint", "physical", "punish", "remote", "slut", "twist", "cap", "criminal", "eight hundred", "inspire", "main", "panda", "sink", "smooth", "snap", "thumb", "witness", "abandon", "affect", "beard", "bucket", "county", "gain", "poem", "reporter", "review", "training", "among", "battery", "deny", "electric", "flavor", "forbid", "fudge", "hooray", "obsess", "positive", "rob", "signal", "solid", "website", "aye", "century", "click", "cracker", "dry-cleaning", "dump", "effort", "gate", "hooker", "impression", "inappropriate", "inspector", "statement", "thrill", "wiener", "bond", "challenge", "filthy", "grandfather", "host", "producer", "witch", "appropriate", "coincidence", "furniture", "laugh", "lemon", "load", "mock", "normally", "physics", "sack", "stock", "storm", "wheelchair", "announce", "bagel", "cocktail", "committee", "crew", "defense", "extremely", "file", "haircut", "loss", "mark", "proof", "salt", "score", "spare", "tape", "ex", "fairy", "fort", "grape", "including", "makeup", "nation", "six hundred", "block", "childhood", "cut", "develop", "race", "reference", "shorts", "talk", "therapist", "tip", "tone", "warm", "accent", "avenue", "below", "cloud", "concern", "distance", "doubt", "exhaust", "goal", "roll", "suggestion", "grocery", "belief", "bump", "inch", "jury", "neat", "odds", "reservation", "rush", "spider", "squirrel", "topic", "toss", "uniform", "violence", "wide", "zoo", "boo", "bullshit", "cruel", "dial", "jeans", "pile", "poison", "pure", "romance", "slave", "slowly", "awake", "belly", "booth", "gentle", "opera", "pipe", "poster", "Russian", "sweep", "sword", "ultimate", "debate", "explode", "float", "justice", "kick", "poker", "pride", "psych", "rain", "rare", "rumor", "title", "vision", "wrestle", "column", "connect", "hockey", "hook", "kidnap", "limo", "painful", "path", "permission", "prince", "process", "senator", "smash", "mean", "anytime", "back", "bat", "ceremony", "declare", "helpful", "iron", "picture", "print", "rope", "sail", "sleep", "attend", "doom", "education", "flush", "fry", "melt", "pardon", "passion", "personally", "poop", "popcorn", "pussy", "trick", "zombie", "ankle", "bail", "bowling", "discount", "entertainment", "excuse", "jazz", "kind", "lion", "match", "prayer", "stamp", "betray", "bracelet", "brownie", "creative", "expression", "lock", "negative", "pickle", "puzzle", "recipe", "stab", "toward", "wave", "wherever", "article", "backwards", "central", "hamburger", "mustache", "pudding", "seek", "slide", "tit", "trial", "trophy", "best", "damage", "decent", "easily", "engine", "journey", "ketchup", "lawn", "place", "pretzel", "routine", "sand", "someplace", "stretch", "whack", "bloody", "brand", "British", "budget", "cabin", "determine", "doggie", "entertain", "helmet", "jackass", "media", "motion", "poke", "practically", "rule", "scotch", "source", "spoil", "teenager", "turtle", "visit", "authority", "bleed", "comment", "compete", "confidence", "convention", "defend", "flash", "heal", "hippie", "inside", "past", "phase", "puppet", "respond", "reveal", "sharp", "transfer", "wig", "aisle", "command", "detective", "dolphin", "exit", "expert", "grand", "heat", "industry", "label", "limit", "mature", "pitch", "pump", "racist", "swing", "whom", "comedian", "compliment", "fork", "guarantee", "magician", "murderer", "occur", "pork", "soccer", "wave", "access", "actress", "backup", "bam", "behave", "encourage", "estate", "fellow", "flirt", "handicap", "hose", "incident", "lost", "occasion", "route", "secretary", "classy", "confess", "drown", "duh", "environment", "false", "highly", "legend", "production", "tale", "talented", "threat", "vegetable", "amen", "capital", "cruise", "experience", "forth", "judgment", "nonsense", "offend", "pepper", "phrase", "picnic", "privacy", "self", "shitty", "smell", "attach", "billion", "cheers", "coma", "contain", "earring", "fascinating", "fear", "gal", "ham", "pronounce", "sample", "victory", "behold", "cereal", "concentrate", "depressed", "drama", "explanation", "fate", "hallway", "ninety", "particular", "pleased", "response", "search", "cold", "ground", "liquor", "lower", "miserable", "pin", "purchase", "satisfy", "sexually", "shout", "site", "thee", "tonight", "unit", "complaint", "condo", "cool", "donate", "fur", "good-looking", "gravy", "imaginary", "lemonade", "muscle", "nearly", "opposite", "rent", "sweat", "tiger", "valuable", "alcoholic", "arrest", "counselor", "failure", "farmer", "horn", "human", "lamp", "orange", "peach", "roller", "staff", "text", "thief", "force", "secret", "approve", "campus", "coast", "cure", "dinosaur", "directly", "dry", "haunt", "location", "open", "owner", "patch", "treatment", "trunk", "abortion", "bark", "bartender", "brunch", "casual", "chaos", "chief", "chop", "dust", "intend", "lottery", "musical", "opening", "personality", "publish", "sausage", "spot", "underneath", "unfair", "violent", "wizard", "aboard", "argue", "attempt", "badly", "bush", "fold", "footage", "height", "labor", "nephew", "pass", "punk", "reaction", "shy", "cage", "civil", "colonel", "courage", "cure", "delightful", "emotion", "expose", "fence", "glue", "noodle", "organ", "pole", "refrigerator", "skull", "taxi", "touch", "airline", "background", "bravo", "cheek", "count", "expense", "Irish", "ladder", "mug", "nickname", "pea", "principal", "rack", "session", "soldier", "urine", "virgin", "wander", "bump", "carrot", "chili", "circus", "former", "generous", "gesture", "halfway", "inside", "ninja", "promise", "prostitute", "round", "shelter", "sperm", "technology", "thanks", "agent", "ability", "awfully", "cherry", "discussion", "documentary", "invest", "jet", "knowledge", "load", "minister", "motherfucker", "offensive", "promote", "retire", "singer", "specific", "sudden", "tuck", "sin", "chart", "grader", "ant", "bull", "carefully", "cleaning", "dork", "dummy", "exam", "helicopter", "moustache", "rate", "recall", "reject", "ski", "spy", "stress", "therapy", "travel", "upper", "very", "sponge", "profit", "assignment", "bounce", "branch", "CD", "daily", "dice", "dignity", "entirely", "flatter", "form", "fraud", "offer", "pit", "slow", "status", "stroke", "terrify", "thirty-five", "yogurt", "random", "blond", "cube", "alley", "arrange", "coupon", "ditch", "drunk", "edge", "gamble", "gray", "hostage", "inform", "likely", "mask", "nasty", "Olympics", "panties", "priest", "rock", "shine", "shower", "left", "capable", "corporation", "delete", "dental", "engagement", "entitle", "howdy", "ironic", "movement", "piano", "scooter", "silver", "slightly", "sort", "tragic", "underpants", "cabinet", "debt", "global", "hereby", "kindergarten", "mustard", "old-fashioned", "procedure", "progress", "scale", "select", "rib", "armed", "astronaut", "backyard", "camp", "capture", "cart", "casino", "Catholic", "chop", "cleaner", "desert", "ex-boyfriend", "executive", "foreign", "fully", "interview", "lean", "louse", "march", "novel", "one thousand", "orange", "sense", "slice", "species", "strip-club", "weigh", "weirdo", "shadow", "accomplish", "barbecue", "cast", "curse", "define", "double", "equipment", "fever", "front", "gig", "harassment", "intelligent", "karate", "manner", "marijuana", "operate", "post", "quest", "raise", "raisin", "record", "religious", "skinny", "slide", "stinky", "sushi", "thy", "whew", "adult", "allergic", "appearance", "cave", "drunken", "fireman", "frame", "graduate", "hammer", "naughty", "nine hundred", "salary", "signature", "slap", "special", "bowl", "border", "afterwards", "curtain", "defeat", "desire", "ex-wife", "firework", "habit", "lane", "late", "management", "mighty", "Muslim", "nanny", "nicely", "polite", "political", "rage", "rainbow", "smelly", "tissue", "triangle", "useless", "visitor", "vodka", "angle", "anonymous", "calendar", "comic", "dibs", "dip", "financial", "flat", "grand", "identity", "light", "living", "register", "shocking", "shopping", "sixth", "spoon", "stop", "strawberry", "target", "teenage", "thick", "Web", "whoops", "counter", "agency", "carnival", "confirm", "corpse", "disagree", "feature", "firm", "fit", "hill", "hopefully", "jam", "lamb", "local", "log", "party", "printer", "sketch", "ski", "steam", "superhero", "terribly", "toothbrush", "top", "council", "deck", "dragon", "election", "hah", "lack", "lawsuit", "lecture", "mysterious", "pleasant", "pregnancy", "professional", "reward", "risk", "rose", "scientific", "specifically", "tune", "acid", "argh", "candidate", "differently", "economy", "eighty", "fee", "flame", "foundation", "joint", "oy", "punishment", "rally", "reception", "request", "resist", "schedule", "seven hundred", "shock", "sore", "suspect", "union", "telephone", "extra", "accuse", "ape", "badge", "barn", "buck", "catch", "divide", "eternity", "hopeless", "intervention", "jam", "knock", "mattress", "millionaire", "observe", "react", "speaker", "spicy", "stadium", "standard", "syndrome", "unhappy", "vow", "will", "youth", "admire", "ambulance", "bonus", "commission", "crotch", "deer", "fountain", "grease", "immigrant", "injury", "intense", "journal", "launch", "medal", "mile", "plant", "rescue", "semester", "tire", "worship", "seed", "dot", "a.m.", "alert", "confident", "currently", "disco", "escape", "film", "found", "frustrate", "improve", "intercourse", "investment", "jelly", "loan", "musical", "protest", "seal", "shock", "silent", "temperature", "testicle", "typical", "unusual", "approach", "April", "attempt", "bow", "castle", "dine", "fiance", "goo", "governor", "lotion", "measure", "modern", "necklace", "nor", "orgasm", "poetry", "reasonable", "run", "scam", "shift", "sneaker", "straighten", "tap", "track", "unlike", "bench", "blowjob", "championship", "Coke", "darkness", "diary", "figure", "guilt", "horny", "intimate", "laughter", "lend", "organization", "outside", "purple", "rice", "seminar", "senior", "shape", "sheep", "sweat", "upside", "vehicle", "seventy", "achieve", "addiction", "architecture", "certain", "demon", "dry-cleaner", "dumbass", "freezer", "guide", "harm", "honk", "install", "mascot", "massive", "metal", "nest", "nineteen", "possibility", "praise", "prescription", "produce", "salmon", "surround", "tragedy", "trip", "troll", "tune", "wagon", "whisper", "yummy", "ban", "bind", "brand-new", "butterfly", "cast", "ceiling", "constantly", "elbow", "electricity", "exchange", "grass", "misunderstanding", "reunion", "rush", "sacred", "sergeant", "solo", "syrup", "therefore", "timing", "where", "whistle", "knight", "awful", "ballet", "blade", "blast", "celebration", "cheesecake", "communicate", "cough", "cripple", "dozen", "fall", "following", "goddamn", "heel", "hold", "lad", "noon", "onion", "pound", "relieve", "saving", "scheme", "silence", "skate", "twenty-two", "volunteer", "application", "cinnamon", "coconut", "construction", "curse", "Easter", "ex-girlfriend", "explore", "frame", "highway", "imply", "pottery", "soak", "sticker", "survivor", "task", "yoga", "beast", "biology", "carve", "destiny", "detention", "direct", "drill", "firm", "grill", "potential", "rehearsal", "related", "ribbon", "spaghetti", "stress", "stuffing", "surely", "twenty-three", "drum", "accountant", "chill", "coaster", "conditioner", "cone", "deaf", "dismiss", "dive", "error", "file", "fly", "hers", "jewelry", "mercy", "naturally", "nickel", "object", "scissors", "secretly", "smack", "supportive", "suspect", "tick", "waffle", "way", "wicked", "wisdom", "woo", "essay", "land", "audition", "butler", "cash", "deadly", "delicate", "document", "emotionally", "establish", "flute", "gorilla", "groom", "horror", "humanity", "maker", "martini", "mud", "needy", "psychic", "reduce", "retarded", "return", "rod", "rubber", "rug", "steer", "symbol", "testify", "thirsty", "traditional", "try", "twin", "succeed", "mob", "basic", "behalf", "booty", "bridesmaid", "chore", "coin", "correct", "crowded", "envelope", "exciting", "greetings", "living", "medication", "necessarily", "organize", "overreact", "pageant", "press", "principle", "rabbi", "reward", "sneeze", "tasty", "tobacco", "volunteer", "whee", "facility", "skirt", "dedicate", "despite", "download", "fetus", "frighten", "grateful", "gravity", "hail", "handle", "hay", "hideous", "hobby", "legally", "lighting", "lobby", "lung", "musician", "petty", "plot", "poison", "presence", "promotion", "radioactive", "relief", "resource", "sailor", "sober", "sofa", "switch", "tavern", "toaster", "annual", "bathe", "bully", "burrito", "catalog", "cheerleader", "combination", "combine", "complete", "conclusion", "cotton", "crab", "damage", "erase", "fairly", "fund", "hog", "hurricane", "July", "mansion", "motorcycle", "off", "pigeon", "politics", "proper", "qualify", "receptionist", "recover", "release", "sarcasm", "sheriff", "shoo", "soy", "suspicious", "sweaty", "technique", "thigh", "toxic", "twenty-seven", "uh-uh", "vest", "crib", "amusement", "average", "backpack", "balance", "beverage", "boxer", "confession", "deeply", "fabric", "herpes", "hobo", "host", "hybrid", "increase", "meth", "prevent", "prisoner", "protection", "queer", "refresh", "request", "shush", "standard", "strap", "tray", "type", "yuck", "vomit", "academy", "anus", "babysitter", "banner", "beneath", "bid", "bladder", "cult", "dealer", "flesh", "fulfill", "heck", "hint", "hump", "identify", "intimidate", "leather", "list", "madness", "merely", "mole", "Nazi", "passionate", "post", "program", "rap", "recent", "satellite", "shampoo", "share", "sink", "sleepy", "smug", "spray", "straw", "supermarket", "tend", "twenty-one", "umbrella", "wrestler", "bud", "boner", "riot", "blast", "chess", "chin", "clip", "concept", "core", "deed", "destruction", "display", "DNA", "eighth", "eliminate", "goose", "homosexual", "invention", "marathon", "meaning", "minus", "ouch", "paperwork", "physical", "pilgrim", "pitch", "plug", "psst", "replacement", "review", "surrender", "towards", "vacuum", "ahoy", "board", "crystal", "data", "done", "dryer", "editor", "evolve", "flow", "germ", "grace", "harsh", "intelligence", "jaw", "juicy", "lipstick", "luckily", "male", "medium", "minority", "overnight", "particularly", "payment", "scenario", "scrub", "shiny", "thoughtful", "tops", "tribe", "worthless", "retirement", "righty", "absolute", "breeze", "adjustment", "adoption", "anyhow", "athlete", "burst", "cocoa", "courtesy", "current", "dough", "embrace", "fax", "foolish", "locate", "memo", "mentally", "minor", "monitor", "moo", "quote", "raccoon", "seduce", "sleeve", "strangle", "sunshine", "tickle", "tower", "troop", "use", "volume", "wingman", "wonder", "wuss", "constant", "cue", "acknowledge", "addition", "alarm", "bass", "blessing", "businessman", "classroom", "clinic", "coward", "driveway", "elect", "exchange", "fist", "giant", "grown-up", "indicate", "ingredient", "instance", "insult", "June", "lighten", "limit", "mailman", "massage", "meteor", "mop", "noble", "opposite", "pajamas", "physically", "print", "pro", "regarding", "reverend", "root", "ship", "slutty", "sniff", "spin", "swell", "thirty-one", "value", "mushroom", "advanced", "amuse", "ancestor", "attic", "broccoli", "circumstance", "clothing", "decade", "disorder", "dungeon", "even", "evolution", "exception", "function", "geek", "glorious", "gossip", "happily", "ID", "March", "military", "nude", "pan", "paradise", "pervert", "pet", "pinch", "pity", "rotten", "seventh", "similar", "stand", "tractor", "tunnel", "vanilla", "violate", "wreck", "zip", "bedtime", "cape", "cost", "crossword", "designer", "enormous", "extreme", "fragile", "genetic", "length", "lightning", "magic", "magnificent", "mankind", "marshmallow", "May", "meatball", "messy", "mount", "navy", "oppose", "passenger", "playground", "possession", "proposal", "psycho", "rainforest", "rap", "stereotype", "sudden", "sunglasses", "temp", "thou", "urinal", "wah", "Republican", "abuse", "adjust", "approval", "certificate", "coach", "complex", "contestant", "drop", "era", "exhibit", "fame", "gene", "hop", "invisible", "kitten", "laptop", "maniac", "myth", "needle", "patch", "prime", "puke", "rating", "reserve", "root", "round", "sarcastic", "shelf", "skeleton", "submit", "supervisor", "symptom", "virus", "weakness", "barrel", "applause", "average", "boil", "brag", "buckle", "burglar", "caller", "carton", "color", "donkey", "environmental", "equal", "expand", "flu", "forehead", "greet", "illness", "injure", "lifestyle", "objection", "outrage", "pasta", "practical", "recess", "recycle", "rehearse", "roast", "salsa", "seventy-five", "shit", "shred", "sloppy", "somewhat", "spray", "suitcase", "take", "tense", "valley", "web", "entrance", "appetite", "association", "attorney", "bikini", "bun", "clubhouse", "confront", "contribute", "critic", "depressing", "dime", "discovery", "effective", "excitement", "expectation", "eyebrow", "fluffy", "headache", "independent", "integrity", "karma", "lizard", "logic", "Lord", "mug", "plain", "poet", "quietly", "relative", "safely", "shrink", "slam", "suit", "surprisingly", "testing", "throughout", "tournament", "twist", "wardrobe", "wrist", "nerve", "deposit", "trail", "collar", "bicycle", "ahem", "alike", "allergy", "atheist", "chamber", "cheeseburger", "conscience", "cuddle", "existence", "gallery", "instinct", "instruction", "loyal", "maintain", "maintenance", "meaningless", "memorize", "missile", "moist", "nag", "ninety-five", "paintball", "pattern", "physicist", "pointless", "polish", "reading", "silence", "sitcom", "slight", "softball", "star", "strip", "temple", "torture", "torture", "wax", "wooden", "inner", "elf", "claw", "layer", "refund", "cycle", "mighty", "awareness", "baggage", "closing", "cricket", "dear", "decorate", "definition", "development", "discipline", "double", "ending", "entry", "explosion", "frozen", "guard", "hack", "homemade", "hunter", "interest", "kite", "master", "meantime", "mortgage", "nacho", "oho", "pact", "palace", "password", "pledge", "pond", "power", "resolution", "rig", "sacrifice", "sector", "steam", "sunset", "superior", "surgeon", "tease", "thus", "twenty-eight", "vegetarian", "videotape", "view", "wide", "workout", "wound", "pyramid", "buffet", "antique", "appeal", "arrangement", "base", "blues", "closure", "dramatic", "generally", "guinea", "hysterical", "ill", "lid", "method", "outrageous", "peel", "rash", "sacrifice", "so-called", "sophisticated", "stool", "struggle", "subtle", "suspend", "tension", "water", "wealthy", "spare", "cushion", "rebel", "Mafia", "aim", "auto", "bowel", "bug", "bummer", "cater", "CEO", "chubby", "comfort", "conflict", "convert", "dairy", "division", "donation", "edit", "expire", "flash", "flyer", "freaky", "goods", "hamster", "honesty", "ignorant", "inspiration", "masturbate", "melon", "metaphor", "midget", "moral", "motel", "net", "one hundred fifty", "participate", "quarterback", "radiation", "raw", "relate", "resident", "seventy-two", "snuggle", "southern", "sticky", "three-way", "tin", "workplace", "zoom", "acting", "spiritual", "slot", "activate", "air", "briefcase", "cologne", "competitive", "cross", "depression", "early", "extend", "fighter", "gently", "gone", "graduation", "grief", "growth", "infect", "latte", "leak", "logical", "lure", "macaroni", "millennium", "ninety-nine", "olive", "ordinary", "outer", "oxygen", "patient", "peaceful", "phony", "protocol", "realistic", "recital", "remarkable", "sabotage", "sadly", "senior", "sparkle", "thirty-two", "tow", "vulnerable", "waters", "welcome", "wolf", "wreck", "yum", "loaf", "perfume", "bare", "resume", "scar", "bang", "address", "agenda", "assign", "autograph", "await", "billionaire", "bookstore", "buyer", "charade", "collapse", "conditioning", "consequence", "crooked", "cultural", "dating", "December", "deli", "examine", "fry", "grown", "hormone", "hush", "insecure", "intention", "international", "karaoke", "line", "maple", "one thousand one hundred", "pose", "psychiatrist", "repair", "shotgun", "steady", "strategy", "tempt", "vet", "witness", "yikes", "hood", "dawn", "motor", "tent", "tumor", "bathtub", "bitch", "broad", "brutal", "buzz", "campaign", "conduct", "copier", "cranky", "eternal", "foam", "FYI", "google", "greedy", "hiya", "kisser", "lava", "like", "loosen", "Lord", "mailbox", "naive", "November", "particle", "perspective", "plug", "privilege", "resolve", "scarf", "start", "trainer", "unable", "unacceptable", "unfortunate", "update", "volcano", "weed", "worthy", "vault", "permanent", "gag", "addicted", "bah", "balance", "bitter", "boo", "braces", "cashmere", "chimp", "comfort", "communist", "consultant", "criticize", "cutie", "decaf", "demonstrate", "denial", "distraction", "dunk", "edition", "embarrassment", "extension", "Frisbee", "grind", "hairy", "handy", "humble", "juggle", "kit", "lift", "loyalty", "lyric", "maestro", "margarita", "molest", "occasionally", "overwhelm", "pee", "pharmacy", "preserve", "properly", "restroom", "secure", "settlement", "sundae", "Thai", "toad", "trailer", "transform", "trust", "uterus", "violation", "weekly", "blog", "flying", "advise", "altar", "assembly", "bath", "beaver", "blazer", "compromise", "conspiracy", "critical", "devastated", "disappointment", "drawing", "dumpster", "feather", "feature", "furious", "goddess", "housekeeper", "immature", "jackpot", "janitor", "jungle", "Korean", "lasagna", "liberty", "opponent", "penguin", "photographer", "process", "psychic", "puffy", "quote", "racket", "region", "robe", "sea", "sew", "shatter", "sickness", "sidekick", "skank", "spice", "starter", "stir", "structure", "sub", "tampon", "touchdown", "pimp", "brief", "anxiety", "approach", "bay", "beat", "buzz", "chemistry", "desperately", "diabetes", "diarrhea", "dictionary", "doorman", "exploit", "fatty", "fluid", "forty-two", "funky", "gender", "glitter", "harass", "hen", "improv", "interfere", "irony", "married", "novelty", "phew", "phony", "previous", "rehab", "reindeer", "reminder", "rescue", "resent", "rodeo", "round", "severe", "slippery", "smoker", "spooky", "ta-da", "tap", "terror", "thirty-eight", "transplant", "villain", "well", "whatsoever", "stunt", "dock", "vomit", "a.k.a.", "accurate", "alter", "ash", "authentic", "basis", "booger", "broken", "chemical", "clap", "closely", "commissioner", "communication", "drive", "Dutch", "engineer", "fax", "federal", "fetch", "forty-eight", "fried", "glory", "godfather", "harmless", "hurry", "importantly", "lounge", "major", "milkshake", "mutant", "near", "negotiate", "notebook", "outlet", "paycheck", "prop", "quantum", "range", "sane", "saxophone", "smiley", "solar", "spaceship", "stem", "sting", "storage", "suite", "trade", "unlock", "urban", "wacky", "warrior", "Congress", "adore", "aggressive", "balcony", "beam", "cappuccino", "charm", "cock", "consume", "crank", "duck", "European", "fatso", "fiction", "headquarters", "holder", "income", "ink", "instrument", "jinx", "kindly", "liquid", "lump", "magnet", "mail", "marketing", "originally", "pfft", "piggy", "pineapple", "poo", "porch", "predict", "priceless", "priority", "public", "ramp", "receipt", "rusty", "sandal", "seize", "servant", "shrink", "sidewalk", "significant", "slaughter", "spine", "stubborn", "stumble", "trouble", "upset", "urge", "urgent", "veal", "wink", "notch", "scum", "sewer", "bargain", "driving", "remark", "abort", "active", "aid", "aspirin", "bakery", "biggie", "bold", "bomb", "brat", "cemetery", "cleanse", "colleague", "colon", "confusion", "congratulate", "contact", "context", "creeps", "demand", "dull", "ego", "fog", "fuzzy", "individual", "insensitive", "investigation", "investor", "irresponsible", "knit", "meter", "nominate", "oatmeal", "omelet", "overcome", "plaque", "pursue", "reflect", "relation", "rep", "retard", "seat", "shack", "sincere", "slice", "snap", "spell", "theft", "tricky", "tummy", "twenty-six", "undo", "unnecessary", "unpleasant", "vase", "waist", "IQ", "swan", "premiere", "thunder", "bizarre", "cliff", "comb", "curl", "disable", "district", "emerge", "emperor", "endless", "fantasize", "folk", "fortunately", "forward", "gap", "goggles", "handshake", "illusion", "infection", "influence", "liberal", "monitor", "mutual", "nun", "object", "p.m.", "patience", "pickup", "portrait", "possess", "rabies", "redneck", "regard", "relevant", "restrain", "satisfaction", "secondly", "single", "sip", "sponsor", "sprinkles", "steel", "subtitle", "tackle", "tacky", "territory", "tourist", "tremendous", "trim", "tux", "valentine", "vegan", "womb", "institute", "paw", "absence", "acceptable", "apart", "assemble", "assistance", "back", "bluff", "bribe", "bum", "conceive", "diner", "donor", "drill", "expel", "finals", "flow", "garlic", "golf", "groin", "handful", "hike", "importance", "intimacy", "inventor", "mentor", "mummy", "ninth", "occupy", "pamphlet", "pause", "pretentious", "ranch", "reverse", "rooster", "scramble", "scrape", "sensation", "shuttle", "stall", "storm", "summon", "tad", "tan", "tango", "teeny", "tenth", "thirty-six", "thrill", "trash", "twenty-nine", "unexpected", "unicorn", "unknown", "unpack", "useful", "variety", "vicious", "voter", "warning", "writing", "photograph", "jerky", "manure", "razor", "animate", "ATM", "atmosphere", "boundary", "cardboard", "cherish", "civilization", "commander", "conquer", "convenience", "cook", "cram", "deadline", "detector", "dresser", "elegant", "empire", "extraordinary", "grandson", "helpless", "hike", "historical", "homeless", "hottie", "identical", "initiate", "investigate", "itch", "Jell-O", "juvenile", "lack", "Latin", "luggage", "mannequin", "motivate", "niece", "passport", "pharmacist", "pitcher", "planetarium", "poisoning", "policeman", "precisely", "profile", "reconsider", "ritual", "robbery", "self-esteem", "semen", "six-pack", "sleigh", "spontaneous", "stat", "stunning", "supper", "tab", "thirty-four", "thirty-seven", "toast", "tolerate", "translate", "tuxedo", "wand", "wipe", "dimension", "serial", "toll", "affection", "African-American", "anchor", "anthropology", "atom", "author", "blend", "blink", "blonde", "brakes", "breed", "brochure", "clay", "coin", "coitus", "correction", "curly", "custody", "direct", "diversity", "dodge", "drain", "dye", "dynamite", "equation", "errand", "exclusive", "halftime", "hearing", "hoop", "hunk", "invade", "invasion", "jealousy", "jingle", "judgmental", "kingdom", "label", "lollipop", "lying", "manipulate", "max", "mechanic", "mode", "narrow", "oath", "odor", "outstanding", "owl", "pope", "pursuit", "rely", "reputation", "restore", "rhyme", "rum", "scoop", "screw", "scrotum", "spoiler", "superstar", "surface", "teens", "tweet", "two thousand", "undercover", "veteran", "viewer", "wax", "whine", "windshield", "bait", "epic", "working", "shovel", "execute", "experiment", "fireplace", "flee", "flood", "half-hour", "heritage", "housewife", "instructor", "leftover", "lieutenant", "lovable", "mayonnaise", "microphone", "nudity", "obligation", "obsession", "patrol", "salty", "schnapps", "setup", "shortly", "sour", "spectacular", "steroid", "stomp", "survival", "sweatshirt", "swell", "theirs", "traitor", "uptight", "wait", "arrival", "autograph", "babysit", "backstage", "bond", "boogie", "busboy", "buttocks", "camel", "cattle", "chalk", "cheater", "clone", "cobra", "contribution", "cough", "devote", "dip", "doodle", "ease", "erection", "harm", "ideal", "lease", "legendary", "absurd", "additional", "advertise", "appetizer", "attraction", "blow", "brick", "canned", "caterer", "center", "chilly", "classmate", "conclude", "consult", "correctly", "cozy", "curiosity", "dorm", "educate", "educational", "enthusiasm", "filth", "fitness", "forgiveness", "foul", "genitals", "goddamned", "guess", "gutter", "heroin", "horribly", "household", "HR", "infinite", "inject", "leaf", "leap", "leap", "link", "loving", "manly", "many", "masterpiece", "obnoxious", "Olympic", "opener", "orientation", "origin", "orphan", "penalty", "residence", "reverse", "shallow", "shipment", "siren", "soothe", "spank", "strict", "strongly", "superpower", "violin", "volunteer", "wake-up", "mass", "scent", "grip", "recorder", "acceptance", "acquaintance", "acquire", "alrighty", "alternative", "anxious", "applaud", "baloney", "capacity", "childish", "circle", "cola", "comment", "detect", "equal", "exotic", "giggle", "gnome", "guidance", "historic", "kiddo", "kosher", "limbo", "marvelous", "minimum", "missy", "mix", "nauseous", "nurse", "observation", "obstacle", "overlook", "philosophy", "racist", "recruit", "sexuality", "shenanigan", "silk", "swipe", "topless", "trauma", "tunnel", "unemployed", "unlikely", "update", "verdict", "virginity", "vitamin", "whistle", "x-ray", "oral", "affairs", "anthem", "ashtray", "assist", "banker", "benefit", "cafe", "caffeine", "chase", "con", "container", "convenient", "cord", "courthouse", "criticism", "curb", "dawg", "delight", "distant", "dizzy", "dressing", "erotic", "eyeball", "facial", "flap", "freshen", "galaxy", "guardian", "highlight", "hoard", "hygiene", "institution", "keeper", "lettuce", "maiden", "membership", "mild", "misery", "mistaken", "mixer", "parlor", "pity", "platter", "pointy", "poorly", "population", "psychology", "risky", "robber", "runner", "runway", "scholarship", "slim", "swimsuit", "temporary", "tequila", "thankful", "triple", "urinate", "valid", "wing", "winning", "convict", "crown", "floss", "stew", "administration", "auction", "billboard", "bouquet", "bourbon", "brainwash", "breakdown", "caramel", "casting", "cease", "cheesy", "chip", "choir", "cocky", "contraction", "creator", "drain", "drip", "espresso", "essence", "exclamation", "experimental", "faculty", "female", "finale", "flaw", "gallon", "gardener", "greasy", "guide", "hammock", "HIV", "hostess", "initiative", "live", "liver", "loan", "mermaid", "military", "mingle", "moose", "mosquito", "mural", "no-one", "non", "opening", "orchestra", "orphanage", "pimple", "portal", "prick", "psychological", "rebuild", "reckon", "recovery", "rip-off", "scare", "simulate", "simulation", "squad", "technical", "tolerance", "tribute", "vice", "vile", "wilderness", "worm", "trigger", "calf", "scoop", "accomplishment", "admission", "age", "beside", "blame", "cabbage", "cocaine", "colorful", "controversial", "courtroom", "crispy", "delay", "domestic", "eastern", "elsewhere", "ex-husband", "exaggerate", "exercise", "father-in-law", "feces", "formula", "freeway", "grade", "grownup", "hallelujah", "hostile", "hurtful", "industrial", "intern", "ironically", "irrelevant", "isolate", "knot", "landlord", "leadership", "majesty", "merge", "monologue", "nine thirty", "orgy", "overdue", "postpone", "rag", "registration", "rhythm", "screwdriver", "scuba", "selection", "shell", "sincerely", "sitter", "startle", "stripe", "stroller", "stud", "subscription", "sunrise", "toupee", "unconscious", "underestimate", "unique", "various", "vein", "weep", "whiskey", "witty", "worry", "memorial", "permit"
        };


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
            int randomIndex = Random.Range(0, 5001);
            toReturn.Append(_wordArray[randomIndex].Replace("'", " ").Replace(".", ""));
            if (i == length - 1) { break; } // Doesn't insert a space if it's the last word (So that there's not an invisible character at the end of every sentence)
            toReturn.Append(" ");
        }

        return toReturn.ToString();
    }
    /// <summary>
    /// Generate a randomly worded sentence of the provided length
    /// </summary>
    /// <param name="length">Length in words of the sentence</param>
    /// <returns>random sentence of length words</returns>
    public string GenerateLowerSentence(int length)
    {
        StringBuilder toReturn = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            int randomIndex = Random.Range(0, 5001);
            toReturn.Append(_wordArray[randomIndex].ToLower());
            if (i == length - 1) { break; } // Doesn't insert a space if it's the last word (So that there's not an invisible character at the end of every sentence)
            toReturn.Append(" ");
        }

        return toReturn.ToString();
    }

    #endregion
}
