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
                    {
                        ButtonWidgetCreateArguments create_arguments;
                        WidgetArgumentsFormatter.Deserialize(new ArgumentsReader(message.CreateArguments), out create_arguments);

                        NamedResourceList res = new NamedResourceList
                        (
                            create_arguments.UpImageResourceName,
                            create_arguments.DownImageResource
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
                                    downImageResource: res[create_arguments.DownImageResource].Resource.Images.Single(),
                                    hoverImageResource: res[create_arguments.UpImageResourceName].Resource.Images.Single()
                                )
                        );

                    }
                    break;
                case "charlist":
                    // @RName("charlist")
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
                case "ccnt":
                    // @RName("ccnt")
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
                case "gameui":
                    // @RName("gameui")
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
                    // @RName("item"), class GItem
                    {
                        GitemWidgetCreateArguments create_arguments;
                        WidgetArgumentsFormatter.Deserialize(new ArgumentsReader(message.CreateArguments), out create_arguments);

                        string resource_name = await ResourceName.GetItemAsync(create_arguments.ResourceID);

                        NamedResourceList res = new NamedResourceList(resource_name);

                        // TODO: Gitem - fix position error

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
                    {
                        EquiporyWidgetCreateArguments create_arguments;
                        WidgetArgumentsFormatter.Deserialize(new ArgumentsReader(message.CreateArguments), out create_arguments);

                        Debugger.Break(); // TODO: Implement next! - "epry"
                    }
                    break;
                case "inv":
                    // @RName("inv")
                    {
                        InventoryWidgetCreateArguments create_arguments;
                        WidgetArgumentsFormatter.Deserialize(new ArgumentsReader(message.CreateArguments), out create_arguments);

                        Debugger.Break(); // TODO: Implement next! - "inv"
                    }
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
            Debug.WriteLine($"wid: {widgetID} ==>", "**DispatchDelayed_RMSG_WDGMSG");

            if (InitializedWidgets.Contains(widgetID))
            {
                Debug.WriteLine($"wid: {widgetID} HIT!", "**DispatchDelayed_RMSG_WDGMSG");
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
            Debug.WriteLine($"wid: {widgetID} <==", "**DispatchDelayed_RMSG_WDGMSG");
        }

        public void RegisterInitializedWidget(ushort widgetID)
        {
            Debug.WriteLine($"wid: {widgetID} ==>", "**RegisterInitializedWidget");
            EnsureDelayed_RMSG_WDGMSG_WidgetQueueExists(widgetID);
            InitializedWidgets.Add(widgetID);
            DispatchDelayed_RMSG_WDGMSG(widgetID);
            Debug.WriteLine($"wid: {widgetID} <==", "**RegisterInitializedWidget");
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

        #region CharactersListWidget RMSG_WDGMSG arguments parsing
        public void Parse_CharactersListWidget_Add_Message(object[] messageArguments, Action<string, object, object> Result)
        {
            Debug.WriteLine("===>", "**Parse_CharactersListWidget_Add_Message");
            CharactersListWidgetAddArguments parsedArguments;
            WidgetArgumentsFormatter.Deserialize(new ArgumentsReader(messageArguments), out parsedArguments);
            Result(parsedArguments.CharacterName, parsedArguments.Desc, parsedArguments.Map);
            Debug.WriteLine("<===", "**Parse_CharactersListWidget_Add_Message");
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
