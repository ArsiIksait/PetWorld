using ConfigDatas;
using ConfigManage;
using IlyfairyLib.GoCqHttpSdk;
using IlyfairyLib.GoCqHttpSdk.Api;

class WorkTask
{
    const int groupId = 522126928;

    public static void AutoWork(Session session, Data config)
    {
        Thread autoWork = new(async () => {
            DateTime startTime = DateTime.Now;
            while (config.Setting.AutoWork)
            {
                if (config.PetData.Energy >= 10)
                {
                    await session.SendGroupMessageAsync(groupId, "打工");
                    config.PetData.Energy -= 10;
                    Thread.Sleep(ConfigDatas.Timeout.sleepTime);
                }
                else
                {
                    if (config.AccountData.Money >= 1988)
                    {
                        DiaLog.Log("精力不足，正在补充");
                        await session.SendGroupMessageAsync(groupId, "购买中精力药*1");
                        await session.SendGroupMessageAsync(groupId, "使用中精力药*1");
                        config.AccountData.Money -= 1988;
                        config.PetData.Energy += 10;
                    }
                    else
                    {
                        //await session.SendGroupMessageAsync(groupId, "精力不足，无法购买中精力药补充精力打工，休息2分钟");
                        DiaLog.Log("精力不足，无法购买中精力药补充精力打工，休息2分钟");
                        Thread.Sleep(ConfigDatas.Timeout.sleepTime);
                    }
                }
            }
            //await session.SendGroupMessageAsync(groupId, "自动打工已关闭");
            DiaLog.Log($"自动打工线程结束 线程开始时间: [{startTime}]");
        });
        autoWork.Start();
        //await session.SendGroupMessageAsync(groupId, "自动打工已开启");
        DiaLog.Log("自动打工已开启");
    }
    public static void AutoExplore(Session session, Data config)
    {
        Thread autoExplore = new(async () => {
            DateTime startTime = DateTime.Now;
            while (config.Setting.AutoExplore)
            {
                if (config.PetData.Energy < 10)
                {
                    if (config.AccountData.Money >= 1988)
                    {
                        DiaLog.Log("精力不足，正在补充");
                        await session.SendGroupMessageAsync(groupId, "购买中精力药");
                        await session.SendGroupMessageAsync(groupId, "使用中精力药");
                        config.AccountData.Money -= 1988;
                        config.PetData.Energy += 10;
                    }
                    else
                    {
                        //await session.SendGroupMessageAsync(groupId, "精力不足，无法购买中精力药补充精力探险，休息2分钟");
                        DiaLog.Log("精力不足，无法购买中精力药补充精力探险，休息2分钟");
                        Thread.Sleep(ConfigDatas.Timeout.sleepTime);
                    }
                }

                if (config.PetData.Mood >= 10)
                {
                    await session.SendGroupMessageAsync(groupId, "探险");
                    config.PetData.Energy -= 10;
                    Thread.Sleep(ConfigDatas.Timeout.sleepTime);
                }
                else
                {
                    DiaLog.Log("心情不足，正在补充");
                    await session.SendGroupMessageAsync(groupId, "玩耍");
                    config.PetData.Mood += 10;
                    config.PetData.Energy -= 10;
                    Thread.Sleep(ConfigDatas.Timeout.sleepTime);
                }
            }
            //await session.SendGroupMessageAsync(groupId, "自动探险已关闭");
            DiaLog.Log($"自动探险线程结束 线程开始时间: [{startTime}]");
        });
        autoExplore.Start();
        //await session.SendGroupMessageAsync(groupId, "自动探险已开启");
        DiaLog.Log("自动探险已开启");
    }
    public static void AutoTrain(Session session, Data config)
    {
        Thread autoTrain = new(async () => {
            DateTime startTime = DateTime.Now;
            int trainCount = 0;
            while (config.Setting.AutoTrain)
            {
                if (config.PetData.Energy >= 10)
                {
                    await session.SendGroupMessageAsync(groupId, "修炼");
                    trainCount++;

                    if (trainCount >= 10)
                    {
                        await session.SendGroupMessageAsync(groupId, "宠物升级");
                        trainCount = 0;
                    }

                    config.PetData.Energy -= 10;
                    Thread.Sleep(ConfigDatas.Timeout.sleepTime);
                }
                else
                {
                    if (config.AccountData.Money >= 1988)
                    {
                        DiaLog.Log("精力不足，正在补充");
                        await session.SendGroupMessageAsync(groupId, "购买中精力药*1");
                        await session.SendGroupMessageAsync(groupId, "使用中精力药*1");
                        config.AccountData.Money -= 1988;
                        config.PetData.Energy += 10;
                    }
                    else
                    {
                        //await session.SendGroupMessageAsync(groupId, "精力不足，无法购买中精力药补充精力修炼，休息2分钟");
                        DiaLog.Log("精力不足，无法购买中精力药补充精力修炼，休息2分钟");
                        Thread.Sleep(ConfigDatas.Timeout.sleepTime);
                    }
                }
            }
            //await session.SendGroupMessageAsync(groupId, "自动修炼已关闭");
            DiaLog.Log($"自动修炼线程结束 线程开始时间: [{startTime}]");
        });
        autoTrain.Start();
        //await session.SendGroupMessageAsync(groupId, "自动修炼已开启");
        DiaLog.Log("自动修炼已开启");
    }
    public static void AutoFishing(Session session, Data config)
    {
        Thread autoFishing = new(async () => {
            DateTime startTime = DateTime.Now;
            while (config.Setting.AutoFishing)
            {
                if (config.BackpackData.FishingRod.durable == 0)
                    config.BackpackData.FishingRod.name = string.Empty;

                if (config.BackpackData.FishingRod.name == string.Empty || config.BackpackData.FishingRod.durable == 0)
                {
                    if (config.AccountData.Money >= 100000)
                    {
                        DiaLog.Log("未装备钓竿 购买青环木钓竿*1");
                        await session.SendGroupMessageAsync(groupId, "购买青环木钓竿");
                        await session.SendGroupMessageAsync(groupId, "装备青环木钓竿");
                        config.BackpackData.FishingRod.name = "青环木钓竿";
                        config.BackpackData.FishingRod.durable = 30;
                    }
                    else if (config.AccountData.Money >= 50000)
                    {
                        DiaLog.Log("未装备钓竿 购买白蜡木钓竿*1");
                        await session.SendGroupMessageAsync(groupId, "购买白蜡木钓竿");
                        await session.SendGroupMessageAsync(groupId, "装备白蜡木钓竿");
                        config.BackpackData.FishingRod.name = "白蜡木钓竿";
                        config.BackpackData.FishingRod.durable = 30;
                    }
                    else
                    {
                        DiaLog.Log("没有足够的钱购买钓竿 休息10分钟");
                        //await session.SendGroupMessageAsync(groupId, "没有足够的钱购买钓竿 休息10分钟");
                        Thread.Sleep(ConfigDatas.Timeout.workSleepTime);
                    }
                }
                else
                {
                    await session.SendGroupMessageAsync(groupId, "钓鱼");
                    Thread.Sleep(ConfigDatas.Timeout.fishingSleepTime);
                }
            }
            //await session.SendGroupMessageAsync(groupId, "自动钓鱼已关闭");
            DiaLog.Log($"自动钓鱼线程结束 线程开始时间: [{startTime}]");
        });
        autoFishing.Start();
        //await session.SendGroupMessageAsync(groupId, "自动钓鱼已开启");
        DiaLog.Log("自动钓鱼已开启");
    }
    public static void AutoEnergy(Data config)
    {
        Thread autoEnergy = new(() =>
        {
            while (true)
{
                if (config.PetData.Energy > 100)
                    config.PetData.Energy = 100;

                while (config.PetData.Energy < 100)
                {
                    Thread.Sleep(10000);
                    config.PetData.Energy++;
                }
                Thread.Sleep(1000);
            }
        });
        autoEnergy.Start();
    }
    public static void AutoSave(Data config)
    {
        Thread autoSave = new(() =>
        {
            while(true)
            {
                Thread.Sleep(300000);
                Config.SaveConfig(config);
                DiaLog.Info("配置已自动保存");
            }
        });
        autoSave.Start();
    }
}