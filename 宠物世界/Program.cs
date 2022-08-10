using ConfigDatas;
using ConfigManage;
using IlyfairyLib.GoCqHttpSdk;
using IlyfairyLib.GoCqHttpSdk.Api;
using IlyfairyLib.GoCqHttpSdk.Models.Chunks;

try
{
    Session session = new("ws://127.0.0.1:6700", "http://127.0.0.1:5700");
    //const int groupId = 522126928;
    const int groupId = 295322097;
    const uint master = 3251242073;
    const uint botQQ = 3409297243;

#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行

    Data? config;

    config = Config.ReadConfig();

    if (config == null)
    {
        Console.Write("配置文件读取失败，请删除配置文件或者打开文本编辑器尝试修复! 请按任意键退出程序. . .");
        Console.ReadKey();
        Environment.Exit(-1);
    }

    //当WebSocket的连接状态发生改变时
    session.UseWebSocketConnect(async (isConnect) =>
    {
        if (isConnect)
        {
            DiaLog.Info("WebSocket已连接");
            DiaLog.Info("\n==============================\n宠物世界打工仔v1.0.0 Made By ArsiIksait\nProvide By ilyfairy's C# GoCqHttp Sdk&GoCqHttp QQ Robot\n发送 #帮助 命令查看帮助!\n==============================");
        }
        else
        {
            DiaLog.Error("ws连接断开, 正在尝试重连");
        }
        return true;
    });

    //生命周期    cqhhtp首次连接 我们会收到这个消息
    session.UseLifecycle(async (v) =>
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Robot连接成功");
        return true;
    });

    //程序退出事件
    AppDomain.CurrentDomain.ProcessExit += (sender, e) => {
        DiaLog.Log("程序退出 保存配置文件");
        Config.SaveConfig(config);
    };

    //接收所有群消息
    session.UseGroupMessage(async v =>
    {
        if (v.GroupId == groupId)
        {
            if (v.QQ == botQQ)
            {

            }
            else if (v.QQ == master)
            {
                switch (v.Message.Text)
                {
                    case "#打工":
                        try
                        {
                            if (config.Setting.AutoWork)
                            {
                                config.Setting.AutoWork = false;
                                DiaLog.Log("自动打工已关闭");
                            }
                            else
                            {
                                config.Setting.AutoWork = true;
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
                                                await session.SendGroupMessageAsync(groupId, "精力不足，无法购买中精力药补充精力打工，休息30分钟");
                                                DiaLog.Log("精力不足，无法购买中精力药补充精力打工，休息30分钟");
                                                Thread.Sleep(ConfigDatas.Timeout.workSleepTime);
                                            }
                                        }
                                    }
                                    await session.SendGroupMessageAsync(groupId, "自动打工已关闭");
                                    DiaLog.Log($"自动打工线程结束 线程开始时间: [{startTime}]");
                                });
                                autoWork.Start();
                                await session.SendGroupMessageAsync(groupId, "自动打工已开启");
                                DiaLog.Log("自动打工已开启");
                            }
                        }
                        catch (Exception ex)
                        {
                            DiaLog.Error($"自动打工时出错: {ex}");
                        }
                        break;
                    case "#探险":
                        try
                        {
                            if (config.Setting.AutoExplore)
                            {
                                config.Setting.AutoExplore = false;
                                DiaLog.Log("自动探险已关闭");
                            }
                            else
                            {
                                config.Setting.AutoExplore = true;
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
                                                await session.SendGroupMessageAsync(groupId, "精力不足，无法购买中精力药补充精力探险，休息30分钟");
                                                DiaLog.Log("精力不足，无法购买中精力药补充精力探险，休息30分钟");
                                                Thread.Sleep(ConfigDatas.Timeout.workSleepTime);
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
                                    await session.SendGroupMessageAsync(groupId, "自动探险已关闭");
                                    DiaLog.Log($"自动探险线程结束 线程开始时间: [{startTime}]");
                                });
                                autoExplore.Start();
                                await session.SendGroupMessageAsync(groupId, "自动探险已开启");
                                DiaLog.Log("自动探险已开启");
                            }
                        }
                        catch (Exception ex)
                        {
                            DiaLog.Error($"自动探险时出错: {ex}");
                        }
                        break;
                    case "#修炼":
                        try
                        {
                            if (config.Setting.AutoTrain)
                            {
                                config.Setting.AutoTrain = false;
                                DiaLog.Log("自动修炼已关闭");
                            }
                            else
                            {
                                config.Setting.AutoTrain = true;
                                Thread autoTrain = new(async () => {
                                    DateTime startTime = DateTime.Now;
                                    while (config.Setting.AutoTrain)
                                    {
                                        if (config.PetData.Energy >= 10)
                                        {
                                            await session.SendGroupMessageAsync(groupId, "修炼");
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
                                                await session.SendGroupMessageAsync(groupId, "精力不足，无法购买中精力药补充精力修炼，休息30分钟");
                                                DiaLog.Log("精力不足，无法购买中精力药补充精力修炼，休息30分钟");
                                                Thread.Sleep(ConfigDatas.Timeout.workSleepTime);
                                            }
                                        }
                                    }
                                    await session.SendGroupMessageAsync(groupId, "自动修炼已关闭");
                                    DiaLog.Log($"自动修炼线程结束 线程开始时间: [{startTime}]");
                                });
                                autoTrain.Start();
                                await session.SendGroupMessageAsync(groupId, "自动修炼已开启");
                                DiaLog.Log("自动修炼已开启");
                            }
                        }
                        catch (Exception ex)
                        {
                            DiaLog.Error($"自动修炼时出错: {ex}");
                        }
                        break;
                    case "#钓鱼":
                        try
                        {
                            if (config.Setting.AutoFishing)
                            {
                                config.Setting.AutoFishing = false;
                                DiaLog.Log("自动钓鱼已关闭");
                            }
                            else
                            {
                                config.Setting.AutoFishing = true;
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
                                                DiaLog.Log("没有足够的钱购买钓竿 休息30分钟");
                                                await session.SendGroupMessageAsync(groupId, "没有足够的钱购买钓竿 休息30分钟");
                                                Thread.Sleep(ConfigDatas.Timeout.workSleepTime);
                                            }
                                        }
                                        else
                                        {
                                            await session.SendGroupMessageAsync(groupId, "钓鱼");
                                            Thread.Sleep(ConfigDatas.Timeout.fishingSleepTime);
                                        }
                                    }
                                    await session.SendGroupMessageAsync(groupId, "自动钓鱼已关闭");
                                    DiaLog.Log($"自动钓鱼线程结束 线程开始时间: [{startTime}]");
                                });
                                autoFishing.Start();
                                await session.SendGroupMessageAsync(groupId, "自动钓鱼已开启");
                                DiaLog.Log("自动钓鱼已开启");
                            }
                        }
                        catch (Exception ex)
                        {
                            DiaLog.Error($"自动钓鱼时出错: {ex}");
                        }
                        break;
                    case "#收集":
                        try
                        {
                            if (config.AccountData.Money >= 12000)
                            {
                                int buyCount = 0;

                                for (var i = config.AccountData.Money; i >= 12000; i -= 12000)
                                {
                                    buyCount++;
                                    config.AccountData.Money -= 12000;
                                }

                                await session.SendGroupMessageAsync(groupId, $"购买一万积分卡*{buyCount}");
                                await session.SendGroupMessageAsync(groupId, "转让物品" + $"一万积分卡*{buyCount}-" + new AtChunk(master));
                                DiaLog.Log($"收集完毕，花费了{buyCount * 12000}积分购买了一万积分卡*{buyCount}");
                            }
                            else
                            {
                                double progress = config.AccountData.Money / 12000.00d * 100;
                                await session.SendGroupMessageAsync(groupId, $"积分不足 ({config.AccountData.Money}/12000) [{Math.Round(progress, 2)}%]");
                                DiaLog.Log("积分不足，无法收集");
                            }
                        }
                        catch (Exception ex)
                        {
                            DiaLog.Error($"收集时出错: {ex}");
                        }
                        break;
                    case "#状态":
                        await session.SendGroupMessageAsync(groupId, @$"当前状态:
自动打工: {config.Setting.AutoWork}
自动探险: {config.Setting.AutoExplore}
自动修炼: {config.Setting.AutoTrain}
自动钓鱼: {config.Setting.AutoFishing}
上次签到时间: {config.SignData.SignTime}
积分: {config.AccountData.Money}
宠物心情: {config.PetData.Mood}
宠物精力: {config.PetData.Energy}
宠物血量: {config.PetData.BloodVolume}
宠物经验: {config.PetData.Experience}
宠物等级: {config.PetData.Grade}
装备钓鱼竿: [{config.BackpackData.FishingRod.name ?? "未装备"}] 剩余使用次数: {config.BackpackData.FishingRod.durable}
");
                        break;
                    case "#帮助":
                        await session.SendGroupMessageAsync(groupId, @"命令   [参数]  作用
#打工 自动打工
#探险 自动探险
#修炼 自动修炼
#钓鱼 自动钓鱼
#收集 收集积分
#状态 查看状态
#执行 [命令]  执行命令
#设置 [变量名 值] 设置变量
#帮助 获得帮助");
                        break;
                }
            }
        }

        return true;
    });

    //MapGroupMessage只会接收与正则匹配的消息
    session.MapGroupMessage(@"^#执行\s*(?<command>.*)$", async (v, group) =>
    {
        try
        {
            if (v.GroupId == groupId && v.QQ == master)
            {
                var command = group["command"].Value;

                if (command == string.Empty)
                {
                    await session.SendGroupMessageAsync(v.GroupId, $"没有指定参数1");
                    DiaLog.Log("执行命令失败: 没有指定参数");
                }
                else
                {
                    await session.SendGroupMessageAsync(v.GroupId, $"{command}");
                    DiaLog.Log($"执行了命令: {command}");
                }
            }
        }
        catch (Exception ex)
        {
            DiaLog.Error($"执行命令时出错： {ex}");
        }
    });

    session.MapGroupMessage(@"^#设置\s*(?<content>.*)$", async (v, group) =>
    {
        try
        {
            if (v.GroupId == groupId && v.QQ == master)
            {
                var contents = group["content"].Value.Split(" ");
                var key = contents[0];
                var value = contents[1];

                if (key == string.Empty || value == string.Empty)
                {
                    await session.SendGroupMessageAsync(v.GroupId, $"没有指定参数 参数1: [{key}] 参数2: [{value}]");
                    DiaLog.Log("执行命令失败: 没有指定参数1 参数2");
                }
                else
                {
                    switch (key)
                    {
                        case "积分" or nameof(Data.AccountData.Money):
                            config.AccountData.Money = long.Parse(value);
                            await session.SendGroupMessageAsync(groupId, $"设置了变量: {key}={value}");
                            DiaLog.Log($"设置了变量: {key}={value}");
                            break;
                        case "心情" or nameof(Data.PetData.Mood):
                            config.PetData.Mood = int.Parse(value);
                            await session.SendGroupMessageAsync(groupId, $"设置了变量: {key}={value}");
                            DiaLog.Log($"设置了变量: {key}={value}");
                            break;
                        case "精力" or nameof(Data.PetData.Energy):
                            config.PetData.Energy = int.Parse(value);
                            await session.SendGroupMessageAsync(groupId, $"设置了变量: {key}={value}");
                            DiaLog.Log($"设置了变量: {key}={value}");
                            break;
                        case "血量" or nameof(Data.PetData.BloodVolume):
                            config.PetData.BloodVolume = int.Parse(value);
                            await session.SendGroupMessageAsync(groupId, $"设置了变量: {key}={value}");
                            DiaLog.Log($"设置了变量: {key}={value}");
                            break;
                        case "经验" or nameof(Data.PetData.Experience):
                            config.PetData.Experience = int.Parse(value);
                            await session.SendGroupMessageAsync(groupId, $"设置了变量: {key}={value}");
                            DiaLog.Log($"设置了变量: {key}={value}");
                            break;
                        case "等级" or nameof(Data.PetData.Grade):
                            config.PetData.Grade = int.Parse(value);
                            await session.SendGroupMessageAsync(groupId, $"设置了变量: {key}={value}");
                            DiaLog.Log($"设置了变量: {key}={value}");
                            break;
                        default:
                            await session.SendGroupMessageAsync(groupId, $"未能找到变量: {key}={value}");
                            DiaLog.Log($"未能找到变量: {key}={value}");
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            DiaLog.Error($"设置变量时出错: {ex}");
        }
    });

    session.Build();
    Thread.Sleep(-1);
}
catch (Exception ex)
{
    DiaLog.Error($"机器人出错: {ex}");
}