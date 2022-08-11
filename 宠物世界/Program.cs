using ConfigDatas;
using ConfigManage;
using IlyfairyLib.GoCqHttpSdk;
using IlyfairyLib.GoCqHttpSdk.Api;
using IlyfairyLib.GoCqHttpSdk.Models.Chunks;
using System.Text.RegularExpressions;

try
{
    Data? config;

    config = Config.ReadConfig();

    if (config == null)
    {
        Console.Write("配置文件读取失败，请删除配置文件或者打开文本编辑器尝试修复! 请按任意键退出程序. . .");
        Console.ReadKey();
        Environment.Exit(-1);
    }

    Session session = new(config.Robot.WebSocketAddress, config.Robot.HttpAddress);
    const int groupId = 522126928;
    //const int groupId = 295322097;
    const uint master = 3251242073;
    const uint gameBotQQ = 3409297243;
    //const uint gameBotQQ = 2794446833;

#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行

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

        if (config.Setting.AutoWork)
            WorkTask.AutoWork(session, config);

        if (config.Setting.AutoExplore)
            WorkTask.AutoExplore(session, config);

        if (config.Setting.AutoTrain)
            WorkTask.AutoTrain(session, config);

        if (config.Setting.AutoFishing)
            WorkTask.AutoFishing(session, config);

        return true;
    });

    //程序退出事件
    AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
    {
        DiaLog.Log("程序退出 保存配置文件");
        Config.SaveConfig(config);
    };

    //接收所有群消息
    session.UseGroupMessage(async v =>
    {
        if (v.GroupId == groupId)
        {
            if (v.QQ == gameBotQQ)
            {
                var message = v.Message.Text;
                var name = Regex.Match(message, @"@(?<at>\S+)").Groups["at"].Value;
                var moodStr = Regex.Match(message, @"心情减少.+?(?<num>-?\d+)").Groups["num"].Value;
                var energyStr = Regex.Match(message, @"((减少|消耗)精力)|(精力恢复).+?(?<num>-?\d+)").Groups["num"].Value;
                var experienceStr = Regex.Match(message, @"(经验减少|增加经验).+?(?<num>-?\d+)").Groups["num"].Value;
                var gradeStr = Regex.Match(message, @"等级提升.+?(?<num>-?\d+)").Groups["num"].Value;
                var moneyStr = Regex.Match(message, @"获得积分.+?(?<num>-?\d+)").Groups["num"].Value;

                if (name != string.Empty && name == config.Robot.BotName)
                {
                    if (message.IndexOf("心情不好了") > 0)
                    {
                        config.PetData.Mood -= 10;
                        DiaLog.Log($"宠物心情 -10 ({config.PetData.Mood})");
                    }
                    if (moodStr != string.Empty)
                    {
                        var mood = int.Parse(moodStr);
                        config.PetData.Mood += mood;
                    }
                    if (energyStr != string.Empty)
                    {
                        var energy = int.Parse(energyStr);
                        config.PetData.Energy += energy;
                    }
                    if (experienceStr != string.Empty)
                    {
                        var experience = int.Parse(experienceStr);
                        config.PetData.Experience += experience;
                    }
                    if (gradeStr != string.Empty)
                    {
                        var grade = int.Parse(gradeStr);
                        config.PetData.Grade += grade;
                    }
                    if (moneyStr != string.Empty)
                    {
                        var money = long.Parse(moneyStr);
                        config.AccountData.Money += money;
                    }
                }
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
                                WorkTask.AutoWork(session, config);
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
                                WorkTask.AutoExplore(session, config);
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
                                WorkTask.AutoTrain(session, config);
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
                                WorkTask.AutoFishing(session, config);
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
                                await session.SendGroupMessageAsync(groupId, $"转让一万积分卡*{buyCount}-" + new AtChunk(master));
                                DiaLog.Log($"收集完毕，花费了{buyCount * 12000}积分购买了一万积分卡*{buyCount}");
                            }
                            else
                            {
                                double progress = config.AccountData.Money / 12000.00d * 100;
                                //await session.SendGroupMessageAsync(groupId, $"积分不足，无法收集 ({config.AccountData.Money}/12000) [{Math.Round(progress, 2)}%]");
                                DiaLog.Log($"积分不足，无法收集 ({config.AccountData.Money}/12000) [{Math.Round(progress, 2)}%]");
                            }
                        }
                        catch (Exception ex)
                        {
                            DiaLog.Error($"收集时出错: {ex}");
                        }
                        break;
                    case "#状态":
                        #region
                        /*await session.SendGroupMessageAsync(groupId, @$"当前状态:
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
");*/
                        #endregion
                        DiaLog.Info(@$"当前状态:
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
装备钓鱼竿: [{config.BackpackData.FishingRod.name}] 剩余使用次数: {config.BackpackData.FishingRod.durable}
");
                        break;
                    case "#帮助":
                        #region
                        /*await session.SendGroupMessageAsync(groupId, @"命令   [参数]  作用
#打工 自动打工
#探险 自动探险
#修炼 自动修炼
#钓鱼 自动钓鱼
#收集 收集积分
#状态 查看状态
#执行 [命令]  执行命令
#设置 [变量名 值] 设置变量
#帮助 获得帮助");*/
                        #endregion
                        DiaLog.Info(@"命令   [参数]  作用
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
                    //await session.SendGroupMessageAsync(v.GroupId, $"没有指定参数1");
                    DiaLog.Log("执行命令失败: 没有指定参数1");
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
                    //await session.SendGroupMessageAsync(v.GroupId, $"没有指定参数 参数1: [{key}] 参数2: [{value}]");
                    DiaLog.Log($"执行命令失败: 没有指定参数 参数1: [{key}] 参数2: [{value}]");
                }
                else
                {
                    switch (key)
                    {
                        case "积分" or nameof(Data.AccountData.Money):
                            config.AccountData.Money = long.Parse(value);
                            //await session.SendGroupMessageAsync(groupId, $"设置了变量: {key}={value}");
                            DiaLog.Log($"设置了变量: {key}={value}");
                            break;
                        case "心情" or nameof(Data.PetData.Mood):
                            config.PetData.Mood = int.Parse(value);
                            //await session.SendGroupMessageAsync(groupId, $"设置了变量: {key}={value}");
                            DiaLog.Log($"设置了变量: {key}={value}");
                            break;
                        case "精力" or nameof(Data.PetData.Energy):
                            config.PetData.Energy = int.Parse(value);
                            //await session.SendGroupMessageAsync(groupId, $"设置了变量: {key}={value}");
                            DiaLog.Log($"设置了变量: {key}={value}");
                            break;
                        case "血量" or nameof(Data.PetData.BloodVolume):
                            config.PetData.BloodVolume = int.Parse(value);
                            //await session.SendGroupMessageAsync(groupId, $"设置了变量: {key}={value}");
                            DiaLog.Log($"设置了变量: {key}={value}");
                            break;
                        case "经验" or nameof(Data.PetData.Experience):
                            config.PetData.Experience = int.Parse(value);
                            //await session.SendGroupMessageAsync(groupId, $"设置了变量: {key}={value}");
                            DiaLog.Log($"设置了变量: {key}={value}");
                            break;
                        case "等级" or nameof(Data.PetData.Grade):
                            config.PetData.Grade = int.Parse(value);
                            //await session.SendGroupMessageAsync(groupId, $"设置了变量: {key}={value}");
                            DiaLog.Log($"设置了变量: {key}={value}");
                            break;
                        default:
                            //await session.SendGroupMessageAsync(groupId, $"未能找到变量: {key}={value}");
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

    WorkTask.AutoSave(config);
    WorkTask.AutoSign(session, config);
    WorkTask.AutoEnergy(config);

    session.Build();
    Thread.Sleep(-1);
}
catch (Exception ex)
{
    DiaLog.Error($"机器人出错: {ex}");
}