# HHSwarm
[Haven &amp; Hearth](http://www.havenandhearth.com/portal/) online game .NET client.

*[Русский](README.md), [한국어](README.ko.md), [日本語](README.ja.md), [简体中文](README.zh-cn.md), [正體中文](README.zh-tw.md)*

## Purpose of this project
Author of the original game has decided to use Java server and client as a platform for the game. There are many pros for it. Same client can run on Linux and on Windows, for example.

But, there are some critical issues that cannot be resolved easily. Some of them: GPU is used just for small ammount of code, most graphic algorithms run on CPU; legacy not effective code that executes communications protocol; insecure authentication communication, use deprecated TLS version.

## Visual Studio code structure
  - [Native](HHSwarm.Native) - hides communication protocol complexity behind .NET interface
  - [Native.Tests](HHSwarm.Native.Tests) - Unit-tests to research and debug different parts of communication protocol.
  - [TestClient](HHSwarm.TestClient) - simple client (no GUI, pure text). Is used for messages/resources interception and for simple gameplay test.
  - [Model](HHSwarm.Model) - higher level of abstraction: object-oriented, independent from protocol implementation, pure game world. 
  - [Automation](HHSwarm.Automation) - "business" layer for player automation. Example: obstacles evasion algorithm.
  - **missing GUI client** - game client. It is supposed to be based on [Unity](https://unity.com).

## How to compile and to run
1. For successful project build you would need:
   1. Visual Studio 2017, or Visual Studio 2019 possibly.
   1. .NET Framework 4.6
1. Make project **TestClient** to be default for run.
1. Find and replace `$LoginName$` with user account name. Find and replace `$Password$` to password. 
1. Run debug.
