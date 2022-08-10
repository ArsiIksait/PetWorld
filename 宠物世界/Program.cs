using ConfigDatas;
using ConfigManage;
using IlyfairyLib.GoCqHttpSdk;
using IlyfairyLib.GoCqHttpSdk.Api;
using IlyfairyLib.GoCqHttpSdk.Models.Chunks;

Session session = new("ws://127.0.0.1:6700", "http://127.0.0.1:5700");
const int groupId = 522126928;
//const int groupId = 295322097;
const uint master = 3251242073;
const uint botQQ = 3409297243;

#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行

//Data? config;

//config = Config.ReadConfig();

Data config = new()
{
    Setting = new Setting(),
    SignData = new SignData(),
    AccountData = new AccountData(),
    PetData = new PetData(),
    BackpackData = new BackpackData()
};

config.AccountData.Money = 800000;

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
        DiaLog.Error("ws连接断开");
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
            switch (v.Message.Text.ToLower())
            {
                case "#打工":
                    if (config.Setting.AutoWork)
                    {
                        config.Setting.AutoWork = false;
                    }
                    else
                    {
                        config.Setting.AutoWork = true;
                        Thread autoWork = new(async () => { 
                            while(config.Setting.AutoWork)
                            {
                                await session.SendGroupMessageAsync(groupId, "打工");
                                Thread.Sleep(ConfigDatas.Timeout.sleepTime);
                            }
                            await session.SendGroupMessageAsync(groupId, "自动打工已关闭");
                        });
                        autoWork.Start();
                        await session.SendGroupMessageAsync(groupId, "自动打工已开启");
                    }
                    break;
                case "#探险":
                    if (config.Setting.AutoExplore)
                    {
                        config.Setting.AutoExplore = false;
                    }
                    else
                    {
                        config.Setting.AutoExplore = true;
                        Thread autoExplore = new(async () => {
                            while (config.Setting.AutoExplore)
                            {
                                await session.SendGroupMessageAsync(groupId, "探险");
                                Thread.Sleep(ConfigDatas.Timeout.sleepTime);
                            }
                            await session.SendGroupMessageAsync(groupId, "自动探险已关闭");
                        });
                        autoExplore.Start();
                        await session.SendGroupMessageAsync(groupId, "自动探险已开启");
                    }
                    break;
                case "#收集":
                    if (config.AccountData.Money >= 12000)
                    {
                        int buyCount = 0;

                        for (var i = config.AccountData.Money; i >= 12000; i-=12000)
                        {
                            buyCount++;
                            config.AccountData.Money -= 12000;
                        }

                        await session.SendGroupMessageAsync(groupId, $"购买一万积分卡*{buyCount}");
                        await session.SendGroupMessageAsync(groupId, "转让物品" + $"一万积分卡*{buyCount}-" + new AtChunk(master));
                    }
                    else
                    {
                        double progress = config.AccountData.Money / 12000.00d * 100;
                        await session.SendGroupMessageAsync(groupId, $"积分不足 ({config.AccountData.Money}/12000) [{Math.Round(progress, 2)}%]");
                    }
                    break;
                case "#帮助":
                    await session.SendGroupMessageAsync(groupId, @"命令   作用
#执行 命令  执行命令
#打工 自动打工
#探险 自动探险
#收集 收集积分
#帮助 获得帮助");
                    break;
                default:
                    break;
            }
        }
    }

    return true;
});

session.Build();
Thread.Sleep(-1);