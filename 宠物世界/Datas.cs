namespace ConfigDatas;

class Data
{
    public Setting Setting = new();
    public SignData SignData = new();
    public AccountData AccountData = new();
    public PetData PetData = new();
    public BackpackData BackpackData = new();
}
public class Setting
{
    bool autoWork;
    bool autoExplore;
    bool autoTrain;
    public bool AutoWork
    {
        get
        {
            return autoWork;
        }
        set
        {
            autoWork = value;
            if (value == true)
            {
                AutoExplore = false;
                AutoTrain = false;
            }
        }
    }
    public bool AutoExplore
    {
        get
        {
            return autoExplore;
        }
        set
        {
            autoExplore = value;
            if (value == true)
            {
                AutoWork = false;
                AutoTrain = false;
            }
        }
    }
    public bool AutoTrain
    {
        get
        {
            return autoTrain;
        }
        set
        {
            autoTrain = value;
            if (value == true)
            {
                AutoWork = false;
                AutoExplore = false;
            }
        }
    }
    public bool AutoFishing;
}

public class SignData
{
    public DateTime SignTime;
}

public class AccountData
{
    public long Money;
}

public class PetData
{
    public int Mood;
    public int Energy;
    public int BloodVolume;
    public int Experience;
    public int Grade;
}

public class BackpackData
{
    public List<(string itemName, int count, int price)> Items = new();
    public (string name, int durable) FishingRod;
}

class Timeout
{
    public static int sleepTime = 120000;
    public static int workSleepTime = 180000;
    public static int fishingSleepTime = 60000;
}