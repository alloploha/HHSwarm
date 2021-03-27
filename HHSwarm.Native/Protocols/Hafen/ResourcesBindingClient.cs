using HHSwarm.Native.Common;
using HHSwarm.Native.GameResources;
using HHSwarm.Native.Protocols.Hafen.Messages;
using HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments;
using HHSwarm.Native.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen
{
    /// <summary>
    /// Разрешает проблему асинхронного получения: команд на создание\изменение объектов, описаний ресурсов, и самих ресурсов.
    /// При получении сообщений типа, например <see cref="RMSG_NEWWDG"/>, для которых необходимы ресурсы:
    /// <list type="number">
    /// <item>удостоверяется, что необходимые ресурсы уже присутствуют, или если их нет, то получает из указанного источника</item>
    /// <item>откладывает команду до момента, когда все необходимые ресурсы станут доступны</item>
    /// <item>вызывает событие исполнения команды и передаёт в неё всю необходимую информацию, включая параметры создания объекта и необходимые для этого ресурсы</item>
    /// </list>
    public class ResourcesBindingClient
    {
        protected TraceSource Trace = new TraceSource("HHSwarm.Resources");

        private RelayClient Relay;
        private IGameResources LocalResources;
        private ISourceOfGameResources RemoteResources;
        private WidgetMessageArgumentsFormatter WidgetArgumentsFormatter;

        public ResourcesBindingClient(RelayClient relay, IGameResources localResources, ISourceOfGameResources remoteResources)
        {
            this.Relay = relay;//?? throw new ArgumentNullException();
            this.LocalResources = localResources;//?? throw new ArgumentNullException();
            this.RemoteResources = remoteResources;//?? throw new ArgumentNullException();

            this.WidgetArgumentsFormatter = new WidgetMessageArgumentsFormatter();

            Relay.RMSG_RESID_Received += Relay_RMSG_RESID_Received;
            Relay.RMSG_NEWWDG_Received += Relay_RMSG_NEWWDG_Received;
            Relay.RMSG_ADDWDG_Received += Relay_RMSG_ADDWDG_Received;
            Relay.RMSG_WDGMSG_Received += Relay_RMSG_WDGMSG_Received;
        }



        #region RMSG_NEWWDG
        private class Delayed_RMSG_NEWWDG_Processing
        {
            public bool IsReady = false;
            public Action Dispatch;
        }

        private Queue<Delayed_RMSG_NEWWDG_Processing> Delayed_RMSG_NEWWDG = new Queue<Delayed_RMSG_NEWWDG_Processing>();

        private async Task ProcessDelayed_RMSG_NEWWDG_Async(IEnumerable<NamedResource> resourceInfo, Action DispatchMessageWhenReady)
        {
            Delayed_RMSG_NEWWDG_Processing delayed_message = new Delayed_RMSG_NEWWDG_Processing()
            {
                IsReady = false,
                Dispatch = DispatchMessageWhenReady
            };

            lock (Delayed_RMSG_NEWWDG)
            {
                Delayed_RMSG_NEWWDG.Enqueue(delayed_message);
            }

            /* PREPARE */

            await Task.WhenAll(resourceInfo.Select(r => EnsureLocalResourceExistsAsync(r.ResourceName)));

            foreach (var r in resourceInfo)
            {
                r.GetLocalResourceTask = LocalResources.GetAsync(r.ResourceName);
            }

            await Task.WhenAll(resourceInfo.Select(r => r.GetLocalResourceTask));

            foreach (var r in resourceInfo)
            {
                r.GetLocalResourceTask = LocalResources.GetAsync(r.ResourceName, r.ResourceVersion);
            }

            /* DISPATCH */

            foreach (var r in resourceInfo)
            {
                if (r.Resource == null) throw new Exception($"{r.ResourceName} is empty!");
            }

            delayed_message.IsReady = true;

            lock (Delayed_RMSG_NEWWDG)
            {
                while (Delayed_RMSG_NEWWDG.Count > 0 && Delayed_RMSG_NEWWDG.Peek().IsReady)
                    Delayed_RMSG_NEWWDG.Dequeue().Dispatch();
            }
        }

        #endregion

        private class NamedResource
        {
            public Task<HavenResource1> GetLocalResourceTask;
            public HavenResource1 Resource => GetLocalResourceTask.Result;

            public string ResourceName;
            public ushort? ResourceVersion;

            public NamedResource(string resourceName, ushort? resourceVersion = null)
            {
                this.ResourceName = resourceName;
                this.ResourceVersion = resourceVersion;
            }
        }

        private class NamedResourceList : IEnumerable<NamedResource>
        {
            private Dictionary<string, NamedResource> List = new Dictionary<string, NamedResource>();

            public NamedResourceList(params string[] resourceNames)
            {
                foreach (var resource_name in resourceNames)
                {
                    List.Add(resource_name, new NamedResource(resource_name));
                }
            }

            public NamedResource this[string resourceName]
            {
                get
                {
                    return List[resourceName];
                }
                set
                {
                    List[resourceName] = value;
                }
            }

            public IEnumerator<NamedResource> GetEnumerator()
            {
                return List.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        #region RMSG_NEWWDG
        private void Relay_RMSG_NEWWDG_Received(RMSG_NEWWDG message)
        {
            Relay_RMSG_NEWWDG_ReceivedAsync(message).WaitAsync((e) => { throw e; });
        }

        private async Task Relay_RMSG_NEWWDG_ReceivedAsync(RMSG_NEWWDG message)
        {
            switch (message.Type)
            {
                case "ibtn":
                    // @RName("ibtn")
                    // https://github.com/dolda2000/hafen-client/blob/394a9d64bc732ed8c2eb6e5df1b57dd08b97c4d8/src/haven/IButton.java#L38
                    {
                        ButtonWidgetCreateArguments create_arguments;
                        WidgetArgumentsFormatter.Deserialize(new ArgumentsReader(message.CreateArguments), out create_arguments);

                        NamedResourceList res = new NamedResourceList
                        (
                            create_arguments.UpImageResourceName,
                            create_arguments.DownImageResourceName
                        );

                        await ProcessDelayed_RMSG_NEWWDG_Async
                        (
                            res,
                            () => CreateButtonWidgetReceived?.Invoke
                                (
                                    widgetID: message.WidgetID,
                                    parentWidgetID: message.ParentID,
                                    addChildArgumentsDeserializer: WidgetArgumentsFormatter,
                                    addChildArguments: message.AddChildArguments,
                                    upImageResource: res[create_arguments.UpImageResourceName].Resource.Images.Single(),
                                    downImageResource: res[create_arguments.DownImageResourceName].Resource.Images.Single(),

                                    // https://github.com/dolda2000/hafen-client/blob/394a9d64bc732ed8c2eb6e5df1b57dd08b97c4d8/src/haven/IButton.java#L59
                                    hoverImageResource: res[create_arguments.UpImageResourceName].Resource.Images.Single() 
                                )
                        );

                    }
                    break;
                case "charlist":
                    // @RName("charlist")
                    // https://github.com/dolda2000/hafen-client/blob/d61d2a6ff4a832f84e55069794578002d37b0ce1/src/haven/Charlist.java#L64
                    {
                        CharactersListWidgetCreateArguments create_arguments;
                        WidgetArgumentsFormatter.Deserialize(new ArgumentsReader(message.CreateArguments), out create_arguments);

                        NamedResourceList res = new NamedResourceList
                        (
                            create_arguments.BackgroundImageResourceName,
                            create_arguments.ScrollUpButtonUpImageResourceName,
                            create_arguments.ScrollUpButtonDownImageResourceName,
                            create_arguments.ScrollUpButtonHoverImageResourceName,
                            create_arguments.ScrollDownButtonUpImageResourceName,
                            create_arguments.ScrollDownButtonDownImageResourceName,
                            create_arguments.ScrollDownButtonHoverImageResourceName
                        );


                        await ProcessDelayed_RMSG_NEWWDG_Async
                        (
                            res,
                            () => CreateCharactersListWidgetReceived?.Invoke
                                (
                                    widgetID: message.WidgetID,
                                    parentWidgetID: message.ParentID,
                                    addChildArgumentsDeserializer: WidgetArgumentsFormatter,
                                    addChildArguments: message.AddChildArguments,
                                    height: create_arguments.Height,
                                    backgroundImageResource: res[create_arguments.BackgroundImageResourceName].Resource.Images.Single(),
                                    scrollUpButtonUpImageResource: res[create_arguments.ScrollUpButtonUpImageResourceName].Resource.Images.Single(),
                                    scrollUpButtonDownImageResource: res[create_arguments.ScrollUpButtonDownImageResourceName].Resource.Images.Single(),
                                    scrollUpButtonHoverImageResource: res[create_arguments.ScrollUpButtonHoverImageResourceName].Resource.Images.Single(),
                                    scrollDownButtonUpImageResource: res[create_arguments.ScrollDownButtonUpImageResourceName].Resource.Images.Single(),
                                    scrollDownButtonDownImageResource: res[create_arguments.ScrollDownButtonDownImageResourceName].Resource.Images.Single(),
                                    scrollDownButtonHoverImageResource: res[create_arguments.ScrollDownButtonHoverImageResourceName].Resource.Images.Single()
                                )
                        );

                    }
                    break;
                case "img":
                    // @RName("img")
                    // https://github.com/dolda2000/hafen-client/blob/a1e7096e2dac9c6798783cece3460889b5eb8c21/src/haven/Img.java#L37
                    {
                        ImageWidgetCreateArguments create_arguments;
                        WidgetArgumentsFormatter.Deserialize(new ArgumentsReader(message.CreateArguments), out create_arguments);

                        NamedResourceList res = new NamedResourceList();

                        if (create_arguments.ImageResourceID.HasValue && String.IsNullOrEmpty(create_arguments.ImageResourceName))
                        {
                            create_arguments.ImageResourceName = await ResourceName.GetItemAsync(create_arguments.ImageResourceID.Value);
                        }

                        Debug.Assert(!String.IsNullOrWhiteSpace(create_arguments.ImageResourceName), $"{nameof(create_arguments.ImageResourceName)} in {nameof(ImageWidgetCreateArguments)} expected to have value");

                        res[create_arguments.ImageResourceName] = new NamedResource(create_arguments.ImageResourceName, create_arguments.ImageResourceVersion);

                        await ProcessDelayed_RMSG_NEWWDG_Async
                        (
                            res,
                            () => CreateImageWidgetReceived?.Invoke
                                (
                                    widgetID: message.WidgetID,
                                    parentWidgetID: message.ParentID,
                                    addChildArgumentsDeserializer: WidgetArgumentsFormatter,
                                    addChildArguments: message.AddChildArguments,
                                    acceptsUserInput: create_arguments.AcceptsUserInput,
                                    imageResource: res[create_arguments.ImageResourceName].Resource.Images.Single()
                                )
                        );
                    }
                    break;
                case "cnt":
                    // @RName("cnt")
                    // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/Widget.java#L59
                    // TODO: @RName("cnt")
                    throw new NotImplementedException();
                    break;
                case "ccnt":
                    // @RName("ccnt")
                    // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/Widget.java#L65
                    {
                        CreateEmptyCenteredWidgetCreateArguments create_arguments;
                        WidgetArgumentsFormatter.Deserialize(new ArgumentsReader(message.CreateArguments), out create_arguments);

                        CreateEmptyCenteredWidgetReceived?.Invoke
                        (
                            widgetID: message.WidgetID,
                            parentWidgetID: message.ParentID,
                            addChildArgumentsDeserializer: WidgetArgumentsFormatter,
                            addChildArguments: message.AddChildArguments,
                            size: create_arguments.Size
                        );
                    }
                    break;
                case "fcnt":
                    // @RName("fcnt")
                    // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/Widget.java#L80
                    // TODO: @RName("fcnt")
                    throw new NotImplementedException();
                    break;
                case "acnt":
                    // @RName("acnt")
                    // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/Widget.java#L128
                    // TODO: @RName("acnt")
                    throw new NotImplementedException();
                    break;
                case "gameui":
                    // @RName("gameui")
                    // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/GameUI.java#L127
                    {
                        GameUserInterfaceWidgetCreateArguments create_arguments;
                        WidgetArgumentsFormatter.Deserialize(new ArgumentsReader(message.CreateArguments), out create_arguments);

                        CreateGameUserInterfaceWidgetReceived?.Invoke
                        (
                            widgetID: message.WidgetID,
                            parentWidgetID: message.ParentID,
                            addChildArgumentsDeserializer: WidgetArgumentsFormatter,
                            addChildArguments: message.AddChildArguments,
                            characterID: create_arguments.CharacterID,
                            plid: create_arguments.plid,
                            genus: create_arguments.genus
                        );
                    }
                    break;
                case "mapview":
                    // @RName("mapview")
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/MapView.java#L480
                    {
                        MapViewWidgetCreateArguments create_arguments;
                        WidgetArgumentsFormatter.Deserialize(new ArgumentsReader(message.CreateArguments), out create_arguments);

                        CreateMapViewWidgetReceived?.Invoke
                        (
                            widgetID: message.WidgetID,
                            parentWidgetID: message.ParentID,
                            addChildArgumentsDeserializer: WidgetArgumentsFormatter,
                            addChildArguments: message.AddChildArguments,
                            size: create_arguments.Size,
                            mc: create_arguments.mc,
                            pgob: create_arguments.pgob
                        );
                    }
                    break;
                case "item":
                    // @RName("item")
                    // https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/GItem.java#L43
                    {
                        GitemWidgetCreateArguments create_arguments;
                        WidgetArgumentsFormatter.Deserialize(new ArgumentsReader(message.CreateArguments), out create_arguments);

                        string resource_name = await ResourceName.GetItemAsync(create_arguments.ResourceID);

                        NamedResourceList res = new NamedResourceList(resource_name);

                        // TODO: @RName("item"): fix position error

                        await ProcessDelayed_RMSG_NEWWDG_Async
                        (
                            res,
                            () => CreateGitemWidgetReceived?.Invoke
                                (
                                    widgetID: message.WidgetID,
                                    parentWidgetID: message.ParentID,
                                    addChildArgumentsDeserializer: WidgetArgumentsFormatter,
                                    addChildArguments: message.AddChildArguments,
                                    resource: res[resource_name].Resource.Images.Single(),
                                    data: create_arguments.Data
                                )
                        );

                    }
                    break;
                case "epry":
                    // @RName("epry")
                    // https://github.com/dolda2000/hafen-client/blob/974366a68c0e61ee175a678d574f863705eac352/src/haven/Equipory.java#L82
                    // TODO: @RName("epry")
                    {
                        EquiporyWidgetCreateArguments create_arguments;
                        WidgetArgumentsFormatter.Deserialize(new ArgumentsReader(message.CreateArguments), out create_arguments);
                    }
                    throw new NotImplementedException();
                    break;
                case "inv":
                    // @RName("inv")
                    // https://github.com/dolda2000/hafen-client/blob/c90a0aea7a84f7ab738c1d7c40556ae50decf03f/src/haven/Inventory.java#L58
                    {
                        InventoryWidgetCreateArguments create_arguments;
                        WidgetArgumentsFormatter.Deserialize(new ArgumentsReader(message.CreateArguments), out create_arguments);

                        CreateInventoryWidgetReceived?.Invoke
                        (
                            widgetID: message.WidgetID,
                            parentWidgetID: message.ParentID,
                            addChildArgumentsDeserializer: WidgetArgumentsFormatter,
                            addChildArguments: message.AddChildArguments,
                            size: create_arguments.Size
                        );
                    }
                    break;
                case "fmg":
                    // @RName("fmg")
                    // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/FightWnd.java#L693
                    // TODO: @RName("fmg")
                    throw new NotImplementedException();
                    break;
                case "chr":
                    // @RName("chr")
                    // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1843
                    // TODO: @RName("chr")
                    throw new NotImplementedException();
                    break;
                case "av":
                    // @RName("av")
                    // https://github.com/dolda2000/hafen-client/blob/d61d2a6ff4a832f84e55069794578002d37b0ce1/src/haven/Avaview.java#L50
                    // TODO: @RName("av")
                    throw new NotImplementedException();
                    break;
                case "buddy":
                    // @RName("buddy")
                    // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/BuddyWnd.java#L79
                    // TODO: @RName("buddy")
                    throw new NotImplementedException();
                    break;
                case "grp":
                    // @RName("grp")
                    // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/BuddyWnd.java#L244
                    // TODO: @RName("grp")
                    throw new NotImplementedException();
                    break;
                case "buff":
                    // @RName("buff")
                    // https://github.com/dolda2000/hafen-client/blob/d61d2a6ff4a832f84e55069794578002d37b0ce1/src/haven/Buff.java#L56
                    // TODO: @RName("buff")
                    throw new NotImplementedException();
                    break;
                case "btn":
                    // @RName("btn")
                    // https://github.com/dolda2000/hafen-client/blob/394a9d64bc732ed8c2eb6e5df1b57dd08b97c4d8/src/haven/Button.java#L56
                    // TODO: @RName("btn")
                    throw new NotImplementedException();
                    break;
                case "ltbtn":
                    // @RName("ltbtn")
                    // https://github.com/dolda2000/hafen-client/blob/394a9d64bc732ed8c2eb6e5df1b57dd08b97c4d8/src/haven/Button.java#L65
                    // TODO: @RName("ltbtn")
                    throw new NotImplementedException();
                    break;
                case "wound":
                    // @RName("wound")
                    // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L895
                    // TODO: @RName("wound")
                    throw new NotImplementedException();
                    break;
                case "quest":
                    // @RName("quest")
                    // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/CharWnd.java#L1383
                    // TODO: @RName("quest")
                    throw new NotImplementedException();
                    break;
                case "schan":
                    // @RName("schan")
                    // https://github.com/dolda2000/hafen-client/blob/9dc7c1e7af3f1e3d49a7e2b42b6a11ec8c463af8/src/haven/ChatUI.java#L845
                    // TODO: @RName("schan")
                    throw new NotImplementedException();
                    break;
                case "mchat":
                    // @RName("mchat")
                    // https://github.com/dolda2000/hafen-client/blob/9dc7c1e7af3f1e3d49a7e2b42b6a11ec8c463af8/src/haven/ChatUI.java#L852
                    // TODO: @RName("mchat")
                    throw new NotImplementedException();
                    break;
                case "pchat":
                    // @RName("pchat")
                    // https://github.com/dolda2000/hafen-client/blob/9dc7c1e7af3f1e3d49a7e2b42b6a11ec8c463af8/src/haven/ChatUI.java#L860
                    // TODO: @RName("pchat")
                    throw new NotImplementedException();
                    break;
                case "pmchat":
                    // @RName("pmchat")
                    // https://github.com/dolda2000/hafen-client/blob/9dc7c1e7af3f1e3d49a7e2b42b6a11ec8c463af8/src/haven/ChatUI.java#L866
                    // TODO: @RName("pmchat")
                    throw new NotImplementedException();
                    break;
                case "chat":
                    // @RName("chat")
                    // https://github.com/dolda2000/hafen-client/blob/d61d2a6ff4a832f84e55069794578002d37b0ce1/src/haven/Chatwindow.java#L33
                    // TODO: @RName("chat")
                    throw new NotImplementedException();
                    break;
                case "chk":
                    // @RName("chk")
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/CheckBox.java#L38
                    // TODO: @RName("chk")
                    throw new NotImplementedException();
                    break;
                case "fsess":
                    // @RName("fsess")
                    // https://github.com/dolda2000/hafen-client/blob/ccfca692c16d0df250bdd5f6762d5461999a9ab6/src/haven/Fightsess.java#L62
                    // TODO: @RName("fsess")
                    throw new NotImplementedException();
                    break;
                case "frv":
                    // @RName("frv")
                    // https://github.com/dolda2000/hafen-client/blob/394a9d64bc732ed8c2eb6e5df1b57dd08b97c4d8/src/haven/Fightview.java#L104
                    // TODO: @RName("frv")
                    throw new NotImplementedException();
                    break;
                case "sm":
                    // @RName("sm")
                    // https://github.com/dolda2000/hafen-client/blob/974366a68c0e61ee175a678d574f863705eac352/src/haven/FlowerMenu.java#L43
                    // TODO: @RName("sm")
                    throw new NotImplementedException();
                    break;
                case "give":
                    // @RName("give")
                    // https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/GiveButton.java#L37
                    // TODO: @RName("give")
                    throw new NotImplementedException();
                    break;
                case "ichk":
                    // @RName("ichk")
                    // https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/ICheckBox.java#L36
                    // TODO: @RName("ichk")
                    throw new NotImplementedException();
                    break;
                case "im":
                    // @RName("im")
                    // https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/IMeter.java#L39
                    // TODO: @RName("im")
                    throw new NotImplementedException();
                    break;
                case "isbox":
                    // @RName("isbox")
                    // https://github.com/dolda2000/hafen-client/blob/d61d2a6ff4a832f84e55069794578002d37b0ce1/src/haven/ISBox.java#L39
                    // TODO: @RName("isbox")
                    throw new NotImplementedException();
                    break;
                case "lbl":
                    // @RName("lbl")
                    // https://github.com/dolda2000/hafen-client/blob/d61d2a6ff4a832f84e55069794578002d37b0ce1/src/haven/Label.java#L37
                    // TODO: @RName("lbl")
                    throw new NotImplementedException();
                    break;
                case "make":
                    // @RName("make")
                    // https://github.com/dolda2000/hafen-client/blob/9eb9b50b2a0cc7029f8f74211d53d9be108a21f9/src/haven/Makewindow.java#L47
                    // TODO: @RName("make")
                    throw new NotImplementedException();
                    break;
                case "mapmod":
                    // @RName("mapmod")
                    // https://github.com/dolda2000/hafen-client/blob/4432d2be09a90eea48d370af6268b13227c146df/src/haven/MapMod.java#L43
                    // TODO: @RName("mapmod")
                    throw new NotImplementedException();
                    break;
                case "scm":
                    // @RName("scm")
                    // https://github.com/dolda2000/hafen-client/blob/25968122bbebc26462e0046ae4b8a0cb480dc65d/src/haven/MenuGrid.java#L50
                    // TODO: @RName("scm")
                    throw new NotImplementedException();
                    break;
                case "npc":
                    // @RName("npc")
                    // https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/NpcChat.java#L36
                    // TODO: @RName("npc")
                    throw new NotImplementedException();
                    break;
                case "pv":
                    // @RName("pv")
                    // https://github.com/dolda2000/hafen-client/blob/d61d2a6ff4a832f84e55069794578002d37b0ce1/src/haven/Partyview.java#L42
                    // TODO: @RName("pv")
                    throw new NotImplementedException();
                    break;
                case "prog":
                    // @RName("prog")
                    // https://github.com/dolda2000/hafen-client/blob/d61d2a6ff4a832f84e55069794578002d37b0ce1/src/haven/Progress.java#L32
                    // TODO: @RName("prog")
                    throw new NotImplementedException();
                    break;
                case "scr":
                    // @RName("scr")
                    // https://github.com/dolda2000/hafen-client/blob/394a9d64bc732ed8c2eb6e5df1b57dd08b97c4d8/src/haven/Scrollport.java#L33
                    // TODO: @RName("scr")
                    throw new NotImplementedException();
                    break;
                case "sess":
                    // @RName("sess")
                    // https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/SessWidget.java#L35
                    // TODO: @RName("sess")
                    throw new NotImplementedException();
                    break;
                case "speedget":
                    // @RName("speedget")
                    // https://github.com/dolda2000/hafen-client/blob/f85b82305e06f850c924d3309de68eedbd9209dd/src/haven/Speedget.java#L54
                    // TODO: @RName("speedget")
                    throw new NotImplementedException();
                    break;
                case "text":
                    // @RName("text")
                    // https://github.com/dolda2000/hafen-client/blob/bc52d6cc6df621b6854614c736ed64193deb7dfc/src/haven/TextEntry.java#L52
                    // TODO: @RName("text")
                    throw new NotImplementedException();
                    break;
                case "log":
                    // @RName("log")
                    // https://github.com/dolda2000/hafen-client/blob/d61d2a6ff4a832f84e55069794578002d37b0ce1/src/haven/Textlog.java#L43
                    // TODO: @RName("log")
                    throw new NotImplementedException();
                    break;
                case "vm":
                    // @RName("vm")
                    // https://github.com/dolda2000/hafen-client/blob/d61d2a6ff4a832f84e55069794578002d37b0ce1/src/haven/VMeter.java#L37
                    // TODO: @RName("vm")
                    throw new NotImplementedException();
                    break;
                case "wnd":
                    // @RName("wnd")
                    // https://github.com/dolda2000/hafen-client/blob/ccfca692c16d0df250bdd5f6762d5461999a9ab6/src/haven/Window.java#L84
                    // TODO: @RName("wnd")
                    throw new NotImplementedException();
                    break;
                default:
                    {
#if DEBUG
                        Debugger.Break();
#endif
                        throw new NotImplementedException($"Specified in arrived {nameof(RMSG_NEWWDG)} type of Widget '{message.Type}' is not known to {nameof(ResourcesBindingClient)}!");
                    }
            }
        }

        #endregion

        #region RMSG_ADDWDG
        private void Relay_RMSG_ADDWDG_Received(RMSG_ADDWDG message)
        {

        }
        #endregion

        #region RMSG_WDGMSG

        private HashSet<ushort> InitializedWidgets = new HashSet<ushort>();
        private Dictionary<ushort, Queue<RMSG_WDGMSG>> Delayed_RMSG_WDGMSG = new Dictionary<ushort, Queue<RMSG_WDGMSG>>();

        private void EnsureDelayed_RMSG_WDGMSG_WidgetQueueExists(ushort widgetID)
        {
            if (!Delayed_RMSG_WDGMSG.ContainsKey(widgetID))
            {
                lock (Delayed_RMSG_WDGMSG)
                {
                    if (!Delayed_RMSG_WDGMSG.ContainsKey(widgetID))
                        Delayed_RMSG_WDGMSG.Add(widgetID, new Queue<RMSG_WDGMSG>());
                }
            }
        }

        private void Relay_RMSG_WDGMSG_Received(RMSG_WDGMSG message)
        {
            EnsureDelayed_RMSG_WDGMSG_WidgetQueueExists(message.WidgetID);

            Queue<RMSG_WDGMSG> widget_queue = Delayed_RMSG_WDGMSG[message.WidgetID];
            lock (widget_queue)
            {
                widget_queue.Enqueue(message);
            }

            DispatchDelayed_RMSG_WDGMSG(message.WidgetID);
        }

        private void DispatchDelayed_RMSG_WDGMSG(ushort widgetID)
        {
            using (var trace = Trace.Scope(nameof(DispatchDelayed_RMSG_WDGMSG), $"wid: {widgetID}"))
            {
                if (InitializedWidgets.Contains(widgetID))
                {
                    trace.Write($"wid: {widgetID} HIT!");

                    Queue<RMSG_WDGMSG> widget_queue = Delayed_RMSG_WDGMSG[widgetID];

                    lock (widget_queue)
                    {
                        while (widget_queue.Count > 0)
                        {
                            RMSG_WDGMSG delayed_message = widget_queue.Dequeue();
                            if (widgetID != delayed_message.WidgetID) throw new Exception($"{nameof(Delayed_RMSG_WDGMSG)} message for {delayed_message.WidgetID} does not match queue widget ID {widgetID}!");
                            WidgetMessageReceived?.Invoke(delayed_message.WidgetID, delayed_message.MessageName, delayed_message.MessageArguments);
                        }
                    }
                }
            }
        }

        public void RegisterInitializedWidget(ushort widgetID)
        {
            using (Trace.Scope(nameof(RegisterInitializedWidget), $"wid: {widgetID}"))
            {
                EnsureDelayed_RMSG_WDGMSG_WidgetQueueExists(widgetID);
                InitializedWidgets.Add(widgetID);
                DispatchDelayed_RMSG_WDGMSG(widgetID);
            }
        }
        #endregion

        #region RMSG_RESID
        private void Relay_RMSG_RESID_Received(RMSG_RESID message)
        {
            Relay_RMSG_RESID_ReceivedAsync(message).WaitAsync((e) => { throw e; });
        }

        private async Task Relay_RMSG_RESID_ReceivedAsync(RMSG_RESID message)
        {
            await EnsureLocalResourceExistsAsync(message.ResourceName, message.ResourceVersion);
            ResourceName[message.ResourceID] = message.ResourceName;
        }
        #endregion

        private AwaitableDictionary<ushort, string> ResourceName = new AwaitableDictionary<ushort, string>(); // ResourceID, ResourceName



        private async Task EnsureLocalResourceExistsAsync(string resourceName)
        {
            if (!LocalResources.Contains(resourceName))
            {
                HavenResource1 resource = await RemoteResources.GetResourceAsync(resourceName);
                await LocalResources.AddAsync(resourceName, resource);
            }
        }

        private async Task EnsureLocalResourceExistsAsync(string resourceName, ushort resourceVersion)
        {
            if (!LocalResources.Contains(resourceName))
            {
                Trace.TraceInformation($"Local resources CACHE MISSED for '{resourceName}' version {resourceVersion}");
                HavenResource1 resource = await RemoteResources.GetResourceAsync(resourceName);

                if (resourceVersion > resource.Version)
                    Trace.TraceWarning($"Received resource '{resourceName}' version {resource.Version} is less than requested one {resourceVersion}!");

                Trace.TraceInformation($"Resource '{resourceName}' loaded from {nameof(RemoteResources)}");

                await LocalResources.AddAsync(resourceName, resource);
            }
        }

        public delegate void CreateImageButtonWidgetDelegate(ushort widgetID, ushort parentWidgetID, IAddChildArgumentsDeserializer addChildArgumentsDeserializer, object[] addChildArguments, ImageResourceLayer upImageResource, ImageResourceLayer downImageResource, ImageResourceLayer hoverImageResource);
        public event CreateImageButtonWidgetDelegate CreateButtonWidgetReceived;

        public delegate void CreateCharactersListWidgetDelegate(ushort widgetID, ushort parentWidgetID, IAddChildArgumentsDeserializer addChildArgumentsDeserializer, object[] addChildArguments, int height, ImageResourceLayer backgroundImageResource, ImageResourceLayer scrollUpButtonUpImageResource, ImageResourceLayer scrollUpButtonDownImageResource, ImageResourceLayer scrollUpButtonHoverImageResource, ImageResourceLayer scrollDownButtonUpImageResource, ImageResourceLayer scrollDownButtonDownImageResource, ImageResourceLayer scrollDownButtonHoverImageResource);
        public event CreateCharactersListWidgetDelegate CreateCharactersListWidgetReceived;

        public delegate void CreateImageWidgetDelegate(ushort widgetID, ushort parentWidgetID, IAddChildArgumentsDeserializer addChildArgumentsDeserializer, object[] addChildArguments, bool acceptsUserInput, ImageResourceLayer imageResource);
        public event CreateImageWidgetDelegate CreateImageWidgetReceived;

        public delegate void CreateEmptyCenteredWidgetDelegate(ushort widgetID, ushort parentWidgetID, IAddChildArgumentsDeserializer addChildArgumentsDeserializer, object[] addChildArguments, Coord2i size);
        public event CreateEmptyCenteredWidgetDelegate CreateEmptyCenteredWidgetReceived;

        public delegate void CreateGameUserInterfaceWidgetDelegate(ushort widgetID, ushort parentWidgetID, IAddChildArgumentsDeserializer addChildArgumentsDeserializer, object[] addChildArguments, string characterID, int plid, string genus);
        public event CreateGameUserInterfaceWidgetDelegate CreateGameUserInterfaceWidgetReceived;

        public delegate void CreateMapViewWidgetDelegate(ushort widgetID, ushort parentWidgetID, IAddChildArgumentsDeserializer addChildArgumentsDeserializer, object[] addChildArguments, Coord2i size, Coord2i mc, int? pgob);
        public event CreateMapViewWidgetDelegate CreateMapViewWidgetReceived;

        public delegate void CreateGitemWidgetDelegate(ushort widgetID, ushort parentWidgetID, IAddChildArgumentsDeserializer addChildArgumentsDeserializer, object[] addChildArguments, ResourceLayer resource, byte[] data);
        public event CreateGitemWidgetDelegate CreateGitemWidgetReceived;

        public delegate void WidgetMessageReceivedDelegate(ushort widgetID, string messageName, object[] messageArguments);
        public event WidgetMessageReceivedDelegate WidgetMessageReceived;

        public delegate void CreateInventoryWidgetDelegate(ushort widgetID, ushort parentWidgetID, IAddChildArgumentsDeserializer addChildArgumentsDeserializer, object[] addChildArguments, Coord2i size);
        public event CreateInventoryWidgetDelegate CreateInventoryWidgetReceived;

        #region CharactersListWidget RMSG_WDGMSG arguments parsing
        public void Parse_CharactersListWidget_Add_Message(object[] messageArguments, Action<string, object, object> Result)
        {
            using (Trace.Scope(nameof(Parse_CharactersListWidget_Add_Message)))
            {
                CharactersListWidgetAddArguments parsedArguments;
                WidgetArgumentsFormatter.Deserialize(new ArgumentsReader(messageArguments), out parsedArguments);
                Result(parsedArguments.CharacterName, parsedArguments.Desc, parsedArguments.Map);
            }
        }

        public void Parse_CharactersListWidget_Ava_Message(object[] messageArguments, Action<string, object, object> Result)
        {
            CharactersListWidgetAvaArguments parsedArguments;
            WidgetArgumentsFormatter.Deserialize(new ArgumentsReader(messageArguments), out parsedArguments);
            Result(parsedArguments.CharacterName, parsedArguments.Desc, parsedArguments.Map);
        }
        #endregion


        /*
        public event RMSG_WDGMSG.Callback MessageForWidgetReceived;
        public event RMSG_DSTWDG.Callback DestroyWidgetReceived;
        public event RMSG_ADDWDG.Callback AddWidgetReceived;
        public event RMSG_MAPIV.Callback InvalidateCachedMapReceived;
        public event RMSG_GLOBLOB.Callback GlobalObjectsReceived;
        public event RMSG_RESID.Callback ResourceIdReceived;
        public event RMSG_PARTY.Callback GamersPartyInfoReceived;
        public event RMSG_SFX.Callback PlaySoundCommandReceived;
        public event RMSG_CATTR.Callback GlobalAttributeUpdateReceived;
        public event RMSG_MUSIC.Callback PlayMusicCommandReceived;
        public event RMSG_SESSKEY.Callback SessionKeyReceived;
        public event MSG_MAPDATA.Callback MapDataReceived;
        */
    }
}
