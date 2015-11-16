﻿using SharpAdbClient.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace SharpAdbClient.Tests
{
    internal class DummyAdbSocket : IAdbSocket, IDummyAdbSocket
    {
        public DummyAdbSocket()
        {
            this.Connected = true;
        }

        public Queue<AdbResponse> Responses
        {
            get;
        } = new Queue<AdbResponse>();

        public Queue<SyncCommand> SyncResponses
        {
            get;
        } = new Queue<SyncCommand>();

        public Queue<byte[]> SyncDataReceived
        {
            get;
        } = new Queue<byte[]>();

        public Queue<byte[]> SyncDataSent
        {
            get;
        } = new Queue<byte[]>();

        public Queue<string> ResponseMessages
        { get; } = new Queue<string>();

        public List<string> Requests
        { get; } = new List<string>();

        public List<Tuple<SyncCommand, string>> SyncRequests
        { get; } = new List<Tuple<SyncCommand, string>>();

        public bool Connected
        {
            get;
            set;
        }

        public Socket Socket
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            this.Connected = false;
        }

        public void Read(byte[] data)
        {
            var actual = this.SyncDataReceived.Dequeue();

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = actual[i];
            }
        }

        public AdbResponse ReadAdbResponse(bool readDiagString)
        {
            var response = this.Responses.Dequeue();

            if (!response.Okay)
            {
                throw new AdbException(response.Message, response);
            }

            return response;
        }

        public string ReadString()
        {
            return this.ResponseMessages.Dequeue();
        }

        public string ReadSyncString()
        {
            return this.ResponseMessages.Dequeue();
        }

        public Task<string> ReadStringAsync()
        {
            return Task.FromResult(this.ReadString());
        }

        public void SendAdbRequest(string request)
        {
            this.Requests.Add(request);
        }

        public int Read(byte[] data, int timeout)
        {
            return 0;
        }

        public void Close()
        {
            this.Connected = false;
        }

        public void SendSyncRequest(string command, int value)
        {
            throw new NotImplementedException();
        }

        public void Send(byte[] data, int length, int timeout)
        {
            this.SyncDataSent.Enqueue(data.Take(length).ToArray());
        }

        public void Read(byte[] data, int length, int timeout)
        {
            var actual = this.SyncDataReceived.Dequeue();

            Assert.AreEqual(actual.Length, length);

            for (int i = 0; i < length; i++)
            {
                data[i] = actual[i];
            }
        }

        public void SendSyncRequest(SyncCommand command, string path)
        {
            this.SyncRequests.Add(new Tuple<SyncCommand, string>(command, path));
        }

        public SyncCommand ReadSyncResponse()
        {
            return this.SyncResponses.Dequeue();
        }

        public void SendSyncRequest(SyncCommand command, int length)
        {
            this.SyncRequests.Add(new Tuple<SyncCommand, string>(command, length.ToString()));
        }

        public void SendSyncRequest(SyncCommand command, string path, int permissions)
        {
            this.SyncRequests.Add(new Tuple<SyncCommand, string>(command, $"{path},{permissions}"));
        }

        public Stream GetShellStream()
        {
            throw new NotImplementedException();
        }
    }
}