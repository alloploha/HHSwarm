using HHSwarm.Model;
using HHSwarm.Model.Haven;
using HHSwarm.Model.Shared;
using HHSwarm.Model.Widgets;
using HHSwarm.Native.Common;
using HHSwarm.Native.Protocols.Hafen;
using HHSwarm.Native.Protocols.Hafen.Messages;
using HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HHSwarm.Native
{
    public class ModelFactory_v17 : IModelFactory
    {
        ResourcesBindingClient Client;
        RelayClient Relay;
        SessionClient Session;
        RelativePositionComputer Computer;

        public ModelFactory_v17(RelayClient relay, SessionClient session, ResourcesBindingClient client)
        {
            this.Session = session;
            this.Client = client;
            this.Relay = relay;

            this.Computer = new AdvancedRelativePositionComputer
            (
                getWidgetPositionRelativeToParent: (widget_id) =>
                {
                    var widget = Widgets[widget_id];
                    Debug.Assert(widget_id == widget.ID);
                    return GetWidgetRelativePosition(widget.ParentID, widget.ID);
                },

                getWidgetPositionRelativeToAnotherWidget: (parent_widget_id, child_widget_id) => GetWidgetRelativePosition(parent_widget_id, child_widget_id),

                getWidgetSize: (widget_id) => GetWidgetSizeByWidetId(widget_id)
            );
        }

        private Dictionary<ushort, Widget> Widgets = new Dictionary<ushort, Widget>();

        private Coord2i GetWidgetSizeByWidetId(ushort widgetId)
        {
            var size = Widgets[widgetId].Size;
            return new Coord2i(size);
        }

        private const ushort ROOT_WIDGET_ID = 65535;

        private Coord2i GetWidgetAbsolutePosition(ushort thisWidgetId)
        {
            var widget = Widgets[thisWidgetId];

            if (widget.ParentID == ROOT_WIDGET_ID)
                return new Coord2i(widget.Position);
            else
            {
                return GetWidgetAbsolutePosition(widget.ParentID) + new Coord2i(widget.Position);
            }
        }

        private Coord2i GetWidgetRelativePosition(ushort anotherWidgetId, ushort thisWidgetId)
        {
            return GetWidgetAbsolutePosition(anotherWidgetId) - GetWidgetAbsolutePosition(thisWidgetId);
        }

        public Account ConstructAccount(Func<AccountCredentials, ISourceOfCredentials> GetCredentialsStore, Func<Task<AccountCredentials>> GetAccountCredentialsAsync)
        {
            TaskCompletionSource<CharactersListWidget> createCharactersListWidget = new TaskCompletionSource<CharactersListWidget>();

            Client.CreateCharactersListWidgetReceived += (widgetID, parentWidgetID, addChildArgumentsDeserializer, addChildArguments, height, backgroundImageResource, scrollUpButtonUpImageResource, scrollUpButtonDownImageResource, scrollUpButtonHoverImageResource, scrollDownButtonUpImageResource, scrollDownButtonDownImageResource, scrollDownButtonHoverImageResource) =>
            {
                var widget = new CharactersListWidget()
                {
                    ID = widgetID,
                    ParentID = parentWidgetID,
                    Position = addChildArgumentsDeserializer.DeserializeCharactersListWidgetPosition(Computer, addChildArguments, parentWidgetID, widgetID),
                    Capacity = height,
                    BackgroundImage = new Image(backgroundImageResource.ID, backgroundImageResource.Image),
                    ScrollDownButtonUpImage = new Image(scrollDownButtonUpImageResource.ID, scrollDownButtonUpImageResource.Image),
                    ScrollDownButtonDownImage = new Image(scrollDownButtonDownImageResource.ID, scrollDownButtonDownImageResource.Image),
                    ScrollDownButtonHoverImage = new Image(scrollDownButtonHoverImageResource.ID, scrollDownButtonHoverImageResource.Image),
                    ScrollUpButtonUpImage = new Image(scrollUpButtonUpImageResource.ID, scrollUpButtonUpImageResource.Image),
                    ScrollUpButtonDownImage = new Image(scrollUpButtonDownImageResource.ID, scrollUpButtonDownImageResource.Image),
                    ScrollUpButtonHoverImage = new Image(scrollUpButtonHoverImageResource.ID, scrollUpButtonHoverImageResource.Image)
                };

                Widgets.Add(widgetID, widget);

                widget.InitializationCompleted += () => Client.RegisterInitializedWidget(widget.ID);

                Client.WidgetMessageReceived += (messageWidgetID, messageName, messageArguments) =>
                {
                    if (widgetID == messageWidgetID)
                    {
                        switch (messageName)
                        {
                            case "add":
                                {
                                    Debug.WriteLine($"wid: {widgetID}, {messageName} =>", "**WidgetMessageReceived");

                                    Client.Parse_CharactersListWidget_Add_Message(messageArguments, (characterName, desc, map) =>
                                    {
                                        widget.OnAdd(this, characterName, desc, map);
                                    });

                                    Debug.WriteLine($"wid: {widgetID}, {messageName}  <=", "**WidgetMessageReceived");
                                }
                                break;
                            case "ava":
                                {
                                    Client.Parse_CharactersListWidget_Ava_Message(messageArguments, (characterName, desc, map) =>
                                    {
                                        widget.OnAva(this, characterName, desc, map);
                                    });
                                }
                                break;
                            default:
                                throw new NotImplementedException($"Received unexpected message {messageName} for widget {nameof(CharactersListWidget)} ID: {messageWidgetID}");
                        }
                    }
                };

                createCharactersListWidget.SetResult(widget);
            };

            var login = new CommandAsync(async () =>
            {
                await Session.ConnectAsync(GetCredentialsStore(await GetAccountCredentialsAsync()));
            });

            Account result = new Account(GetAccountCredentialsAsync().Result.LoginName, createCharactersListWidget.Task, login);

            return result;
        }

        public Character ConstructCharacter(CharactersListWidget widget, string characterName)
        {
            var play = new CommandAsync(async () =>
            {
                await Relay.SendAsync(new RMSG_WDGMSG()
                {
                    WidgetID = widget.ID,
                    MessageName = "play",
                    MessageArguments = new object[] { characterName }
                });
            });

            Character result = new Character(characterName, play);

            return result;
        }
    }
}
