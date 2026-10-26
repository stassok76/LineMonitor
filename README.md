# LineMonitor · 模拟产线监控上位机

一个基于 **C# / .NET 8 / WinForms** 的工业上位机练习项目：通过手写的 **Modbus TCP** 协议与 **OpenPLC** 软 PLC 通信，实现产线状态实时监控、启停控制、超温报警、数据存储和历史曲线查询。

> 全部使用免费开源工具完成，无需任何硬件：PLC 由运行在 WSL2 中的 OpenPLC Runtime 模拟。

<!-- 在这里放一张主界面截图，例如：![主界面](docs/main.png) -->

## 功能

- **实时监控**：每 500 ms 轮询 PLC，显示运行状态、报警状态、温度、产量
- **启停控制**：通过写线圈下发启动 / 停止命令，界面状态以 PLC 读回值为准
- **断线自动重连**：PLC 掉线后自动重试，恢复后无需重启程序；连接状态指示灯
- **超温报警**：边沿检测报警的发生与解除，记录发生时间、持续时长和当时温度
- **数据存储**：报警记录与产线历史数据保存到 SQLite，重启后报警历史自动加载
- **实时曲线**：ScottPlot 显示最近 2 分钟温度曲线，带 80 ℃ 报警线
- **历史查询**：按时间段查询温度曲线，可缩放拖动，并统计最高 / 最低温度
- **配置文件**：PLC 地址、端口、轮询间隔等写在 `appsettings.json`，修改无需重新编译
- **日志**：Serilog 按天滚动记录连接、报警、用户操作和异常，保留 30 天

## 架构

```
MainForm（界面层）            HistoryForm（历史查询）
   │ 订阅事件，BeginInvoke 回到界面线程
   ▼
PollingService ──事件──→ AlarmService ──→ AlarmRepository ──┐
   │  后台循环：连接 → 读取 → 通知 → 等待     HistoryRecorder ──→ HistoryRepository ─┤→ SQLite
   ▼
PlcService（业务层）：点表地址 → LineStatus 模型；温度 ÷10、32 位产量拼接
   ▼
ModbusTcpMaster / ModbusRtuMaster（通信层，手写 Modbus 协议）
   ▼
OpenPLC Runtime（WSL2）
```

设计要点：

- **分层**：界面不接触寄存器地址，Service 不接触控件；业务层依赖抽象 `ModbusMaster`，TCP / RTU 可互换
- **事件驱动**：轮询服务通过事件发布数据，界面、报警、存储各自订阅，新增功能无需修改发布者；订阅者之间异常隔离
- **线程安全**：状态使用不可变 `record` 快照在线程间传递；通信层使用 `SemaphoreSlim` 保证同一连接上的请求串行
- **命令与状态分离**：按钮只下发命令，界面显示只认 PLC 读回的真实状态

## 技术栈

| 类别 | 技术 |
|---|---|
| 语言 / 框架 | C# 12、.NET 8、WinForms |
| 通信 | 手写 Modbus TCP / RTU（功能码 01 / 03 / 05 / 06 / 16，CRC16，MBAP 报文头） |
| PLC | OpenPLC Editor v4 + OpenPLC Runtime v4（ST 语言），运行于 WSL2 Ubuntu |
| 数据库 | SQLite（Microsoft.Data.Sqlite，手写 SQL） |
| 图表 | ScottPlot 5 |
| 日志 | Serilog + Serilog.Sinks.File |
| 配置 | System.Text.Json |

## 项目结构

```
LineMonitor/
├── LineMonitor.APP/          # 主程序：产线监控上位机
│   ├── Config/               #   配置类与读取
│   ├── Data/                 #   SQLite 数据访问（Repository）
│   ├── Models/               #   点表地址、LineStatus、AlarmEvent
│   ├── Services/             #   PlcService、PollingService、AlarmService、HistoryRecorder
│   ├── MainForm.cs           #   主界面
│   ├── HistoryForm.cs        #   历史曲线查询
│   └── appsettings.json      #   配置文件
├── LineMonitor.Modbus/       # 手写 Modbus 协议类库（TCP / RTU）
├── LineMonitor.SerialTool/   # 串口调试助手
├── LineMonitor.TcpTool/      # TCP 调试助手（服务端多客户端 + 客户端自动重连）
└── LineMonitor.Playground/   # 控制台练习项目
```

## Modbus 点表

| 变量 | 类型 | Modbus 地址 | 读写 | 说明 |
|---|---|---|---|---|
| start_cmd | 线圈 | 0 | 写 | 启动命令，PLC 自动复位 |
| stop_cmd | 线圈 | 1 | 写 | 停止命令，PLC 自动复位 |
| running | 线圈 | 2 | 读 | 传送带运行中 |
| alarm | 线圈 | 3 | 读 | 超温报警（≥ 80.0 ℃ 触发，≤ 50.0 ℃ 解除） |
| temperature | 保持寄存器 | 0 | 读 | 温度 × 10，INT |
| count_hi | 保持寄存器 | 1 | 读 | 产量高 16 位 |
| count_lo | 保持寄存器 | 2 | 读 | 产量低 16 位 |

## 运行

**环境**：Windows 10/11、Visual Studio 2022、.NET 8 SDK、WSL2 (Ubuntu)

1. 在 WSL2 中安装 OpenPLC Runtime v4，用 OpenPLC Editor 下载 PLC 程序，并在 Device → Servers 中启用 Modbus TCP 服务（0.0.0.0:502）
2. 确认 PLC 正在监听：`ss -tlnp | grep 502`
3. 用 Visual Studio 打开 `LineMonitor.sln`，将 **LineMonitor.APP** 设为启动项目，F5 运行
4. 如需连接其他 PLC，修改输出目录中的 `appsettings.json`：

```json
{
  "Plc": { "Host": "127.0.0.1", "Port": 502, "UnitId": 1, "TimeoutMs": 1000 },
  "PollIntervalMs": 500,
  "HistoryIntervalSeconds": 5,
  "DatabaseFile": "linemonitor.db"
}
```

运行后，数据库 `linemonitor.db` 和日志目录 `logs/` 会生成在程序所在目录。

## 已知不足与改进方向

- 重启程序时若报警尚未解除，会重复记录一次"报警发生"（可在启动时读取最后一条报警记录恢复状态）
- 实时曲线点数随运行时间增长（可改用定长环形缓冲）
- 长时间段历史查询在界面线程同步执行（可改为异步并在 SQL 中按分钟降采样）
- 尚未实现历史数据的定期清理

## 学习路线

本项目按 9 个阶段完成：C# 基础 → WinForms 多线程 → 串口助手 → TCP 助手 → 手写 Modbus RTU/TCP → OpenPLC 软 PLC → 分层监控程序 → SQLite + 曲线 → 配置 / 日志 / 发布。
