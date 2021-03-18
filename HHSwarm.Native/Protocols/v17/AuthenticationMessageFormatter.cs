using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHSwarm.Native.Protocols.v17.Messages;
using System.IO;
using System.Diagnostics;

namespace HHSwarm.Native.Protocols.v17
{
    public class AuthenticationMessageFormatter : MessageFormatter<Queue<AuthenticationProtocol.AuthenticationContext>>, IAuthenticationMessageSerializer
    {
        private TraceSource Trace = new TraceSource("HHSwarm.Authentication");
        private const int MAX_MESSAGE_SIZE = ushort.MaxValue * 8;

        private AuthenticationProtocol.AuthenticationContext CurrentContext
        {
            get
            {
                return MessageContext.Count > 0 ? MessageContext.Peek() : AuthenticationProtocol.AuthenticationContext.Mktoken;
            }
        }

        public AuthenticationMessageFormatter() : base(new Queue<AuthenticationProtocol.AuthenticationContext>())
        {
        }

        protected override void CallSerializerFromMessage(object message, BinaryWriter writer)
        {
            (message as ISerializableAuthenticationMessage)?.CallSerializer(this, writer);
        }

        #region CMD_TOKEN
        public void Serialize(CMD_TOKEN.REQUEST message, BinaryWriter writer)
        {
            MessageContext.Enqueue(AuthenticationProtocol.AuthenticationContext.Token);
            writer.Write("token");
            writer.Write(message.TokenName);
            writer.Write(message.Token);
        }

        public void Deserialize(BinaryReader reader, out CMD_TOKEN.REQUEST message)
        {
            message = new CMD_TOKEN.REQUEST()
            {
                TokenName = reader.ReadString(),
                Token = reader.ReadBytes(MAX_MESSAGE_SIZE)
            };
        }

        public void Serialize(CMD_TOKEN.RESPONSE_OK message, BinaryWriter writer)
        {
            writer.Write("ok");
            writer.Write(message.AccountName);
        }

        public void Deserialize(BinaryReader reader, out CMD_TOKEN.RESPONSE_OK message)
        {
            message = new CMD_TOKEN.RESPONSE_OK()
            {
                AccountName = reader.ReadString()
            };
        }

        public void Serialize(CMD_TOKEN.RESPONSE_NO message, BinaryWriter writer)
        {
            writer.Write("no");
            writer.Write(message.ErrorMessage);
        }

        public void Deserialize(BinaryReader reader, out CMD_TOKEN.RESPONSE_NO message)
        {
            message = new CMD_TOKEN.RESPONSE_NO()
            {
                ErrorMessage = reader.ReadString()
            };
        }
        #endregion

        #region CMD_PW
        public void Serialize(CMD_PW.REQUEST message, BinaryWriter writer)
        {
            MessageContext.Enqueue(AuthenticationProtocol.AuthenticationContext.Pw);
            writer.Write("pw");
            writer.Write(message.LoginName);
            writer.Write(message.PasswordHash);
        }

        public void Deserialize(BinaryReader reader, out CMD_PW.REQUEST message)
        {
            message = new CMD_PW.REQUEST()
            {
                LoginName = reader.ReadString(),
                PasswordHash = reader.ReadBytes(MAX_MESSAGE_SIZE)
            };
        }

        public void Serialize(CMD_PW.RESPONSE_OK message, BinaryWriter writer)
        {
            writer.Write("ok");
            writer.Write(message.TokenName);
        }

        public void Deserialize(BinaryReader reader, out CMD_PW.RESPONSE_OK message)
        {
            message = new CMD_PW.RESPONSE_OK()
            {
                TokenName = reader.ReadString()
            };
        }

        public void Serialize(CMD_PW.RESPONSE_NO message, BinaryWriter writer)
        {
            writer.Write("no");
            writer.Write(message.ErrorMessage);
        }

        public void Deserialize(BinaryReader reader, out CMD_PW.RESPONSE_NO message)
        {
            message = new CMD_PW.RESPONSE_NO()
            {
                ErrorMessage = reader.ReadString()
            };
        }
        #endregion

        #region CMD_MKTOKEN
        public void Serialize(CMD_MKTOKEN.REQUEST message, BinaryWriter writer)
        {
            MessageContext.Enqueue(AuthenticationProtocol.AuthenticationContext.Mktoken);
            writer.Write("mktoken");
        }

        public void Deserialize(BinaryReader reader, out CMD_MKTOKEN.REQUEST message)
        {
            message = new CMD_MKTOKEN.REQUEST();
        }

        public void Serialize(CMD_MKTOKEN.RESPONSE_OK message, BinaryWriter writer)
        {
            writer.Write("ok");
            writer.Write(message.Token);
        }

        public void Deserialize(BinaryReader reader, out CMD_MKTOKEN.RESPONSE_OK message)
        {
            message = new CMD_MKTOKEN.RESPONSE_OK()
            {
                Token = reader.ReadBytes(MAX_MESSAGE_SIZE)
            };
        }

        public void Serialize(CMD_MKTOKEN.RESPONSE_NO message, BinaryWriter writer)
        {
            writer.Write("no");
        }

        public void Deserialize(BinaryReader reader, out CMD_MKTOKEN.RESPONSE_NO message)
        {
            message = new CMD_MKTOKEN.RESPONSE_NO();
        }
        #endregion

        #region CMD_COOKIE
        public void Serialize(CMD_COOKIE.REQUEST message, BinaryWriter writer)
        {
            MessageContext.Enqueue(AuthenticationProtocol.AuthenticationContext.Cookie);
            writer.Write("cookie");
        }

        public void Deserialize(BinaryReader reader, out CMD_COOKIE.REQUEST message)
        {
            message = new CMD_COOKIE.REQUEST();
        }

        public void Serialize(CMD_COOKIE.RESPONSE_OK message, BinaryWriter writer)
        {
            writer.Write("ok");
            writer.Write(message.Cookie);
        }

        public void Deserialize(BinaryReader reader, out CMD_COOKIE.RESPONSE_OK message)
        {
            message = new CMD_COOKIE.RESPONSE_OK()
            {
                Cookie = reader.ReadBytes(MAX_MESSAGE_SIZE)
            };
        }

        public void Serialize(CMD_COOKIE.RESPONSE_NO message, BinaryWriter writer)
        {
            writer.Write("no");
        }

        public void Deserialize(BinaryReader reader, out CMD_COOKIE.RESPONSE_NO message)
        {
            message = new CMD_COOKIE.RESPONSE_NO();
        }
        #endregion

        public async Task DeserializeAsync(BinaryReader reader, IAuthenticationMessagesReceiverAsync receiver)
        {
            string type = reader.ReadString(); // 'stat'

            switch (type)
            {
                case "token":
                    {
                        MessageContext.Enqueue(AuthenticationProtocol.AuthenticationContext.Token);
                        CMD_TOKEN.REQUEST message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case "pw":
                    {
                        MessageContext.Enqueue(AuthenticationProtocol.AuthenticationContext.Pw);
                        CMD_PW.REQUEST message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case "cookie":
                    {
                        MessageContext.Enqueue(AuthenticationProtocol.AuthenticationContext.Cookie);
                        CMD_COOKIE.REQUEST message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case "mktoken":
                    {
                        MessageContext.Enqueue(AuthenticationProtocol.AuthenticationContext.Mktoken);
                        CMD_MKTOKEN.REQUEST message;
                        Deserialize(reader, out message);
                        await receiver.ReceiveAsync(message);
                    }
                    break;
                case "ok":
                    switch (CurrentContext)
                    {
                        case AuthenticationProtocol.AuthenticationContext.Token:
                            {
                                CMD_TOKEN.RESPONSE_OK message;
                                Deserialize(reader, out message);
                                await receiver.ReceiveAsync(message);
                            }
                            break;
                        case AuthenticationProtocol.AuthenticationContext.Pw:
                            {
                                CMD_PW.RESPONSE_OK message;
                                Deserialize(reader, out message);
                                await receiver.ReceiveAsync(message);
                            }
                            break;
                        case AuthenticationProtocol.AuthenticationContext.Mktoken:
                            {
                                CMD_MKTOKEN.RESPONSE_OK message;
                                Deserialize(reader, out message);
                                await receiver.ReceiveAsync(message);
                            }
                            break;
                        case AuthenticationProtocol.AuthenticationContext.Cookie:
                            {
                                CMD_COOKIE.RESPONSE_OK message;
                                Deserialize(reader, out message);
                                await receiver.ReceiveAsync(message);
                            }
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                    MessageContext.Dequeue();
                    break;
                case "no": // TODO: Check actual string returned for 'cookie' and 'mktoken'
                    switch (CurrentContext)
                    {
                        case AuthenticationProtocol.AuthenticationContext.Token:
                            {
                                CMD_TOKEN.RESPONSE_NO message;
                                Deserialize(reader, out message);
                                await receiver.ReceiveAsync(message);
                            }
                            break;
                        case AuthenticationProtocol.AuthenticationContext.Pw:
                            {
                                CMD_PW.RESPONSE_NO message;
                                Deserialize(reader, out message);
                                await receiver.ReceiveAsync(message);
                            }
                            break;
                        case AuthenticationProtocol.AuthenticationContext.Mktoken:
                            {
                                CMD_MKTOKEN.RESPONSE_NO message;
                                Deserialize(reader, out message);
                                await receiver.ReceiveAsync(message);
                            }
                            break;
                        case AuthenticationProtocol.AuthenticationContext.Cookie:
                            {
                                CMD_COOKIE.RESPONSE_NO message;
                                Deserialize(reader, out message);
                                await receiver.ReceiveAsync(message);
                            }
                            break;
                        default:
                            {
                                string server_error_message = reader.ReadString();
                                throw new InvalidOperationException(server_error_message);
                            }
                    }
                    MessageContext.Dequeue();
                    break;
                default:
                    throw new NotImplementedException($"{nameof(type)}: {type}");
            }

            return;
        }
    }
}
