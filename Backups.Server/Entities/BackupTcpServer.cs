using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Backups.Entities;

namespace Backups.Server.Entities
{
    public class BackupTcpServer
    {
        private BackupJoba _backupJoba;
        private string _backupsDirectory;
        public BackupTcpServer(string backupsDirectory)
        {
            _backupJoba = null;
            _backupsDirectory = backupsDirectory;
        }

        public void Start(IPAddress ipAddress, int port)
        {
            TcpListener server = null;
            try
            {
                server = new TcpListener(ipAddress, port);
                server.Start();
                byte[] dataSize = new byte[4];
                byte[] jsonData = null;
                byte[] fileRepresentation = null;
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    stream.Read(dataSize, 0, dataSize.Length);
                    int jsonSize = BitConverter.ToInt32(dataSize);
                    jsonData = new byte[jsonSize];
                    stream.Read(jsonData, 0, jsonSize);
                    stream.Read(dataSize, 0, dataSize.Length);
                    int fileSize = BitConverter.ToInt32(dataSize);
                    fileRepresentation = new byte[fileSize];
                    stream.Read(fileRepresentation, 0, fileSize);
                    string jsonDataString = Encoding.Default.GetString(jsonData);
                    string fileRepresentationString = Encoding.Default.GetString(fileRepresentation);
                    client.Close();
                    ProcessData(jsonDataString, fileRepresentationString);
                }
            }
            catch (SocketException)
            {
                throw;
            }
            finally
            {
                server.Stop();
            }
        }

        public void ProcessData(string jsonDataString, string fileRepresentationString)
        {
            var jsonDocument = JsonDocument.Parse(jsonDataString);
            JsonElement root = jsonDocument.RootElement;
            string messageType = root.GetProperty("MessageType").ToString();
            switch (messageType)
            {
                case "Configuration":
                    Configure(root);
                    break;
                case "AddObject":
                    AddObject(root, fileRepresentationString);
                    break;
                case "RemoveObject":
                    RemoveObject(root, fileRepresentationString);
                    break;
                case "ProcessObjects":
                    _backupJoba.ProcessObjects();
                    break;
            }
        }

        private void Configure(JsonElement configurationJsonElement)
        {
            string backupJobaName = configurationJsonElement.GetProperty("BackupJobaName").ToString();
            string algorithmName = configurationJsonElement.GetProperty("Algorithm").ToString();
            var backupsFactory = new BackupsFactory();
            _backupJoba = new BackupJoba(
                backupJobaName,
                backupsFactory.GetAlgorithm(algorithmName),
                backupsFactory.GetRepository(configurationJsonElement, _backupsDirectory));
        }

        private void AddObject(JsonElement objectJsonElement, string representation)
        {
            string name = objectJsonElement.GetProperty("Name").ToString();
            string extension = objectJsonElement.GetProperty("Extension").ToString();

            _backupJoba.AddJobObject(new UniversalJobObject(name, extension, representation));
        }

        private void RemoveObject(JsonElement objectJsonElement, string representation)
        {
            string name = objectJsonElement.GetProperty("Name").ToString();
            string extension = objectJsonElement.GetProperty("Extension").ToString();

            _backupJoba.RemoveJobObject(new UniversalJobObject(name, extension, representation));
        }
    }
}
